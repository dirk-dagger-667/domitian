using domitian.Business.Contracts;
using domitian.Business.Services;
using domitian.Business.Services.LoginService;
using domitian.Business.Services.RegisterService;
using domitian.Business.Services.TokenService;
using domitian.Infrastructure.Configuration.Authentication;
using domitian.Infrastructure.Configuration.OptionsSetup;
using domitian.Models.Extensions;
using domitian_api.Helpers;
using domitian_api.Infrastructure.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
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
      services.AddKeyedScoped<IRegisterService, RegisterService>(AppConstants.InnerKey);
      services.AddKeyedScoped<IRegisterService, RegisterServiceCC>(AppConstants.CrossCuttingKey);

      services.AddKeyedScoped<ILoginService, LoginService>(AppConstants.InnerKey);
      services.AddKeyedScoped<ILoginService, LoginServiceCC>(AppConstants.CrossCuttingKey);

      services.AddKeyedScoped<ITokenService, TokenService>(AppConstants.InnerKey);
      services.AddKeyedScoped<ITokenService, TokenServiceCC>(AppConstants.CrossCuttingKey);

      services.AddScoped<IEmailSender, EmailSenderService>();
      services.AddScoped<XmlSerializerOutputFormatter>();

      return services;
    }

    public static IServiceCollection AddDomitianAuthentication(this IServiceCollection services)
    {
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme =
        options.DefaultForbidScheme =
        options.DefaultScheme =
        options.DefaultSignInScheme =
        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer();

      return services;
    }

    public static IServiceCollection AddDomitianProblemDetails(this IServiceCollection services)
    {
      services.AddProblemDetails(options =>
      {
        options.CustomizeProblemDetails = context =>
        {
          context.ProblemDetails.WithInstance($"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}");
          context.ProblemDetails.WithRequestId(context.HttpContext.TraceIdentifier);
          context.ProblemDetails.WithTraceId(context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity?.Id);
        };
      });

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
