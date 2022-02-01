using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.WebSite.Models;

namespace CMS.WebSite.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public CustomExceptionFilterAttribute(
            IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {

            var viewResult = new ViewResult() { };
            viewResult.ViewName = "Error";
            var viewModel = new ErrorViewModel();
            try
            {


                viewModel.Error = context.Exception.Message;
                viewModel.StackTrace = context.Exception.StackTrace;
                viewModel.Environment = _hostingEnvironment.EnvironmentName;

                var modelState = string.Join(" - <br> -  ", context.ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList());

                viewModel.ModelStateErrors = modelState;


            }
            catch (System.Exception ex)
            {
                Log.Error("OnException : " + context.Exception.Message + " - " + context.Exception.StackTrace);
            }

            Log.Error("OnException : " + context.Exception.Message + " - " + context.Exception.StackTrace);

            viewResult.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
                                {
                                    {"ErrorViewModel", JsonConvert.SerializeObject(viewModel) },
                                };

            context.Result = viewResult;
            return;
        }
    }
}
