using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HalobizMigrations.Data;
using HalobizMigrations.Models.OnlinePortal;
using HalobizMigrations.Models.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class CartContractRepositoryImpl : ICartContractRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<CartContractRepositoryImpl> _logger;
        private readonly IMapper _mapper;
        public CartContractRepositoryImpl(HalobizContext context, 
            ILogger<CartContractRepositoryImpl> logger,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartContract> SaveCartContract(long userId, CartContractDTO cartContractDTO)
        {
            //var cartContract = new CartContract
            //{
            //    Id = entity.Id,
            //    CreatedAt = DateTime.Now,
            //    CreatedById = entity.CreatedById,
            //    GroupContractCategory = (GroupContractCategory)entity.GroupContractCategory,
            //    GroupInvoiceNumber = entity.GroupInvoiceNumber,
            //    CustomerDivisionId = entity.CustomerDivisionId
            //};



            //var cartContractEntity = await _context.CartContracts.AddAsync(cartContract);

            //if (await SaveChanges())
            //{
            //    return cartContractEntity.Entity;
            //}
            //return null;

            CartContract savedcartContract = null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                   
                    var cartContract = _mapper.Map<CartContract>(cartContractDTO);
                   // var cartContractService = cartContract.CartContractServices;

                 //   quote.QuoteServices = null;
                    cartContract.CreatedById = userId;
                     await _context.CartContracts.AddAsync(cartContract);
                     await _context.SaveChangesAsync();

                    savedcartContract = cartContract;

                    if (savedcartContract == null)
                    {
                        return savedcartContract;
                    }

                    //foreach (var item in quoteService)
                    //{
                    //    item.QuoteId = savedQuote.Id;
                    //    item.CreatedById = createdById;
                    //}

                  //  var savedSuccessfully = await _quoteServiceRepo.SaveQuoteServiceRange(quoteService);
                    //if (!savedSuccessfully)
                    //{
                    //    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    //}

                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    await transaction.RollbackAsync();
                    return savedcartContract;
                }
            }

         //   var cartContractDB = await _cartContractRepository.FindCartContractById(userId, savedcartContract.Id);
            
         //   var newCartContractDTO = _mapper.Map<CartContractDTO>(cartContractDB);
            return savedcartContract;

        }

        public async Task<CartContract> FindCartContractById(long userId, long Id)
        {
            try
            {
                return await _context.CartContracts
              //.Include(x => x.ContractServices)
              .FirstOrDefaultAsync(x => x.Id == Id && x.CreatedById == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
          
        }

        public async Task<IEnumerable<CartContract>> FindAllCartContract()
        {
            return await _context.CartContracts
                // .Include(x => x.ContractServices)
                .Include(x => x.CustomerDivision)
                .OrderByDescending(entity => entity.Id)
                .ToListAsync();
        }
        public async Task<CartContract> FindCartContractByReferenceNumber(string refNo)
        {
            
            return await _context.CartContracts
              //  .Include(x => x.ContractServices)
                .Include(x => x.CustomerDivision)
                .FirstOrDefaultAsync();
        }

        public async Task<CartContract> UpdateCartContract(CartContract entity)
        {
            var contractEntity = _context.CartContracts.Update(entity);
            if (await SaveChanges())
            {
                return contractEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteCartContract(CartContract entity)
        {
            _context.CartContracts.Update(entity);
            return await SaveChanges();
        }
        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
