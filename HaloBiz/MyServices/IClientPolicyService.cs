using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IClientPolicyService
    {
        Task<ApiCommonResponse> AddClientPolicy(HttpContext context, List<ClientPolicyReceivingDTO> clientPolicyReceivingDTO);
        Task<ApiCommonResponse> GetAllClientPolicies();
        Task<ApiCommonResponse> GetClientPolicyById(long id);
        Task<ApiCommonResponse> FindClientPolicyByContractServiceId(long id);
        Task<ApiCommonResponse> UpdateClientPolicy(HttpContext context, List<ClientPolicyReceivingDTO> clientPolicyReceivingDTO);
        Task<ApiCommonResponse> DeleteClientPolicy(long id);
    }
}
