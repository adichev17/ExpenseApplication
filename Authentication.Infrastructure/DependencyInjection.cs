using Authentication.Application.Common.Interfaces.Repositories;
using Authentication.Application.Common.Interfaces.Services;
using Authentication.Infrastructure.Persistence;
using Authentication.Infrastructure.Repositories;
using Authentication.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthenticationDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("AuthDatabase")));
            services.AddScoped<IAuthenticationDbContext>(provider => provider.GetService<AuthenticationDbContext>());
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            return services;
        }
    }
}
