using AutoMapper;
using HalobizMigrations.Data;
using HaloBiz.DTOs.ApiDTOs;
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

namespace HaloBiz.MyServices.Impl
{
    public class AccountMasterServiceImpl : IAccountMasterService
    {
        private readonly ILogger<AccountMasterServiceImpl> _logger;
        private readonly IAccountMasterRepository _accountMasterRepo;
        private readonly IMapper _mapper;
        private readonly  IInvoiceRepository _invoiceRepo;
        private readonly  HalobizContext _context;
        private readonly  IConfiguration _configuration;
        private readonly  string RETAIL = "retail";

        private string VALUE_ADDED_TAX;
        private bool isRetail;
        private string RETAIL_RECEIVABLE_ACCOUNT;
        private long LoggedInUserId;
        private string SALES_INVOICE_VOUCHER;

        public AccountMasterServiceImpl(
                    IConfiguration configuration,
                    IAccountMasterRepository accountMasterRepo,
                    ILogger<AccountMasterServiceImpl> logger, 
                    IMapper mapper,
                    IInvoiceRepository invoiceRepo,
                    HalobizContext context)
        {
            this._configuration = configuration;
            this._mapper = mapper;
            this._accountMasterRepo = accountMasterRepo;
            this._logger = logger;
            this._invoiceRepo = invoiceRepo;
            this._context = context;
            this.RETAIL_RECEIVABLE_ACCOUNT = _configuration.GetSection("AccountsInformation:RetailReceivableAccount").Value;
            this.SALES_INVOICE_VOUCHER = _configuration.GetSection("VoucherTypes:SalesInvoiceVoucher").Value;
            this.VALUE_ADDED_TAX = _configuration.GetSection("AccountsInformation:ValueAddedTask").Value;
        }

        public async Task<ApiResponse> AddAccountMaster(HttpContext context, AccountMasterReceivingDTO accountMasterReceivingDTO)
        {
            var acctClass = _mapper.Map<AccountMaster>(accountMasterReceivingDTO);
            acctClass.CreatedById = context.GetLoggedInUserId();
            var savedAccountMaster = await _accountMasterRepo.SaveAccountMaster(acctClass);
            if (savedAccountMaster == null)
            {
                return new ApiResponse(500);
            }
            var AccountMasterTransferDTOs = _mapper.Map<AccountMasterTransferDTO>(acctClass);
            return new ApiOkResponse(AccountMasterTransferDTOs);
        }

