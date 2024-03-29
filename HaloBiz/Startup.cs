using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Adapters;
using HaloBiz.Adapters.Impl;
using HalobizMigrations.Data;
using HaloBiz.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using HaloBiz.Data;
using HaloBiz.MyServices;
using HaloBiz.MyServices.Impl;
using System.Reflection;
//using Halobiz.Common.MyServices.RoleManagement;
using Halobiz.Repository.RoleManagement;
using Halobiz.Common.MyServices;
using Halobiz.Common.Repository;
using Halobiz.Common.MyServices.RoleManagement;
using HaloBiz.DTOs;
using Halobiz.MyServices;

namespace HaloBiz
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (env.IsDevelopment())
            {
                services.AddDbContext<HalobizContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
                services.AddHttpContextAccessor();
                services.AddSingleton<IUriService>(o =>
                {
                    var accessor = o.GetRequiredService<IHttpContextAccessor>();
                    var request = accessor.HttpContext.Request;
                    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                    return new UriService(uri);
                });
            }
            else
            {
                var server = Configuration["DbServer"];
                var port = Configuration["DbPort"];
                var user = Configuration["DbUser"];
                var password = Configuration["DbPassword"];
                var database = Configuration["Database"];
                services.AddDbContext<HalobizContext>(options =>
                    options.UseSqlServer($"Server={server},{port};Database={database};User Id={user};Password={password};"));
                services.AddHttpContextAccessor();
                services.AddSingleton<IUriService>(o =>
                {
                    var accessor = o.GetRequiredService<IHttpContextAccessor>();
                    var request = accessor.HttpContext.Request;
                    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                    return new UriService(uri);
                });
            }
            
            

            //Authentication with JWT Setup
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                   options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWTSecretKey"] ?? Configuration.GetSection("AppSettings:JWTSecretKey").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            /*if (env.IsProduction())
            {
                services.AddAuthorization(options => {
                    options.FallbackPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                });
            }*/

            // singletons
            //services.AddSingleton(Configuration.GetSection("AppSettings").Get<AppSettings>());
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddScoped<IEmailService,EmailService>();
            services.AddTransient<IProjectManagementMailService, ProjectManagementMailService>();
            services.AddHealthChecks().AddAsyncCheck("Http", async () =>
            {
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    try
                    {
                        string appSwaggerUrl = Configuration.GetConnectionString("HalobizSwaggerUrl");
                        var response = await client.GetAsync(appSwaggerUrl);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception("Url not responding with 200 OK");
                        }
                    }
                    catch (Exception)
                    {
                        return await Task.FromResult(Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Unhealthy());
                    }
                }

                return await Task.FromResult(Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());
            });

            services.RegisterServiceLayerDi();

            services.AddScoped<IRoleService, RoleServiceImpl>();
            services.AddScoped<IRoleRepository, RoleRepositoryImpl>();
            services.AddScoped<IUserProfileService, UserProfileServiceImpl>();
            services.AddScoped<IProjectResolver, ProjectResolver>();
            services.AddScoped<IUserProfileRepository, UserProfileRepositoryImpl>();
            services.AddScoped<IUserAuthentication, UserAuthentication>();

            //leave as singleton along with the dbcontext with lifespan as singleton
            services.AddSingleton<IJwtHelper, JwtHelper>();
            services.AddMemoryCache();

            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

           

            services.AddControllers()
                .AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HaloBiz", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { 
                    Description = @"JWT Authorization header using the Bearer scheme.
                                    Enter your token in the text input below.
                                    Example: 'eyJhbGciOiJSUzI1NiIsImt'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }else
            {
                //Sets Global Exception handler
                app.UseExceptionHandler( builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if(error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HaloBiz v1"));

            app.UseHealthChecks("/healthcheck", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    var result = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(c => new
                        {
                            check = c.Key,
                            result = c.Value.Status.ToString()
                        }),
                    });

                    context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }
            });


            app.UseRouting();
            app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseMiddleware<AuthenticationHandler>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
