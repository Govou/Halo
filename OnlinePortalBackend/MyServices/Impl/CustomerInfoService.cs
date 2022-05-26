using Halobiz.Common.DTOs.ApiDTOs;
using OnlinePortalBackend.Repository;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class CustomerInfoService : ICustomerInfoService
    {
        private readonly ICustomerInfoRepository _customerInfoRepo;
        public CustomerInfoService(ICustomerInfoRepository customerInfoRepo)
        {
            _customerInfoRepo = customerInfoRepo;
        }

        public async Task<ApiCommonResponse> FetchContractInfos(int customerId)
        {
            var result = await _customerInfoRepo.GetCotractInfos(customerId);

            if (result == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }
    }
}
