using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IAppFeedbackRepository
    {
        Task<AppFeedback> SaveAppFeedback(AppFeedback appFeedback);
        Task<AppFeedback> FindAppFeedbackById(long Id);
        Task<IEnumerable<AppFeedback>> FindAllAppFeedbacks();
        Task<AppFeedback> UpdateAppFeedback(AppFeedback appFeedback);
        Task<bool> DeleteAppFeedback(AppFeedback appFeedback);
    }
}
