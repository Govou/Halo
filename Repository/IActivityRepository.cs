using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IActivityRepository
    {
        Task<Activity> SaveActivity(Activity activity);
        Task<Activity> FindActivityById(long Id);
        Task<Activity> FindActivityByName(string name);
        Task<IEnumerable<Activity>> FindAllActivitys();
        Task<Activity> UpdateActivity(Activity activity);
        Task<bool> DeleteActivity(Activity activity);
    }
}
