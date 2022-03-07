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
    public class VehicleRepositoryImpl : IVehiclesRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<VehicleRepositoryImpl> _logger;
        public VehicleRepositoryImpl(HalobizContext context, ILogger<VehicleRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        //public async Task<bool> DeleteVehicle(Vehicle vehicle)
        //{
        //    vehicle.IsDeleted = true;
        //    _context.Vehicles.Update(vehicle);
        //    return await SaveChanges();
        //}

        public async Task<bool> DeleteVehicleType(VehicleType vehicleType)
        {
            vehicleType.IsDeleted = true;
            _context.VehicleTypes.Update(vehicleType);
            return await SaveChanges();
        }

        //public Task<IEnumerable<Vehicle>> FindAllVehicles()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<IEnumerable<VehicleType>> FindAllVehicleTypes()
        {
            return await _context.VehicleTypes.Where(r => r.IsDeleted == false)
                           .ToListAsync();
        }

        //public Task<Vehicle> FindVehicleById(long Id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<VehicleType> FindVehicleTypeById(long Id)
        {
            return await _context.VehicleTypes
                                                 .FirstOrDefaultAsync(v => v.Id == Id && v.IsDeleted == false);
        }

        //public Task<Vehicle> SaveVehicle(Vehicle vehicle)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<VehicleType> SaveVehicleType(VehicleType vehicleType)
        {
            var savedEntity = await _context.VehicleTypes.AddAsync(vehicleType);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        //public Task<Vehicle> UpdateVehicle(Vehicle vehicle)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<VehicleType> UpdateVehicleType(VehicleType vehicleType)
        {
            var updatedEntity = _context.VehicleTypes.Update(vehicleType);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public VehicleType GetTypename(string Name)
        {
            return _context.VehicleTypes.Where(c => c.TypeName == Name && c.IsDeleted == false).FirstOrDefault();
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
