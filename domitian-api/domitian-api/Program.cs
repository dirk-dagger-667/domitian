using domitian.Infrastructure.Configuration.Authentication;
using domitian.Infrastructure.Configuration.Exceptions;
using domitian_api.Data.Identity;
using domitian_api.Extensions;
using domitian_api.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
builder
  .AddDomitianCors()
  .AddDomitianDataAccessConfig<DomitianIDDbContext, DomitianIDUser>();

if(builder.Environment.IsDevelopment())
  Serilog.Debugging.SelfLog.Enable(Console.Error);

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

builder.Services
  .AddHttpContextAccessor()
  .AddDomitianProblemDetails()
  .AddExceptionHandler<GlobalExceptionHandler>()
  .AddDomitianAuthentication()
  .AddValidatorsFromAssemblyContaining<RegisterRequestValidator>()
  .AddServices()
  .AddHelpers()
  .AddEndpointsApiExplorer()
  .AddSwaggerGetWithJwtAuth() // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  .AddConfigurationOptions(builder.Configuration)
  .AddAuthorizationBuilder();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  await using (var scope = app.Services.CreateAsyncScope())
  await using (var context = scope.ServiceProvider.GetRequiredService<DomitianIDDbContext>())
  {
    await context.Database.MigrateAsync();
  }

  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();
app.UseRouting();

app.UseCors(AllowedOriginOptions.SectionName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
