using ExpenseTracker.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ExpenseTracker.Infrastructure.Services
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContext;
        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public int GetUserId()
        {
            var rs = _httpContext.HttpContext.User.Claims.ToList();
            return int.Parse(_httpContext.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
