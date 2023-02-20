using ExpenseTracker.API.Common.Http;
using ExpenseTracker.Domain.Common.Errors.Controls;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors, HttpStatusCode httpStatusCode)
        {
            if (errors.Count is 0)
            {
                return Problem();
            }

            if (errors.All(x => x is ValidationError))
            {
                return ValidationProblem(errors);
            }

            var firstError = errors.First();

            HttpContext.Items[HttpContextItemKeys.Errors] = firstError;

            return Problem(statusCode: (int)httpStatusCode, title: firstError.Message);
        }

        private IActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                foreach (var errorMetadata in error.Metadata)
                {
                    modelStateDictionary.AddModelError(errorMetadata.Key, errorMetadata.Value.ToString());
                }
            }

            return ValidationProblem(modelStateDictionary);
        }
    }
}
