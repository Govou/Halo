using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTO;
using Halobiz.Common.DTOs.ReceivingDTOs;
using HalobizMigrations.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.MyServices
{
    public class ClientAccountCreation
    {
        private IUserProfileService _userProfileService;
        private readonly HalobizContext _context;
        private readonly ILogger<ClientAccountCreation> _logger;
        public ClientAccountCreation(IUserProfileService userProfileService,
            ILogger<ClientAccountCreation> logger)
        {
            _userProfileService = userProfileService;
            _logger = logger;
        }

        public async Task<ApiCommonResponse> CreateAccount(LoginDTO user)
        {
            try
            {
                var response = await _userProfileService.FindUserByEmail(user.Email);

                if (!response.responseCode.Contains("00"))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "You already have a profile");
                }

                //check if this customer division has an email
                if (!_context.CustomerDivisions.Any(x=>x.Email==user.Email))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"This email {user.Email} does not exist for a customer");
                }

                //hash password for this guy
                await _userProfileService.AddUserProfile(new UserProfileReceivingDTO
                {

                });

                //send code to the client
                return CommonResponse.Send(ResponseCodes.SUCCESS, null, $"A confirmation code has been send to {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "System error");
            }
        }
    }
}
