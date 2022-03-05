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
    public class AppFeedbackRepositoryImpl : IAppFeedbackRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<AppFeedbackRepositoryImpl> _logger;
        public AppFeedbackRepositoryImpl(HalobizContext context, ILogger<AppFeedbackRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteAppFeedback(AppFeedback appFeedback)
        {
            appFeedback.IsDeleted = true;
            _context.AppFeedback.Update(appFeedback);
            return await SaveChanges();
        }

        public async Task<IEnumerable<AppFeedback>> FindAllAppFeedbacks()
        {
            return await _context.AppFeedback
               .Where(appFeedback => appFeedback.IsDeleted == false)
               .OrderBy(appFeedback => appFeedback.CreatedAt)
               .ToListAsync();
        }

        public async Task<AppFeedback> FindAppFeedbackById(long Id)
        {
            return await _context.AppFeedback
                .Where(appFeedback => appFeedback.IsDeleted == false)
                .FirstOrDefaultAsync(appFeedback => appFeedback.Id == Id && appFeedback.IsDeleted == false);

        }

        public async Task<AppFeedback> SaveAppFeedback(AppFeedback appFeedback)
        {
            var appFeedbackEntity = await _context.AppFeedback.AddAsync(appFeedback);
            if (await SaveChanges())
            {
                return appFeedbackEntity.Entity;
            }
            return null;
        }

        public async Task<AppFeedback> UpdateAppFeedback(AppFeedback appFeedback)
        {
            var appFeedbackEntity = _context.AppFeedback.Update(appFeedback);
            if (await SaveChanges())
            {
                return appFeedbackEntity.Entity;
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
