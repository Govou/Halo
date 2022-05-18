using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models.OnlinePortal;
using HalobizMigrations.Data;
using OnlinePortalBackend.DTOs.TransferDTOs;

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

        public async Task<ServiceRating> SaveServiceRating(ServiceRating rating)
        {
            rating.UpdatedAt = DateTime.Now;
            rating.CustomerDivisionId = rating.CustomerDivisionId;
            rating.CreatedById = _context.UserProfiles.FirstOrDefault(x => x.FirstName.ToLower() == "seeder").Id;
            rating.CreatedAt = DateTime.Now;
            var addedRating = await _context.ServiceRatings.AddAsync(rating);

            if (await SaveChanges())
            {
                return addedRating.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateServiceRatings(IEnumerable<ServiceRating> controlRoomAlerts)
        {
            _context.ServiceRatings.UpdateRange(controlRoomAlerts);
            return await SaveChanges();
        }

        public async Task<ServiceRatingsDTO> FindServiceRatingById(long Id)
        {
            decimal averageRatings = 0;
            var ratings = _context.ServiceRatings.Where(x => x.ServiceId == Id && x.IsDeleted == false).Select(
                x => new ServiceRatingsDetailDTO
                {
                    DatePosted = x.CreatedAt,
                    Rating = x.Rating,
                    Review = x.Review,
                }).ToList();

            if (ratings.Any())
            {
                averageRatings = ratings.Select(x => x.Rating).Average();
            }

            var result = new ServiceRatingsDTO { AverageRating = averageRatings, Details = ratings };
            return result;
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

        public async Task<AppRating> SaveAppRating(AppRating rating)
        {
            rating.CreatedById = _context.UserProfiles.FirstOrDefault(x => x.FirstName.ToLower() == "seeder").Id;
            var addedRating = await _context.AppRatings.AddAsync(rating);
             
            if (await SaveChanges())
            {
                return addedRating.Entity;
            }
            return null;
        }

        public async Task<IEnumerable<AppRating>> FindAllAppRatings(int appId)
        {
            return await _context.AppRatings.Include(x => x.Application).Include(x => x.CustomerDivision)
              .Where(x => x.IsDeleted == false && x.ApplicationId == appId)
              .ToListAsync();
        }

        public async Task<IEnumerable<Application>> FindAllApplications()
        {
            return await _context.Applications.ToListAsync();
        }

        public Task<string> GetServiceReviews(int serviceId, int pageSIze = 10)
        {
           // var reviews = _context.ServiceRatings.Where(x => x.ServiceId == serviceId && !String.IsNullOrEmpty(x.Review)).Select(x => new )
           throw new NotImplementedException();
        }
    }
}