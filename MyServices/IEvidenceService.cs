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
        Task<ApiResponse> AddEvidence(HttpContext context, EvidenceReceivingDTO evidenceReceivingDTO);
        Task<ApiResponse> GetAllEvidence();
        Task<ApiResponse> GetEvidenceById(long id);
        Task<ApiResponse> GetEvidenceByName(string name);
        Task<ApiResponse> UpdateEvidence(HttpContext context, long id, EvidenceReceivingDTO evidenceReceivingDTO);
        Task<ApiResponse> DeleteEvidence(long id);
    }
}
