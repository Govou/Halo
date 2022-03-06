using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IProspectService
    {
        Task<ApiCommonResponse> AddProspect(HttpContext context, ProspectReceivingDTO prospectReceivingDTO);
        Task<ApiCommonResponse> GetAllProspect();
        Task<ApiCommonResponse> GetProspectById(long id);
        Task<ApiCommonResponse> GetProspectByEmail(string email);
        Task<ApiCommonResponse> UpdateProspect(HttpContext context, long id, ProspectReceivingDTO prospectReceivingDTO);
        Task<ApiCommonResponse> DeleteProspect(long id);
    }
}
