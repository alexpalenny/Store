using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using SeaStore.Entities;
using SeaStore.Entities.DbContexts;
using SeaStore.Services.Common;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SeaStore
{
  public class Startup
  {
    private readonly IHostingEnvironment _env;
    public Startup(IConfiguration configuration, IHostingEnvironment env)
    {
      Configuration = configuration;
      _env = env;
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
      services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<SeaStoreDbContext>()
        .AddDefaultTokenProviders();
      services.ConfigureApplicationCookie(options =>
      {
        options.AccessDeniedPath = new PathString("/Account/Login");
        options.LoginPath = new PathString("/Account/Login");
        options.LogoutPath = new PathString("/Account/LogOff");
      });
      services.AddAuthentication(options =>
      {
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = OpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
      })
      //services.AddAuthentication(options =>
      //{
      //  options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      //  options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      //  options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      //})
     .AddFacebook(o =>
     {
       o.AppId = Configuration["Authorization:Facebook:AppId"];
       o.AppSecret = Configuration["Authorization:Facebook:AppSecret"];
     })
     .AddTwitter(o =>
     {
       o.ConsumerKey = Configuration["Authorization:Twitter:ConsumerKey"];
       o.ConsumerSecret = Configuration["Authorization:Twitter:ConsumerSecret"];
     })
     .AddGoogle(o =>
     {
       o.ClientId = Configuration["Authorization:Google:ClientId"];
       o.ClientSecret = Configuration["Authorization:Google:ClientSecret"];
       // o.AuthorizationEndpoint = "https://accounts.google.com";
     })
     //.AddOpenIdConnect(o =>
     //{
     //  o.Authority = "https://accounts.google.com";
     //  o.RequireHttpsMetadata = false;
     //  o.ClientId = "mvc";
     //  o.SaveTokens = true;       
     //  o.ResponseType = "code";
     //  o.GetClaimsFromUserInfoEndpoint = true;
     //})
     .AddCookie();

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
            options.UseMvc();

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
            // options.DisableHttpsRequirement();

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

      //if (!_env.IsDevelopment())
      //  services.Configure<MvcOptions>(o =>
      //      o.Filters.Add(new RequireHttpsAttribute()));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
    public void Configure(IApplicationBuilder app)
    {
      if (_env.IsDevelopment())
      {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

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

      app.UseStatusCodePagesWithReExecute("/error");
      app.UseAuthentication();
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        RequireHeaderSymmetry = false,
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      app.UseMvcWithDefaultRoute();

      // Seed the database with the sample applications.
      // Note: in a real world application, this step should be part of a setup script.
      app.UseMvcWithDefaultRoute();
      InitializeAsync(app.ApplicationServices).GetAwaiter().GetResult();

      //app.UseSwagger();
      //app.UseSwaggerUI(c =>
      //{
      //  c.SwaggerEndpoint("/swagger/SeaStore/swagger.json", "API swagger");
      //});
    }
    private async Task InitializeAsync(IServiceProvider services)
    {
      // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
      using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
      {
        var context = scope.ServiceProvider.GetRequiredService<SeaStoreDbContext>();
        await context.Database.EnsureCreatedAsync();

        var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

        if (await manager.FindByClientIdAsync("mvc") == null)
        {
          var descriptor = new OpenIddictApplicationDescriptor
          {
            ClientId = "mvc",
            ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
            DisplayName = "MVC client application",
            PostLogoutRedirectUris = { new Uri("http://localhost:56884/signout-callback-oidc") },
            RedirectUris = { new Uri("http://localhost:56884/signin-oidc") },
            Permissions =
                        {
                            OpenIddictConstants.Permissions.Endpoints.Authorization,
                            OpenIddictConstants.Permissions.Endpoints.Logout,
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                            OpenIddictConstants.Permissions.Scopes.Email,
                            OpenIddictConstants.Permissions.Scopes.Profile,
                            OpenIddictConstants.Permissions.Scopes.Roles
                        }
          };

          await manager.CreateAsync(descriptor);
        }

        // To test this sample with Postman, use the following settings:
        //
        // * Authorization URL: http://localhost:56884/connect/authorize
        // * Access token URL: http://localhost:56884/connect/token
        // * Client ID: postman
        // * Client secret: [blank] (not used with public clients)
        // * Scope: openid email profile roles
        // * Grant type: authorization code
        // * Request access token locally: yes
        if (await manager.FindByClientIdAsync("postman") == null)
        {
          var descriptor = new OpenIddictApplicationDescriptor
          {
            ClientId = "postman",
            DisplayName = "Postman",
            RedirectUris = { new Uri("https://www.getpostman.com/oauth2/callback") },
            Permissions =
                        {
                            OpenIddictConstants.Permissions.Endpoints.Authorization,
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                            OpenIddictConstants.Permissions.Scopes.Email,
                            OpenIddictConstants.Permissions.Scopes.Profile,
                            OpenIddictConstants.Permissions.Scopes.Roles
                        }
          };

          await manager.CreateAsync(descriptor);
        }
      }
    }
  }
}
