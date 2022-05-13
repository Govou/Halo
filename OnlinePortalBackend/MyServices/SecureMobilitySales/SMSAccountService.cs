using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Repository;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public class SMSAccountService : ISMSAccountService
    {
        private readonly ISMSAccountRepository _accountRepository;
        private readonly IOnlineAccounts _authService;
        private readonly IMapper _mapper;
        public SMSAccountService(ISMSAccountRepository accountRepository, IOnlineAccounts authService, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _authService = authService;
        }
        public async Task<ApiCommonResponse> CreateBusinessAccount(SMSBusinessAccountDTO request)
        {
            var result = await _accountRepository.CreateBusinessAccount(request);

            if (result.success)
            {
                var authResult = await _authService.SendConfirmCodeToClient_v2(request.AccountLogin.Email);
                return authResult;
            }
            return CommonResponse.Send(ResponseCodes.FAILURE, null, result.message);
        }

        public async Task<ApiCommonResponse> CreateIndividualAccount(SMSIndividualAccountDTO request)
        {

            var result = await _accountRepository.CreateIndividualAccount(request);

            if (result.success)
            {
                var authResult = await _authService.SendConfirmCodeToClient_v2(request.AccountLogin.Email);
                return authResult;
            }
            return CommonResponse.Send(ResponseCodes.FAILURE, null, result.message);
        }

        public async Task<ApiCommonResponse> GetCustomerProfile(int profileId)
        {
            var result = await _accountRepository.GetCustomerProfile(profileId);

            if (result == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Profile does not exist");
            }
            var profile = new OnlineProfileDTO
            {
                CreatedAt = result.CreatedAt,
                Email = result.Email,
                Name = result.Name,
                Id = result.Id
            };

            return CommonResponse.Send(ResponseCodes.SUCCESS, profile, "Success");
        }
    }
}
