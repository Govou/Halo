//using HalobizMigrations.Data;
//using HalobizMigrations.Models.OnlinePortal;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using OnlinePortalBackend.DTOs.TransferDTOs;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace OnlinePortalBackend.Repository.Impl
//{
//    public class CartContractDetailsRepositoryImpl : ICartContractDetailsRepository
//    {
//        private readonly HalobizContext _context;
//        private readonly ILogger<CartContractDetailsRepositoryImpl> _logger;
//        public CartContractDetailsRepositoryImpl(HalobizContext context, ILogger<CartContractDetailsRepositoryImpl> logger)
//        {
//            this._logger = logger;
//            this._context = context;
//        }
      

//        public async Task<CartContractService> FindContractServiceById(long Id)
//        {
//            return await _context.CartContractServices
//                .Include(x => x.CartContract)
//                .Include(x => x.Service)
//                .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
//        }

//        public async Task<IEnumerable<CartContractService>> FindAllContractServicesForAContract(long contractId)
//        {
//            return await _context.CartContractServices
//                .Include(x => x.CartContract)
//                .Include(x => x.Service)
//                .Where(x => x.ContractId == contractId && x.IsDeleted == false)
//                .ToListAsync();
//        }
//        public async Task<IEnumerable<CartContractService>> FindContractServicesByReferenceNumber(string refNo)
//        {
//            //todo
//            return await _context.CartContractServices
//                .Include(x => x.CartContract)
//                .Include(x => x.Service)
//                .Where(x => x.IsDeleted == false)
//                .ToListAsync();
//        }

//        public async Task<CartContractService> FindContractServiceByTag(string tag)
//        {
//            return await _context.CartContractServices
//                .Where(x => x.UniqueTag == tag)
//                .FirstOrDefaultAsync();
//        }

//        public async Task<IEnumerable<CartContractService>> FindContractServicesByGroupInvoiceNumber(string groupInvoiceNumber)
//        {
//            //to check
//            return await _context.CartContractServices
//                .Include(x => x.Service)
//                .Where(x => x.CartContract.GroupInvoiceNumber == groupInvoiceNumber && x.IsDeleted == false)
//                .ToListAsync();
//        }

//        public async Task<CartContractService> UpdateContractService(CartContractService entity)
//        {
//            var contractServiceEntity = _context.CartContractServices.Update(entity);
//            if (await SaveChanges())
//            {
//                return contractServiceEntity.Entity;
//            }
//            return null;
//        }

//        public async Task<bool> DeleteContractService(CartContractService entity)
//        {
//            entity.IsDeleted = true;
//            _context.CartContractServices.Update(entity);
//            return await SaveChanges();
//        }
//        private async Task<bool> SaveChanges()
//        {
//            try
//            {
//                return await _context.SaveChangesAsync() > 0;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.Message);
//                return false;
//            }
//        }

//        public async Task<List<CartContractService>> FindAllContractServices(long CustomerDivisionId)
//        {
//            return await _context.CartContractServices
//                .Include(t => t.Service).Include(x => x.CartContract)
//                 .Where(x => x.IsDeleted == false && x.CartContract.CustomerDivisionId == CustomerDivisionId && x.ContractEndDate > DateTime.Now)
//                 .ToListAsync();
//        }
//        public async Task<IEnumerable<CartContractService>> FindAllContractServices()
//        {
//            return await _context.CartContractServices
//                .Include(t => t.CartContract)
//                 .Where(x => x.IsDeleted == false && x.ContractEndDate > DateTime.Now)
//                 .ToListAsync();
//        }
//    }
//}
