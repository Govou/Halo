using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IComplaintHandlingService
    {
        Task<ApiResponse> GetComplaintHandlingStats(HttpContext context);
        Task<ApiResponse> GetComplaintsHandling(HttpContext context);
        Task<ApiResponse> PickComplaint(HttpContext context, PickComplaintDTO model);
        Task<ApiResponse> MoveComplaintToNextStage(HttpContext context, MoveComplaintToNextStageDTO model);
        Task<ApiResponse> TrackComplaint(ComplaintTrackingRecievingDTO model);
        Task<ApiResponse> GetUserEscalationLevelDetails(HttpContext context);
        Task<ApiResponse> ConfirmComplaintResolved(long complaintId);
        Task<ApiResponse> RunComplaintConfirmationCronJob();
    }
}
