using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IComplaintTypeService
    {
        Task<ApiResponse> AddComplaintType(HttpContext context, ComplaintTypeReceivingDTO complaintTypeReceivingDTO);
        Task<ApiResponse> GetAllComplaintType();
        Task<ApiResponse> GetComplaintTypeById(long id);
        Task<ApiResponse> GetComplaintTypeByName(string name);
        Task<ApiResponse> UpdateComplaintType(HttpContext context, long id, ComplaintTypeReceivingDTO complaintTypeReceivingDTO);
        Task<ApiResponse> DeleteComplaintType(long id);
    }
}
