﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Mockaco
{
    public class ResponseDelayMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseDelayMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IMockacoContext mockacoContext, ILogger<ResponseDelayMiddleware> logger)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(httpContext);

            if(mockacoContext.TransformedTemplate == null)
            {
                return;
            }

            int responseDelay = mockacoContext.TransformedTemplate.Response.Delay.GetValueOrDefault();
            var remainingDelay = responseDelay - (int)stopwatch.ElapsedMilliseconds;
            if (remainingDelay > 0)
            {
                logger.LogDebug($"Delaying the response for at least {responseDelay} milliseconds");

                await Task.Delay(remainingDelay);
            }
        }
    }
}
