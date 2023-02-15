using ExpenseTracker.API.Common.Http;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(IError error, HttpStatusCode httpStatusCode)
        {
            HttpContext.Items[HttpContextItemKeys.Errors] = error;

            return Problem(statusCode: (int)httpStatusCode, title: error.Message);
        }
    }
}
