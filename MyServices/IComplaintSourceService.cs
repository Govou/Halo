using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IComplaintSourceService
    {
        Task<ApiResponse> AddComplaintSource(HttpContext context, ComplaintSourceReceivingDTO complaintSourceReceivingDTO);
        Task<ApiResponse> GetAllComplaintSource();
        Task<ApiResponse> GetComplaintSourceById(long id);
        Task<ApiResponse> GetComplaintSourceByName(string name);
        Task<ApiResponse> UpdateComplaintSource(HttpContext context, long id, ComplaintSourceReceivingDTO complaintSourceReceivingDTO);
        Task<ApiResponse> DeleteComplaintSource(long id);
    }
}
