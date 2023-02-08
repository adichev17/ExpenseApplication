using JwtAuthenticationManager.Models;
using JwtAuthenticationManager.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtAuthenticationManager
{
    public static class JwtAuthExtension
    {
        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            var cb = GetJwtSettingsConfiguration();
            var secret = cb.GetValue<string>($"{JwtSettings.SectionName}:Secret");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false; // release - true
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret))
                };
            });

        }

        public static void AddJwtManagerDependency(this IServiceCollection services)
        {          
            var configuration = GetJwtSettingsConfiguration();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IJwtTokenHandler, JwtTokenHandler>();
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        }

        private static IConfiguration GetJwtSettingsConfiguration()
        {
            string basePath = System.AppContext.BaseDirectory;
            return new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile("jwtsettings.json").Build();
        }
    }
}
