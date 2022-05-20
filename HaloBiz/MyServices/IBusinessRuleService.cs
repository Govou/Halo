using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddBusinessRule(HttpContext context, BusinessRuleReceivingDTO businessRuleReceivingDTO);
        Task<ApiCommonResponse> GetAllBusinessRules();
        Task<ApiCommonResponse> GetAllPairableBusinessRules();
        Task<ApiCommonResponse> GetBusinessRuleById(long id);
     
        Task<ApiCommonResponse> UpdateBusinessRule(HttpContext context, long id, BusinessRuleReceivingDTO businessRuleReceivingDTO);
        Task<ApiCommonResponse> DeleteBusinessRule(long id);

        //BRPairable
        Task<ApiCommonResponse> AddPairable(HttpContext context, BRPairableReceivingDTO bRPairableReceivingDTO);
        //Task<ApiCommonResponse> AddPairables(HttpContext context, BRPairableReceivingDTO[] bRPairableReceivingDTO);
        Task<ApiCommonResponse> GetAllPairables();
        Task<ApiCommonResponse> GetAllActivePairables();
        Task<ApiCommonResponse> GetPairableById(long id);
        Task<ApiCommonResponse> CheckIfServiceCanBePairedById(BRPairableCheckReceivingDTO businessRuleCheck);
        Task<ApiCommonResponse> UpdatePairable(HttpContext context, long id, BRPairableReceivingDTO bRPairableReceivingDTO);
        Task<ApiCommonResponse> DeletePairable(long id);
    }
}
