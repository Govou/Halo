using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddLeadDivisionContact(HttpContext context, long leadDivisionId, LeadDivisionContactReceivingDTO leadDivisionContactReceivingDTO);
        Task<ApiCommonResponse> GetAllLeadDivisionContact();
        Task<ApiCommonResponse> GetLeadDivisionContactById(long id);
        Task<ApiCommonResponse> UpdateLeadDivisionContact(HttpContext context, long id, LeadDivisionContactReceivingDTO leadDivisionContactReceivingDTO);

    }
}
