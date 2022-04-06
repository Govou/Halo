using AutoMapper;
using HalobizMigrations.Data;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using halobiz_backend.DTOs.QueryParamsDTOs;
using halobiz_backend.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.MyServices.LAMS;

namespace HaloBiz.MyServices.Impl
{
    public class AccountMasterServiceImpl : IAccountMasterService
    {
        private readonly ILogger<AccountMasterServiceImpl> _logger;
        private readonly IAccountMasterRepository _accountMasterRepo;
        private readonly IMapper _mapper;
        private readonly HalobizContext _context;
        private readonly IConfiguration _configuration;
        private long LoggedInUserId;
        private string SALES_INVOICE_VOUCHER;
        private readonly ILeadConversionService _leadConversionService;


        public AccountMasterServiceImpl(
                    IConfiguration configuration,
                    IAccountMasterRepository accountMasterRepo,
                    ILogger<AccountMasterServiceImpl> logger, 
                    IMapper mapper,
                     ILeadConversionService leadConversionService,
                    HalobizContext context)
        {
            _configuration = configuration;
            _mapper = mapper;
            _accountMasterRepo = accountMasterRepo;
            _logger = logger;
            _context = context;
            SALES_INVOICE_VOUCHER = _configuration.GetSection("VoucherTypes:SalesInvoiceVoucher").Value;
            _leadConversionService = leadConversionService;
        }

        public async Task<ApiCommonResponse> AddAccountMaster(HttpContext context, AccountMasterReceivingDTO accountMasterReceivingDTO)
        {
            var acctClass = _mapper.Map<AccountMaster>(accountMasterReceivingDTO);
            acctClass.CreatedById = context.GetLoggedInUserId();
            var savedAccountMaster = await _accountMasterRepo.SaveAccountMaster(acctClass);
            if (savedAccountMaster == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var AccountMasterTransferDTOs = _mapper.Map<AccountMasterTransferDTO>(acctClass);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountMasterTransferDTOs);
        }

