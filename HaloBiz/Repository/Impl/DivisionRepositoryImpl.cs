using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace HaloBiz.Repository.Impl
{
    public class DivisionRepositoryImpl : IDivisionRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<DivisionRepositoryImpl> _logger;
        private readonly IOperatingEntityRepository _operatingEntityRepository;
        public DivisionRepositoryImpl(HalobizContext context, ILogger<DivisionRepositoryImpl> logger, IOperatingEntityRepository operatingEntityRepository)
        {
            this._logger = logger;
            this._context = context;
            this._operatingEntityRepository = operatingEntityRepository;
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
                .Include(division => division.OperatingEntities
                .Where(operatingEntity => operatingEntity.IsDeleted == false))
                .ThenInclude(x => x.ServiceGroups
                .Where(serviceGroup => serviceGroup.IsDeleted == false))
                .ThenInclude(x => x.ServiceCategories
                .Where(serviceCategories => serviceCategories.IsDeleted == false))
                .FirstOrDefaultAsync( division => division.Id == Id && division.IsDeleted == false);
        }

        public async Task<Division> FindDivisionByName(string name)
        {
            return await _context.Divisions
                .Include(division => division.Head)
                .Include(division => division.OperatingEntities
                .Where(operatingEntity => operatingEntity.IsDeleted == false))
                .ThenInclude(x => x.ServiceGroups
                .Where(serviceGroup => serviceGroup.IsDeleted == false))
                .ThenInclude(x => x.ServiceCategories
                .Where(serviceCategories => serviceCategories.IsDeleted == false))
                .FirstOrDefaultAsync( division => division.Name == name && division.IsDeleted == false);
        }
        public async Task<IEnumerable<Division>> GetAllDivisionAndSbu()
        {
            return await _context.Divisions
                .Include(division => division.OperatingEntities
                .Where(operatingEntity => operatingEntity.IsDeleted == false))
                .ThenInclude(x => x.StrategicBusinessUnits)
                .Where( division => division.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Division>> FindAllDivisions()
        {
            return await _context.Divisions
                .Include(division => division.Head)
                .Include(x=>x.Company)
                .Include(division => division.OperatingEntities
                .Where(operatingEntity => operatingEntity.IsDeleted == false))
                .ThenInclude(x => x.ServiceGroups
                .Where(serviceGroup => serviceGroup.IsDeleted == false))
                .ThenInclude(x => x.ServiceCategories
                .Where(serviceCategories => serviceCategories.IsDeleted == false))
                .Where(division => division.IsDeleted == false)
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
            await _operatingEntityRepository.DeleteOperatingEntityRange(division.OperatingEntities);

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