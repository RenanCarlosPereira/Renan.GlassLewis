using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Renan.GlassLewis.Service.Authentication;
using Renan.GlassLewis.Service.CompaniesUseCases;
using System.Text;

namespace Renan.GlassLewis.Service.Extentions
{
    public static class ApplicationExtention
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthenticationManager, AuthenticationManager>();
            services.AddTransient<ICompanyUseCase, CompanyUseCase>();

            var section = configuration.GetSection("JwtOptions");
            services.Configure<JwtOptions>(section);
            var jwtOptions = section.Get<JwtOptions>();

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                });
            return services;
        }
    }
}