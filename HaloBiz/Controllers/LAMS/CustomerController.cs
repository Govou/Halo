using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            this._customerService = customerService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetCustomers()
        {
            return await _customerService.GetAllCustomers();
        }

        [HttpGet("GroupType/{groupType}")]
        public async Task<ApiCommonResponse> GetCustomersByGroupTypeId(long groupType)
        {
            return await _customerService.GetCustomersByGroupType(groupType);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _customerService.GetCustomerById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewCustomer(CustomerReceivingDTO customerReceiving)
        {
            return await _customerService.AddCustomer(HttpContext, customerReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, CustomerReceivingDTO customerReceiving)
        {
            return await _customerService.UpdateCustomer(HttpContext, id, customerReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _customerService.DeleteCustomer(id);
        }
    }
}