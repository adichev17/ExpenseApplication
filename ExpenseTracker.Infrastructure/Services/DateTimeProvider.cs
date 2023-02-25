using ExpenseTracker.Application.Common.Interfaces.Services;

namespace ExpenseTracker.Infrastructure.Services
{
    public sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
