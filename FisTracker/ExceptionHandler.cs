using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using FisTracker.Data;
using Microsoft.Extensions.Logging;
using System;

namespace FisTracker
{
    public static class ExceptionHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    //context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        string message;
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        switch (contextFeature.Error)
                        {
                            case AppException ae:
                                message = ae.Message;
                                context.Response.StatusCode = (int)ae.ResultCode;
                                break;
                            case UnauthorizedAccessException uae:
                                message = "User is not logged in";
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                break;

                            default:
                                message = "Internal Server Error.";
                                break;
                        }
                        logger.LogError($"Something went wrong: {contextFeature.Error.Message}");
                        await context.Response.WriteAsJsonAsync(new MessageResult()
                        {
                            IsError = true,
                            Message = message
                        });
                    }
                });
            });
        }
    }
}
