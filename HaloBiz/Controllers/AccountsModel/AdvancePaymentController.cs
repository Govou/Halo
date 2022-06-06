using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.MyServices.Impl;
using HalobizMigrations.Models.Halobiz;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HaloBiz.Controllers.AccountsModel
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Finance, 127)]
    public class AdvancePaymentController : Controller
    {
        private readonly IAdvancePaymentService _advancePaymentService;
        public AdvancePaymentController(IAdvancePaymentService advancePaymentService)
        {
            _advancePaymentService = advancePaymentService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetAllAdvancePayments()
        {
            return await _advancePaymentService.GetAllPayments();
        }


        [HttpGet("GetByCustomerDivisionId/{id}")]
        public async Task<ApiCommonResponse> GetByCustomerDivisionId(long id)
        {
            return await _advancePaymentService.GetCustomerPayment(id);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _advancePaymentService.GetById(id);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> Update(long id, AdvancePayment payment)
        {
            return await _advancePaymentService.UpdatePayment(HttpContext, id, payment);
        }

        [HttpPost]
        public async Task<ApiCommonResponse> Post(AdvancePayment payment)
        {
            return await _advancePaymentService.AddPayment(HttpContext, payment);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> Delete(long id)
        {
            return await _advancePaymentService.DeletePayment(HttpContext, id);
        }
    }
}
