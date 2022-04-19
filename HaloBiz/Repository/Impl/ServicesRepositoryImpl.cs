using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HaloBiz.Helpers;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Models.Shared;
using Microsoft.Extensions.Configuration;

namespace HaloBiz.Repository.Impl
{
    public class ServicesRepositoryImpl : IServicesRepository
    {
        private readonly HalobizContext _context;
        private readonly IConfiguration _configuration;
        private readonly List<string> _agencies;
        //private readonly string _agencies;
        private readonly ILogger<ServicesRepositoryImpl> _logger;

        public ServicesRepositoryImpl(HalobizContext context, ILogger<ServicesRepositoryImpl> logger, IConfiguration configuration)
        {
            this._context = context;
            this._logger = logger;
            _configuration = configuration;
            //_agencies = _configuration["Codes"] ?? _configuration.GetSection("AppSettings:Codes").Value;
            //_agencies = _configuration.GetSection("AppSettings:ServiceCodes").Get<string[]>().ToList();
        }

         public async Task<Service> SaveService(Service service)
        {
            var savedEntity = await _context.Services.AddAsync(service);
            if(! await SaveChanges())
            {
                return null;
            }

            var savedService = savedEntity.Entity;

            //check if this is an admin and if the direct service is specified
            if (service.ServiceRelationshipEnum == ServiceRelationshipEnum.Admin)
            {
                //map this relationship to the ServiceRelationships
                await _context.ServiceRelationships.AddAsync(new ServiceRelationship
                {
                    AdminServiceId = savedService.Id,
                    DirectServiceId = service.DirectServiceId,
                    CreatedAt = DateTime.Now,
                    CreatedById = savedService.CreatedById,
                    IsDeleted = false                    
                }); 

                await _context.SaveChangesAsync();
            }
            

            var serviceCode = $"{savedService.DivisionId}/{savedService.OperatingEntityId}/{savedService.ServiceGroupId}/{savedService.ServiceCategoryId}/{savedService.Id}";
            savedService.ServiceCode = serviceCode;
            
            return await UpdateServices(savedService);
        }

        public async Task<Service> FindServicesById(long Id)
        {
            var service = await _context.Services
                 .Where(service => service.Id == Id && service.IsDeleted == false)
                 .Include(service => service.DirectRelationship)
                 .Include(service => service.AdminRelationship)
                .FirstOrDefaultAsync( );
            
            if(service == null)
            {
                return null;
            }
            if(service.AccountId > 0)
            {
                service.Account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == service.AccountId.Value && x.IsActive.Value && !x.IsDeleted);
            }
            if(service.ServiceTypeId > 0)
            {
                service.ServiceType = await _context.ServiceTypes.FirstOrDefaultAsync(x => x.Id == service.ServiceTypeId && !x.IsDeleted);
            }
            if (service.OperatingEntityId > 0)
            {
                service.OperatingEntity = await _context.OperatingEntities.FirstOrDefaultAsync(x => x.Id == service.OperatingEntityId && !x.IsDeleted.Value);
            }
            if (service.TargetId > 0)
            {
                service.Target = await _context.Targets.FirstOrDefaultAsync(x => x.Id == service.TargetId && !x.IsDeleted);
            }
            service.ServiceRequiredServiceDocuments = await _context.ServiceRequiredServiceDocuments
                            .Include(x => x.RequiredServiceDocument)
                            .Where(x => x.ServicesId == service.Id).ToListAsync();
            service.ServiceRequredServiceQualificationElements = await _context.ServiceRequredServiceQualificationElements
                            .Include(x => x.RequredServiceQualificationElement)
                            .Where(x => x.ServicesId == service.Id).ToListAsync();

            return service;
        }

        public async Task<ServiceDivisionDetails> GetServiceDetails(long Id)
        {
            var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == Id);
            var serviceDivsionDetails = new ServiceDivisionDetails();
            if(service == null)
            {
                return null;
            }

