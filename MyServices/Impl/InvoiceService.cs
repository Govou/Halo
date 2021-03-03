using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.LAMS;
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
        private readonly DataContext _context;
        private readonly IAccountMasterRepository _accountMasterRepo;
        private readonly IAccountDetailsRepository _accountDetailsRepo;
        private readonly IServicesRepository _serviceRepo;
        private readonly ICustomerDivisionRepository _customerDivisionRepo;
        private readonly  IAccountRepository _accountRepo;
        private bool isRetail = false;

        
        private readonly string SALESINVOICEVOUCHER = "Sales Invoice";
        private readonly string ReceivableControlAccount = "Receivable";
        private readonly string VALUEADDEDTAX = "VALUE ADDED TAX";
        private readonly string RETAIL_INCOME_ACCOUNT = "RETAIL RECEIVABLE ACCOUNT";
        private readonly string RETAIL = "RETAIL";

        public long LoggedInUserId;
        public long branchId;
        public long officeId;

        public InvoiceService(
                                IContractServiceRepository contractServiceRepo, 
                                IModificationHistoryRepository historyRepo, 
                                IInvoiceRepository invoiceRepo, 
                                ILogger<InvoiceService> logger, 
                                IMapper mapper, DataContext context,
                                IAccountMasterRepository accountMasterRepo,
                                IAccountDetailsRepository accountDetailsRepo,
                                IServicesRepository serviceRepo,
                                ICustomerDivisionRepository customerDivisionRepo,
                                IAccountRepository accountRepo
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
        }
        public async  Task<ApiResponse> AddInvoice(HttpContext context, InvoiceReceivingDTO invoiceReceivingDTO)
        {
            using(var transaction  = await _context.Database.BeginTransactionAsync())
            {
                try{
                    this.LoggedInUserId = context.GetLoggedInUserId();

                    var invoice = _mapper.Map<Invoice>(invoiceReceivingDTO);
                    var contractService = await  _contractServiceRepo.FindContractServiceById(invoice.ContractServiceId);
                    
                    invoice.ContractId = contractService.ContractId;
                    invoice.CreatedById = this.LoggedInUserId;
                    invoice.InvoiceType = InvoiceType.New;
                    invoice.IsFinalInvoice = false;
                    invoice.TransactionId = $"{contractService.Service.ServiceCode}/{contractService.Id}";

                    var savedInvoice = await _invoiceRepo.SaveInvoice(invoice);
                    var customerDivision = await _customerDivisionRepo.FindCustomerDivisionById(invoice.CustomerDivisionId);  
                    var service = await _serviceRepo.FindServicesById(contractService.ServiceId);

                    this.isRetail = customerDivision.Customer.GroupName == RETAIL;

                    FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType == this.SALESINVOICEVOUCHER);

                    var invoiceTransferDTO = _mapper.Map<InvoiceTransferDTO>(invoice);

                    var VAT = invoiceReceivingDTO.BillableAmount * (7.5 / 100.0);

                    var totalAfterTax = invoiceReceivingDTO.BillableAmount - VAT;
                    var branch = await _context.Branches.FirstOrDefaultAsync();
                    var office = await _context.Offices.FirstOrDefaultAsync();


                    var accountMaster = await CreateAccountMaster(
                                                            invoice,
                                                            contractService.QuoteService,
                                                            customerDivision,
                                                            service,
                                                            contractService.Id,
                                                            accountVoucherType.Id,
                                                            branch.Id,
                                                            office.Id
                                                            );
                    System.Console.WriteLine(accountMaster.Id);
                    await PostCustomerReceivablAccounts(
                                     contractService.QuoteService,
                                     contractService.Id,
                                     customerDivision,
                                     branch.Id,
                                     office.Id,
                                     accountVoucherType.Id,
                                     invoiceReceivingDTO.Value,
                                     accountMaster.Id
                                    );
                    
                    System.Console.WriteLine(accountMaster.Id);

                    await PostVATAccountDetails(
                                            contractService.QuoteService,
                                            contractService.Id,
                                            customerDivision,
                                            branch.Id,
                                            office.Id,
                                            accountVoucherType.Id,
                                            accountMaster.Id,
                                            VAT
                                            );
                    await PostIncomeAccountMasterAndDetails(
                                                            contractService.QuoteService,
                                                            contractService.Id,
                                                            customerDivision,
                                                            branch.Id,
                                                            office.Id,
                                                            accountVoucherType.Id,
                                                            accountMaster.Id,
                                                            totalAfterTax
                                                                )   ;
                    await GenerateAmortizations(contractService, customerDivision, invoice);
                    
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

        public async  Task<AccountMaster> CreateAccountMaster(Invoice invoice,
                                         QuoteService quoteService,
                                          CustomerDivision customerDivision,
                                          Services service,
                                          long contractServiceId,
                                          long voucherId,
                                          long branchId, long officeId)
        {
            var accountMaster = new AccountMaster(){
                Description = $"Sales of {service.Name} with service ID: {quoteService.Id} to {customerDivision.DivisionName}",
                IntegrationFlag = false,
                Value = invoice.Value,
                VoucherId = voucherId,
                TransactionId = $"{service.ServiceCode}/{contractServiceId}",
                BranchId = branchId,
                OfficeId = officeId,
                CustomerDivisionId = customerDivision.Id,
                CreatedById =  this.LoggedInUserId
            };
            var savedAccountMaster = await _context.AccountMasters.AddAsync(accountMaster);
            await _context.SaveChangesAsync();
            return savedAccountMaster.Entity;
        }


        private async Task<AccountDetail> PostAccountDetail(
                                                    QuoteService quoteService,
                                                    long contractServiceId,  
                                                    long accountVoucherTypeId, 
                                                    long branchId, 
                                                    long officeId,
                                                    double amount,
                                                    bool isCredit,
                                                    long accountMasterId,
                                                    long accountId,
                                                    CustomerDivision customerDivision
                                                    )
        {

            AccountDetail accountDetail = new AccountDetail(){
                Description = $"Sales of {quoteService.Service.Name}  with service ID: {quoteService.Id} to {customerDivision.DivisionName}",
                IntegrationFlag = false,
                VoucherId = accountVoucherTypeId,
                TransactionId = $"{quoteService.Service.ServiceCode}/{contractServiceId}",
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
                                    QuoteService quoteService,
                                    long contractServiceId,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId,
                                    long accountVoucherTypeId,
                                    double totalContractBillable,
                                    long accountMasterId
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

                var savedAccount = await _accountRepo.SaveAccount(account);

                customerDivision.AccountId = savedAccount.Id;
                accountId = savedAccount.Id;

                _context.CustomerDivisions.Update(customerDivision);
                await _context.SaveChangesAsync();

            }

            await PostAccountDetail(quoteService, 
                                    contractServiceId, 
                                    accountVoucherTypeId, 
                                    branchId, 
                                    officeId,
                                    totalContractBillable, 
                                    false,
                                    accountMasterId,
                                    accountId,
                                    customerDivision
                                    );
            
            return true;
        }

        private async Task<long> GetRetailAccount(CustomerDivision customerDivision ){
            
            Account retailAccount  = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == this.RETAIL_INCOME_ACCOUNT);
            long accountId = 0;
            if(retailAccount == null)
            {
                ControlAccount controlAccount = await _context.ControlAccounts
                        .FirstOrDefaultAsync(x => x.Caption == this.ReceivableControlAccount);

                Account account = new Account(){
                    Name = this.RETAIL_INCOME_ACCOUNT,
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
                                    QuoteService quoteService,
                                    long contractServiceId,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId,
                                    long accountVoucherTypeId,
                                    long accountMasterId,
                                    double totalVAT
                                    )
        {
            var vatAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == this.VALUEADDEDTAX);
 
            await PostAccountDetail(quoteService, 
                                    contractServiceId, 
                                    accountVoucherTypeId, 
                                    branchId, 
                                    officeId,
                                    totalVAT, 
                                    true,
                                    accountMasterId,
                                    vatAccount.Id,
                                    customerDivision);
            
            return true;
        }

        private async Task<bool> PostIncomeAccountMasterAndDetails(
                                    QuoteService quoteService,
                                    long contractServiceId,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId,
                                    long accountVoucherTypeId,
                                    long accountMasterId,
                                    double totalBillableAfterTax
                                    )
        {
            
            var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == quoteService.ServiceId);

            await PostAccountDetail(quoteService, 
                                    contractServiceId, 
                                    accountVoucherTypeId, 
                                    branchId, 
                                    officeId,
                                    totalBillableAfterTax, 
                                    true,
                                    accountMasterId,
                                    (long) service.AccountId,
                                    customerDivision
                                    );
            
            return true;
        }

        private async  Task<bool> GenerateAmortizations(ContractService contractService, CustomerDivision customerDivision, Invoice invoice)
        {
            DateTime startDate = (DateTime) contractService.ContractStartDate;
            DateTime endDate = (DateTime) contractService.ContractEndDate;
            var InitialYear = startDate.Year;
            List<Amortization> amortizations = new List<Amortization>();

            var totalContractBillable = CalculateTotalAmountForContract((double)contractService.BillableAmount, 
                                                                        (DateTime)startDate, 
                                                                        (DateTime) endDate, 
                                                                        (TimeCycle) contractService.InvoicingInterval);


            for(int i = startDate.Year; i <= endDate.Year; i++)
            {
                amortizations.Add(new Amortization(){
                    Year = i,
                    ClientId = customerDivision.CustomerId,
                    DivisionId = customerDivision.Id,
                    ContractId = contractService.ContractId,
                    ContractServiceId = contractService.Id,
                    ContractValue =  invoice.Value,
                    January = DateTime.Parse($"{i}/01/31") > startDate &&  DateTime.Parse($"{i}/01/31") <= endDate ? (double)contractService.BillableAmount : 0,
                    February = DateTime.Parse($"{i}/02/28") > startDate && DateTime.Parse($"{i}/02/28") <= endDate ? (double)contractService.BillableAmount : 0,
                    March =  DateTime.Parse($"{i}/03/31") > startDate && DateTime.Parse($"{i}/03/31") <= endDate ? (double)contractService.BillableAmount : 0,
                    April = DateTime.Parse($"{i}/04/30") > startDate && DateTime.Parse($"{i}/04/30") <= endDate ? (double)contractService.BillableAmount : 0,
                    May = DateTime.Parse($"{i}/05/31") > startDate && DateTime.Parse($"{i}/05/31") <= endDate ? (double)contractService.BillableAmount : 0,
                    June = DateTime.Parse($"{i}/06/30") > startDate && DateTime.Parse($"{i}/06/30") <= endDate ? (double)contractService.BillableAmount : 0,
                    July = DateTime.Parse($"{i}/07/31") > startDate && DateTime.Parse($"{i}/07/31") <= endDate ? (double)contractService.BillableAmount : 0,
                    August = DateTime.Parse($"{i}/08/31") > startDate && DateTime.Parse($"{i}/08/31") <= endDate ? (double)contractService.BillableAmount : 0,
                    September = DateTime.Parse($"{i}/09/30") > startDate && DateTime.Parse($"{i}/09/30") <= endDate ? (double)contractService.BillableAmount : 0,
                    October = DateTime.Parse($"{i}/10/31") > startDate && DateTime.Parse($"{i}/10/31") <= endDate ?  (double)contractService.BillableAmount : 0,
                    November = DateTime.Parse($"{i}/11/30") > startDate && DateTime.Parse($"{i}/11/30") <= endDate ? (double)contractService.BillableAmount : 0,
                    December = DateTime.Parse($"{i}/12/31") > startDate && DateTime.Parse($"{i}/12/31") <= endDate ? (double)contractService.BillableAmount : 0,
                    
                });
            }

           await  _context.Amortizations.AddRangeAsync(amortizations);
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
    }
}