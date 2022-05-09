using Halobiz.Common.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Repository;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class UtilityService : IUtilityService
    {
        private readonly IUtilityRepository _utilityRepository;

        public UtilityService(IUtilityRepository utilityRepository)
        {
            _utilityRepository = utilityRepository;
        }

        public async Task<ApiCommonResponse> GetLocalGovtAreas(int stateId)
        {
            var result =  _utilityRepository.GetLocalGovtAreas(stateId);

            if (result == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

        public async Task<ApiCommonResponse> GetStates()
        {
            var result = _utilityRepository.GetStates();

            if (result == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }
    }
}
