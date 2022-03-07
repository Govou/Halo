using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddComplaintType(HttpContext context, ComplaintTypeReceivingDTO complaintTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllComplaintType();
        Task<ApiCommonResponse> GetComplaintTypeById(long id);
        Task<ApiCommonResponse> GetComplaintTypeByName(string name);
        Task<ApiCommonResponse> UpdateComplaintType(HttpContext context, long id, ComplaintTypeReceivingDTO complaintTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteComplaintType(long id);
    }
}