        public async Task<ApiCommonResponse> DeleteAccountMaster(long id)
        {
            var AccountMasterToDelete = await _accountMasterRepo.FindAccountMasterById(id);
            if (AccountMasterToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _accountMasterRepo.DeleteAccountMaster(AccountMasterToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAccountMasterById(long id)
        {
            var AccountMaster = await _accountMasterRepo.FindAccountMasterById(id);
            if (AccountMaster == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var AccountMasterTransferDTOs = _mapper.Map<AccountMasterTransferDTO>(AccountMaster);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountMasterTransferDTOs);
        }

        public async Task<ApiCommonResponse> QueryAccountMasters(AccountMasterTransactionDateQueryParams query)
        {
            if(query.VoucherTypeIds != null && query.VoucherTypeIds.Count > 0  
                    && query.StartDate != null && query.EndDate != null){
                return await GetAllAccountMastersByVoucherId(query);
            }
            else if(query.StartDate != null && query.EndDate != null && query.TransactionId == null)
            {
                return await GetAllAccountMastersByTransactionDate(query);
            }
            else if(query.ClientId != null && query.ClientId > 0 & query.Years != null)
            {
                return await GetAllAccountMastersByCustomerIdAndContractYear(query);
            }
            else if(query.TransactionId != null)
            {
                return await GetAllAccountMastersByTransactionId(query.TransactionId );
            }else{
                return await GetAllAccountMasters();
            }
        }
        public async Task<ApiCommonResponse> GetAllAccountMasters()
        {
            var AccountMaster = await _accountMasterRepo.FindAllAccountMasters();
            if (AccountMaster == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var AccountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(AccountMaster);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountMasterTransferDTOs);
        }
        public async Task<ApiCommonResponse> GetAllAccountMastersByTransactionDate(AccountMasterTransactionDateQueryParams query)
        {
            try{
                //var queryable =  _accountMasterRepo.GetAccountMastersQueryable();
                var accountMasters = await _context.AccountMasters
               .Include(x => x.AccountDetails)
                   .ThenInclude(x => x.Account)
               .Include(x => x.Voucher)
               .Where(x=> x.CreatedAt >= query.StartDate
                            && x.CreatedAt <= query.EndDate && x.IsDeleted == false).ToListAsync();

                //var accountMasters = await queryable.Where(x => x.CreatedAt >= query.StartDate
                //            && x.CreatedAt <= query.EndDate && x.IsDeleted == false).ToListAsync();
                var AccountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
                return CommonResponse.Send(ResponseCodes.SUCCESS,AccountMasterTransferDTOs);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }
        public async Task<ApiCommonResponse> GetAllAccountMastersByVoucherId(AccountMasterTransactionDateQueryParams query)
        {
            try{
                //var queryable =  _accountMasterRepo.GetAccountMastersQueryable();

                var accountMasters = await _context.AccountMasters
                    .Where(x => !x.IsDeleted && x.CreatedAt >= query.StartDate
                            && x.CreatedAt <= query.EndDate && !x.IsDeleted && query.VoucherTypeIds.Contains(x.VoucherId))
                    .Include(x => x.Voucher)
                    .Include(x => x.AccountDetails)
                        .ThenInclude(x => x.Account)
                    .ToListAsync();

                //var accountMasters = await queryable.Where(x => x.CreatedAt >= query.StartDate
                //            && x.CreatedAt <= query.EndDate && !x.IsDeleted && query.VoucherTypeIds.Contains(x.VoucherId)).ToListAsync();
               // var AccountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
                return CommonResponse.Send(ResponseCodes.SUCCESS, accountMasters);

            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }
        public async Task<ApiCommonResponse> GetAllAccountMastersByTransactionId(string transactionId)
        {
            var accountMasters = await _accountMasterRepo.FindAccountMastersByTransactionId(transactionId);
            if (accountMasters == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var accountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
            return CommonResponse.Send(ResponseCodes.SUCCESS,accountMasterTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetAllAccountMastersByCustomerIdAndContractYear(AccountMasterTransactionDateQueryParams query)
        {
            try{
                var accountMasters = await _accountMasterRepo.FindAllAccountMastersByCustomerId(query);
                var accountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
                return CommonResponse.Send(ResponseCodes.SUCCESS,accountMasterTransferDTOs);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            
        }

        public async Task<ApiCommonResponse> UpdateAccountMaster(long id, AccountMasterReceivingDTO accountMasterReceivingDTO)
        {
            var AccountMasterToUpdate = await _accountMasterRepo.FindAccountMasterById(id);
            if (AccountMasterToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            AccountMasterToUpdate.Description = accountMasterReceivingDTO.Description;
            AccountMasterToUpdate.OfficeId = accountMasterReceivingDTO.OfficeId;
            AccountMasterToUpdate.BranchId = accountMasterReceivingDTO.BranchId;
            AccountMasterToUpdate.CustomerDivisionId = accountMasterReceivingDTO.CustomerDivisionId;
            AccountMasterToUpdate.DtrackJournalCode = accountMasterReceivingDTO.DTrackJournalCode;
            var updatedAccountMaster = await _accountMasterRepo.UpdateAccountMaster(AccountMasterToUpdate);

            if (updatedAccountMaster == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var AccountMasterTransferDTOs = _mapper.Map<AccountMasterTransferDTO>(updatedAccountMaster);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountMasterTransferDTOs);
        }

        public async Task<ApiCommonResponse> PostPeriodicAccountMaster(HttpContext httpContext)
        {               
            
                try
                {
                    _logger.LogInformation("Searching for Invoices to Post.......");

                   var today = DateTime.Now;

                var invoices = await _context.Invoices
                            .Where(x => x.IsAccountPosted==false && x.IsFinalInvoice==true && x.StartDate.Date == today )
                            .ToListAsync();                  

                    if(!invoices.Any())
                    {
                        _logger.LogInformation($"No Invoice Scheduled For Posting Today {DateTime.Now.Date}.......");
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    foreach (var invoice in invoices)
                    {
                        var transaction = await _context.Database.BeginTransactionAsync();
                        try
                        {
                            _logger.LogInformation($"Posting Invoice with Id: {invoice.Id}");

                            this.LoggedInUserId = invoice.CreatedById ?? httpContext.GetLoggedInUserId();

                            var contractService = await _context.ContractServices
                                        .Include(x => x.Service)
                                        .FirstOrDefaultAsync(x => x.Id == invoice.ContractServiceId);

                            var customerDivision = await _context.CustomerDivisions
                                            .Where(x => x.Id == invoice.CustomerDivisionId)
                                            .Include(x => x.Customer)
                                                .ThenInclude(x => x.GroupType)
                                            .FirstOrDefaultAsync();

                            FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                                    .FirstOrDefaultAsync(x => x.VoucherType == this.SALES_INVOICE_VOUCHER);

                            
                            await _leadConversionService.CreateAccounts(contractService, customerDivision, (long)contractService.BranchId
                                , (long)contractService.OfficeId, contractService.Service, accountVoucherType, null, LoggedInUserId, false, invoice);

                            invoice.IsAccountPosted = true;
                            invoice.DatePosted = DateTime.Now;
                            _context.Invoices.Update(invoice);
                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            _logger.LogError($"Error ocurred posting account for invoice with id {invoice.Id}");
                            _logger.LogError(ex.StackTrace);
                        }

                     _logger.LogInformation($"Posted Account Master For Invoice with Id: {invoice.Id}\n\n");
                    }                   

                    return CommonResponse.Send(ResponseCodes.SUCCESS);
                }
                catch (System.Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }            
        }   
      
    }
}
