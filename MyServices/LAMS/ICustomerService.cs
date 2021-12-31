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
    public interface ICustomerService
    {
        Task<ApiResponse> AddCustomer(HttpContext context, CustomerReceivingDTO customerReceivingDTO);
        Task<ApiResponse> GetAllCustomers();
        Task<ApiResponse> GetCustomersByGroupType(long groupTypeId);
        Task<ApiResponse> GetCustomerById(long id);
        Task<ApiResponse> GetCustomerByName(string name);
        Task<ApiResponse> UpdateCustomer(HttpContext context, long id, CustomerReceivingDTO customerReceivingDTO);
        Task<ApiResponse> DeleteCustomer(long id);
    }
}
