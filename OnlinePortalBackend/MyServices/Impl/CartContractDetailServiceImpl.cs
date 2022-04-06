using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Repository;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class CartContractDetailServiceImpl : ICartContractDetailService
    {
        private readonly ILogger<CartContractServiceImpl> _logger;
        private readonly ICartContractDetailsRepository _cartContractRepo;
        private readonly IMapper _mapper;

    

        public async Task<ApiCommonResponse> GetAllContractsServcieForAContract(long contractId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiCommonResponse> GetCartContractServiceById(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}
