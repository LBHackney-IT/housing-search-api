using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace HousingSearchApi.V1.Infrastructure
{
    public class CORSMiddleware
    {
        private readonly RequestDelegate _next;

        public CORSMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Response.Headers[Constants.CorsHeader].Count == 0)
            {
                context.Response.Headers[Constants.CorsHeader] = "*";
            }
            if (context.Response.Headers[Constants.CorsHeaderMethods].Count == 0)
            {
                context.Response.Headers[Constants.CorsHeaderMethods] = "OPTIONS,HEAD,GET,POST,PUT,PATCH,DELETE";
            }
            if (context.Response.Headers[Constants.CorsHeaderHeaders].Count == 0)
            {
                context.Response.Headers[Constants.CorsHeaderHeaders] = "Authorization,Content-Type,X-Amz-Date,X-Amz-Security-Token,X-Api-Key";
            }

            if (_next != null)
                await _next(context).ConfigureAwait(false);
        }
    }

    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers[Constants.CorrelationId].Count == 0)
            {
                context.Request.Headers[Constants.CorrelationId] = Guid.NewGuid().ToString();
            }

            if (_next != null)
                await _next(context).ConfigureAwait(false);
        }
    }

    public static class CorrelationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelation(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationMiddleware>();
        }
        
        public static IApplicationBuilder UseCORS(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CORSMiddleware>();
        }
    }
}
