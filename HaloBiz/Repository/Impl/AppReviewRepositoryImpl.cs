using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class AppReviewRepositoryImpl : IAppReviewRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<AppReviewRepositoryImpl> _logger;
        public AppReviewRepositoryImpl(HalobizContext context, ILogger<AppReviewRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteAppReview(AppReview appReview)
        {
            appReview.IsDeleted = true;
            _context.AppReviews.Update(appReview);
            return await SaveChanges();
        }

        public async Task<IEnumerable<AppReview>> FindAllAppReviews()
        {
            return await _context.AppReviews
               .Where(appReview => appReview.IsDeleted == false)
               .OrderBy(appReview => appReview.CreatedAt)
               .ToListAsync();
        }

        public async Task<AppReview> FindAppReviewById(long Id)
        {
            return await _context.AppReviews
                .Where(appReview => appReview.IsDeleted == false)
                .FirstOrDefaultAsync(appReview => appReview.Id == Id && appReview.IsDeleted == false);

        }

        public async Task<AppReview> SaveAppReview(AppReview appReview)
        {
            var appReviewEntity = await _context.AppReviews.AddAsync(appReview);
            if (await SaveChanges())
            {
                return appReviewEntity.Entity;
            }
            return null;
        }

        public async Task<AppReview> UpdateAppReview(AppReview appReview)
        {
            var appReviewEntity = _context.AppReviews.Update(appReview);
            if (await SaveChanges())
            {
                return appReviewEntity.Entity;
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
