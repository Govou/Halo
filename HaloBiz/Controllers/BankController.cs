using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
    
namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BankController : Controller
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetBank()
        {
            return await _bankService.GetAllBank();
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _bankService.GetBankByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _bankService.GetBankById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewBank(BankReceivingDTO bankReceiving)
        {
            return await _bankService.AddBank(HttpContext, bankReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, BankReceivingDTO bankReceiving)
        {
            return await _bankService.UpdateBank(HttpContext, id, bankReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _bankService.DeleteBank(id);
        }
    }
}
