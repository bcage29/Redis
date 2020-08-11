using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace SampleAPI.Filters
{
    public class HttpResponseExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is Exception)
            {

                var errorResponse = new
                {
                    error = new {
                        code = (int)HttpStatusCode.InternalServerError,
                        message = context.Exception.Message
                    }
                };

                context.Result = new JsonResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
