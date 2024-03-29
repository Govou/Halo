using AutoMapper;
using HaloBiz.Data;
using OnlinePortalBackend.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OnlinePortalBackend.MyServices;
using OnlinePortalBackend.MyServices.Impl;
using OnlinePortalBackend.Repository;
using OnlinePortalBackend.Repository.Impl;
using HalobizMigrations.Data;
using OnlinePortalBackend.Adapters;
using Halobiz.Common.Repository;
using CronJobServiceImpl = OnlinePortalBackend.MyServices.Impl.CronJobServiceImpl;
using ProspectServiceImpl = OnlinePortalBackend.MyServices.Impl.ProspectServiceImpl;
using OnlinePortalBackend.MyServices.SecureMobilitySales;

namespace OnlinePortalBackend
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
            services.AddDbContext<HalobizContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

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

            //services
            services.AddScoped<ISecurityQuestionService, SecurityQuestionServiceImpl>();
            services.AddScoped<IWelcomeNoteService, WelcomeNoteServiceImpl>();
            services.AddScoped<IUserFriendlyQuestionService, UserFriendlyQuestionServiceImpl>();
            services.AddScoped<IPortalComplaintService, PortalComplaintServiceImpl>();
            services.AddScoped<IServiceRatingService, ServiceRatingServiceImpl>();
            services.AddScoped<ICartService, CartServiceImpl>();
            services.AddScoped<IProspectService, ProspectServiceImpl>();
            services.AddScoped<IServiceWishlistService, ServiceWishlistServiceImpl>();
            services.AddScoped<ICronJobService, CronJobServiceImpl>();
           // services.AddScoped<IReceiptService, ReceiptServiceImpl>();
            services.AddScoped<IExistingCustomerService, ExistingCustomerServiceImpl>();

            //repositories
            services.AddScoped<ISecurityQuestionRepository, SecurityQuestionRepositoryImpl>();
            services.AddScoped<IWelcomeNoteRepository, WelcomeNoteRepositoryImpl>();
            services.AddScoped<IUserFriendlyQuestionRepository, UserFriendlyQuestionRepositoryImpl>();
            services.AddScoped<IPortalComplaintRepository, PortalComplaintRepositoryImpl>();
            services.AddScoped<IModificationHistoryRepository, ModificationHistoryRepositoryImpl>();
            services.AddScoped<IServiceRatingRepository, ServiceRatingRepositoryImpl>();
            services.AddScoped<IServiceWishlistRepository, ServiceWishlistRepositoryImpl>();
            services.AddScoped<ICartContractRepository, CartContractRepositoryImpl>();
            services.AddScoped<IEndorsementRepository, EndorsementRepositoryImpl>();
            services.AddScoped<IServicesRepo, ServicesRepo>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository> ();
            services.AddScoped<IComplaintRepository, ComplaintRepository > ();
            services.AddScoped<ISMSAccountRepository, SMSAccountRepository>();
            services.AddScoped<ISMSContractsRepository, SMSContractsRepository>();  
            services.AddScoped<IUtilityRepository, UtilityRepository> ();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ISMSInvoiceRepository, SMSInvoiceRepository>();
            services.AddScoped<ICustomerInfoRepository, CustomerInfoRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();

            services.AddScoped<IPaymentAdapter, PaymentAdapter>();
            services.AddScoped<IApiInterceptor, ApiInterceptor>();
            services.AddScoped<IPaymentAdapter, PaymentAdapter>();

            services.AddScoped<IJwtHelper, JwtHelper>();
            //services and repositories
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IOnlineAccounts, OnlineAccounts>();
            services.AddSingleton<JwtHelper>();
            services.AddScoped<IUserProfileRepository, UserProfileRepositoryImpl>();
            services.AddScoped<ICustomer, Customer>();
            services.AddScoped<IReceiptService, MyServices.Impl.ReceiptServiceImpl>();
            services.AddScoped<ICartContractService, CartContractServiceImpl>();
            services.AddScoped<IEndorsementService, EndorsementServiceImpl>();
            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IInvoiceService, MyServices.Impl.InvoiceService>();
            services.AddScoped<IComplaintService, MyServices.Impl.ComplaintServiceImpl>();
            services.AddScoped<ISMSAccountService, SMSAccountService>();
            services.AddScoped<ISMSContractsService, SMSContractsService>();
            services.AddScoped<IUtilityService, UtilityService>();
            services.AddScoped<ISMSWalletService, SMSWalletService>();
            services.AddScoped<ICustomerInfoService, CustomerInfoService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddAutoMapper(typeof(Startup));


            services.AddControllers()
                .AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddHttpClient();
            services.AddMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlinePortalBackend", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();              
            }
            else
            {
                //Sets Global Exception handler
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseSwagger();

            app.UseExceptionHandlerMiddleware();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlinePortalBackend v1"));

            PrepDb.PrepDatabase(app);

            app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

          //  app.UseAuthentication();

           // app.UseAuthorization();
           // app.UseMiddleware<AuthenticationHandler>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
