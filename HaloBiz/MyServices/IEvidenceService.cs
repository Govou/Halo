using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IEvidenceService
    {
        Task<ApiCommonResponse> AddEvidence(HttpContext context, EvidenceReceivingDTO evidenceReceivingDTO);
        Task<ApiCommonResponse> GetAllEvidence();
        Task<ApiCommonResponse> GetEvidenceById(long id);
        Task<ApiCommonResponse> GetEvidenceByName(string name);
        Task<ApiCommonResponse> UpdateEvidence(HttpContext context, long id, EvidenceReceivingDTO evidenceReceivingDTO);
        Task<ApiCommonResponse> DeleteEvidence(long id);
    }
}
