using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using OnlinePortalBackend.Repository;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public class SMSAccountService : ISMSAccountService
    {
        private readonly ISMSAccountRepository _accountRepository;
        private readonly IOnlineAccounts _authService;
        public SMSAccountService(ISMSAccountRepository accountRepository, IOnlineAccounts authService)
        {
            _accountRepository = accountRepository;
            _authService = authService;
        }
        public async Task<ApiCommonResponse> CreateBusinessAccount(SMSBusinessAccountDTO request)
        {
            var result = await _accountRepository.CreateBusinessAccount(request);

            if (result)
            {
                var authResult = await _authService.SendConfirmCodeToClient(request.AccountLogin.Email);
                return authResult;
            }
            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Registration failed. Please try again");
        }

        public async Task<ApiCommonResponse> CreateIndividualAccount(SMSIndividualAccountDTO request)
        {
            var result = await _accountRepository.CreateIndividualAccount(request);

            if (result)
            {
                var authResult = await _authService.SendConfirmCodeToClient(request.AccountLogin.Email);
                return authResult;
            }
            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Registration failed. Please try again");
        }
    }
}
