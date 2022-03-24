using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models.OnlinePortal;

namespace OnlinePortalBackend.Repository
{
    public interface IServiceRatingRepository
    {
        Task<ServiceRating> SaveServiceRating(ServiceRating serviceRating);
        Task<bool> UpdateServiceRatings(IEnumerable<ServiceRating> serviceRating);
        Task<ServiceRating> FindServiceRatingById(long Id);
        Task<IEnumerable<ServiceRating>> GetReviewHistoryByServiceId(long Id);
        Task<IEnumerable<ServiceRating>> FindServiceRatingsByUserId(long Id);
        Task<IEnumerable<ServiceRating>> FindAllServiceRatings();
        Task<ServiceRating> UpdateServiceRating(ServiceRating serviceRating);
        Task<bool> RemoveServiceRating(ServiceRating serviceRating);
    }
}