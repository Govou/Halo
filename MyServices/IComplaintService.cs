using HaloBiz.DTOs.ApiDTOs;
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
        Task<ApiResponse> AddComplaint(HttpContext context, ComplaintReceivingDTO complaintReceivingDTO);
        Task<ApiResponse> GetAllComplaint();
        Task<ApiResponse> GetComplaintById(long id);
        Task<ApiResponse> GetComplaintByName(string name);
        Task<ApiResponse> UpdateComplaint(HttpContext context, long id, ComplaintReceivingDTO complaintReceivingDTO);
        Task<ApiResponse> DeleteComplaint(long id);
    }
}
