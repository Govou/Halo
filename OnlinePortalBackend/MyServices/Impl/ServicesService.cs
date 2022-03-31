using Halobiz.Common.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.Repository;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class ServicesService : IServicesService
    {
        private readonly IServicesRepo _servicesRepo;
        public ServicesService(IServicesRepo servicesRepo)
        {
            _servicesRepo = servicesRepo;
        }


        public async Task<ApiCommonResponse> GetServiceDetails(int contractServiceId)
        {
            var serviceDetail = await _servicesRepo.GetContractServciceById(contractServiceId);
            if (serviceDetail == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); 
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, serviceDetail);
        }
    }
}
