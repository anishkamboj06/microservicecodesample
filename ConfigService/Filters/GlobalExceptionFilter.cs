using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Net;

namespace ConfigurationService.Filters
{

    /// <summary>
    /// This Class used for check the Global Exception
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        /// <summary>
        /// AlleryCategory Controller Comstructor
        /// </summary>
        public HttpGlobalExceptionFilter(ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<HttpGlobalExceptionFilter>();

        }

        /// <summary>
        /// This Method used for customize the global exception message
        /// </summary>
        public void OnException(ExceptionContext context)

        {
            var ex = context.Exception;
            string Content = string.Format("{0}\r\n\r{1}\r\n\r\n{2}\r\n\r{3}\r\n\r{4}\r\n{5}\r\n{6}\r\n\rn{7}\r\n\r", "", "ERROR OCCURRED", "DATE & TIME: " + DateTime.Now.ToString("MM-dd-yyyy") + " " + DateTime.Now.ToLongTimeString(), "SOURCE: " + ex.Source, "METHOD: " + ex.TargetSite, "ERROR: " + ex.InnerException, "STACKTRACE: " + ex.StackTrace, "MESSAGE: " + ex.Message);

            _logger.LogError(Content);
          
            ErrorModel oResultModel = new ErrorModel(Constants.EXCEPTION, ex.Message);

            context.Result = new JsonResult(oResultModel);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
        }
    }
}
