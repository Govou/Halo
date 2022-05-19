using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models.OnlinePortal;
using OnlinePortalBackend.DTOs.TransferDTOs;

namespace OnlinePortalBackend.Repository
{
    public interface IServiceRatingRepository
    {
        Task<ServiceRating> SaveServiceRating(ServiceRating serviceRating);
        Task<AppRating> SaveAppRating(AppRating appRating);
        Task<bool> UpdateServiceRatings(IEnumerable<ServiceRating> serviceRating);
        Task<ServiceRatingsDTO> FindServiceRatingById(long Id);
        Task<IEnumerable<ServiceRating>> GetReviewHistoryByServiceId(long Id);
        Task<IEnumerable<ServiceRating>> FindServiceRatingsByUserId(long Id);
        Task<IEnumerable<ServiceRating>> FindAllServiceRatings();
        Task<IEnumerable<AppRating>> FindAllAppRatings(int Id);
        Task<IEnumerable<Application>> FindAllApplications();
        Task<ServiceRating> UpdateServiceRating(ServiceRating serviceRating);
        Task<bool> RemoveServiceRating(ServiceRating serviceRating);
        Task<IEnumerable<ServiceReviewDTO>> GetServiceReviews(int serviceId, int pageSIze = 10);
    }
}