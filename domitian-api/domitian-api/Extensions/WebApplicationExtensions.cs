namespace domitian_api.Extensions
{
  public static class WebApplicationExtensions
  {
    public static IApplicationBuilder UseDomitianExceptionHnadler(this WebApplication app)
    {
      if (!app.Environment.IsDevelopment())
        app.UseExceptionHandler("/error");

      return app;
    }

    public static IApplicationBuilder UseDomitianSwagger(this WebApplication app)
    {
      if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("LocalTest"))
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      return app;
    }
  }
}
