using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace APICatalogo.Errors
{
    public static class UseExceptionHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Trace = contextFeature.Error.StackTrace
                        }.ToString());
                    }
                });
            });
        }
    }
}

/*   
    public class MeuMiddleware : IMiddleware
    {

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("Antes");
            next(context);
            Console.WriteLine("Depois");
            return Task.CompletedTask;

        }
    }
    public static class MiddlewareAPI
    {
        public static void Teste(this IApplicationBuilder app)
        {
            app.UseMiddleware<MeuMiddleware>();
        }
    } 

 
 */