using HalobizMigrations.Data;
using HalobizMigrations.Models.Armada;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class DTSDetailGenericDaysRepositoryImpl:IDTSDetailGenericDaysRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<DTSDetailGenericDaysRepositoryImpl> _logger;
        public DTSDetailGenericDaysRepositoryImpl(HalobizContext context, ILogger<DTSDetailGenericDaysRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteArmedEscortGeneric(ArmedEscortDTSDetailGenericDay armedEscort)
        {
            armedEscort.IsDeleted = true;
            _context.ArmedEscortDTSDetailGenericDays.Update(armedEscort);
            return await SaveChanges();
        }

        public async Task<bool> DeleteCommanderGeneric(CommanderDTSDetailGenericDay commander)
        {
            commander.IsDeleted = true;
            _context.CommanderDTSDetailGenericDays.Update(commander);
            return await SaveChanges();
        }

        public async Task<bool> DeletePilotGeneric(PilotDTSDetailGenericDay pilot)
        {
            pilot.IsDeleted = true;
            _context.PilotDTSDetailGenericDays.Update(pilot);
            return await SaveChanges();
        }

        public async Task<bool> DeleteVehicleGeneric(VehicleDTSDetailGenericDay vehicle)
        {
            vehicle.IsDeleted = true;
            _context.VehicleDTSDetailGenericDays.Update(vehicle);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ArmedEscortDTSDetailGenericDay>> FindAllArmedEscortGenerics()
        {
            return await _context.ArmedEscortDTSDetailGenericDays.Where(dts => dts.IsDeleted == false)
            .Include(dts => dts.CreatedBy).Include(dts => dts.DTSMaster).Include(dts => dts.DTSMaster.GenericDays)
            .ToListAsync();
        }

        public async Task<IEnumerable<CommanderDTSDetailGenericDay>> FindAllCommanderGenerics()
        {
            return await _context.CommanderDTSDetailGenericDays.Where(dts => dts.IsDeleted == false)
           .Include(dts => dts.CreatedBy).Include(dts => dts.DTSMaster).Include(dts => dts.DTSMaster.GenericDays)
           .ToListAsync();
        }

        public async Task<IEnumerable<PilotDTSDetailGenericDay>> FindAllPilotGenerics()
        {
            return await _context.PilotDTSDetailGenericDays.Where(dts => dts.IsDeleted == false)
          .Include(dts => dts.CreatedBy).Include(dts => dts.DTSMaster).Include(dts => dts.DTSMaster.GenericDays)
          .ToListAsync();
        }

        public async Task<IEnumerable<VehicleDTSDetailGenericDay>> FindAllVehicleGenerics()
        {
            return await _context.VehicleDTSDetailGenericDays.Where(dts => dts.IsDeleted == false)
          .Include(dts => dts.CreatedBy).Include(dts => dts.DTSMaster).Include(dts => dts.DTSMaster.GenericDays)
          .ToListAsync();
        }

        public async Task<ArmedEscortDTSDetailGenericDay> FindArmedEscortGenericById(long Id)
        {
            return await _context.ArmedEscortDTSDetailGenericDays.Include(dts => dts.DTSMaster)
                 .Include(dts => dts.CreatedBy).Include(dts => dts.DTSMaster.GenericDays)
                 .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<CommanderDTSDetailGenericDay> FindCommanderGenericById(long Id)
        {
            return await _context.CommanderDTSDetailGenericDays.Include(dts => dts.DTSMaster)
                .Include(dts => dts.CreatedBy).Include(dts => dts.DTSMaster.GenericDays)
                .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<PilotDTSDetailGenericDay> FindPilotGenericById(long Id)
        {
            return await _context.PilotDTSDetailGenericDays.Include(dts => dts.DTSMaster)
                .Include(dts => dts.CreatedBy).Include(dts => dts.DTSMaster.GenericDays)
                .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<VehicleDTSDetailGenericDay> FindVehicleGenericById(long Id)
        {
            return await _context.VehicleDTSDetailGenericDays.Include(dts => dts.DTSMaster)
                .Include(dts => dts.CreatedBy).Include(dts => dts.DTSMaster.GenericDays)
                .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public CommanderDTSDetailGenericDay GetCommandername(string Name)
        {
            throw new NotImplementedException();
        }

        public PilotDTSDetailGenericDay GetPilotname(string Name)
        {
            throw new NotImplementedException();
        }

        public ArmedEscortDTSDetailGenericDay GetTypename(string Name)
        {
            throw new NotImplementedException();
        }

        public VehicleDTSDetailGenericDay GetVehiclename(string Name)
        {
            throw new NotImplementedException();
        }

        public async Task<ArmedEscortDTSDetailGenericDay> SaveArmedEscortGeneric(ArmedEscortDTSDetailGenericDay armedEscort)
        {
            var savedEntity = await _context.ArmedEscortDTSDetailGenericDays.AddAsync(armedEscort);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<CommanderDTSDetailGenericDay> SaveCommanderGeneric(CommanderDTSDetailGenericDay commander)
        {
            var savedEntity = await _context.CommanderDTSDetailGenericDays.AddAsync(commander);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotDTSDetailGenericDay> SavePilotGeneric(PilotDTSDetailGenericDay pilot)
        {
            var savedEntity = await _context.PilotDTSDetailGenericDays.AddAsync(pilot);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<VehicleDTSDetailGenericDay> SaveVehicleGeneric(VehicleDTSDetailGenericDay vehicle)
        {
            var savedEntity = await _context.VehicleDTSDetailGenericDays.AddAsync(vehicle);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmedEscortDTSDetailGenericDay> UpdateArmedEscortGeneric(ArmedEscortDTSDetailGenericDay armedEscort)
        {
            var updatedEntity = _context.ArmedEscortDTSDetailGenericDays.Update(armedEscort);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<CommanderDTSDetailGenericDay> UpdateCommanderGeneric(CommanderDTSDetailGenericDay commander)
        {
            var updatedEntity = _context.CommanderDTSDetailGenericDays.Update(commander);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotDTSDetailGenericDay> UpdatePilotGeneric(PilotDTSDetailGenericDay pilot)
        {
            var updatedEntity = _context.PilotDTSDetailGenericDays.Update(pilot);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<VehicleDTSDetailGenericDay> UpdatevehicleGenric(VehicleDTSDetailGenericDay vehicle)
        {
            var updatedEntity = _context.VehicleDTSDetailGenericDays.Update(vehicle);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
