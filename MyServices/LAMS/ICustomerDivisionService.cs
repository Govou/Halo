using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.LAMS
{
    public interface ICustomerDivisionService
    {
        Task<ApiResponse> AddCustomerDivision(HttpContext context, CustomerDivisionReceivingDTO CustomerDivisionReceivingDTO);
        Task<ApiResponse> GetAllCustomerDivisions();
        Task<ApiResponse> GetCustomerDivisionById(long id);
        Task<ApiResponse> GetTaskAndFulfillmentsByCustomerDivisionId(long customerDivisionId);
        Task<ApiResponse> GetCustomerDivisionByName(string name);
        Task<ApiResponse> UpdateCustomerDivision(HttpContext context, long id, CustomerDivisionReceivingDTO CustomerDivisionReceivingDTO);
        Task<ApiResponse> DeleteCustomerDivision(long id);
        Task<ApiResponse> GetCustomerDivisionsByGroupType(long groupTypeId);
        Task<ApiResponse> GetCustomerDivisionBreakDownById(long id);
    }
}