        public async Task<ApiResponse> DeleteAccountMaster(long id)
        {
            var AccountMasterToDelete = await _accountMasterRepo.FindAccountMasterById(id);
            if (AccountMasterToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _accountMasterRepo.DeleteAccountMaster(AccountMasterToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAccountMasterById(long id)
        {
            var AccountMaster = await _accountMasterRepo.FindAccountMasterById(id);
            if (AccountMaster == null)
            {
                return new ApiResponse(404);
            }
            var AccountMasterTransferDTOs = _mapper.Map<AccountMasterTransferDTO>(AccountMaster);
            return new ApiOkResponse(AccountMasterTransferDTOs);
        }

        public async Task<ApiResponse> QueryAccountMasters(AccountMasterTransactionDateQueryParams query)
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
        public async Task<ApiResponse> GetAllAccountMasters()
        {
            var AccountMaster = await _accountMasterRepo.FindAllAccountMasters();
            if (AccountMaster == null)
            {
                return new ApiResponse(404);
            }
            var AccountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(AccountMaster);
            return new ApiOkResponse(AccountMasterTransferDTOs);
        }
        public async Task<ApiResponse> GetAllAccountMastersByTransactionDate(AccountMasterTransactionDateQueryParams query)
        {
            try{
                var queryable =  _accountMasterRepo.GetAccountMastersQueryable();
                var accountMasters = await queryable.Where(x => x.CreatedAt >= query.StartDate
                            && x.CreatedAt <= query.EndDate && x.IsDeleted == false).ToListAsync();
                var AccountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
                return new ApiOkResponse(AccountMasterTransferDTOs);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
        }
        public async Task<ApiResponse> GetAllAccountMastersByVoucherId(AccountMasterTransactionDateQueryParams query)
        {
            try{
                var queryable =  _accountMasterRepo.GetAccountMastersQueryable();
                var accountMasters = await queryable.Where(x => x.CreatedAt >= query.StartDate
                            && x.CreatedAt <= query.EndDate && !x.IsDeleted && query.VoucherTypeIds.Contains(x.VoucherId)).ToListAsync();
                var AccountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
                return new ApiOkResponse(AccountMasterTransferDTOs);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
        }
        public async Task<ApiResponse> GetAllAccountMastersByTransactionId(string transactionId)
        {
            var accountMasters = await _accountMasterRepo.FindAccountMastersByTransactionId(transactionId);
            if (accountMasters == null)
            {
                return new ApiResponse(404);
            }
            var accountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
            return new ApiOkResponse(accountMasterTransferDTOs);
        }

        public async Task<ApiResponse> GetAllAccountMastersByCustomerIdAndContractYear(AccountMasterTransactionDateQueryParams query)
        {
            try{
                var accountMasters = await _accountMasterRepo.FindAllAccountMastersByCustomerId(query);
                var accountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
                return new ApiOkResponse(accountMasterTransferDTOs);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
            
        }

        public async Task<ApiResponse> UpdateAccountMaster(long id, AccountMasterReceivingDTO accountMasterReceivingDTO)
        {
            var AccountMasterToUpdate = await _accountMasterRepo.FindAccountMasterById(id);
            if (AccountMasterToUpdate == null)
            {
                return new ApiResponse(404);
            }
            AccountMasterToUpdate.Description = accountMasterReceivingDTO.Description;
            AccountMasterToUpdate.OfficeId = accountMasterReceivingDTO.OfficeId;
            AccountMasterToUpdate.BranchId = accountMasterReceivingDTO.BranchId;
            AccountMasterToUpdate.CustomerDivisionId = accountMasterReceivingDTO.CustomerDivisionId;
            AccountMasterToUpdate.DtrackJournalCode = accountMasterReceivingDTO.DTrackJournalCode;
            var updatedAccountMaster = await _accountMasterRepo.UpdateAccountMaster(AccountMasterToUpdate);

            if (updatedAccountMaster == null)
            {
                return new ApiResponse(500);
            }
            var AccountMasterTransferDTOs = _mapper.Map<AccountMasterTransferDTO>(updatedAccountMaster);
            return new ApiOkResponse(AccountMasterTransferDTOs);
        }

        public async Task<ApiResponse> PostPeriodicAccountMaster()
        {
            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _logger.LogInformation("Searching for Invoices to Post.......");
                    var queryable = _invoiceRepo.GetInvoiceQueryiable();
                    var today = DateTime.Now;

                    //var today = DateTime.Parse("2021-04-01");

                    var invoices = await queryable
                        .Where(x => !x.IsAccountPosted.Value && x.IsFinalInvoice.Value && x.StartDate.Date == today.Date)
                            .ToListAsync();

                    ContractService contractService;
                    CustomerDivision customerDivision;
                    Service service;
                    double VAT;

                    if(invoices.Count() == 0)
                    {
                        _logger.LogInformation($"No Invoice Scheduled For Posting Today {DateTime.Now.Date}.......");
                    }

                    foreach (var invoice in invoices)
                    {
                        _logger.LogInformation($"Posting Invoice with Id: {invoice.Id}");

                        this.LoggedInUserId = invoice.CreatedById?? 31;

                        contractService = await _context.ContractServices.FirstOrDefaultAsync(x => x.Id == invoice.ContractServiceId);
                        customerDivision = await _context.CustomerDivisions
                                        .Include(x => x.Customer)
                                        .FirstOrDefaultAsync(x => x.Id == invoice.CustomerDivisionId);

                        this.isRetail = customerDivision.Customer.GroupName.ToLower() == this.RETAIL;

                        FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                                .FirstOrDefaultAsync(x => x.VoucherType == this.SALES_INVOICE_VOUCHER);

                        service = await _context.Services
                                .FirstOrDefaultAsync(x => x.Id == contractService.ServiceId);

                        VAT = invoice.Value * (7.5 / 107.5);

                        await PostAccounts( contractService,
                                            customerDivision,
                                            accountVoucherType.Id,
                                            VAT, 
                                            invoice.Value, 
                                            service
                                         );

                        invoice.IsAccountPosted = true;

                        _logger.LogInformation($"Posted Account Master For Invoice with Id: {invoice.Id}\n\n");
                    }
                    _context.Invoices.UpdateRange(invoices);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ApiOkResponse(true);
                }
                catch (System.Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await transaction.RollbackAsync();
                    return new ApiResponse(500);
                }
            }
        }

        private async Task<bool>  PostAccounts(ContractService contractService,
                                         CustomerDivision customerDivision,
                                         long accountVoucherId,
                                         double VAT, 
                                         double billableAmount, 
                                         Service service
                                         )
        {

            var totalAfterTax = billableAmount - VAT;

            string description = $"Sales of {service.Name} with service ID: {service.Id} to {customerDivision.DivisionName}";
            
            string transactionId  = GenerateTransactionNumber(service.ServiceCode, contractService);

            var accountMaster = await CreateAccountMaster(
                                                    billableAmount,
                                                    customerDivision.Id,
                                                    transactionId,
                                                    contractService.Id,
                                                    accountVoucherId,
                                                    (long)contractService.BranchId,
                                                    (long)contractService.OfficeId,
                                                    description
                                                    );
            await PostCustomerReceivablAccounts(
                             description,
                             contractService.Id,
                             customerDivision,
                             (long)contractService.BranchId,
                             (long)contractService.OfficeId,
                             accountVoucherId,
                             billableAmount,
                             accountMaster.Id,
                             transactionId
                            );
            

            await PostVATAccountDetails(
                                    description,
                                    contractService.Id,
                                    customerDivision,
                                    (long)contractService.BranchId,
                                    (long)contractService.OfficeId,
                                    accountVoucherId,
                                    accountMaster.Id,
                                    VAT,
                                    transactionId
                                    );
            await PostIncomeAccountMasterAndDetails(
                                                    description,
                                                    contractService.Id,
                                                    customerDivision,
                                                    (long)contractService.BranchId,
                                                    (long)contractService.OfficeId,
                                                    accountVoucherId,
                                                    accountMaster.Id,
                                                    totalAfterTax,
                                                    transactionId,
                                                    service
                                                        );

            return true;
        }

        private string GenerateTransactionNumber(string serviceCode, ContractService contractService)
        {
            return String.IsNullOrWhiteSpace(contractService.GroupInvoiceNumber) ?  $"{serviceCode}/{contractService.Id}"
            : $"{contractService.GroupInvoiceNumber.Replace("GINV", "TRS")}/{contractService.Id}" ;
                    
        } 

        public async  Task<AccountMaster> CreateAccountMaster(double value,
                                          long customerDivisionId,
                                          string transactionId,
                                          long contractServiceId,
                                          long voucherId,
                                          long branchId, 
                                          long officeId, 
                                          string description)
        {
            var accountMaster = new AccountMaster(){
                Description = description,
                IntegrationFlag = false,
                Value = value,
                VoucherId = voucherId,
                TransactionId = transactionId,
                BranchId = branchId,
                OfficeId = officeId,
                CustomerDivisionId = customerDivisionId,
                CreatedById =  this.LoggedInUserId
            };
            var savedAccountMaster = await _accountMasterRepo.SaveAccountMaster(accountMaster);
            await _context.SaveChangesAsync();
            return savedAccountMaster;
        }


        private async Task<AccountDetail> PostAccountDetail(
                                                    string description,
                                                    long contractServiceId,  
                                                    long accountVoucherTypeId, 
                                                    long branchId, 
                                                    long officeId,
                                                    double amount,
                                                    bool isCredit,
                                                    long accountMasterId,
                                                    long accountId,
                                                    string transactionId
                                                    )
        {

            AccountDetail accountDetail = new AccountDetail(){
                Description =description,
                IntegrationFlag = false,
                VoucherId = accountVoucherTypeId,
                TransactionId = transactionId,
                TransactionDate = DateTime.Now,
                Credit = isCredit ? amount : 0,
                Debit = !isCredit ? amount : 0,
                AccountId = accountId,
                BranchId = branchId,
                OfficeId = officeId,
                AccountMasterId = accountMasterId,
                CreatedById = this.LoggedInUserId,
            };

            var savedAccountDetails = await _context.AccountDetails.AddAsync(accountDetail);
            await _context.SaveChangesAsync();
            return savedAccountDetails.Entity;
        }

        private async Task<bool> PostCustomerReceivablAccounts(
                                    string description,
                                    long contractServiceId,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId,
                                    long accountVoucherTypeId,
                                    double totalContractBillable,
                                    long accountMasterId,
                                    string transactionId
                                    )
        {
            long accountId = 0;
            if(this.isRetail)
            {
                accountId = await GetRetailAccount(customerDivision);
            }else{
                accountId = (long) customerDivision.ReceivableAccountId;
            }
            await PostAccountDetail(description, 
                                    contractServiceId, 
                                    accountVoucherTypeId, 
                                    branchId, 
                                    officeId,
                                    totalContractBillable, 
                                    false,
                                    accountMasterId,
                                    accountId,
                                    transactionId
                                    );
            
            return true;
        }

        private async Task<long> GetRetailAccount(CustomerDivision customerDivision )
        {
            
            Account retailAccount  = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == this.RETAIL_RECEIVABLE_ACCOUNT);
        
            return retailAccount.Id;

        } 


        
        private async Task<bool> PostVATAccountDetails(
                                    string description,
                                    long contractServiceId,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId,
                                    long accountVoucherTypeId,
                                    long accountMasterId,
                                    double totalVAT,
                                    string transactionId
                                    )
        {
            var vatAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == this.VALUE_ADDED_TAX);
 
            await PostAccountDetail(description, 
                                    contractServiceId, 
                                    accountVoucherTypeId, 
                                    branchId, 
                                    officeId,
                                    totalVAT, 
                                    true,
                                    accountMasterId,
                                    vatAccount.Id,
                                    transactionId);
            
            return true;
        }

        private async Task<bool> PostIncomeAccountMasterAndDetails(
                                    string description,
                                    long contractServiceId,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId,
                                    long accountVoucherTypeId,
                                    long accountMasterId,
                                    double totalBillableAfterTax,
                                    string transactionId,
                                    Service service
                                    )
        {
            

            await PostAccountDetail(description, 
                                    contractServiceId, 
                                    accountVoucherTypeId, 
                                    branchId, 
                                    officeId,
                                    totalBillableAfterTax, 
                                    true,
                                    accountMasterId,
                                    (long) service.AccountId,
                                    transactionId
                                    );
            
            return true;
        }
    }
    }
