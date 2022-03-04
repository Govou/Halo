using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IAppReviewRepository
    {
        Task<AppReview> SaveAppReview(AppReview appReview);
        Task<AppReview> FindAppReviewById(long Id);
        Task<IEnumerable<AppReview>> FindAllAppReviews();
        Task<AppReview> UpdateAppReview(AppReview appReview);
        Task<bool> DeleteAppReview(AppReview appReview);
    }
}
