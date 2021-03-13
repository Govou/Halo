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
    public class OfficeRepositoryImpl : IOfficeRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<OfficeRepositoryImpl> _logger;
        public OfficeRepositoryImpl(DataContext context, ILogger<OfficeRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<Office> SaveOffice(Office office)
        {
            var officeEntity = await _context.Offices.AddAsync(office);
            if(await SaveChanges())
            {
                return officeEntity.Entity;
            }
            return null;
        }

        public async Task<Office> FindOfficeById(long Id)
        {
            return await _context.Offices
                .Include(office => office.Head)
                .Include(office => office.State)
                .Include(office => office.LGA)
                .Include(office => office.Branch)                
                .FirstOrDefaultAsync( office => office.Id == Id && office.IsDeleted == false);
        }

        public async Task<Office> FindOfficeByName(string name)
        {
            return await _context.Offices
                .Include(office => office.Head)
                .Include(office => office.State)
                .Include(office => office.LGA)
                .Include(office => office.Branch) 
                .FirstOrDefaultAsync( office => office.Name == name && office.IsDeleted == false);
        }

        public async Task<IEnumerable<Office>> FindAllOffices()
        {
            return await _context.Offices.Where(division => division.IsDeleted == false)
                .Include(office => office.Head)
                .Include(office => office.State)
                .Include(office => office.LGA)
                .Include(office => office.Branch)
                .ToListAsync();
        }

        public async Task<Office> UpdateOffice(Office office)
        {
            var officeEntity =  _context.Offices.Update(office);
            if(await SaveChanges())
            {
                return officeEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteOffice(Office office)
        {
            office.IsDeleted = true;
            _context.Offices.Update(office);
            return await SaveChanges();
        }

        public async Task<bool> DeleteOfficeRange(IEnumerable<Office> offices)
        {
            foreach (var office in offices)
            {
                office.IsDeleted = true;
            }
            _context.Offices.UpdateRange(offices);
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