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
        Task<ApiCommonResponse> AddClientPolicy(HttpContext context, ClientPolicyReceivingDTO clientPolicyReceivingDTO);
        Task<ApiCommonResponse> GetAllClientPolicies();
        Task<ApiCommonResponse> GetClientPolicyById(long id);
        Task<ApiCommonResponse> FindClientPolicyByContractServiceId(long id);
        Task<ApiCommonResponse> FindClientPolicyByContractId(long id);
        //Task<ApiCommonResponse> GetClientPolicyByName(string name);
        Task<ApiCommonResponse> UpdateClientPolicy(HttpContext context, long id, ClientPolicyReceivingDTO clientPolicyReceivingDTO);
        Task<ApiCommonResponse> DeleteClientPolicy(long id);
    }
}
