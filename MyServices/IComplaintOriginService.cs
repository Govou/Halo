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
        Task<ApiCommonResponse> AddComplaintOrigin(HttpContext context, ComplaintOriginReceivingDTO complaintOriginReceivingDTO);
        Task<ApiCommonResponse> GetAllComplaintOrigin();
        Task<ApiCommonResponse> GetComplaintOriginById(long id);
        Task<ApiCommonResponse> GetComplaintOriginByName(string name);
        Task<ApiCommonResponse> UpdateComplaintOrigin(HttpContext context, long id, ComplaintOriginReceivingDTO complaintOriginReceivingDTO);
        Task<ApiCommonResponse> DeleteComplaintOrigin(long id);
    }
}
