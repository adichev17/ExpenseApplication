using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    public class ErrorController : ApiController
    {
        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; 
            if (exception is ValidationException validationException)
            {
                return ValidationProblem(validationException.Errors);
            }

            return Problem();
        }

        private IActionResult ValidationProblem(IEnumerable<ValidationFailure> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return ValidationProblem(modelStateDictionary);
        }
    }
}
