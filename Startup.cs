using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Adapters;
using HaloBiz.Adapters.Impl;
using HaloBiz.Data;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using HaloBiz.MyServices.Impl;
using HaloBiz.MyServices.Impl.LAMS;
using HaloBiz.MyServices.Impl.RoleManagement;
using HaloBiz.MyServices.LAMS;
using HaloBiz.MyServices.RoleManagement;
using HaloBiz.Repository;
using HaloBiz.Repository.Impl;
using HaloBiz.Repository.Impl.LAMS;
using HaloBiz.Repository.Impl.RoleManagement;
using HaloBiz.Repository.LAMS;
using HaloBiz.Repository.RoleManagement;
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

//             if (env.IsProduction())
//             {
//                 services.AddAuthorization(options => {
//                     options.FallbackPolicy = new AuthorizationPolicyBuilder()
//                         .RequireAuthenticatedUser()
//                         .Build();
//                 });
//             }      

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
            services.AddScoped<IFinancialVoucherTypeService, FinancialVoucherTypeServiceImpl>();
            services.AddScoped<IAccountService, AccountServiceImpl>();
            services.AddScoped<IGroupTypeService, GroupTypeServiceImpl>();
            services.AddScoped<IRelationshipService, RelationshipServiceImpl>();
            services.AddScoped<IBankService, BankServiceImpl>();
            services.AddScoped<ITargetService, TargetServiceImpl>();
            services.AddScoped<IServiceTypeService, ServiceTypeServiceImpl>();
            services.AddScoped<IStandardSLAForOperatingEntitiesService, StandardSLAForOperatingEntitiesServiceImpl>();
            services.AddScoped<IMeansOfIdentificationService, MeansOfIdentificationServiceImpl>();
            services.AddScoped<IAccountMasterService, AccountMasterServiceImpl>();
            services.AddScoped<IAccountDetailService, AccountDetailServiceImpl>();
            services.AddScoped<IServiceCategoryTaskService, ServiceCategoryTaskServiceImpl>();
            services.AddScoped<IServiceTaskDeliverableService, ServiceTaskDeliverableServiceImpl>();
            services.AddScoped<IRequiredServiceDocumentService, RequiredServiceDocumentServiceImpl>();
            services.AddScoped<IRequredServiceQualificationElementService, RequredServiceQualificationElementServiceImpl>();
            services.AddScoped<IDropReasonService, DropReasonServiceImpl>();
            services.AddScoped<ILeadContactService, LeadContactServiceImpl>();
            services.AddScoped<ILeadKeyPersonService, LeadKeyPersonServiceImpl>();
            services.AddScoped<ILeadDivisionContactService, LeadDivisionContactServiceImpl>();
            services.AddScoped<ICustomerService, CustomerServiceImpl>();
            services.AddScoped<ICustomerDivisionService, CustomerDivisionServiceImpl>();
            services.AddScoped<ILeadService, LeadServiceImpl>();
            services.AddScoped<ILeadDivisionKeyPersonService, LeadDivisionKeyPersonServiceImpl>();
            services.AddScoped<IControlAccountService, ControlAccountServiceImpl>();
            services.AddScoped<IQuoteService, QuoteServiceImpl>();
            services.AddScoped<IQuoteServiceService, QuoteServiceServiceImpl>();
            services.AddScoped<ILeadDivisionService, LeadDivisionServiceImpl>();
            services.AddScoped<IClosureDocumentService, ClosureDocumentServiceImpl>();
            services.AddScoped<IQuoteServiceDocumentService, QuoteServiceDocumentServiceImpl>();
            services.AddScoped<IContractService, ContractServiceImpl>();
            services.AddScoped<IContractServiceService, ContractServiceServiceImpl>();
            services.AddScoped<ISBUToQuoteServiceProportionsService, SBUToQuoteServiceProportionsServiceImpl>();
            services.AddScoped<IRegionService, RegionServiceImpl>();
            services.AddScoped<IZoneService, ZoneServiceImpl>();
            services.AddScoped<INegotiationDocumentService, NegotiationDocumentServiceImpl>();
            services.AddScoped<IRoleService, RoleServiceImpl>();
            services.AddScoped<ILeadConversionService, LeadConversionServiceImpl>();
            services.AddScoped<IOtherLeadCaptureInfoService, OtherLeadCaptureInfoServiceImpl>();
            services.AddScoped<ITaskFulfillmentService, TaskFulfillmentServiceImpl>();
            services.AddScoped<IDeliverableFulfillmentService, DeliverableFulfillmentServiceImpl>();
            services.AddScoped<IIndustryService, IndustryServiceImpl>();
            services.AddScoped<IDesignationService, DesignationServiceImpl>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IReceiptService, ReceiptServiceImpl>();
            services.AddScoped<IApproverLevelService, ApproverLevelServiceImpl>();
            services.AddScoped<IApprovalLimitService, ApprovalLimitServiceImpl>();
            services.AddScoped<IProcessesRequiringApprovalService, ProcessesRequiringApprovalServiceImpl>();
            services.AddScoped<IApprovalService, ApprovalServiceImpl>();
            services.AddScoped<ICompanyService, CompanyServiceImpl>();
            services.AddScoped<IEndorsementTypeService, EndorsementTypeServiceImpl>();
            services.AddScoped<IClientBeneficiaryService, ClientBeneficiaryServiceImpl>();
            services.AddScoped<ISBUProportionService, SBUProportionServiceImpl>();


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
            services.AddScoped<IFinancialVoucherTypeRepository, FinancialVoucherTypeRepositoryImpl>();
            services.AddScoped<IAccountRepository, AccountRepositoryImpl>();
            services.AddScoped<IGroupTypeRepository, GroupTypeRepositoryImpl>();
            services.AddScoped<IRelationshipRepository, RelationshipRepositoryImpl>();
            services.AddScoped<IBankRepository, BankRepositoryImpl>();
            services.AddScoped<ITargetRepository, TargetRepositoryImpl>();
            services.AddScoped<IServiceTypeRepository, ServiceTypeRepositoryImpl>();
            services.AddScoped<IStandardSLAForOperatingEntitiesRepository, StandardSLAForOperatingEntitiesRepositoryImpl>();
            services.AddScoped<IMeansOfIdentificationRepository, MeansOfIdentificationRepositoryImpl>();
            services.AddScoped<IAccountDetailsRepository, AccountDetailRepositoryImpl>();
            services.AddScoped<IAccountMasterRepository, AccountMasterRepositoryImpl>();
            services.AddScoped<IServiceCategoryTaskRepository, ServiceCategoryTaskRepositoryImpl>();
            services.AddScoped<IServiceTaskDeliverableRepository, ServiceTaskDeliverableRepositoryImpl>();
            services.AddScoped<IRequiredServiceDocumentRepository, RequiredServiceDocumentRepositoryImpl>();
            services.AddScoped<IServiceRequiredServiceDocumentRepository, ServiceRequiredServiceDocumentRepositoryImpl>();
            services.AddScoped<IRequredServiceQualificationElementRepository, RequredServiceQualificationElementRepositoryImpl>();
            services.AddScoped<IDropReasonRepository, DropReasonRepositoryImpl>();
            services.AddScoped<IDeleteLogRepository, DeleteLogRepositoryImpl>();
            services.AddScoped<ILeadContactRepository, LeadContactRepositoryImpl>();
            services.AddScoped<ILeadKeyPersonRepository, LeadKeyPersonRepositoryImpl>();
            services.AddScoped<IServiceRequredServiceQualificationElementRepository, ServiceRequredServiceQualificationElementRepositoryImpl>();
            services.AddScoped<ILeadDivisionContactRepository, LeadDivisionContactRepositoryImpl>();
            services.AddScoped<ICustomerRepository, CustomerRepositoryImpl>();
            services.AddScoped<ICustomerDivisionRepository, CustomerDivisionRepositoryImpl>();
            services.AddScoped<ILeadRepository, LeadRepositoryImpl>();
            services.AddScoped<IReferenceNumberRepository, ReferenceNumberRepositoryImpl>();
            services.AddScoped<ILeadDivisionKeyPersonRepository, LeadDivisionKeyPersonRepositoryImpl>();
            services.AddScoped<IControlAccountRepository, ControlAccountRepositoryImpl>();
            services.AddScoped<IQuoteRepository, QuoteRepositoryImpl>();
            services.AddScoped<IQuoteServiceRepository, QuoteServiceRepositoryImpl>();
            services.AddScoped<ILeadDivisionRepository, LeadDivisionRepositoryImpl>();
            services.AddScoped<IContractRepository, ContractRepositoryImpl>();
            services.AddScoped<IClosureDocumentRepository, ClosureDocumentRepositoryImpl>();
            services.AddScoped<IQuoteServiceDocumentRepository, QuoteServiceDocumentRepositoryImpl>();
            services.AddScoped<IContractServiceRepository, ContractServiceRepositoryImpl>();
            services.AddScoped<ISBUToQuoteServiceProportionRepository, SBUToQuoteServiceProportionRepositoryImpl>();
            services.AddScoped<IRegionRepository, RegionRepositoryImpl>();
            services.AddScoped<IZoneRepository, ZoneRepositoryImpl>();
            services.AddScoped<INegotiationDocumentRepository, NegotiationDocumentRepositoryImpl>();
            services.AddScoped<IRoleRepository, RoleRepositoryImpl>();
            services.AddScoped<IOtherLeadCaptureInfoRepository, OtherLeadCaptureInfoRepositoryImpl>();
            services.AddScoped<ITaskFulfillmentRepository, TaskFulfillmentRepositoryImpl>();
            services.AddScoped<IDeliverableFulfillmentRepository, DeliverableFulfillmentRepositoryImpl>();
            services.AddScoped<IIndustryRepository, IndustryRepositoryImpl>();
            services.AddScoped<IDesignationRepository, DesignationRepositoryImpl>();
            services.AddScoped<IInvoiceRepository, InvoiceRepositoryImpl>();
            services.AddScoped<IReceiptRepository, ReceiptRepositoryImpl>();
            services.AddScoped<IApproverLevelRepository, ApproverLevelRepositoryImpl>();
            services.AddScoped<IApprovalLimitRepository, ApprovalLimitRepositoryImpl>();
            services.AddScoped<IProcessesRequiringApprovalRepository, ProcessesRequiringApprovalRepositoryImpl>();
            services.AddScoped<IApprovalRepository, ApprovalRepositoryImpl>();
            services.AddScoped<ICompanyRepository, CompanyRepositoryImpl>();
            services.AddScoped<IEndorsementTypeRepository, EndorsementTypeRepositoryImpl>();
            services.AddScoped<IClientBeneficiaryRepository, ClientBeneficiaryRepositoryImpl>();
            services.AddScoped<IGroupInvoiceTrackerRepository, GroupInvoiceTrackerRepositoryImpl>();
            services.AddScoped<ISBUProportionRepository, SBUProportionRepositoryImpl>();

            // Adapters
            services.AddScoped<IMailAdapter, MailAdapter>();

            services.AddAutoMapper(typeof(Startup));

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
