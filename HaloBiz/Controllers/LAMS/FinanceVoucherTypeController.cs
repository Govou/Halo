using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Finance,18)]

    public class FinanceVoucherTypeController : ControllerBase
    {
        private readonly IFinancialVoucherTypeService _finacialVoucherTypeService;

        public FinanceVoucherTypeController(IFinancialVoucherTypeService finacialVoucherTypeService)
        {
            this._finacialVoucherTypeService = finacialVoucherTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetFinancialVoucherTypes()
        {
            return await _finacialVoucherTypeService.GetAllFinancialVoucherTypes();
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _finacialVoucherTypeService.GetFinancialVoucherTypeById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewFinancialVoucherType(FinancialVoucherTypeReceivingDTO voucherTypeReceiving)
        {
            return await _finacialVoucherTypeService.AddFinancialVoucherType(HttpContext, voucherTypeReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, FinancialVoucherTypeReceivingDTO voucherTypeReceiving)
        {
            return await _finacialVoucherTypeService.UpdateFinancialVoucherType(HttpContext, id, voucherTypeReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _finacialVoucherTypeService.DeleteFinancialVoucherType(id);
        }
    }
}