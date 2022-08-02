using goLaundryWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Authorization
{
    public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    {
        public AuthorizationPolicy Policy { get; }

        public CustomAuthorizeFilter(AuthorizationPolicy policy)
        {
            Policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                                 .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            // Allow Anonymous skips all authorization
            if (hasAllowAnonymous)
            {
                return;
            }

            var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
            var authenticateResult = await policyEvaluator.AuthenticateAsync(Policy, context.HttpContext);
            var authorizeResult = await policyEvaluator.AuthorizeAsync(Policy, authenticateResult, context.HttpContext, context);

            //ResponseModel2 resp = new ResponseModel2();

            RespModel<object> resp = new RespModel<object>();

            if (authorizeResult.Challenged)
            {
                // Return custom 401 result
                //context.Result = new CustomUnauthorizedResult("Authorization failed.");

                //resp.setResponse(-1, "Authorization failed.", true);
                //context.Result = new CustomUnauthorizedResult(resp); ;

                resp.Error(Convert.ToString(Convert.ToInt32(HttpStatusCode.Unauthorized)), "Authorization failed.");
                context.Result = new CustomUnauthorizedResult(resp);
            }
            else if (authorizeResult.Forbidden)
            {
                // Return default 403 result
                context.Result = new ForbidResult(Policy.AuthenticationSchemes.ToArray());
            }
        }
    }
}
