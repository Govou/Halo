using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using HalobizMigrations.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.MyServices.SecureMobilitySales;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers.SecureMobilitySales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSContractController : ControllerBase
    {
        private readonly ISMSContractsService _contractsService;
        public SMSContractController(ISMSContractsService contractsService)
        {
            _contractsService = contractsService;
        }

        [HttpPost("CreateNewContract")]
        public async Task<ApiCommonResponse> CreateContract(SMSContractDTO request)
        {
            return await _contractsService.CreateContract(request);
        }
    }
}
