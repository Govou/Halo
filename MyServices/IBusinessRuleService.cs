using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IBusinessRuleService
    {
        Task<ApiResponse> AddBusinessRule(HttpContext context, BusinessRuleReceivingDTO businessRuleReceivingDTO);
        Task<ApiResponse> GetAllBusinessRules();
        Task<ApiResponse> GetAllPairableBusinessRules();
        Task<ApiResponse> GetBusinessRuleById(long id);
        Task<ApiResponse> UpdateBusinessRule(HttpContext context, long id, BusinessRuleReceivingDTO businessRuleReceivingDTO);
        Task<ApiResponse> DeleteBusinessRule(long id);

        //BRPairable
        Task<ApiResponse> AddPairable(HttpContext context, BRPairableReceivingDTO bRPairableReceivingDTO);
        //Task<ApiResponse> AddPairables(HttpContext context, BRPairableReceivingDTO[] bRPairableReceivingDTO);
        Task<ApiResponse> GetAllPairables();
        Task<ApiResponse> GetAllActivePairables();
        Task<ApiResponse> GetPairableById(long id);
        Task<ApiResponse> UpdatePairable(HttpContext context, long id, BRPairableReceivingDTO bRPairableReceivingDTO);
        Task<ApiResponse> DeletePairable(long id);
    }
}
