using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using HaloBiz.MyServices.Impl;
using HaloBiz.MyServices.Impl.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.Impl;
using HaloBiz.Repository.Impl.LAMS;
using HaloBiz.Repository.LAMS;
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
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            }
            else
            {
                var server = Configuration["DbServer"];
                var port = Configuration["DbPort"];
                var user = Configuration["DbUser"];
                var password = Configuration["DbPassword"];
                var database = Configuration["Database"];
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer($"Server={server},{port};Database={database};User Id={user};Password={password};"));

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


            // singletons
            //services.AddSingleton(Configuration.GetSection("AppSettings").Get<AppSettings>());

            //services
            services.AddScoped<IStatesService, StatesServiceImpl>();
            services.AddScoped<IBranchService, BranchServiceImpl>();
            services.AddScoped<IDivisonService, DivisionServiceImpl>();
            services.AddScoped<IOperatingEntityService, OperatingEntityServiceImpl>();
            services.AddScoped<IOfficeService, OfficeServiceImpl>();
            services.AddScoped<IStrategicBusinessUnitService, StrategicBusinessUnitServiceImpl>();
            services.AddScoped<IUserProfileService, UserProfileServiceImpl>();
            services.AddScoped<IServiceGroupService, ServiceGroupServiceImpl>();
            services.AddScoped<IServiceCategoryService, ServiceCategoryServiceImpl>();
            services.AddScoped<IServicesService, ServicesServiceImpl>();
            services.AddScoped<IAccountClassService, AccountClassServiceImpl>();
            services.AddScoped<ILeadTypeService, LeadTypeServiceImpl>();
            services.AddScoped<ILeadOriginService, LeadOriginServiceImpl>();
            //repositories
            services.AddScoped<IStateRepository, StateRepositoryImpl>();
            services.AddScoped<IBranchRepository, BranchRepositoryImpl>();
            services.AddScoped<IDivisionRepository, DivisionRepositoryImpl>();
            services.AddScoped<IOfficeRepository, OfficeRepositoryImpl>();
            services.AddScoped<IOperatingEntityRepository, OperatingEntityRepositoryImpl>();
            services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepositoryImpl>();
            services.AddScoped<IServiceGroupRepository, ServiceGroupRepositoryImpl>();
            services.AddScoped<IStrategicBusinessUnitRepository, StrategicBusinessUnitRepositoryImpl>();
            services.AddScoped<IUserProfileRepository, UserProfileRepositoryImpl>();
            services.AddScoped<IModificationHistoryRepository, ModificationHistoryRepositoryImpl>();
            services.AddScoped<IServicesRepository, ServicesRepositoryImpl>();
            services.AddScoped<IAccountClassRepository, AccountClassRepositoryImpl>();
            services.AddScoped<ILeadTypeRepository, LeadTypeRepositoryImpl>();
            services.AddScoped<ILeadOriginRepository, LeadOriginRepositoryImpl>();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers()
                .AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HaloBiz", Version = "v1" });
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


            PrepDb.PrepDatabase(app);

            app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
