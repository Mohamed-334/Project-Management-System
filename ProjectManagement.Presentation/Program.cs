using ProjectManagement.Core;
using ProjectManagement.Core.Shared.CustomMiddleware;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Shared.EmailModels;
using ProjectManagement.Domain.Shared.JwtModels;
using ProjectManagement.Infrastructure;
using ProjectManagement.Infrastructure.Context;
using ProjectManagement.Infrastructure.Context.Interceptors;
using ProjectManagement.Infrastructure.Hubs;
using ProjectManagement.Infrastructure.Seeder;
using ProjectManagement.Service;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ProjectManagement.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            #region Swagger Config
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Base Project",
                    Version = "v1",
                    Description = "API Documentation for Base Project"
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        securityScheme,
                        new string[] {}
                    }
                };
                c.AddSecurityRequirement(securityRequirement);
            });

            #endregion

            #region Context Registration

            // Register the DbContext with the connection string from configuration
            builder.Services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<LoggerSaveChangesInterceptor>();
                options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectManagement"));
                options.AddInterceptors(interceptor);
            });

            #endregion

            #region Identity Config
            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.User.AllowedUserNameCharacters = null;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<EmailTokenProvider<User>>(TokenOptions.DefaultEmailProvider);
            #endregion

            #region Application Cookies Config
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = string.Empty;
                options.AccessDeniedPath = string.Empty;
                options.SlidingExpiration = true;
            });
            #endregion

            #region Jwt Authentication Config
            var jwtSettings = new JwtSettings();
            builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
            builder.Services.AddSingleton(jwtSettings);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ValidAudience = jwtSettings.Audience,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidateLifetime = jwtSettings.ValidateLifeTime,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        var PrefixLength = 15;
                        var SuffixLength = 10;
                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                        {
                            var EncryptedToken = authHeader.Substring("Bearer ".Length).Trim();
                            var CurrentToken = Encoding.UTF8.GetString(Convert.FromBase64String(EncryptedToken));
                            var CleanedToken = CurrentToken.Substring(PrefixLength, CurrentToken.Length - PrefixLength - SuffixLength);
                            context.Token = CleanedToken;
                        }
                        else
                        {
                            var cookieToken = context.Request.Cookies["AccessToken"];
                            if (!string.IsNullOrEmpty(cookieToken))
                            {
                                var CurrentToken = Encoding.UTF8.GetString(Convert.FromBase64String(cookieToken ?? ""));
                                var CleanedToken = CurrentToken.Substring(PrefixLength, CurrentToken.Length - PrefixLength - SuffixLength);
                                context.Token = CleanedToken;
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? string.Empty;
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? string.Empty;
                options.CallbackPath = "/api/Authentication/GoogleAuthenticationCallBack";
                options.SaveTokens = true;
            })
           .AddOAuth("LinkedIn", options =>
           {
               var linkedInConfig = builder.Configuration.GetSection("Authentication:LinkedIn");

               options.ClientId = linkedInConfig["ClientId"] ?? string.Empty;
               options.ClientSecret = linkedInConfig["ClientSecret"] ?? string.Empty;
               options.CallbackPath = "/api/Authentication/LinkedInAuthenticationCallBack";

               options.AuthorizationEndpoint = "https://www.linkedin.com/oauth/v2/authorization";
               options.TokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken";
               options.UserInformationEndpoint = "https://api.linkedin.com/v2/userinfo";

               options.Scope.Add("openid");
               options.Scope.Add("profile");
               options.Scope.Add("email");

               options.SaveTokens = true;

               options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
               options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
               options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
               options.ClaimActions.MapJsonKey("urn:linkedin:picture", "picture");
               options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
               options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");

               options.Events = new OAuthEvents
               {
                   OnCreatingTicket = async context =>
                   {
                       var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                       request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                       request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                       var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                       response.EnsureSuccessStatusCode();

                       var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                       context.RunClaimActions(user.RootElement);
                   },
                   OnRemoteFailure = context =>
                   {
                       context.Response.Redirect("/api/auth/linkedin-error?error=" + context.Failure.Message);
                       context.HandleResponse();
                       return Task.CompletedTask;
                   }
               };
           });


            #endregion

            #region Dependency Injection
            builder.Services.AddInfrastructureDependencies()
                .AddServiceDependencies()
                .AddCoreDependencies();
            #endregion

            #region Localization configuration
            builder.Services.AddControllersWithViews();
            builder.Services.AddLocalization(opt => opt.ResourcesPath = "");

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("de-DE"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("ar-EG")
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            #endregion

            #region AllowCORS
            var CorsConfigName = builder.Configuration.GetSection("CorsConfig")["CorsConfigName"]!;

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CorsConfigName,
                    policy =>
                    {
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                        policy.SetIsOriginAllowed(origin => true);
                        policy.AllowCredentials();
                    });
            });
            #endregion

            #region Email Config
            var emailSettings = new EmailSettings();
            builder.Configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);
            builder.Services.AddSingleton(emailSettings);

            #endregion

            #region SignalR Services
            builder.Services.AddSignalR();
            #endregion

            #region Hangfire Services
            builder.Services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(builder.Configuration.GetConnectionString("ProjectManagement"));
            });
            builder.Services.AddHangfireServer();

            #endregion

            var app = builder.Build();

            #region Seeders
            using (var scope = app.Services.CreateScope())
            {
                var Users = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var Roles = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                await RoleSeeder.SeedAsync(Roles);
                await UserSeeder.SeedAsync(Users);
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Base Architecture v1"));
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Base Architecture v1"));
            }
            app.UseStaticFiles();
            #region Localization middleware
            var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions!.Value);
            #endregion

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseHttpsRedirection();

            app.UseCors(builder.Configuration.GetSection("CorsConfig")["CorsConfigName"]!);

            app.UseAuthentication();
            app.UseAuthorization();

            #region SignalR Middleware

            app.MapHub<NotificationHub>("/notificationHub");

            #endregion

            #region Hangfire Dashboard
            app.UseHangfireDashboard("/dashboard");
            #endregion 
            app.MapControllers();

            app.Run();
        }
    }
}