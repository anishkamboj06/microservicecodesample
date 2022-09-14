using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.Json;

namespace ConfigurationService.Filters
{

    /// <summary>
    /// This Class used for check the Model validation as filter
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ValidateModelFilter : ActionFilterAttribute
    {

        /// <summary>
        /// Override the Method for return the custom message
        /// Pass CompanyReg as Parameter
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var msg = context.ModelState.Keys
                .SelectMany(key => context.ModelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .FirstOrDefault();
         //       string Message = msg.Field.Replace("$.", "");
                ErrorModel oResultModel = new ErrorModel(Constants.VALIDATION_ERROR,  msg.Message );
                oResultModel.Success = true;//true means right request with wrong validation
                context.Result = new JsonResult(oResultModel);

            }
        }

    }


    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }


        /// <summary>
        /// This Method used fo return the custom validation message
        /// </summary>
        public ValidationError(string field, string message)
        {
            try
            {//message.Trim().ToLower().Contains("cannot") ||

                Field = field != string.Empty ? field : null;
                if (message.Trim().ToLower().Contains("identifier") || message.Trim().ToLower().Contains("invalid") || message.Trim().ToLower().Contains("exception") ||  message.Trim().ToLower().Contains("converting value") || message.Trim().ToLower().Contains("could not convert") || message.Trim().ToLower().Contains("unexpected character") || message.Trim().ToLower().Contains("parsing") || message.Trim().ToLower().Contains("position"))
                    Message = "Invalid input/Coversion error";
                else
                    Message = message.Replace("$.", "");
            }
            catch (Exception)
            {
               
                Message = "Invalid input/Coversion error";
            }
        }


        /// <summary>
        /// This Method used fo return the timespan
        /// </summary>
      
    }





}
