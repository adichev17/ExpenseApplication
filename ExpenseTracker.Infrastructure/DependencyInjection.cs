using ExpenseTracker.Application.Common.Interfaces.Repositories;
using ExpenseTracker.Application.Common.Interfaces.Services;
using ExpenseTracker.Infrastructure.Persistence;
using ExpenseTracker.Infrastructure.Repositories;
using ExpenseTracker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ExpenseTrackerDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("ExpenseTrackerDatabase")));

            services.AddScoped<IExpenseTrackerDBContext>(provider => provider.GetService<ExpenseTrackerDBContext>());
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            return services;
        }
    }
}
