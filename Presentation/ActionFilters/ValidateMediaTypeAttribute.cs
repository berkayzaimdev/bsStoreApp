using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class ValidateMediaTypeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var acceptHeaderPresent = context.HttpContext
                .Request
                .Headers
                .ContainsKey("Accept");
            // istek başarılı bir şekilde alındıysa

            if(!acceptHeaderPresent) 
            {
                context.Result = new BadRequestObjectResult($"Accept header is missing.");
                return;
            }

            var mediaType = context.HttpContext
                .Response
                .Headers["Accept"]
                .FirstOrDefault();
            // yanıt başarılı bir şekilde verildiyse

            if(MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
            {
                // yanıt header'ında mediatype geçersiz bir değer ise
                context.Result = new BadRequestObjectResult($"Accept header is missing. " +
                    $"Please add Accept header with required media type.");
                return;
            }

            context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);
        }
    }
}
