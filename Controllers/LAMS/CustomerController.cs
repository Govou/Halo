using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
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
            var response = await _customerService.GetAllCustomers();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var customer = ((ApiOkResponse)response).Result;
            return Ok(customer);
        }

        [HttpGet("GroupType/{groupType}")]
        public async Task<ApiCommonResponse> GetCustomersByGroupTypeId(long groupType)
        {
            var response = await _customerService.GetCustomersByGroupType(groupType);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var customer = ((ApiOkResponse)response).Result;
            return Ok(customer);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _customerService.GetCustomerById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var customer = ((ApiOkResponse)response).Result;
            return Ok(customer);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewCustomer(CustomerReceivingDTO customerReceiving)
        {
            var response = await _customerService.AddCustomer(HttpContext, customerReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var customer = ((ApiOkResponse)response).Result;
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, CustomerReceivingDTO customerReceiving)
        {
            var response = await _customerService.UpdateCustomer(HttpContext, id, customerReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var customer = ((ApiOkResponse)response).Result;
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _customerService.DeleteCustomer(id);
            return StatusCode(response.StatusCode);
        }
    }
}