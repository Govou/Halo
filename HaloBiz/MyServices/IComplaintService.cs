using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IComplaintService
    {
        Task<ApiCommonResponse> AddComplaint(HttpContext context, ComplaintReceivingDTO complaintReceivingDTO, string applicationBaseUrl);
        Task<ApiCommonResponse> GetAllComplaint();
        Task<ApiCommonResponse> GetComplaintsStats(HttpContext context);
        Task<ApiCommonResponse> GetComplaintById(long id);
        //Task<ApiCommonResponse> GetComplaintByName(string name);
        Task<ApiCommonResponse> UpdateComplaint(HttpContext context, long id, ComplaintReceivingDTO complaintReceivingDTO);
        Task<ApiCommonResponse> DeleteComplaint(long id);
    }
}
