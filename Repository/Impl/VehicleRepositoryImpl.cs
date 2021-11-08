using HalobizMigrations.Data;
using HalobizMigrations.Models.Armada;
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
        public async Task<bool> DeleteVehicle(Vehicle vehicle)
        {
            vehicle.IsDeleted = true;
            _context.Vehicles.Update(vehicle);
            return await SaveChanges();
        }

        public async Task<bool> DeleteVehicleType(VehicleType vehicleType)
        {
            vehicleType.IsDeleted = true;
            _context.VehicleTypes.Update(vehicleType);
            return await SaveChanges();
        }

        public Task<IEnumerable<Vehicle>> FindAllVehicles()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VehicleType>> FindAllVehicleTypes()
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle> FindVehicleById(long Id)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleType> FindVehicleTypeById(long Id)
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle> SaveVehicle(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleType> SaveVehicleType(VehicleType vehicleType)
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle> UpdateVehicle(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleType> UpdateVehicleType(VehicleType vehicleType)
        {
            throw new NotImplementedException();
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
