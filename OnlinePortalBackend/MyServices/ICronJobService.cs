using OnlinePortalBackend.DTOs.ApiDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface ICronJobService
    {
        public Task<ApiResponse> RetryPaymentProcessing();
    }
}
