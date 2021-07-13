using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppi5.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                HandleException(httpContext, ex);
            }
        }

        private void HandleException(HttpContext httpContext, Exception exception)
        {
            string filePath = @"logs.txt";
            if (!File.Exists(filePath)){
                using (StreamWriter streamWriter = File.CreateText(filePath))
                {
                    streamWriter.WriteLine("Exception Logs:");
                }
            }
            using (StreamWriter streamWriter = File.AppendText(filePath))
            {
                streamWriter.WriteLine(DateTime.Now+ " Exception " + exception.Message);
            }
        }
    }
}
