using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.LAMS
{
    public interface ICustomerService
    {
        Task<ApiCommonResponse> AddCustomer(HttpContext context, CustomerReceivingDTO customerReceivingDTO);
        Task<ApiCommonResponse> GetAllCustomers();
        Task<ApiCommonResponse> GetCustomersByGroupType(long groupTypeId);
        Task<ApiCommonResponse> GetCustomerById(long id);
        Task<ApiCommonResponse> GetCustomerByName(string name);
        Task<ApiCommonResponse> UpdateCustomer(HttpContext context, long id, CustomerReceivingDTO customerReceivingDTO);
        Task<ApiCommonResponse> DeleteCustomer(long id);
    }
}
