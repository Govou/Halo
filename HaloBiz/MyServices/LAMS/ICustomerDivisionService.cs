using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.MyServices.LAMS
{
    public interface ICustomerDivisionService
    {
        Task<ApiCommonResponse> AddCustomerDivision(HttpContext context, CustomerDivisionReceivingDTO CustomerDivisionReceivingDTO);
        Task<ApiCommonResponse> GetAllCustomerDivisions();
        Task<ApiCommonResponse> GetCustomerDivisionById(long id);
        Task<ApiCommonResponse> GetCustomerDivisionByDTrackCustomerNumber(string dTrackCustomerNumber);
        Task<ApiCommonResponse> GetTaskAndFulfillmentsByCustomerDivisionId(long customerDivisionId);
        Task<ApiCommonResponse> GetClientsWithSecuredMobilityContractServices();
        Task<ApiCommonResponse> GetCustomerDivisionByName(string name);
        Task<ApiCommonResponse> UpdateCustomerDivision(HttpContext context, long id, CustomerDivisionReceivingDTO CustomerDivisionReceivingDTO);
        Task<ApiCommonResponse> DeleteCustomerDivision(long id);
        Task<ApiCommonResponse> GetCustomerDivisionsByGroupType(long groupTypeId);
        Task<ApiCommonResponse> GetCustomerDivisionBreakDownById(long id);
        Task<ApiCommonResponse> GetClientsUnAssignedToRMSbu();
        Task<ApiCommonResponse> GetClientsAttachedToRMSbu(long sbuId);
        Task<ApiCommonResponse> GetRMSbuClientsByGroupType(long sbuId, long groupTypeId);
        Task<ApiCommonResponse> AttachClientToRMSbu(HttpContext context, long id, long sbuId);
    }
}
