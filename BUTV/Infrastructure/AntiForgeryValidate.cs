using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BUTV.Infrastructure
{
    public class AntiForgeryValidate : ActionFilterAttribute
    {
        

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Request.Headers["RequestVerificationToken"];
        }

    }
}
