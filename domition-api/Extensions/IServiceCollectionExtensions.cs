using dominitian.Business.Contracts;
using dominitian.Business.Services;
using dominitian.Infrastructure.Configuration;
using dominitian_api.Helpers;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace domition_api.Extensions
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

            return services;
        }
    }
}
