using domitian.Infrastructure.Configuration.Authentication;
using domitian_api.Extensions;
using domitian_api.Data.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using domitian_api.Infrastructure.Constants;
using domitian.Infrastructure.Validators;

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
builder.Services.AddControllers(o => o.Filters.Add(new AuthorizeFilter()));

builder.Services.AddDbContext<DomitianIDDbContext>(options => options
        .UseSqlServer(builder
                    .Configuration
                    .GetConnectionString(AppConstants.DominitianConnectionString),
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

app.UseRouting();

app.UseCors(AllowedOriginOptions.SectionName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
