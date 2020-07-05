using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warthog.Api.Infrastructure.Filters
{
    public class RestrictHttpsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.IsHttps)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.HttpVersionNotSupported);
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
    }
}
