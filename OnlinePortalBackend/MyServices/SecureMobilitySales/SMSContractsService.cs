using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using HalobizMigrations.Models;
using OnlinePortalBackend.Repository;
using System;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public class SMSContractsService : ISMSContractsService
    {
        private readonly ISMSContractsRepository _contractsRepo;
        public SMSContractsService(ISMSContractsRepository contractsRepo)
        {
            _contractsRepo = contractsRepo;
        }
        public async Task<ApiCommonResponse> CreateContract(SMSContractDTO contractDTO)
        {
           var result = await _contractsRepo.AddNewContract(contractDTO);

            if (!result.isSuccess)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

    }
}
