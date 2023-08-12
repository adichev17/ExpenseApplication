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

        public Guid GetUserId()
        {
            var userId = _httpContext?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new InvalidOperationException(nameof(userId));
            }

            return Guid.Parse(userId);
        }
    }
}
