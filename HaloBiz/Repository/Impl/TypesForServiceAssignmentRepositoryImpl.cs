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
    public class TypesForServiceAssignmentRepositoryImpl : ITypesForServiceAssignmentRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<TypesForServiceAssignmentRepositoryImpl> _logger;
        public TypesForServiceAssignmentRepositoryImpl(HalobizContext context, ILogger<TypesForServiceAssignmentRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<bool> DeletePassengerType(PassengerType passengerType)
        {
            passengerType.IsDeleted = true;
            _context.PassengerTypes.Update(passengerType);
            return await SaveChanges();
        }

        public async Task<bool> DeleteReleaseType(ReleaseType releaseType)
        {
            releaseType.IsDeleted = true;
            _context.ReleaseTypes.Update(releaseType);
            return await SaveChanges();
        }

        public async Task<bool> DeleteSourceType(SourceType sourceType)
        {
            sourceType.IsDeleted = true;
            _context.SourceTypes.Update(sourceType);
            return await SaveChanges();
        }

        public async Task<bool> DeleteTripType(TripType tripType)
        {
            tripType.IsDeleted = true;
            _context.TripTypes.Update(tripType);
            return await SaveChanges();
        }

        public async Task<IEnumerable<PassengerType>> FindAllPassengerTypes()
        {
            return await _context.PassengerTypes.Where(rank => rank.IsDeleted == false)
              .OrderByDescending(x => x.Id)
              .ToListAsync();
        }

        public async Task<IEnumerable<ReleaseType>> FindAllReleaseTypes()
        {
            return await _context.ReleaseTypes.Where(rank => rank.IsDeleted == false)
             .OrderByDescending(x => x.Id)
             .ToListAsync();
        }

        public async Task<IEnumerable<SourceType>> FindAllSourceTypes()
        {
            return await _context.SourceTypes.Where(rank => rank.IsDeleted == false)
              .OrderByDescending(x => x.Id)
              .ToListAsync();
        }

        public async Task<IEnumerable<TripType>> FindAllTripTypes()
        {
            return await _context.TripTypes.Where(rank => rank.IsDeleted == false)
              .OrderByDescending(x => x.Id)
              .ToListAsync();
        }

        public async Task<PassengerType> FindPassengerTypeById(long Id)
        {
            return await _context.PassengerTypes
               .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<ReleaseType> FindReleaseTypeById(long Id)
        {
            return await _context.ReleaseTypes
               .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<SourceType> FindSourceTypeById(long Id)
        {
            return await _context.SourceTypes
               .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<TripType> FindTripTypeById(long Id)
        {
            return await _context.TripTypes
              .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public TripType GetName(string name)
        {
            return _context.TripTypes.Where(c => c.TypeName == name && c.IsDeleted == false).FirstOrDefault();
        }

        public PassengerType GetPassengerName(string name)
        {
            return _context.PassengerTypes.Where(c => c.TypeName == name && c.IsDeleted == false).FirstOrDefault();
        }

        public ReleaseType GetReleaseName(string name)
        {
            return _context.ReleaseTypes.Where(c => c.TypeName == name && c.IsDeleted == false).FirstOrDefault();
        }

        public SourceType GetSourceName(string name)
        {
            return _context.SourceTypes.Where(c => c.TypeName == name && c.IsDeleted == false).FirstOrDefault();
        }

        public async Task<PassengerType> SavePassengerType(PassengerType passengerType)
        {
            var savedEntity = await _context.PassengerTypes.AddAsync(passengerType);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ReleaseType> SaveReleaseType(ReleaseType ReleaseType)
        {
            var savedEntity = await _context.ReleaseTypes.AddAsync(ReleaseType);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<SourceType> SaveSourceType(SourceType sourceType)
        {
            var savedEntity = await _context.SourceTypes.AddAsync(sourceType);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<TripType> SaveTripType(TripType tripType)
        {
            var savedEntity = await _context.TripTypes.AddAsync(tripType);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<PassengerType> UpdatePassengerType(PassengerType passengerType)
        {
            var updatedEntity = _context.PassengerTypes.Update(passengerType);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<ReleaseType> UpdateReleaseType(ReleaseType releaseType)
        {
            var updatedEntity = _context.ReleaseTypes.Update(releaseType);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<SourceType> UpdateSourceType(SourceType sourceType)
        {
            var updatedEntity = _context.SourceTypes.Update(sourceType);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<TripType> UpdateTripType(TripType tripType)
        {
            var updatedEntity = _context.TripTypes.Update(tripType);
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
