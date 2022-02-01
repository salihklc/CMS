using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Models.CommonModels;

namespace CMS.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;


        public CustomExceptionFilter(
            IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public override void OnException(ExceptionContext context)
        {

            var jsonResult = new JsonResponse();

            try
            {
                jsonResult.IsSuccess = false;

                jsonResult.Message += " - <br> exception : - " + context.Exception.Message;
                jsonResult.Message += " - <br> stackTrace: - " + context.Exception.StackTrace;
                jsonResult.Message += " - <br> hostingEnv: - " + _hostingEnvironment.EnvironmentName;


                var modelState = string.Join(" - <br> -  ", context.ModelState.Values.SelectMany(m => m.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList());

                jsonResult.ExtraData = modelState;

            }
            catch (Exception ex)
            {
                Log.Error("OnException : " + context.Exception.Message + " - " + context.Exception.StackTrace);
            }

            Log.Error("OnException : " + context.Exception.Message + " - " + context.Exception.StackTrace);

            context.Result = new JsonResult(jsonResult);
            return;
        }
    }
}
