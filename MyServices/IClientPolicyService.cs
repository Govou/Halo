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
        Task<ApiResponse> AddClientPolicy(HttpContext context, ClientPolicyReceivingDTO clientPolicyReceivingDTO);
        Task<ApiResponse> GetAllClientPolicies();
        Task<ApiResponse> GetClientPolicyById(long id);
        //Task<ApiResponse> GetClientPolicyByName(string name);
        Task<ApiResponse> UpdateClientPolicy(HttpContext context, long id, ClientPolicyReceivingDTO clientPolicyReceivingDTO);
        Task<ApiResponse> DeleteClientPolicy(long id);
    }
}
