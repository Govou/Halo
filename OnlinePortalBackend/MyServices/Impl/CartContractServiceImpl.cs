using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using OnlinePortalBackend.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class CartContractServiceImpl : ICartContractService
    {
        private readonly ILogger<CartContractServiceImpl> _logger;
        private readonly ICartContractRepository _cartContractRepo;
        private readonly IMapper _mapper;

        public CartContractServiceImpl(ILogger<CartContractServiceImpl> logger, 
            ICartContractRepository cartContractRepo, 
            IMapper mapper)
        {
            _logger = logger;
            _cartContractRepo = cartContractRepo;   
            _mapper = mapper;
        }

        public async Task<ApiCommonResponse> CreateCartContract(HttpContext context, CartContractDTO cartContract)
        {
            var userId = context.GetLoggedInUserId();
            var result = await _cartContractRepo.SaveCartContract(userId, cartContract);
            if (result == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            var cartContractDTO = _mapper.Map<CartContractDTO>(cartContract);
            return CommonResponse.Send(ResponseCodes.SUCCESS, cartContractDTO);
        }

        public async Task<ApiCommonResponse> GetCartContractServiceById(HttpContext context, long id)
        {
            var userId = context.GetLoggedInUserId();
            var cartContract = await _cartContractRepo.FindCartContractById(userId, id);
            if (cartContract == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var cartContractDTOs = _mapper.Map<CartContractDTO>(cartContract);
            return CommonResponse.Send(ResponseCodes.SUCCESS, cartContractDTOs);
        }

       
        //public Task<ApiCommonResponse> GetAllCartContractsServcie()
        //{
        //    throw new System.NotImplementedException();
        //}

        //public Task<ApiCommonResponse> GetCartContractServiceById(long id)
        //{
        //    throw new System.NotImplementedException();
        //}
    }


}
