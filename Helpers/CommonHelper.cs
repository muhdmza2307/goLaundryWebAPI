using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace goLaundryWebAPI.Helpers
{
    public class CommonHelper
    {
        //temp - will move to front end
        public string hashPass(string userName, string pass)
        {
            // generate a salt variable
            byte[] salt = Encoding.ASCII.GetBytes(userName);

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashedPass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pass,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashedPass;
        }

        public static List<T> ConvertDataTableToListObject<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetObjectItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetObjectItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, (dr[column.ColumnName] == DBNull.Value) ? string.Empty : dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
