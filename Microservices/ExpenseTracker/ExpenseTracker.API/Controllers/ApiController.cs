using ExpenseTracker.API.Common.Http;
using ExpenseTracker.Application.Common.Errors.Controls;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<IError> errors, HttpStatusCode httpStatusCode)
        {
            if (errors.Count is 0)
            {
                return Problem();
            }

            var firstError = errors.First();

            HttpContext.Items[HttpContextItemKeys.Errors] = firstError;

            return Problem(statusCode: (int)httpStatusCode, title: firstError.Message);
        }

        // Not specifically using keyword - params.
        protected IActionResult Problem(IError error, HttpStatusCode httpStatusCode)
        {
            return Problem(new List<IError> { error }, httpStatusCode);
        }
    }
}
