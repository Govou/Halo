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

        public async Task<CartContract> SaveCartContract(CartContract cartContract)
        {
            var cartContractEntity = await _context.CartContracts.AddAsync(cartContract);
            if (await SaveChanges())
            {
                return cartContractEntity.Entity;
            }
            return null;
        }

        public async Task<bool> SaveCartContractServiceRange(IEnumerable<CartContractService> cartContractServices)
        {
            await _context.CartContractServices.AddRangeAsync(cartContractServices);
            return await SaveChanges();
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
