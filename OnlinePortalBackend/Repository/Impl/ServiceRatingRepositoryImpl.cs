using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models.OnlinePortal;
using HalobizMigrations.Data;

namespace OnlinePortalBackend.Repository.Impl
{
    public class ServiceRatingRepositoryImpl : IServiceRatingRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceRatingRepositoryImpl> _logger;
        public ServiceRatingRepositoryImpl(HalobizContext context, ILogger<ServiceRatingRepositoryImpl> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ServiceRating> SaveServiceRating(ServiceRating controlRoomAlert)
        {
            var controlRoomAlertEntity = await _context.ServiceRatings.AddAsync(controlRoomAlert);
            if (await SaveChanges())
            {
                return controlRoomAlertEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateServiceRatings(IEnumerable<ServiceRating> controlRoomAlerts)
        {
            _context.ServiceRatings.UpdateRange(controlRoomAlerts);
            return await SaveChanges();
        }

        public async Task<double> FindServiceRatingById(long Id)
        {
            var ratings =  _context.ServiceRatings.Where(x => x.ServiceId == Id && x.IsDeleted == false).Select(x => x.Rating);
            if (ratings.Any())
                return ratings.Average();
            else
                return 0.0;
        }

        public async Task<IEnumerable<ServiceRating>> GetReviewHistoryByServiceId(long Id)
        {
            return await _context.ServiceRatings
                            .Where(x => x.ServiceId == Id && !x.IsDeleted)
                            .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRating>> FindServiceRatingsByUserId(long Id)
        {
            return await _context.ServiceRatings
                .Where(x => x.CreatedById == Id && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRating>> FindAllServiceRatings()
        {
            return await _context.ServiceRatings
                .Where(user => user.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<ServiceRating> UpdateServiceRating(ServiceRating controlRoomAlert)
        {
            var ServiceRatingEntity = _context.ServiceRatings.Update(controlRoomAlert);
            if (await SaveChanges())
            {
                return ServiceRatingEntity.Entity;
            }
            return null;
        }

        public async Task<bool> RemoveServiceRating(ServiceRating controlRoomAlert)
        {
            controlRoomAlert.IsDeleted = true;
            _context.ServiceRatings.Update(controlRoomAlert);
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