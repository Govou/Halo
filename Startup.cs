using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.MyServices;
using HaloBiz.MyServices.Impl;
using HaloBiz.Repository;
using HaloBiz.Repository.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace HaloBiz
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
            services.AddScoped<IStatesService, StatesServiceImpl>();
            services.AddScoped<IUserProfileService, UserProfileServiceImpl>();

            services.AddScoped<IStateRepository, StateRepositoryImpl>();
            services.AddScoped<IBranchRepository, BranchRepositoryImpl>();
            services.AddScoped<IDivisionRepository, DivisionRepositoryImpl>();
            services.AddScoped<IOfficesRepository, OfficesRepositoryImpl>();
            services.AddScoped<IOperatingEntityRepository, OperatingEntityRepositoryImpl>();
            services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepositoryImpl>();
            services.AddScoped<IServiceGroupRepository, ServiceGroupRepositoryImpl>();
            services.AddScoped<IStrategicBusinessUnitRepository, StrategicBusinessUnitRepositoryImpl>();
            services.AddScoped<IUserProfileRepository, UserProfileRepositoryImpl>();
            services.AddScoped<IModificationHistoryRepository, ModificationHistoryRepositoryImpl>();
            
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HaloBiz v1"));
            }

            PrepDb.PrepDatabase(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
