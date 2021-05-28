using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Adapters;
using HaloBiz.CustomExceptions;
using HalobizMigrations.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.MailDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;

using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ILogger<InvoiceService> _logger;
        private readonly IContractServiceRepository _contractServiceRepo;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IMapper _mapper;
        private readonly HalobizContext _context;
        private readonly IAccountMasterRepository _accountMasterRepo;
        private readonly IAccountDetailsRepository _accountDetailsRepo;
        private readonly IServicesRepository _serviceRepo;
        private readonly ICustomerDivisionRepository _customerDivisionRepo;
        private readonly  IAccountRepository _accountRepo;
        private readonly  IMailAdapter _mailAdapter;
        private bool isRetail = false;
        private readonly string SALESINVOICEVOUCHER = "Sales Invoice";
        private readonly string ReceivableControlAccount = "Receivable";
        private readonly string VALUEADDEDTAX = "VALUE ADDED TAX";
        private readonly string RETAIL_RECEIVABLE_ACCOUNT = "RETAIL RECEIVABLE ACCOUNT";
        private readonly string RETAIL = "RETAIL";

        public long LoggedInUserId;
        public long branchId;
        public long officeId;

        public InvoiceService(
                                IContractServiceRepository contractServiceRepo, 
                                IModificationHistoryRepository historyRepo, 
                                IInvoiceRepository invoiceRepo, 
                                ILogger<InvoiceService> logger, 
                                IMapper mapper, HalobizContext context,
                                IAccountMasterRepository accountMasterRepo,
                                IAccountDetailsRepository accountDetailsRepo,
                                IServicesRepository serviceRepo,
                                ICustomerDivisionRepository customerDivisionRepo,
                                IAccountRepository accountRepo,
                                IMailAdapter mailAdapter
                                )
        {
            this._mapper = mapper;
            this._context = context;
            this._contractServiceRepo = contractServiceRepo;
            this._historyRepo = historyRepo;
            this._invoiceRepo = invoiceRepo;
            this._logger = logger;
            this._accountMasterRepo = accountMasterRepo;
            this._accountDetailsRepo = accountDetailsRepo;
            this._customerDivisionRepo = customerDivisionRepo;
            this._accountRepo = accountRepo;
            this._serviceRepo = serviceRepo;
            this._mailAdapter = mailAdapter;
        }
        

        public async  Task<ApiResponse> AddInvoice(HttpContext context, InvoiceReceivingDTO invoiceReceivingDTO)
        {
            using(var transaction  = await _context.Database.BeginTransactionAsync())
            {
                try{
                    this.LoggedInUserId = context.GetLoggedInUserId();

                    var invoice = _mapper.Map<Invoice>(invoiceReceivingDTO);
                    var contractService = await _context.ContractServices
                                .FirstOrDefaultAsync(x => x.Id == invoice.ContractServiceId);

                    if(invoice.Value + contractService.AdHocInvoicedAmount > contractService.BillableAmount)
                    {
                        throw new InvalidAdHocBillableAmount($"Total Invoiced value cannot be greater than the billable amount for contract with id: {contractService.Id}");
                    }
                    
                    var service = await _context.Services
                                    .FirstOrDefaultAsync(x => x.Id == contractService.ServiceId);

                    invoice.InvoiceNumber = $"INV{contractService.Id.ToString().PadLeft(8, '0')}";
                    invoice.ContractId = contractService.ContractId;
                    invoice.CreatedById = this.LoggedInUserId;
                    invoice.InvoiceType = (int)InvoiceType.New;
                    invoice.IsFinalInvoice = false;
                    invoice.TransactionId = GenerateTransactionNumber(service.ServiceCode, contractService);

                    var savedInvoice = await _context.Invoices.AddAsync(invoice);
                    await _context.SaveChangesAsync();
                    var invoiceTransferDTO = _mapper.Map<InvoiceTransferDTO>(savedInvoice.Entity);                    
                    await transaction.CommitAsync();
                    return new ApiOkResponse(invoiceTransferDTO);
                }
                catch(InvalidAdHocBillableAmount e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await transaction.RollbackAsync();
                    return new ApiResponse(400, e.Message);
                }catch(Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await transaction.RollbackAsync();
                    return new ApiResponse(500);
                }
            }
           
        }


        public async Task<ApiResponse> AddGroupInvoice(HttpContext httpContext, GroupInvoiceDto groupInvoiceDto)
        {
            using(var transaction  = await _context.Database.BeginTransactionAsync())
            {
                try{
                    this.LoggedInUserId = httpContext.GetLoggedInUserId();

                    var primaryContractService = await  _context.ContractServices
                                .FirstOrDefaultAsync(x => x.GroupInvoiceNumber == groupInvoiceDto.GroupInvoiceNumber && !x.IsDeleted);
                    
                    var contractService = GenerateBulkContractService(primaryContractService, groupInvoiceDto.TotalBillable, groupInvoiceDto.VAT);

                    Invoice invoice = await GenerateInvoice(groupInvoiceDto, contractService);

                    var savedInvoice = await _context.Invoices.AddAsync(invoice);
                    await _context.SaveChangesAsync();
                    var invoiceTransferDTO = _mapper.Map<InvoiceTransferDTO>(savedInvoice.Entity);
                    
                    await transaction.CommitAsync();
                    return new ApiOkResponse(invoiceTransferDTO);
                }
                catch(Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await transaction.RollbackAsync();
                    return new ApiResponse(500);
                }
            }

        }

        public async Task<ApiResponse> ConvertProformaInvoiceToFinalInvoice(HttpContext httpContext, long invoiceId)
        {
             var invoiceToUpdate = await _invoiceRepo.FindInvoiceById(invoiceId);
             invoiceToUpdate.IsAccountPosted = true;
             this.LoggedInUserId = httpContext.GetLoggedInUserId();
             if(invoiceToUpdate == null)
             {
                 return new ApiResponse(404);
             }
             
             if(String.IsNullOrWhiteSpace(invoiceToUpdate.GroupInvoiceNumber))
             {
                 return await ConvertInvoiceToFinalInvoice( invoiceToUpdate);
             }
             else
             {
                 return await ConvertGroupInvoiceToFinalInvoice(invoiceToUpdate);
             }

        }

        


        
        private async Task<ApiResponse> ConvertGroupInvoiceToFinalInvoice(Invoice invoice)
        {
            using(var transaction  = await _context.Database.BeginTransactionAsync())
            {
                try{

                    var primaryContractService = await _context.ContractServices.FirstOrDefaultAsync(x => x.Id == invoice.ContractServiceId && !x.IsDeleted);
                    
                    await UpdateEachAndGeneratePrimaryContractService(invoice.GroupInvoiceNumber, invoice.Value);

                    var VAT =  invoice.Value * (7.5 / 107.5);

                    var contractService = GenerateBulkContractService(primaryContractService, invoice.Value, VAT);                    

                    var customerDivision = await _context.CustomerDivisions
                                        .Include(x => x.Customer)
                                        .FirstOrDefaultAsync(x => x.Id == invoice.CustomerDivisionId);

                    this.isRetail = customerDivision.Customer.GroupName == RETAIL;

                    FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType == this.SALESINVOICEVOUCHER);

                    var service = await _context.Services
                            .FirstOrDefaultAsync(x => x.Id == contractService.ServiceId);

                    await PostAccounts(contractService, customerDivision, accountVoucherType.Id, 
                                    (double)contractService.Vat, (double)contractService.BillableAmount, service);
                    await GenerateAmortizations(contractService, customerDivision, 
                                    (double) contractService.BillableAmount, (double)contractService.BillableAmount, 
                                    invoice.DateToBeSent);

                    if(!await _context.GroupInvoiceDetails.AnyAsync(x => x.InvoiceNumber == invoice.GroupInvoiceNumber))
                    {
                        await GenerateAndSaveGroupInvoiceDetails(invoice.GroupInvoiceNumber);
                    }
                    invoice.IsFinalInvoice = true;
                    _context.Invoices.Update(invoice);
                    await _context.SaveChangesAsync();
                    var invoiceTransferDTO = _mapper.Map<InvoiceTransferDTO>(invoice);
                    
                    await transaction.CommitAsync();
                    return new ApiOkResponse(invoice);

                }catch(Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await transaction.RollbackAsync();
                    return new ApiResponse(500);
                }
            }
            
        }





        private async Task<ApiResponse> ConvertInvoiceToFinalInvoice(Invoice invoice)
        {
            using(var transaction  = await _context.Database.BeginTransactionAsync())
            {
                try{
                    var contractService = await _context.ContractServices
                                .Include(x => x.QuoteService)
                                .FirstOrDefaultAsync(x => x.Id == invoice.ContractServiceId);

                    contractService.AdHocInvoicedAmount+= invoice.Value;
                    _context.ContractServices.Update(contractService);
                    await _context.SaveChangesAsync();

                    var customerDivision = await _context.CustomerDivisions
                                        .Include(x => x.Customer)
                                        .FirstOrDefaultAsync(x => x.Id == invoice.CustomerDivisionId);

                    this.isRetail = customerDivision.Customer.GroupName == RETAIL;

                    FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType == this.SALESINVOICEVOUCHER);
                    
                    var service = await _context.Services
                                    .FirstOrDefaultAsync(x => x.Id == contractService.ServiceId);


                    var VAT = (double) invoice.Value * (7.5 / 107.5);

                    await PostAccounts(contractService, customerDivision, accountVoucherType.Id, 
                                        VAT, invoice.Value, service);

                    invoice.IsFinalInvoice = true;
                    _context.Invoices.Update(invoice);
                    await _context.SaveChangesAsync();
                    var invoiceTransferDTO = _mapper.Map<InvoiceTransferDTO>(invoice);

                    await GenerateAmortizations(contractService, customerDivision, 
                                   (double) contractService.BillableAmount, invoice.Value, invoice.DateToBeSent );
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ApiOkResponse(invoiceTransferDTO);
                    }catch(Exception e)
                    {
                        _logger.LogError(e.Message);
                        _logger.LogError(e.StackTrace);
                        await transaction.RollbackAsync();
                        return new ApiResponse(500);
                    }
            }

        }


        private async Task<bool> GenerateAndSaveGroupInvoiceDetails(string groupInvoiceNumber)
        {
            var contractServices = await _context.ContractServices.Where(x => x.GroupInvoiceNumber == groupInvoiceNumber && !x.IsDeleted).ToListAsync();
            var groupInvoiceDetails = new List<GroupInvoiceDetail>();
            foreach (var contractService in contractServices)
            {
                groupInvoiceDetails.Add(
                    new GroupInvoiceDetail()
                    {
                        InvoiceNumber = contractService.GroupInvoiceNumber,
                        Description = $"Invoice details for Group Invoice {contractService.GroupInvoiceNumber}",
                        UnitPrice = (double) contractService.UnitPrice,
                        Quantity =(int) contractService.Quantity,
                        Vat  = (double) contractService.Vat,
                        Value = (double) (contractService.BillableAmount - contractService.Vat),
                        BillableAmount = (double) contractService.BillableAmount,
                        ContractServiceId = contractService.Id
                    }
                );
            }

            await _context.GroupInvoiceDetails.AddRangeAsync(groupInvoiceDetails);
            await _context.SaveChangesAsync();
            return true;
        }

        private  ContractService GenerateBulkContractService(ContractService contractService, double billable, double VAT)
        {
            var newContractService = _mapper.Map<ContractService>(contractService);
            newContractService.BillableAmount = billable;
            newContractService.Vat = VAT;
            
            return newContractService;
        }

        private async Task<ContractService> UpdateEachAndGeneratePrimaryContractService(string groupInvoiceNumber, double billableAmount)
        {
            var contractServices = await _context.ContractServices
                .Where(x => x.GroupInvoiceNumber == groupInvoiceNumber && x.BillableAmount != x.AdHocInvoicedAmount).ToListAsync();
            
            int counter = 0;
            ContractService contractService = null;
            double amountToPost = 0.0;
            while(billableAmount > 0 || counter > contractServices.Count())
            {
                contractService = contractServices[counter];
                amountToPost =(double) contractService.BillableAmount - contractService.AdHocInvoicedAmount;
                
                if( amountToPost <= billableAmount )
                {
                    contractService.AdHocInvoicedAmount = contractService.BillableAmount?? 0;
                    billableAmount-= amountToPost;
                }else{
                    contractService.AdHocInvoicedAmount += billableAmount;
                    billableAmount = 0;
                }
                counter++;
            }

            _context.ContractServices.UpdateRange(contractService);
            await _context.SaveChangesAsync();

            return contractServices[0];
        }

        private async Task<Invoice> GenerateInvoice(GroupInvoiceDto groupInvoiceDto, ContractService contractService)
        {
            var index = await _context.Invoices.Where(x => x.GroupInvoiceNumber == groupInvoiceDto.GroupInvoiceNumber).CountAsync();
            return new  Invoice()
            {
                InvoiceNumber = $"{groupInvoiceDto.GroupInvoiceNumber}/{index + 1}",
                TransactionId = $"{groupInvoiceDto.GroupInvoiceNumber.Replace("GINV", "TRS")}/{contractService.Id}",
                UnitPrice = 0,
                Quantity = 0,
                Discount  = 0,
                Value = groupInvoiceDto.TotalBillable,
                DateToBeSent = groupInvoiceDto.DateToBeSent,
                StartDate = (DateTime)contractService.ContractStartDate,
                EndDate = (DateTime)contractService.ContractEndDate,
                CustomerDivisionId = groupInvoiceDto.CustomerDivisionId,
                GroupInvoiceNumber = groupInvoiceDto.GroupInvoiceNumber,
                IsFinalInvoice = false,
                InvoiceType = (int)InvoiceType.AdHoc,
                ContractId = contractService.ContractId,
                ContractServiceId = contractService.Id,
                IsAccountPosted = true,
                CreatedById = this.LoggedInUserId
            };
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
            }else if(customerDivision.AccountId > 0){
                accountId = (long) customerDivision.AccountId;
            }else{
                //Create Customer Account, Account master and account details
                var customerAccountId = await CreateCustomerAccount(customerDivision);
                customerDivision.AccountId = customerAccountId;
                accountId = customerAccountId;
                _context.CustomerDivisions.Update(customerDivision);
                await _context.SaveChangesAsync();

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


        private async Task<long> CreateCustomerAccount(CustomerDivision customerDivision)
        {
            ControlAccount controlAccount = await _context.ControlAccounts
                        .FirstOrDefaultAsync(x => x.Caption == this.ReceivableControlAccount);

                Account account = new Account(){
                    Name = $"{customerDivision.DivisionName} Receivable",
                    Description = $"Receivable Account of {customerDivision.DivisionName}",
                    Alias = GenerateClientAlias(customerDivision.DivisionName),
                    IsDebitBalance = true,
                    ControlAccountId = controlAccount.Id,
                    CreatedById = this.LoggedInUserId
                };

                var savedAccount = await SaveAccount(account);

                return savedAccount.Id;
        }

        private async Task<long> GetRetailAccount(CustomerDivision customerDivision ){
            
            Account retailAccount  = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == this.RETAIL_RECEIVABLE_ACCOUNT);
            long accountId = 0;
            if(retailAccount == null)
            {
                ControlAccount controlAccount = await _context.ControlAccounts
                        .FirstOrDefaultAsync(x => x.Caption == this.ReceivableControlAccount);

                Account account = new Account(){
                    Name = this.RETAIL_RECEIVABLE_ACCOUNT,
                    Description = $"Income Receivable Account of Retail Clients",
                    Alias = "RIA",
                    IsDebitBalance = true,
                    ControlAccountId = controlAccount.Id,
                    CreatedById = this.LoggedInUserId
                };
                var savedAccount = await SaveAccount(account);

                customerDivision.AccountId = savedAccount.Id;
                _context.CustomerDivisions.Update(customerDivision);
                await _context.SaveChangesAsync();
                accountId = savedAccount.Id;
            }else{
                accountId = retailAccount.Id;
            }

            return accountId;

        } 

        private async Task<Account> SaveAccount(Account account)
        {
            try{
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts ON");

                var  lastSavedAccount = await _context.Accounts.Where(x => x.ControlAccountId == account.ControlAccountId)
                    .OrderBy(x => x.Id).LastOrDefaultAsync();
                if(lastSavedAccount == null || lastSavedAccount.Id < 1000000000)
                {
                    account.Id = (long) account.ControlAccountId + 1;
                }else{
                    account.Id = lastSavedAccount.Id + 1;
                }
                var savedAccount = await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                return savedAccount.Entity;
            }catch(Exception)
            {
                throw;
            }finally{
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts OFF");
            }
           
        }

        private string GenerateClientAlias(string divisionName){
            string[] names = divisionName.Split(" ");
            string initial = "";
            foreach (var name in names)
            {
                initial+= name.Substring(0,1).ToUpper();
            }
            return initial;
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
            var vatAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == this.VALUEADDEDTAX);
 
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

        private async  Task<bool> GenerateAmortizations(ContractService contractService, CustomerDivision customerDivision, 
                                                        double billableAmount, double paidAmount, 
                                                        DateTime amortizationDate
                                                        )
        {
            DateTime startDate = (DateTime) contractService.ContractStartDate;
            DateTime endDate = (DateTime) contractService.ContractEndDate;
            var year = amortizationDate.Year;
            var month = amortizationDate.Month;

                var amortization = new Amortization() {
                    Year = year,
                    ClientId = customerDivision.CustomerId,
                    DivisionId = customerDivision.Id,
                    ContractId = contractService.ContractId,
                    ContractServiceId = contractService.Id,
                    ContractValue =  billableAmount,
                    January = DateTime.Parse($"{year}/01/31").Month == month ? paidAmount : 0,
                    February = DateTime.Parse($"{year}/02/28").Month == month ? paidAmount : 0,
                    March =  DateTime.Parse($"{year}/03/31").Month == month ? paidAmount : 0,
                    April = DateTime.Parse($"{year}/04/30").Month == month ? paidAmount : 0,
                    May = DateTime.Parse($"{year}/05/31").Month == month ? paidAmount : 0,
                    June = DateTime.Parse($"{year}/06/30").Month == month ? paidAmount : 0,
                    July = DateTime.Parse($"{year}/07/31").Month == month ? paidAmount : 0,
                    August = DateTime.Parse($"{year}/08/31").Month == month ? paidAmount : 0,
                    September = DateTime.Parse($"{year}/09/30").Month == month ? paidAmount : 0,
                    October = DateTime.Parse($"{year}/10/31").Month == month ? paidAmount : 0,
                    November = DateTime.Parse($"{year}/11/30").Month == month ? paidAmount : 0,
                    December = DateTime.Parse($"{year}/12/31").Month == month ? paidAmount : 0,
                    
                };

           await  _context.Amortizations.AddAsync(amortization);
            return true;
        }

         private double CalculateTotalAmountForContract(double priceOfService, DateTime contractStartDate, DateTime contractEndDate, TimeCycle timeCycle )
        {
            int numberOfMonth = 0;

            if(timeCycle == TimeCycle.OneTime)
            {
                return priceOfService;
            }

            while(contractStartDate <= contractEndDate)
            {
                numberOfMonth++;
                contractStartDate = contractStartDate.AddMonths(1);
            }
            
            return priceOfService * numberOfMonth;
        }
        public async Task<ApiResponse> DeleteInvoice(long id)
        {
            var invoiceToDelete = await _invoiceRepo.FindInvoiceById(id);
            if(invoiceToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _invoiceRepo.DeleteInvoice(invoiceToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllInvoice()
        {
            var invoice = await _invoiceRepo.GetInvoice();
            if (invoice == null)
            {
                return new ApiResponse(404);
            }
            var invoiceTransferDTO = _mapper.Map<IEnumerable<InvoiceTransferDTO>>(invoice);
            return new ApiOkResponse(invoiceTransferDTO);
        }
        public async Task<ApiResponse> GetAllInvoicesByContactserviceId(long contractServiceId)
        {
            var invoice = await _invoiceRepo.GetInvoiceByContractServiceId(contractServiceId);
            if (invoice == null)
            {
                return new ApiResponse(404);
            }
            var invoiceTransferDTO = _mapper.Map<IEnumerable<InvoiceTransferDTO>>(invoice);
            return new ApiOkResponse(invoiceTransferDTO);
        }

        public async Task<ApiResponse> GetAllInvoicesById(long id)
        {
            var invoice = await _invoiceRepo.FindInvoiceById(id);
            if (invoice == null)
            {
                return new ApiResponse(404);
            }
            var invoiceTransferDTO = _mapper.Map<InvoiceTransferDTO>(invoice);
            return new ApiOkResponse(invoiceTransferDTO);
        }


        public  async Task<ApiResponse> UpdateInvoice(HttpContext context, long id, InvoiceReceivingDTO invoiceReceivingDTO)
        {
            var invoiceToUpdate = await _invoiceRepo.FindInvoiceById(id);
            if (invoiceToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {invoiceToUpdate.ToString()} \n" ;

            invoiceToUpdate.UnitPrice = invoiceReceivingDTO.UnitPrice;
            invoiceToUpdate.Quantity = invoiceReceivingDTO.Quantity;
            invoiceToUpdate.Discount = invoiceReceivingDTO.Discount;
            invoiceToUpdate.Value = invoiceReceivingDTO.Value;
            invoiceToUpdate.DateToBeSent = invoiceReceivingDTO.DateToBeSent;
            invoiceToUpdate.StartDate = invoiceReceivingDTO.StartDate;
            invoiceToUpdate.EndDate = invoiceReceivingDTO.EndDate;
            invoiceToUpdate.CustomerDivisionId = invoiceReceivingDTO.CustomerDivisionId;
            invoiceToUpdate.ContractServiceId = invoiceReceivingDTO.ContractServiceId;

            var updatedInvoice = await _invoiceRepo.UpdateInvoice(invoiceToUpdate);

            summary += $"Details after change, \n {updatedInvoice.ToString()} \n";

            if (updatedInvoice == null)
            {
                return new ApiResponse(500);
            }

            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Invoice",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedInvoice.Id
            };

            await _historyRepo.SaveHistory(history);

            var invoiceTransferDTOs = _mapper.Map<InvoiceTransferDTO>(updatedInvoice);
            return new ApiOkResponse(invoiceTransferDTOs);
        }


        public async Task<ApiResponse> SendPeriodicInvoices()
        {
            List<Invoice> invoices = new List<Invoice>();
            try
            {
                DateTime today = DateTime.Now.Date;
                //var today = DateTime.Parse("2021-03-24 00:00:00.0000000").Date;
                invoices = await _context.Invoices
                    .Where(x => x.IsFinalInvoice.Value && !x.IsDeleted 
                            && x.DateToBeSent.Date == today && !x.IsInvoiceSent).ToListAsync();
                
                
                foreach (var invoice in invoices)
                {
                    invoice.IsInvoiceSent = await SendInvoice(invoice);
                }

                await _context.SaveChangesAsync();
                return new ApiOkResponse(true);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }finally{
                try
                {
                    if(invoices.Count() > 0)
                        _context.Invoices.UpdateRange(invoices);
                }
                catch (System.Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                }
            }
            
        }
        private async Task<bool> SendInvoice(Invoice invoice)
        {
            try
            {
                InvoiceMailDTO invoiceMailDTO = await GenerateInvoiceMailDTO(invoice);
                ApiResponse response = await _mailAdapter.SendPeriodicInvoice(invoiceMailDTO);
                return response.StatusCode == 200;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"An Error occured while trying to send Invoice with Id: {invoice.Id}");
                _logger.LogError($"Error: {ex.Message}");
                _logger.LogError($"Error: {ex.StackTrace}");
                return false;
            }

        }
        public async Task<ApiResponse> GetInvoiceDetails(long invoiceId)
        {
            try
            {
                Invoice invoice = await _context.Invoices
                            .FirstOrDefaultAsync(x => x.Id == invoiceId && !x.IsDeleted);

                if(invoice == null)
                {
                    return new ApiResponse(404);
                }

                InvoiceMailDTO invoiceMailDTO = await GenerateInvoiceMailDTO(invoice);
                return new ApiOkResponse(invoiceMailDTO);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"An Error occured while trying to send Invoice with Id: {invoiceId}");
                _logger.LogError($"Error: {ex.Message}");
                _logger.LogError($"Error: {ex.StackTrace}");
                return new ApiResponse(500);
            }

        }

        public async Task<ApiResponse> SendInvoice(long invoiceId)
        {
            try
            {
                Invoice invoice = await _context.Invoices
                            .FirstOrDefaultAsync(x => x.Id == invoiceId && !x.IsDeleted);

                if(invoice == null)
                {
                    return new ApiResponse(404);
                }

                InvoiceMailDTO invoiceMailDTO = await GenerateInvoiceMailDTO(invoice);
                ApiResponse response = await _mailAdapter.SendPeriodicInvoice(invoiceMailDTO);
                if(response.StatusCode == 200)
                {
                    return new ApiOkResponse(true);
                }
                return new ApiResponse(500, "Invoice Not Sent");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"An Error occured while trying to send Invoice with Id: {invoiceId}");
                _logger.LogError($"Error: {ex.Message}");
                _logger.LogError($"Error: {ex.StackTrace}");
                return new ApiResponse(500, "Invoice Not Sent");
            }

        }


        private async Task<InvoiceMailDTO> GenerateInvoiceMailDTO(Invoice invoice)
        {
                var customerDivision = await _context.CustomerDivisions
                                .Include(x => x.PrimaryContact)
                                .Include(x => x.SecondaryContact)
                                .Include(x => x.State)
                                .Include(x => x.Lga)
                                .FirstOrDefaultAsync(x => x.Id == invoice.CustomerDivisionId);

            
                IEnumerable<ContractService> contractServices;
                if(String.IsNullOrWhiteSpace(invoice.GroupInvoiceNumber))
                {
                    contractServices = await _context.ContractServices
                            .Include(x => x.Service)
                            .Where(x => x.Id == invoice.ContractServiceId && !x.IsDeleted).ToListAsync();
                }else{
                    contractServices = await _context.ContractServices
                            .Include(x => x.Service)
                            .Where(x => x.GroupInvoiceNumber == invoice.GroupInvoiceNumber && !x.IsDeleted).ToListAsync();
                }

                double discount = 0.0;
                double subTotal = 0.0;
                double unInvoicedAmount = 0.0;
                double VAT = 0.0;
                string invoiceCycle = null;
                string keyServiceName = "";
                List<string> recepients = new List<string>();
                recepients.Add(customerDivision.Email);
                if(customerDivision.SecondaryContact != null)
                    recepients.Add(customerDivision.SecondaryContact.Email);
                
                if(customerDivision.PrimaryContact != null)
                    recepients.Add(customerDivision.PrimaryContact.Email);
                List<ContractServiceMailDTO> contractServiceMailDTOs = new List<ContractServiceMailDTO>();
                
                foreach (var contractService in contractServices)
                {
                    discount += contractService.Discount;
                    subTotal += (double)contractService.UnitPrice * (double) contractService.Quantity;
                    VAT += (double) contractService.Vat;
                    unInvoicedAmount += ((double)contractService.BillableAmount - contractService.AdHocInvoicedAmount);
                    invoiceCycle = contractService.InvoicingInterval.ToString();
                    keyServiceName = contractService.Service.Name;

                    contractServiceMailDTOs.Add(new ContractServiceMailDTO()
                    {
                        Description = contractService.Service.Name,
                        UnitPrice = contractService.UnitPrice??0,
                        Quantity = contractService.Quantity,
                        Total = contractService.Quantity * contractService.UnitPrice??0,
                        Discount = contractService.Discount,
                    });

                }

                ClientInfoMailDTO client  = new ClientInfoMailDTO()
                {
                    Name = customerDivision.DivisionName,
                    Email = customerDivision.Email,
                    Street = customerDivision.Street,
                    State = customerDivision.State != null ? customerDivision.State.Name : "No State Provided",
                    LGA = customerDivision.Lga != null? customerDivision.Lga.Name : "No LGA Provided"
                };
                
                InvoiceMailDTO invoiceMailDTO = new InvoiceMailDTO()
                {
                    Id = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    Total = invoice.Value,
                    SubTotal = subTotal,
                    VAT = subTotal * (7.5 / 100),
                    UnInvoicedAmount = unInvoicedAmount,
                    Discount = discount,
                    InvoicingCycle = invoiceCycle,
                    StartDate = invoice.StartDate,
                    EndDate = invoice.EndDate,
                    Subject = $"Invoice {invoice.InvoiceNumber} for {keyServiceName} due {invoice.EndDate.ToString("dddd, dd MMMM yyyy")}",
                    Recepients = recepients.ToArray(),
                    DaysUntilDeadline = (int) invoice.EndDate.Subtract(DateTime.Now).TotalDays,
                    ClientInfo = client,
                    ContractServices = contractServiceMailDTOs
                };

                return invoiceMailDTO;
        }

    }
}