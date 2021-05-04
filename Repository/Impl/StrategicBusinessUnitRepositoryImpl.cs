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
    public class StrategicBusinessUnitRepositoryImpl : IStrategicBusinessUnitRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<StrategicBusinessUnitRepositoryImpl> _logger;
        public StrategicBusinessUnitRepositoryImpl(HalobizContext context, ILogger<StrategicBusinessUnitRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<StrategicBusinessUnit> SaveStrategyBusinessUnit(StrategicBusinessUnit sbu)
        {
            var savedEntity = await _context.StrategicBusinessUnits.AddAsync(sbu);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<StrategicBusinessUnit> FindStrategyBusinessUnitById(long Id)
        {
            return await _context.StrategicBusinessUnits
                .Include(sbu => sbu.OperatingEntity)
                .FirstOrDefaultAsync( sbu => sbu.Id == Id && sbu.IsDeleted == false);
        }

        public async Task<StrategicBusinessUnit> FindStrategyBusinessUnitByName(string name)
        {
            return await _context.StrategicBusinessUnits
                .Include(sbu => sbu.OperatingEntity)
                .FirstOrDefaultAsync( sbu => sbu.Name == name && sbu.IsDeleted == false);
        }

        public async Task<IEnumerable<StrategicBusinessUnit>> FindAllStrategyBusinessUnits()
        {
            return await _context.StrategicBusinessUnits
                .Include(sbu => sbu.UserProfiles.Where(x => x.IsDeleted == false)).AsNoTracking()
                .Include(sbu => sbu.OperatingEntity)
                .Where(sbu => sbu.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<StrategicBusinessUnit> UpdateStrategyBusinessUnit(StrategicBusinessUnit sbu)
        {
            var updatedEntity =  _context.StrategicBusinessUnits.Update(sbu);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteStrategyBusinessUnit(StrategicBusinessUnit sbu)
        {
            sbu.IsDeleted = true;
            _context.StrategicBusinessUnits.Update(sbu);
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

        public async Task<IEnumerable<object>> GetRMSbus()
        {
            var sbus = await _context.StrategicBusinessUnits
                .Include(x => x.UserProfiles)
                .Where(x => !x.IsDeleted.Value && x.OperatingEntity.Name == "Client Retention")
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    Members = x.UserProfiles
                })
                .ToListAsync();

            return sbus;
        }

        public async Task<IEnumerable<object>> GetRMSbusWithClientsInfo()
        {
            var sbus = await _context.StrategicBusinessUnits
                .Include(x => x.CustomerDivisions)
                .ThenInclude(x => x.Customer)
                .ThenInclude(x => x.GroupType)
                .Where(x => !x.IsDeleted.Value && x.OperatingEntity.Name == "Client Retention")
                .Select(x => new 
                {
                    x.Id,
                    SbuName = x.Name,
                    x.CustomerDivisions
                })
                .ToListAsync();

            var outputs = new List<object>();
            foreach (var sbu in sbus)
            {
                var perGroupTypeBreakDown = sbu.CustomerDivisions
                        .GroupBy(x => x.Customer.GroupType.Caption)
                        .Select(x => 
                            new Dictionary<string, List<CustomerDivision>> {  
                                { x.Key, x.ToList() }
                            })
                        .ToList();

                outputs.Add(new
                {
                    sbu.Id,
                    sbu.SbuName,
                    Clients = perGroupTypeBreakDown
                });
            }

            return outputs;
        }
    }
}