using AuthenticationApiService.Http;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AuthenticationApiService.Controllers
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
