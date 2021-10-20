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
    public class ServiceRelationshipRepositoryImpl : IServiceRelationshipRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceRelationshipRepositoryImpl> _logger;

        public ServiceRelationshipRepositoryImpl(HalobizContext context, ILogger<ServiceRelationshipRepositoryImpl> logger)
        {
            this._context = context;
            this._logger = logger;
        }

         public async Task<ServiceRelationship> SaveService(ServiceRelationship service)
        {
            var savedEntity = await _context.ServiceRelationships.AddAsync(service);
            var affected = await _context.SaveChangesAsync();

            return affected > 0 ? savedEntity.Entity : null;
        }

        public async Task<ServiceRelationship> FindServiceRelationshipByAdminId(long Id)
        {
            return await _context.ServiceRelationships
                           .Where(x => x.AdminServiceId == Id && x.IsDeleted == false)
                           .Include(x=>x.AdminService)
                           .Include(x=>x.DirectService)
                           .FirstOrDefaultAsync();
        }

        public async Task<ServiceRelationship> FindServiceRelationshipByDirectId(long Id)
        {
            return await _context.ServiceRelationships
                           .Where(x => x.DirectServiceId == Id && x.IsDeleted == false)
                            .Include(x => x.AdminService)
                           .Include(x => x.DirectService)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Service>> FindAllUnmappedDirects()
        {
            return await _context.Services
                          .Where(x => x.IsDeleted == false 
                                && x.ServiceRelationshipEnum == ServiceRelationshipEnum.Direct
                                && !_context.ServiceRelationships.Any(z => z.DirectServiceId == x.Id))                        
                          .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRelationship>> FindAllRelationships()
        {
            return await _context.ServiceRelationships
                           .Where(x=>x.IsDeleted == false)
                           .Include(x => x.AdminService)
                           .Include(x => x.DirectService)
                           .ToListAsync();
        }
    }
}