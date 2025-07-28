using domitian.Infrastructure.Configuration.Authentication;
using domitian.Infrastructure.Configuration.Exceptions;
using domitian.Infrastructure.Validators;
using domitian.Models.Extensions;
using domitian_api.Data.Identity;
using domitian_api.Extensions;
using domitian_api.Infrastructure.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: AllowedOriginOptions.SectionName,
                    policy =>
                    {
                      policy
                        .WithOrigins(builder.Configuration["AllowedOrigin:Url"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
});

builder.Services.AddHttpContextAccessor();

builder.Services
  .AddControllers(options =>
  {
    options.Filters.Add(new AuthorizeFilter());
    options.OutputFormatters.Add(new StringOutputFormatter());
  })
  .AddXmlSerializerFormatters()
  .AddMvcOptions(options =>
  {
    options.RespectBrowserAcceptHeader = true;
  });

builder.Services.AddProblemDetails(options =>
{
  options.CustomizeProblemDetails = context =>
  {
    context.ProblemDetails.WithInstance($"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}");
    context.ProblemDetails.WithRequestId(context.HttpContext.TraceIdentifier);
    context.ProblemDetails.WithTraceId(context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity?.Id);
  };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<DomitianIDDbContext>(options => options
        .UseSqlServer(builder
                    .Configuration
                    .GetConnectionString(AppConstants.DomitianConnectionString),
                    options => options.MigrationsAssembly(AppConstants.dbContextAssembly)));

builder.Services.AddIdentity<DomitianIDUser, IdentityRole>(options =>
{
  options.SignIn.RequireConfirmedAccount = true;
  options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<DomitianIDDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
 options.DefaultAuthenticateScheme =
 options.DefaultChallengeScheme =
 options.DefaultForbidScheme =
 options.DefaultScheme =
 options.DefaultSignInScheme =
 options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

builder.Services.AddAuthorizationBuilder();

// Registers all validators from the assembly containing the specified Validator class
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
builder.Services.AddServices();
builder.Services.AddHelpers();
builder.Services.AddConfigurationOptions(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGetWithJwtAuth();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();
app.UseRouting();

app.UseCors(AllowedOriginOptions.SectionName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
