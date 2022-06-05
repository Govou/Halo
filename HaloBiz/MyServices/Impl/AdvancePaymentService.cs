using Halobiz.Common.DTOs.ApiDTOs;
using HalobizMigrations.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class AdvancePaymentService
    {
        private readonly HalobizContext _context;
        private readonly ILogger<AdvancePaymentService> _logger;
        public AdvancePaymentService(HalobizContext context, ILogger<AdvancePaymentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiCommonResponse> AddPayment(HttpContext context, AdvancePayments bankReceivingDTO)
        {
            bank.CreatedById = context.GetLoggedInUserId();
            var savedBank = await _context.Advan.SaveBank(bank);
            if (savedBank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var bankTransferDTO = _mapper.Map<BankTransferDTO>(bank);
            return CommonResponse.Send(ResponseCodes.SUCCESS, bankTransferDTO);
        }
    }
}
