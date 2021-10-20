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

namespace HaloBiz.Repository.Impl
{
    public class ServicesRepositoryImpl : IServicesRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServicesRepositoryImpl> _logger;

        public ServicesRepositoryImpl(HalobizContext context, ILogger<ServicesRepositoryImpl> logger)
        {
            this._context = context;
            this._logger = logger;
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
                .FirstOrDefaultAsync( service => service.Id == Id && service.IsDeleted == false);
            
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
            return await _context.Services
                .Include(service => service.Target)
                .Include(service => service.ServiceType)
                .Include(service => service.Account)
                .Include(service => service.ServiceCategory).AsNoTracking()
                .Include(service => service.ServiceGroup).AsNoTracking()
                .Include(service => service.Division).AsNoTracking()
                .Include(service => service.OperatingEntity).AsNoTracking()
                .Include(service => service.CreatedBy)
                .Include(service => service.ServiceRequiredServiceDocuments.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequiredServiceDocument)
                    .Include(service => service.ServiceRequredServiceQualificationElements.Where(row => row.IsDeleted == false))
                    .ThenInclude(row => row.RequredServiceQualificationElement)
                .Where(service => service.IsRequestedForPublish == true && service.IsPublished == false && service.IsDeleted == false)
                    .ToListAsync();
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