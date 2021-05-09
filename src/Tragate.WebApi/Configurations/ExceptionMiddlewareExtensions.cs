using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Tragate.Common.Result;

namespace Tragate.WebApi.Configurations
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app){
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null){
                        await context.Response.WriteAsync(new BadResult()
                        {
                            Success = false,
                            Errors = new List<string>() {"Internal Server Error."}
                        }.ToString());
                    }
                });
            });
        }
    }
}