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
    public class DivisionRepositoryImpl : IDivisionRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<DivisionRepositoryImpl> _logger;
        public DivisionRepositoryImpl(DataContext context, ILogger<DivisionRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }

        public async Task<Division> SaveDivision(Division division)
        {
            var DivisionEntity = await _context.Divisions.AddAsync(division);
            if(await SaveChanges())
            {
                return DivisionEntity.Entity;
            }
            return null;
        }

        public async Task<Division> FindDivisionById(long Id)
        {
            return await _context.Divisions
                .Include(division => division.Head)
                .Include(division => division.OperatingEntities)
                .ThenInclude(x => x.ServiceGroups)
                .ThenInclude(x => x.ServiceCategories)
                .FirstOrDefaultAsync( division => division.Id == Id && division.IsDeleted == false);
        }

        public async Task<Division> FindDivisionByName(string name)
        {
            return await _context.Divisions
                .Include(division => division.Head)
                .Include(division => division.OperatingEntities)
                .ThenInclude(x => x.ServiceGroups)
                .ThenInclude(x => x.ServiceCategories)
                .FirstOrDefaultAsync( division => division.Name == name && division.IsDeleted == false);
        }

        public async Task<IEnumerable<Division>> FindAllDivisions()
        {
            return await _context.Divisions.Where(division => division.IsDeleted == false)
                .Include(division => division.Head)
                .Include(division => division.OperatingEntities)
                .ThenInclude(x => x.ServiceGroups)
                .ThenInclude(x => x.ServiceCategories)
                .ToListAsync();
        }

        public async Task<Division> UpdateDivision(Division division)
        {
            var divisionEntity =  _context.Divisions.Update(division);
            if(await SaveChanges())
            {
                return divisionEntity.Entity;
            }
            return null;
        }

        public async Task<bool> RemoveDivision(Division division)
        {
            division.IsDeleted = true;
            _context.Divisions.Update(division);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            try{
                return await _context.SaveChangesAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }


    }
}