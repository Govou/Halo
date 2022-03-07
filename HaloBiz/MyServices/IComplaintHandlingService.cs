using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> GetComplaintHandlingStats(HttpContext context);
        Task<ApiCommonResponse> GetComplaintsHandling(HttpContext context);
        Task<ApiCommonResponse> PickComplaint(HttpContext context, PickComplaintDTO model);
        Task<ApiCommonResponse> MoveComplaintToNextStage(HttpContext context, MoveComplaintToNextStageDTO model);
        Task<ApiCommonResponse> TrackComplaint(ComplaintTrackingRecievingDTO model);
        Task<ApiCommonResponse> GetUserEscalationLevelDetails(HttpContext context);
        Task<ApiCommonResponse> ConfirmComplaintResolved(long complaintId);
        Task<ApiCommonResponse> RunComplaintConfirmationCronJob();
        Task<ApiCommonResponse> AssignComplaintToUser(HttpContext context, AssignComplaintReceivingDTO model);
        Task<ApiCommonResponse> MiniTrackComplaint(long ComplaintId);
        Task<ApiCommonResponse> GetHandlersRatings(HandlersRatingReceivingDTO model);
    }
}
