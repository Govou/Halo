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
    public class ActivityRepositoryImpl : IActivityRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ActivityRepositoryImpl> _logger;
        public ActivityRepositoryImpl(HalobizContext context, ILogger<ActivityRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteActivity(Activity activity)
        {
            activity.IsDeleted = true;
            _context.Activities.Update(activity);
            return await SaveChanges();
        }

        public async Task<IEnumerable<Activity>> FindAllActivitys()
        {
            return await _context.Activities
               .Where(activity => activity.IsDeleted == false)
               .OrderBy(activity => activity.CreatedAt)
               .ToListAsync();
        }

        public async Task<Activity> FindActivityById(long Id)
        {
            return await _context.Activities
                .Where(activity => activity.IsDeleted == false)
                .FirstOrDefaultAsync(activity => activity.Id == Id && activity.IsDeleted == false);

        }

        public async Task<Activity> FindActivityByName(string name)
        {
            return await _context.Activities
                 .Where(activity => activity.IsDeleted == false)
                 .FirstOrDefaultAsync(activity => activity.Subject == name && activity.IsDeleted == false);

        }

        public async Task<Activity> SaveActivity(Activity activity)
        {
            var activityEntity = await _context.Activities.AddAsync(activity);
            if (await SaveChanges())
            {
                return activityEntity.Entity;
            }
            return null;
        }

        public async Task<Activity> UpdateActivity(Activity activity)
        {
            var activityEntity = _context.Activities.Update(activity);
            if (await SaveChanges())
            {
                return activityEntity.Entity;
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
