using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.MyServices;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    public class CustomerInformationController : Controller
    {
        private readonly ICustomerInfoService _customerInfoService;
        public CustomerInformationController(ICustomerInfoService customerInfoService)
        {
            _customerInfoService = customerInfoService;
        }

        [HttpGet("GetCustomerContractInfos")]
        public async Task<ApiCommonResponse> CreateContract(int customerId)
        {
            return await _customerInfoService.FetchContractInfos(customerId);
        }

     
    }
}
