using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HalobizMigrations.Data;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using OnlinePortalBackend.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class CartContractServiceImpl : ICartContractService
    {
        private readonly ILogger<CartContractServiceImpl> _logger;
        private readonly ICartContractRepository _cartContractRepo;
        private readonly IMapper _mapper;
        private readonly HalobizContext _context;

        public CartContractServiceImpl(HalobizContext context, ILogger<CartContractServiceImpl> logger, 
            ICartContractRepository cartContractRepo, 
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _cartContractRepo = cartContractRepo;   
            _mapper = mapper;
        }

        public async Task<ApiCommonResponse> CreateCartContract(HttpContext context, CartContractDTO cartContractDTO)
        {
            CartContract savedCartContract = null;
            var createdById = context.GetLoggedInUserId();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var cartContract = _mapper.Map<CartContract>(cartContractDTO);
                    cartContract.CartContractServices = _mapper.Map<ICollection<CartContractService>>(cartContractDTO.CartContractServices);
                    var cartContractService = cartContract.CartContractServices;

                    cartContract.CartContractServices = null;
                    cartContract.CreatedById = createdById;
                    savedCartContract = await _cartContractRepo.SaveCartContract(cartContract);

                    if (savedCartContract == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }

                    foreach (var item in cartContractService)
                    {
                        item.ContractId = savedCartContract.Id;
                        item.CreatedById = createdById;
                    }

                    var savedSuccessfully = await _cartContractRepo.SaveCartContractServiceRange(cartContractService);
                    if (!savedSuccessfully)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    await transaction.RollbackAsync();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }

            var quoteFromDatabase = await _cartContractRepo.FindCartContractById(createdById, savedCartContract.Id);

            //var serializedQuote = JsonConvert.SerializeObject(quoteFromDatabase, new JsonSerializerSettings { 
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //});

            //Action action = async () => {
            //    await _mailAdapter.SendQuoteNotification(serializedQuote);
            //};
            //action.RunAsTask();
            var quoteTransferDTO = _mapper.Map<CartContractDTO>(quoteFromDatabase);
            return CommonResponse.Send(ResponseCodes.SUCCESS, quoteTransferDTO);
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
