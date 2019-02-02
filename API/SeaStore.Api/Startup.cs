using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeaStore.Services.Common;
using System.Net;
using System.Text;

namespace SeaStore
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      ServiceRegister.Register(services, Configuration);
      services.AddLogging();
      services.AddResponseCaching();
      services.AddMvc();
      services.AddCors();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      //if (env.IsDevelopment())
      //{
      app.UseDeveloperExceptionPage();
      //}

      app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                //.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
        );
      app.UseResponseCaching();
      app.UseStaticFiles();

      //client ID: "720025220681-h64t9rstmp6sbma9v6bag7bt0evicphu.apps.googleusercontent.com"
      //client secret: "iN-qlGgbxoP-oekt3cqfF8FY"

      app.UseExceptionHandler(builder =>
      {
        builder.Run(async context =>
        {
          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          context.Response.ContentType = "application/json";
          var error = context.Features.Get<IExceptionHandlerFeature>();

          if (error != null)
          {
            var errorMessage = $"Unhandled {error.Error.GetType()} while executing {context.Request.Method} {context.Request.Path}\n";

            var tempEx = error.Error;
            errorMessage += tempEx;
            await context.Response.WriteAsync(errorMessage).ConfigureAwait(false);
          }
        });
      });
      //app.UseSwagger();
      //app.UseSwaggerUI(c =>
      //{
      //  c.SwaggerEndpoint("/swagger/SeaStore/swagger.json", "API swagger");
      //});
      app.UseAuthentication();
      app.UseMvc();

    }
  }
}
