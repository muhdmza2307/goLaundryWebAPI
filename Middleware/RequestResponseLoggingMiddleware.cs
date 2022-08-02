using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using Newtonsoft.Json;
using goLaundryWebAPI.Log;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Net;
using Microsoft.IO;
using goLaundryWebAPI.Models;

namespace goLaundryWebAPI.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private ILog logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILog logger)
        {
            _next = next;
            this.logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }
        public async Task Invoke(HttpContext context)
        {
            LogMetaData log = new LogMetaData();
            log = await LogRequest(context, log);
            await LogResponse(context, log);
            var message = JsonConvert.SerializeObject(log);
            logger.Information(message);
        }

        private async Task<LogMetaData> LogRequest(HttpContext context, LogMetaData log)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            log.RequestMethod = context.Request.Method;
            log.RequestTimestamp = DateTime.Now;
            log.RequestUri = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path} {context.Request.QueryString}";
            log.RequestContent = ReadStreamInChunks(requestStream);

            context.Request.Body.Position = 0;

            return log;
        }
        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }

        private async Task<LogMetaData> LogResponse(HttpContext context, LogMetaData log)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            log.ResponseStatusCode = (HttpStatusCode)context.Response.StatusCode;
            log.ResponseTimestamp = DateTime.Now;
            log.ResponseContentType = context.Response.ContentType;
            log.ResponseContent = text;

            await responseBody.CopyToAsync(originalBodyStream);

            return log;
        }

    }


}