            serviceDivsionDetails.Division = await _context.Divisions.Where(x => x.Id == service.DivisionId).Select(x => x.Name).FirstOrDefaultAsync();
            serviceDivsionDetails.OperatingEntity = await _context.OperatingEntities.Where(x => x.Id == service.OperatingEntityId).Select(x => x.Name).FirstOrDefaultAsync();
            serviceDivsionDetails.ServiceCategory = await _context.ServiceCategories.Where(x => x.Id == service.ServiceCategoryId).Select(x => x.Name).FirstOrDefaultAsync();
            serviceDivsionDetails.ServiceGroup = await _context.ServiceGroups.Where(x => x.Id == service.ServiceGroupId).Select(x => x.Name).FirstOrDefaultAsync();
            serviceDivsionDetails.Service = service.Name;

            return serviceDivsionDetails;
        }

        public async Task<Service> FindServiceByName(string name)
        {
            return await _context.Services
                .Include(service => service.Target)
                .Include(service => service.ServiceType)
                 .Include(x => x.DirectRelationship)
                .Include(x => x.AdminRelationship)
                .Include(service => service.Account)
                .Include(service => service.ServiceRequiredServiceDocuments.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequiredServiceDocument)
                .Include(service => service.ServiceRequredServiceQualificationElements.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequredServiceQualificationElement)
                .FirstOrDefaultAsync( service => service.Name == name && service.IsDeleted == false);
        }

        public async Task<IEnumerable<Service>> FindAllServices()
        {
            return await _context.Services
                .Include(service => service.Target)               
                .Include(service => service.ServiceType)
                .Include(service => service.Account)
                .Include(service => service.ServiceRequiredServiceDocuments.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequiredServiceDocument)
                    .Include(service => service.ServiceRequredServiceQualificationElements.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequredServiceQualificationElement)                  
                .Where(service => service.IsDeleted == false)               
                    .ToListAsync();
        }

        public async Task<IEnumerable<Service>> FindAllUnplishedServices()
        {
            var contract = new Contract();
            return await _context.Services
                .Include(service => service.Target)
                
                .Include(service => service.ServiceType)
                .Include(service => service.Account)
                .Include(service=>service.Division)
                .Include(service => service.ServiceCategory).AsNoTracking()
                
                .Include(service => service.ServiceGroup).AsNoTracking()   
                .Include(service => service.Division)
                .Include(service => service.OperatingEntity).AsNoTracking()
                 
                .Include(service => service.CreatedBy)
                .Include(service => service.ServiceRequiredServiceDocuments.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequiredServiceDocument)
                    .Include(service => service.ServiceRequredServiceQualificationElements.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequredServiceQualificationElement)
                .Where(service => service.IsRequestedForPublish == true && service.IsPublished == false && service.IsDeleted == false)
                    .ToListAsync();

           
        }

        public IEnumerable<Service> FindAllSecuredMobilityServices()
        {

            //var _agencies = _configuration.GetSection("AppSettings:Codes").Value;
        
                List<Service> services = new List<Service>();
                var _agencies = new List<string>();
                _agencies.Add("40/45/51/52");
                _agencies.Add("35/33/32/33");
                _agencies.Add("40/45/51/54");
                //var _agencies = _configuration.GetSection("AppSettings:ServiceCodes").Get<string[]>().ToList();
                    var quuery = _context.Services
                    .Include(service => service.Target)
                    .Include(service => service.ServiceType)
                    .Include(service => service.Account)
                     .Where(service => service.IsDeleted == false && service.PublishedApprovedStatus == true);
            //.Include(service => service.ServiceRequiredServiceDocuments.Where(row => row.IsDeleted == false))
            //.ThenInclude(row => row.RequiredServiceDocument)
            //.Include(service => service.ServiceRequredServiceQualificationElements.Where(row => row.IsDeleted == false))
            //.ThenInclude(row => row.RequredServiceQualificationElement)


            foreach (var items in _agencies)
                {
                    //quuery.Where(x => x.ServiceCode.Contains(items));
                    services.AddRange(quuery.Where(x => x.ServiceCode.Contains(items)));
                }
                return services.ToList();

        }

        public async Task<IEnumerable<Service>> FindOnlinePortalServices()
        {
            var services = await _context.Services.Where(x => !x.IsDeleted.Value && x.CanBeSoldOnline == true).ToListAsync();
            return services;
        }

        public async Task<Service> UpdateServices(Service service)
        {
            var updatedEntity =  _context.Services.Update(service);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteService(Service service)
        {
            service.IsDeleted = true;
            _context.Services.Update(service);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
           try{
               return  await _context.SaveChangesAsync() > 0;
           }catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }

       
    }
}