using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IComplaintOriginService
    {
        Task<ApiResponse> AddComplaintOrigin(HttpContext context, ComplaintOriginReceivingDTO complaintOriginReceivingDTO);
        Task<ApiResponse> GetAllComplaintOrigin();
        Task<ApiResponse> GetComplaintOriginById(long id);
        Task<ApiResponse> GetComplaintOriginByName(string name);
        Task<ApiResponse> UpdateComplaintOrigin(HttpContext context, long id, ComplaintOriginReceivingDTO complaintOriginReceivingDTO);
        Task<ApiResponse> DeleteComplaintOrigin(long id);
    }
}
