using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddComplaintSource(HttpContext context, ComplaintSourceReceivingDTO complaintSourceReceivingDTO);
        Task<ApiCommonResponse> GetAllComplaintSource();
        Task<ApiCommonResponse> GetComplaintSourceById(long id);
        Task<ApiCommonResponse> GetComplaintSourceByName(string name);
        Task<ApiCommonResponse> UpdateComplaintSource(HttpContext context, long id, ComplaintSourceReceivingDTO complaintSourceReceivingDTO);
        Task<ApiCommonResponse> DeleteComplaintSource(long id);
    }
}
