using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadDivisionContactService
    {
        Task<ApiResponse> AddLeadDivisionContact(HttpContext context, long leadDivisionId, LeadDivisionContactReceivingDTO leadDivisionContactReceivingDTO);
        Task<ApiResponse> GetAllLeadDivisionContact();
        Task<ApiResponse> GetLeadDivisionContactById(long id);
        Task<ApiResponse> UpdateLeadDivisionContact(HttpContext context, long id, LeadDivisionContactReceivingDTO leadDivisionContactReceivingDTO);

    }
}
