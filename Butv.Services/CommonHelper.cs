using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace BUTV.Services
{
    public static class CommonHelper
    {        
        
        public static string MapPath(string path, IHostingEnvironment env)
        {           
            //not hosted. For example, run in unit tests
            string baseDirectory = env.WebRootPath;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }
        public static string EnsureNotNull(string str)
        {
            return str ?? string.Empty;
        }
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }

            return str;
        }
    }
}
