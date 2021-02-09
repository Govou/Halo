using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{

    public class DesignationRepositoryImpl : IDesignationRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<DesignationRepositoryImpl> _logger;
        public DesignationRepositoryImpl(DataContext context, ILogger<DesignationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<Designation> SaveDesignation(Designation designation)
        {
            var designationEntity = await _context.Designations.AddAsync(designation);
            if(await SaveChanges())
            {
                return designationEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<Designation>> GetDesignations()
        {
            return await _context.Designations
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<Designation> UpdateDesignation(Designation designation)
        {
             var designationEntity =  _context.Designations.Update(designation);
            if(await SaveChanges())
            {
                return designationEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteDesignation(Designation designation)
        {
            designation.IsDeleted = true;
            _context.Designations.Update(designation);
            return await SaveChanges();
        }
        public async Task<Designation> FindDesignationById(long Id)
        {
           return await _context.Designations
            .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        private async Task<bool> SaveChanges()
        {
           try
           {
               return  await _context.SaveChangesAsync() > 0;
           }
           catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}