using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FisTracker.Controllers
{
    public class PersistSessionAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //context.HttpContext.Session.Set("persist-session", new byte[] { 1 });
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Session.Set("persist-session", new byte[] { 1 });
        }
        /*
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //context.HttpContext.Session.Set("persist-session", new byte[] { 1 });
            //return Task.CompletedTask;
        }*/
    }
}
