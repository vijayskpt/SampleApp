using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleApp
{
    public class CustomExceptionFilter : FilterAttribute,IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {            
            if (!filterContext.ExceptionHandled)
            {
                filterContext.Result = new RedirectResult("ErrorPage.html");
                filterContext.ExceptionHandled = true;
            }
        }
    }
}