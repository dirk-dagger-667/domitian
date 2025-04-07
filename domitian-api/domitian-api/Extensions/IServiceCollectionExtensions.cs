using domitian.Business.Contracts;
using domitian.Business.Services;
using domitian.Infrastructure.Configuration.Authentication;
using domitian.Infrastructure.Configuration.OptionsSetup;
using domitian_api.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.OpenApi.Models;

namespace domitian_api.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IEmailSender, EmailSenderService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            services.AddScoped<IReturnResultsHelper, ReturnResultsHelper>();

            return services;
        }

        public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, ConfigurationManager manager)
        {
            services.Configure<ApiUrlOptions>(manager.GetSection(ApiUrlOptions.SectionName));
            services.Configure<AllowedOriginOptions>(manager.GetSection(AllowedOriginOptions.SectionName));
            services.Configure<JwtTokenOptions>(manager.GetSection(JwtTokenOptions.SectionName));

            services.ConfigureOptions<JwtBearerOptionsSetup>();

            return services;
        }

        public static IServiceCollection AddSwaggerGetWithJwtAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Dominitian API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id=JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[]{}
                    }
                });
            });

            return services;
        }
    }
}
