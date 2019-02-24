using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using SeaStore.Entities;
using SeaStore.Entities.DbContexts;
using SeaStore.Services.Common;
using System;
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
    //public object IdentityServerConstants { get; private set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      ServiceRegister.Register(services, Configuration);
      services.AddLogging();
      services.AddResponseCaching();
      services.AddCors();
      services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<SeaStoreDbContext>()
        .AddDefaultTokenProviders();
      services.ConfigureApplicationCookie(options => {
        options.AccessDeniedPath = new PathString("/Account/Login");
        options.LoginPath = new PathString("/Account/Login");
        options.LogoutPath = new PathString("/Account/LogOff");
      });
      services.AddAuthentication(options =>
      {
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
      })
      .AddFacebook(o =>
      {
        o.AppId = Configuration["Authorization:Facebook:ClientSecret"];
        o.AppSecret = Configuration["Authorization:Facebook:ClientSecret"];
      })
      //.AddOpenIdConnect(o =>
      //{
      //  o.ClientId = Configuration["Authorization:Google:ClientId"];
      //  o.ClientSecret = Configuration["Authorization:Google:ClientSecret"];
      //  o.Authority = "https://accounts.google.com";
      //  o.ResponseType = "code";
      //  o.GetClaimsFromUserInfoEndpoint = true;
      //})
      .AddGoogle(o =>
      {
        o.ClientId = Configuration["Authorization:Google:ClientId"];
        o.ClientSecret = Configuration["Authorization:Google:ClientSecret"];
        o.AuthorizationEndpoint = "https://accounts.google.com";
      });

      services.AddOpenIddict()

          // Register the OpenIddict core services.
          .AddCore(options =>
          {
            // Configure OpenIddict to use the Entity Framework Core stores and models.
            options.UseEntityFrameworkCore()
                   .UseDbContext<SeaStoreDbContext>();
          })

          // Register the OpenIddict server handler.
          .AddServer(options =>
          {
            // Register the ASP.NET Core MVC services used by OpenIddict.
            // Note: if you don't call this method, you won't be able to
            // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
            //options.UseMvc();

            // Enable the authorization, logout, token and userinfo endpoints.
            options.EnableAuthorizationEndpoint("/connect/authorize")
                   .EnableLogoutEndpoint("/connect/logout")
                   .EnableTokenEndpoint("/connect/token")
                   .EnableUserinfoEndpoint("/api/userinfo");

            // Note: the Mvc.Client sample only uses the code flow and the password flow, but you
            // can enable the other flows if you need to support implicit or client credentials.
            options.AllowAuthorizationCodeFlow()
                   .AllowPasswordFlow()
                   .AllowRefreshTokenFlow();

            // Mark the "email", "profile" and "roles" scopes as supported scopes.
            options.RegisterScopes(OpenIddictConstants.Scopes.Email,
                                   OpenIddictConstants.Scopes.Profile,
                                   OpenIddictConstants.Scopes.Roles);

            // When request caching is enabled, authorization and logout requests
            // are stored in the distributed cache by OpenIddict and the user agent
            // is redirected to the same page with a single parameter (request_id).
            // This allows flowing large OpenID Connect requests even when using
            // an external authentication provider like Google, Facebook or Twitter.
            options.EnableRequestCaching();

            // During development, you can disable the HTTPS requirement.
            options.DisableHttpsRequirement();

            // Note: to use JWT access tokens instead of the default
            // encrypted format, the following lines are required:
            //
            // options.UseJsonWebTokens();
            // options.AddEphemeralSigningKey();

            // Note: if you don't want to specify a client_id when sending
            // a token or revocation request, uncomment the following line:
            //
            // options.AcceptAnonymousClients();

            // Note: if you want to process authorization and token requests
            // that specify non-registered scopes, uncomment the following line:
            //
            // options.DisableScopeValidation();

            // Note: if you don't want to use permissions, you can disable
            // permission enforcement by uncommenting the following lines:
            //
            // options.IgnoreEndpointPermissions()
            //        .IgnoreGrantTypePermissions()
            //        .IgnoreScopePermissions();
          })

          // Register the OpenIddict validation handler.
          // Note: the OpenIddict validation handler is only compatible with the
          // default token format or with reference tokens and cannot be used with
          // JWT tokens. For JWT tokens, use the Microsoft JWT bearer handler.
          .AddValidation();
      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }
      //app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
      //{
      //  Authority = Configuration["Settings:Authentication:Authority"],
      //  RequireHttpsMetadata = false,

      //  ApiName = Configuration["Settings:Authentication:ApiName"],
      //  ApiSecret = Configuration["Settings:Authentication:ApiSecret"],
      //  EnableCaching = true,
      //  CacheDuration = TimeSpan.FromMinutes(10),

      //  AutomaticAuthenticate = true,
      //  AutomaticChallenge = true
      //});

      app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                //.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
        );
      app.UseResponseCaching();
      app.UseStaticFiles();
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
      app.UseAuthentication();
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        RequireHeaderSymmetry = false,
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      //app.UseSwagger();
      //app.UseSwaggerUI(c =>
      //{
      //  c.SwaggerEndpoint("/swagger/SeaStore/swagger.json", "API swagger");
      //});
      app.UseMvc(routes =>
      {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
