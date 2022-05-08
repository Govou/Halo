using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.Helpers;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlinePortalBackend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class SMSContractsRepository : ISMSContractsRepository
    {
        private readonly HalobizContext _context;

        private readonly ILogger<SMSContractsRepository> _logger;
        private long LoggedInUserId = 0;
        private bool? isRetail = null;

        private readonly string ReceivableControlAccount = "Receivable";
        private readonly string VatControlAccount = "VAT";

        private readonly string SALESINVOICEVOUCHER = "Sales Invoice";

        private readonly string RETAIL_RECEIVABLE_ACCOUNT = "RETAIL RECEIVABLE ACCOUNT";
        private readonly string RETAIL_VAT_ACCOUNT = "RETAIL VAT ACCOUNT";

        private readonly string RETAIL = "RETAIL";

        public SMSContractsRepository(HalobizContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNewContract(SMSContractDTO contractDTO)
        {
            var service = _context.Services.FirstOrDefault(x => x.Id == contractDTO.ServiceId);
            var createdBy = 0;

            CustomerDivision customerDivision = new CustomerDivision();
            long contractId = 0;
            LeadDivision leadDivision = new LeadDivision();

            try
            {
                var leadDiv = _context.LeadDivisions.FirstOrDefault(x => x.Email == contractDTO.Email);
                if (leadDiv == null)
                    return false;

                var suspect = _context.Suspects.FirstOrDefault(x => x.Email == contractDTO.Email);

                var customer = new Customer
                {
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedById = createdBy,
                    Email = contractDTO.Email,
                    GroupName = leadDiv.DivisionName,
                    CustomerLeadId = leadDiv.LeadId,
                    Industry = leadDiv.Industry,
                    GroupTypeId = suspect.GroupTypeId,
                    Rcnumber = leadDiv.Rcnumber,
                    PhoneNumber = leadDiv.PhoneNumber,
                    LogoUrl = leadDiv.LogoUrl,
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                var customerDiv = new CustomerDivision
                {
                    Address = suspect.Address,
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedById = createdBy,
                    CustomerId = customer.Id,
                    Industry = leadDiv.Industry,
                    DivisionName = leadDiv.DivisionName,
                    Email = leadDiv.Email,
                    LeadDivisionId = leadDiv.Id,
                    Lgaid = leadDiv.Lgaid,
                    LogoUrl = leadDiv.LogoUrl,
                    StateId = leadDiv.StateId,
                    PhoneNumber = leadDiv.PhoneNumber,
                    Rcnumber = leadDiv.Rcnumber,
                    Street = leadDiv.Street
                };

                _context.CustomerDivisions.Add(customerDiv);
                await _context.SaveChangesAsync();

                var contract = new Contract
                {
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    IsApproved = true,
                    CustomerDivisionId = customerDiv.Id,
                    CreatedById = createdBy,
                    HasAddedSBU = true
                };

                await _context.Contracts.AddAsync(contract);
                await _context.SaveChangesAsync();


                var contractServiceToSave = new ContractService()
                {
                    UnitPrice = contractDTO.UnitPrice,
                    Quantity = contractDTO.Quantity,
                    Discount = contractDTO.Discount,
                    Vat = contractDTO.Vat,
                    BillableAmount = contractDTO.BillableAmount,
                    ContractStartDate = contractDTO.ContractStartDate,
                    ContractEndDate = contractDTO.ContractEndDate,
                    PaymentCycle = contractDTO.PaymentCycle,
                    FirstInvoiceSendDate = contractDTO.FirstInvoiceSendDate,
                    InvoicingInterval = contractDTO.InvoicingInterval,
                    ActivationDate = contractDTO.ActivationDate,
                    FulfillmentStartDate = contractDTO.FulfillmentStartDate,
                    FulfillmentEndDate = contractDTO.FulfillmentEndDate,
                    ContractId = contract.Id,
                    CreatedById = createdBy,
                    ServiceId = contractDTO.ServiceId,
                    OfficeId = contractDTO.OfficeId,
                    BranchId = contractDTO.BranchId,
                    UniqueTag = contractDTO.UniqueTag,
                };

                await _context.ContractServices.AddAsync(contractServiceToSave);
                await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();

                var contractService = await _context.ContractServices
                            .Where(x => x.Id == contractServiceToSave.Id)
                            .Include(x => x.Contract)
                            .Include(x => x.Service)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();


                FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                      .FirstOrDefaultAsync(x => x.VoucherType == SALESINVOICEVOUCHER);

                var (createSuccess, createMsg) = await CreateAccounts(
                                      contractService,
                                      customerDivision,
                                      (long)leadDivision.BranchId,
                                     (long)leadDivision.OfficeId,
                                     service,
                                     accountVoucherType,
                                     null,
                                     createdBy,
                                     false, null);

                var _serviceCode = service?.ServiceCode ?? contractService.Service?.ServiceCode;
                var (invoiceSuccess, invoiceMsg) = await GenerateInvoices(contractService, customerDivision.Id, _serviceCode, LoggedInUserId, "");
                var (amoSuccess, amoMsg) = await GenerateAmortizations(contractService, customerDivision, (double)contractService.BillableAmount);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error converting", ex);
                throw;
            }

            return true;
        }
        public async Task<(bool, string)> GenerateAmortizations(ContractService contractService, CustomerDivision customerDivision, double billableAmount, ContractServiceForEndorsement endorsement = null)
        {
            try
            {
                DateTime startDate = endorsement == null ? (DateTime)contractService.ContractStartDate : (DateTime)endorsement?.DateForNewContractToTakeEffect;
                DateTime endDate = (DateTime)contractService.ContractEndDate;
                var InitialYear = startDate.Year;
                var amount = billableAmount;

                var group = await _context.Customers.Where(x => x.Id == customerDivision.CustomerId)
                    .Include(x => x.GroupType)
                    .FirstOrDefaultAsync();
                var _customerType = group?.GroupType?.Id;

                var (interval, billableForInvoicingPeriod, vat) = CalculateTotalBillableForPeriod(contractService);
                var allMonthAndYear = new List<MonthsAndYears>();
                double? proratedAmount = null;

                billableAmount = billableAmount == 0 ? billableForInvoicingPeriod : billableAmount;

                billableAmount *= interval;

                if (contractService.InvoicingInterval == (int)TimeCycle.OneTime)

                {
                    endDate = startDate.AddMonths(1).AddDays(-1);
                }
                else if (contractService.InvoicingInterval == (int)TimeCycle.MonthlyProrata && startDate.Day != 1)
                {

                    var firstDayOfMonth = new DateTime(startDate.Year, startDate.Month, 1);
                    var lastDayOfFirstMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                    //get the first date and how many days left in the month
                    var daysCounted = (lastDayOfFirstMonth - startDate).TotalDays + 1;
                    var daysInMonth = (lastDayOfFirstMonth - firstDayOfMonth).TotalDays + 1;

                    proratedAmount = double.Parse((daysCounted / daysInMonth * amount).ToString("#.##"));

                }

                while (startDate < endDate)
                {
                    allMonthAndYear.Add(new MonthsAndYears { Month = startDate.Month, Year = startDate.Year });
                    startDate = startDate.AddMonths(interval);
                }

                for (int i = InitialYear; i <= endDate.Year; i++)
                {
                    var thisYearValues = allMonthAndYear.Where(x => x.Year == i).ToList();

                    if (thisYearValues.Count > 0)
                    {
                        var repAmoritizationMaster = new RepAmortizationMaster()
                        {
                            Year = i,
                            ClientId = customerDivision.Id,
                            DivisionId = contractService.Service?.DivisionId,
                            OperatingEntityId = contractService.Service?.OperatingEntityId,
                            ServiceCategoryId = contractService.Service?.ServiceCategoryId,
                            ServiceGroupId = contractService.Service?.ServiceGroupId,
                            ContractId = contractService.ContractId,
                            ServiceId = contractService.ServiceId,
                            ContractServiceId = contractService.Id,
                            GroupInvoiceNumber = contractService?.Contract?.GroupInvoiceNumber,
                            QuoteServiceId = contractService.QuoteServiceId,
                            ClientTypeId = _customerType,
                            DateCreated = DateTime.Now,
                            CreatedById = LoggedInUserId > 0 ? LoggedInUserId : contractService.CreatedById,
                            CustomerDivisionId = customerDivision.Id,
                            EndorsementTypeId = endorsement?.EndorsementTypeId ?? 1, //new 

                        };

                        await _context.RepAmortizationMasters.AddAsync(repAmoritizationMaster);
                        var affected = await _context.SaveChangesAsync();

                        if (affected == 0)
                            throw new Exception($"no data saved for year {i} for contract service with quote id {contractService?.QuoteServiceId}");


                        List<RepAmortizationDetail> repAmortizationDetails = new List<RepAmortizationDetail>();
                        int counter = 0;
                        foreach (var item in thisYearValues)
                        {
                            if (counter == 0 && proratedAmount != null)
                            {
                                repAmortizationDetails.Add(new RepAmortizationDetail
                                {
                                    Month = item.Month,
                                    BillableAmount = (double)proratedAmount,
                                    RepAmortizationMasterId = repAmoritizationMaster.Id,
                                });
                                ++counter;
                                continue;
                            }

                            repAmortizationDetails.Add(new RepAmortizationDetail
                            {
                                Month = item.Month,
                                BillableAmount = billableAmount,
                                RepAmortizationMasterId = repAmoritizationMaster.Id,
                            });
                            ++counter;
                        }

                        await _context.RepAmortizationDetails.AddRangeAsync(repAmortizationDetails);
                        await _context.SaveChangesAsync();
                        _context.ChangeTracker.Clear();
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex);
                return (false, ex.Message);
            }

            return (true, "success");
        }

        public List<Invoice> GenerateListOfInvoiceCycle(
                                           ContractService contractService,
                                           long customerDivisionId,
                                           string serviceCode,
                                           long loggedInUserId, string _startDate
                                           )
        {
            try
            {
                int invoiceNumber = 1;
                DateTime startDate = string.IsNullOrEmpty(_startDate) ? (DateTime)contractService.ContractStartDate : DateTime.Parse(_startDate);
                DateTime firstInvoiceSendDate = (DateTime)contractService.FirstInvoiceSendDate;
                DateTime endDate = (DateTime)contractService.ContractEndDate;
                TimeCycle cycle = (TimeCycle)contractService.InvoicingInterval;
                double amount = (double)contractService.BillableAmount;

                var hasPreviousInvoices = _context.Invoices.Any(x => x.ContractId == contractService.ContractId);

                List<Invoice> invoices = new List<Invoice>();

                var (interval, billableForInvoicingPeriod, vat) = CalculateTotalBillableForPeriod(contractService);


                if (cycle == TimeCycle.OneTime)
                {
                    invoices.Add(GenerateInvoice(startDate, endDate, amount, firstInvoiceSendDate,
                                                     contractService, customerDivisionId, serviceCode, invoiceNumber, loggedInUserId));

                }
                else if (cycle == TimeCycle.MonthlyProrata && startDate.Day != 1)
                {
                    var firstDayOfMonth = new DateTime(startDate.Year, startDate.Month, 1);
                    var lastDayOfFirstMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                    //get the first date and how many days left in the month
                    var daysCounted = (lastDayOfFirstMonth - startDate).TotalDays + 1;
                    var daysInMonth = (lastDayOfFirstMonth - firstDayOfMonth).TotalDays + 1;
                    var proratedAmount = double.Parse((daysCounted / daysInMonth * amount).ToString("#.##"));

                    invoices.Add(GenerateInvoice(startDate, lastDayOfFirstMonth, proratedAmount, firstInvoiceSendDate,
                                                     contractService, customerDivisionId, serviceCode, invoiceNumber, loggedInUserId));

                    //change start date to next day
                    startDate = lastDayOfFirstMonth.AddDays(1);
                    //get the next month and start from the first, add 1 month till end of contract date
                    while (startDate < endDate)
                    {
                        var invoiceEndDateToPost = startDate.AddMonths(interval);

                        //todo where is VAT in this?
                        invoices.Add(GenerateInvoice(startDate,
                                                    invoiceEndDateToPost,
                                                    amount,
                                                    firstInvoiceSendDate,
                                                    contractService,
                                                    customerDivisionId,
                                                    serviceCode,
                                                    invoiceNumber,
                                                    loggedInUserId));
                        startDate = startDate.AddMonths(interval);
                        invoiceNumber++;
                    }
                }
                else
                {

                    while (startDate < endDate)
                    {
                        var invoiceValueToPost = billableForInvoicingPeriod; var invoiceEndDateToPost = startDate.AddMonths(interval);

                        //todo where is VAT in this?
                        invoices.Add(GenerateInvoice(startDate,
                                                    invoiceEndDateToPost,
                                                    invoiceValueToPost,
                                                    firstInvoiceSendDate,
                                                    contractService,
                                                    customerDivisionId,
                                                    serviceCode,
                                                    invoiceNumber,
                                                    loggedInUserId));
                        startDate = startDate.AddMonths(interval);
                        invoiceNumber++;
                    }
                }
                return invoices;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.ToString()}");
                return null;
            }
        }

        private Invoice GenerateInvoice(
                              DateTime from, DateTime to,
                              double amount, DateTime sendDate,
                              ContractService contractService,
                              long customerDivisionId,
                              string serviceCode,
                              int invoiceIndex,
                              long loggedInUserId)
        {
            try
            {
                string invoiceNumber = String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) ?
                           $"INV{contractService.Id.ToString().PadLeft(8, '0')}"
                                   : contractService.Contract?.GroupInvoiceNumber;
                string transactionId = String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) ?
                                 $"TRS{serviceCode}/{contractService.Id}"
                                        : $"{contractService.Contract?.GroupInvoiceNumber?.Replace("GINV", "TRS")}/{contractService.Id}";


                return new Invoice()
                {
                    InvoiceNumber = $"{invoiceNumber}/{invoiceIndex}",
                    UnitPrice = (double)contractService.UnitPrice,
                    Quantity = contractService.Quantity,
                    Discount = contractService.Discount,
                    Value = amount,
                    TransactionId = transactionId,
                    DateToBeSent = sendDate,
                    IsInvoiceSent = false,
                    CustomerDivisionId = customerDivisionId,
                    ContractId = contractService.ContractId,
                    ContractServiceId = contractService.Id,
                    StartDate = from,
                    EndDate = to,
                    IsReceiptedStatus = (int)InvoiceStatus.NotReceipted,
                    IsFinalInvoice = true,
                    InvoiceType = (int)InvoiceType.New,
                    CreatedById = loggedInUserId,
                    GroupInvoiceNumber = string.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) ? null : invoiceNumber,
                    IsAccountPosted = invoiceIndex == 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GenerateInvoice: {ex.ToString()}");
                return null;
            }
        }


        public async Task<(bool, string)> GenerateInvoices(ContractService contractService, long customerDivisionId, string serviceCode, long loggedInUserId, string startDate)
        {
            List<Invoice> invoicesToSave = GenerateListOfInvoiceCycle(
                                                                        contractService,
                                                                        customerDivisionId,
                                                                        serviceCode,
                                                                        loggedInUserId, startDate);
            try
            {
                await _context.Invoices.AddRangeAsync(invoicesToSave);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }


            return (true, "success");
        }
        public async Task<(bool, string)> CreateAccounts(
                                   ContractService contractService,
                                   CustomerDivision customerDivision,
                                   long branchId,
                                   long officeId,
                                   Service service,
                                   FinanceVoucherType accountVoucherType,
                                   QuoteService quoteService,
                                   long loggedInUserId,
                                   bool isReversal,
                                   Invoice invoice,
                                   bool setIntegrationFlag = false)
        {

            double totalContractBillable, totalVAT = 0;
            int interval;

            if (invoice == null)
            {
                if (contractService.InvoicingInterval == (int)TimeCycle.MonthlyProrata)
                {
                    var startDate = (DateTime)contractService.ContractStartDate;
                    var firstDayOfMonth = new DateTime(startDate.Year, startDate.Month, 1);
                    var lastDayOfFirstMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                    //get the first date and how many days left in the month
                    var daysCounted = (lastDayOfFirstMonth - startDate).TotalDays + 1;
                    var daysInMonth = (lastDayOfFirstMonth - firstDayOfMonth).TotalDays + 1;

                    totalContractBillable = double.Parse((daysCounted / daysInMonth * (double)contractService.BillableAmount).ToString("#.##"));
                    if (contractService?.Service?.IsVatable == true)
                    {
                        totalVAT = totalContractBillable * 0.075;
                    }
                }
                else
                {
                    (interval, totalContractBillable, totalVAT) = CalculateTotalBillableForPeriod(contractService);
                }
            }
            else
            {

                totalContractBillable = invoice.Value;

                if (contractService?.Service?.IsVatable == true)
                {
                    totalContractBillable = invoice.Value - totalVAT;
                    totalVAT = invoice.Value * (7.5 / 107.5);
                    //make it 2dp
                    var vatString = totalVAT.ToString("#.##");
                    totalVAT = double.Parse(vatString);
                }
            }

            var totalAfterTax = totalContractBillable - totalVAT;

            var savedAccountMasterId = await CreateAccountMaster(service,
                                                         contractService,
                                                         accountVoucherType,
                                                         branchId,
                                                         officeId,
                                                         totalContractBillable,
                                                         customerDivision,
                                                         loggedInUserId
                                                         );

            if (quoteService != null)
            {
                // this breaks the conversion.
                bool succeded = await SaveRangeSbuaccountMaster(savedAccountMasterId, quoteService.SbutoQuoteServiceProportions);
            }

            try
            {
                var (isSuccesReceivable, messageReceivale) = await PostCustomerReceivablAccounts(
                                    service,
                                    contractService,
                                    customerDivision,
                                    branchId,
                                    officeId,
                                    accountVoucherType,
                                    totalContractBillable,
                                    savedAccountMasterId,
                                    loggedInUserId,
                                    isReversal
                                   );


                var (isSuccessVat, messageVat) = await PostVATAccountDetails(
                                          service,
                                          contractService,
                                          customerDivision,
                                          branchId,
                                          officeId,
                                          accountVoucherType,
                                          savedAccountMasterId,
                                          totalVAT,
                                         loggedInUserId,
                                         !isReversal
                                         );
                await PostIncomeAccount(
                                        service,
                                        contractService,
                                        customerDivision,
                                        branchId,
                                        officeId,
                                        accountVoucherType,
                                        savedAccountMasterId,
                                        totalAfterTax,
                                        loggedInUserId,
                                        !isReversal
                                                );

            }
            catch (Exception ex)
            {
                _logger.LogError("CreateAccounts", ex);
                throw;
            }

            return (true, "success");
        }

        public (int, double, double) CalculateTotalBillableForPeriod(ContractService contractService)
        {
            int interval = 1;
            TimeCycle cycle = (TimeCycle)contractService.InvoicingInterval;
            double amount = (double)contractService.BillableAmount;
            double vat = 0;
            if (contractService?.Service?.IsVatable == true)
            {
                vat = (double)contractService.Vat;
            }


            switch (cycle)
            {
                case TimeCycle.Monthly:
                    interval = 1;
                    break;
                case TimeCycle.BiMonthly:
                    interval = 2;
                    break;
                case TimeCycle.Quarterly:
                    interval = 3;
                    break;
                case TimeCycle.BiAnnually:
                    interval = 6;
                    break;
                case TimeCycle.Annually:
                    interval = 12;
                    break;
            }

            if (cycle == TimeCycle.OneTime)
            {
                return (interval, amount, vat);
            }
            else if (cycle == TimeCycle.Adhoc)
            {
                return (1, amount, vat);
            }
            else
            {
                return (interval, amount * interval, vat * interval);
            }
        }

        private async Task<bool> SaveRangeSbuaccountMaster(long accountMasterId, IEnumerable<SbutoQuoteServiceProportion> sBUToQuoteServiceProp)
        {
            try
            {
                List<SbuaccountMaster> listOfSbuaccountMaster = new List<SbuaccountMaster>();
                foreach (var prop in sBUToQuoteServiceProp)
                {
                    listOfSbuaccountMaster.Add(new SbuaccountMaster()
                    {
                        StrategicBusinessUnitId = prop.StrategicBusinessUnitId,
                        AccountMasterId = accountMasterId
                    });
                }

                listOfSbuaccountMaster = listOfSbuaccountMaster.GroupBy(i => new { i.StrategicBusinessUnitId, i.AccountMasterId })
                    .Select(g => g.First())
                    .ToList();

                _context.ChangeTracker.Clear();
                await _context.SbuaccountMasters.AddRangeAsync(listOfSbuaccountMaster);
                var affected = await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();

            }
            catch (Exception ex)
            {
                _logger.LogError("", ex);
                throw;
            }
            return true;
        }


        public async Task<long> CreateAccountMaster(Service service,
                                                        ContractService contractService,
                                                        FinanceVoucherType accountVoucherType,
                                                        long branchId,
                                                        long officeId,
                                                        double amount,
                                                        CustomerDivision customerDivision,
                                                        long createdById)
        {
            try
            {
                string transactionId = String.IsNullOrWhiteSpace(contractService?.Contract?.GroupInvoiceNumber) ?
                             $"TRS{service?.ServiceCode}/{contractService?.Id}"
                                    : $"{contractService?.Contract?.GroupInvoiceNumber?.Replace("GINV", "TRS")}/{contractService?.Id}";

                if (service == null)
                {
                    service = contractService?.Service;
                }

                AccountMaster accountMaster = new AccountMaster()
                {
                    Description = GetAccountMasterDescription(accountVoucherType, service, customerDivision, contractService),
                    IntegrationFlag = false,
                    VoucherId = accountVoucherType.Id,
                    BranchId = branchId,
                    OfficeId = officeId,
                    Value = amount,
                    TransactionId = transactionId,
                    CreatedById = createdById,
                    CustomerDivisionId = customerDivision.Id
                };

                _logger.LogInformation($"Data account master: {JsonConvert.SerializeObject(accountMaster)}");

                _context.ChangeTracker.Clear();
                //_context.Database.SetCommandTimeout(80000);

                var savedAccountMaster = await _context.AccountMasters.AddAsync(accountMaster);
                await _context.SaveChangesAsync();
                long id = accountMaster.Id;
                _context.ChangeTracker.Clear();

                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in CreateAccountMaster", ex);
                throw;
            }

        }

        private string GetAccountMasterDescription(FinanceVoucherType financeVoucherType, Service service, CustomerDivision customerDivision, ContractService contractService = null)
        {
            var fvt = financeVoucherType?.VoucherType.ToLower();

            var description = $"Sales of {service?.Name} to {customerDivision?.DivisionName} with tag '{contractService?.UniqueTag}'";

            if (fvt == "debit note")
            {
                description = $"Debit Note on {service?.Name} to {customerDivision?.DivisionName}";
            }
            else if (fvt == "credit note")
            {
                description = $"Credit Note on {service?.Name} to {customerDivision?.DivisionName}";
            }

            return description;
        }

        public async Task<(bool, string)> PostVATAccountDetails(
                                   Service service,
                                   ContractService contractService,
                                   CustomerDivision customerDivision,
                                   long branchId,
                                   long officeId,
                                   FinanceVoucherType accountVoucherType,
                                   long accountMasterId,
                                   double totalVAT,
                                   long loggedInUserId,
                                   bool isCredit
                                   )
        {
            long accountId = 0;

            try
            {
                if (isRetail == true)
                {
                    accountId = await GetRetailVATAccount(customerDivision);
                }
                else if (customerDivision.VatAccountId > 0)
                {
                    accountId = (long)customerDivision.VatAccountId;
                }
                else
                {
                    //Create customer vat account
                    ControlAccount controlAccount = await _context.ControlAccounts
                            .FirstOrDefaultAsync(x => x.Caption == VatControlAccount);

                    Account account = new Account()
                    {
                        Name = $"{customerDivision.DivisionName} VAT",
                        Description = $"VAT Account of {customerDivision.DivisionName}",
                        Alias = customerDivision.DTrackCustomerNumber,
                        IsDebitBalance = true,
                        ControlAccountId = controlAccount.Id,
                        CreatedById = loggedInUserId
                    };

                    _context.ChangeTracker.Clear();
                    var savedAccountId = await SaveAccount(account);

                    customerDivision.VatAccountId = savedAccountId;
                    accountId = savedAccountId;

                    _context.CustomerDivisions.Update(customerDivision);
                    var affected = await _context.SaveChangesAsync();
                    _context.ChangeTracker.Clear();


                }

                var (isPosted, details) = await PostAccountDetail(service,
                                        contractService,
                                        accountVoucherType,
                                        branchId,
                                        officeId,
                                        totalVAT,
                                        isCredit,
                                        accountMasterId,
                                        accountId,
                                        customerDivision,
                                        loggedInUserId
                                        );
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex.Message);
                throw;
                //return (false, ex.Message);
            }

            return (true, "success");
        }

        private async Task<(bool, AccountDetail)> PostAccountDetail(
                                                  Service service,
                                                  ContractService contractService,
                                                  FinanceVoucherType accountVoucherType,
                                                  long branchId,
                                                  long officeId,
                                                  double amount,
                                                  bool isCredit,
                                                  long accountMasterId,
                                                  long accountId,
                                                  CustomerDivision customerDivision,
                                                  long loggedInUserId
                                                  )
        {

            try
            {
                if (service == null)
                {
                    service = contractService?.Service;
                }

                AccountDetail accountDetail = new AccountDetail()
                {
                    Description = GetAccountMasterDescription(accountVoucherType, service, customerDivision, contractService),
                    IntegrationFlag = false,
                    VoucherId = accountVoucherType.Id,
                    TransactionId = $"{service?.ServiceCode}/{contractService?.Id}",
                    TransactionDate = DateTime.Now,
                    Credit = isCredit ? amount : 0,
                    Debit = !isCredit ? amount : 0,
                    AccountId = accountId,
                    BranchId = branchId,
                    OfficeId = officeId,
                    AccountMasterId = accountMasterId,
                    CreatedById = loggedInUserId,
                };

                var savedAccountDetails = await _context.AccountDetails.AddAsync(accountDetail);
                var affected = await _context.SaveChangesAsync();
                if (affected == 0)
                    throw new Exception("An error occured in the posting");
                else
                    return (true, savedAccountDetails.Entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex.Message);
                throw;
            }
        }

        private async Task<long> GetRetailVATAccount(CustomerDivision customerDivision)
        {

            try
            {
                Account vatAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == RETAIL_VAT_ACCOUNT);
                long accountId = 0;
                if (vatAccount == null)
                {
                    ControlAccount controlAccount = await _context.ControlAccounts
                            .FirstOrDefaultAsync(x => x.Caption == VatControlAccount);

                    Account account = new Account()
                    {
                        Name = RETAIL_VAT_ACCOUNT,
                        Description = $"VAT Account of Retail Clients",
                        Alias = "HA_RET",
                        IsDebitBalance = true,
                        ControlAccountId = controlAccount.Id,
                        CreatedById = LoggedInUserId
                    };
                    var savedAccountId = await SaveAccount(account);

                    customerDivision.VatAccountId = savedAccountId;
                    _context.CustomerDivisions.Update(customerDivision);
                    await _context.SaveChangesAsync();
                    accountId = savedAccountId;
                }
                else
                {
                    if (customerDivision.VatAccountId == null)
                    {
                        customerDivision.VatAccountId = vatAccount.Id;

                        _context.CustomerDivisions.Update(customerDivision);
                        await _context.SaveChangesAsync();
                        _context.ChangeTracker.Clear();
                    }

                    accountId = vatAccount.Id;

                }

                return accountId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }

        }

        public async Task<(bool, string)> PostCustomerReceivablAccounts(
                                 Service service,
                                 ContractService contractService,
                                 CustomerDivision customerDivision,
                                 long branchId,
                                 long officeId,
                                 FinanceVoucherType accountVoucherType,
                                 double totalContractBillable,
                                 long accountMasterId,
                                 long createdById,
                                 bool isCredit
                                 )
        {
            try
            {
                long accountId = 0;
                if (isRetail == true)
                {
                    accountId = await GetRetailReceivableAccount(customerDivision);
                }
                else if (customerDivision.ReceivableAccountId > 0)
                {
                    accountId = (long)customerDivision.ReceivableAccountId;
                }
                else
                {
                    //Create Customer Receivable Account
                    ControlAccount controlAccount = await _context.ControlAccounts
                            .FirstOrDefaultAsync(x => x.Caption == ReceivableControlAccount);

                    Account account = new Account()
                    {
                        Name = $"{customerDivision.DivisionName} Receivable",
                        Description = $"Receivable Account of {customerDivision.DivisionName}",
                        Alias = customerDivision.DTrackCustomerNumber,
                        IsDebitBalance = true,
                        ControlAccountId = controlAccount.Id,
                        CreatedById = createdById,
                        CreatedAt = DateTime.Now
                    };

                    var savedAccountId = await SaveAccount(account);

                    customerDivision.ReceivableAccountId = savedAccountId;
                    accountId = savedAccountId;

                    _context.CustomerDivisions.Update(customerDivision);
                    var affected = await _context.SaveChangesAsync();
                    _context.ChangeTracker.Clear();

                }


                var (success, details) = await PostAccountDetail(service,
                                         contractService,
                                         accountVoucherType,
                                         branchId,
                                         officeId,
                                         totalContractBillable,
                                         isCredit,
                                         accountMasterId,
                                         accountId,
                                         customerDivision,
                                         createdById
                                         );
            }
            catch (Exception ex)
            {
                // return (false, ex.Message);
                throw;
            }

            return (true, "success");
        }

        private async Task<long> SaveAccount(Account account)
        {
            try
            {

                var lastSavedAccount = await _context.Accounts.Where(x => x.ControlAccountId == account.ControlAccountId)
                    .OrderBy(x => x.Id).LastOrDefaultAsync();
                if (lastSavedAccount == null || lastSavedAccount?.AccountNumber < 1000000000)
                {
                    var _controlAccount = await _context.ControlAccounts.Where(x => x.Id == account.ControlAccountId).FirstOrDefaultAsync();

                    account.AccountNumber = _controlAccount.AccountNumber + 1;
                }
                else
                {
                    account.AccountNumber = lastSavedAccount.AccountNumber + 1;
                }

                _context.ChangeTracker.Clear();
                //remove exception throwing
                account.Alias = account.Alias ?? "";

                var savedAccount = await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();
                return savedAccount.Entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }

        }

        private async Task<long> GetServiceIncomeAccountForClient(CustomerDivision customerDivision, Service service)
        {
            string serviceClientIncomeAccountName = $"{service.Name} Income for {customerDivision.DivisionName}";

            Account serviceClientIncomeAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.ControlAccountId == (long)service.ControlAccountId && x.Name == serviceClientIncomeAccountName);

            long accountId = 0;
            if (serviceClientIncomeAccount == null)
            {
                Account account = new Account()
                {
                    Name = serviceClientIncomeAccountName,
                    Description = $"{service.Name} Income Account for {customerDivision.DivisionName}",
                    Alias = customerDivision.DTrackCustomerNumber ?? "",
                    IsDebitBalance = true,
                    ControlAccountId = (long)service.ControlAccountId,
                    CreatedById = LoggedInUserId
                };
                var savedAccountId = await SaveAccount(account);
                accountId = savedAccountId;
                await _context.SaveChangesAsync();
            }
            else
            {
                accountId = serviceClientIncomeAccount.Id;
            }

            return accountId;
        }

        private async Task<long> GetServiceIncomeAccountForRetailClient(Service service)
        {
            string serviceClientIncomeAccountName = $"{service.Name} Income for {RETAIL}";

            Account serviceClientIncomeAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.ControlAccountId == (long)service.ControlAccountId && x.Name == serviceClientIncomeAccountName);

            long accountId = 0;
            if (serviceClientIncomeAccount == null)
            {
                Account account = new Account()
                {
                    Name = serviceClientIncomeAccountName,
                    Description = $"{service.Name} Income Account for {RETAIL}",
                    Alias = "HA_RET",
                    IsDebitBalance = true,
                    ControlAccountId = (long)service.ControlAccountId,
                    CreatedById = LoggedInUserId
                };

                var savedAccountId = await SaveAccount(account);
                accountId = savedAccountId;
                await _context.SaveChangesAsync();
            }
            else
            {
                accountId = serviceClientIncomeAccount.Id;
            }

            return accountId;
        }

        public async Task<bool> PostIncomeAccount(
                                   Service service,
                                   ContractService contractService,
                                   CustomerDivision customerDivision,
                                   long branchId,
                                   long officeId,
                                   FinanceVoucherType accountVoucherType,
                                   long accountMasterId,
                                   double totalBillableAfterTax,
                                   long loggedInUserId,
                                   bool isCredit
                                   )
        {
            long accountId;
            if (service == null) service = contractService?.Service;

            if (isRetail == true)
            {
                accountId = await GetServiceIncomeAccountForRetailClient(service);
            }
            else
            {
                this.LoggedInUserId = loggedInUserId;
                accountId = await GetServiceIncomeAccountForClient(customerDivision, service);
            }


            await PostAccountDetail(service,
                                    contractService,
                                    accountVoucherType,
                                    branchId,
                                    officeId,
                                    totalBillableAfterTax,
                                    isCredit,
                                    accountMasterId,
                                    accountId,
                                    customerDivision,
                                    loggedInUserId
                                    );

            return true;
        }
        private async Task<long> GetRetailReceivableAccount(CustomerDivision customerDivision)
        {
            try
            {
                Account retailAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == RETAIL_RECEIVABLE_ACCOUNT);
                long accountId = 0;
                if (retailAccount == null)
                {
                    ControlAccount controlAccount = await _context.ControlAccounts
                            .FirstOrDefaultAsync(x => x.Caption == ReceivableControlAccount);

                    Account account = new Account()
                    {
                        Name = RETAIL_RECEIVABLE_ACCOUNT,
                        Description = $"Receivable Account of Retail Clients",
                        Alias = "HA_RET",
                        IsDebitBalance = true,
                        ControlAccountId = controlAccount.Id,
                        CreatedById = this.LoggedInUserId
                    };
                    var savedAccountId = await SaveAccount(account);

                    _context.ChangeTracker.Clear();
                    customerDivision.ReceivableAccountId = savedAccountId;
                    _context.CustomerDivisions.Update(customerDivision);
                    await _context.SaveChangesAsync();
                    _context.ChangeTracker.Clear();

                    accountId = savedAccountId;
                }
                else
                {
                    _context.ChangeTracker.Clear();

                    customerDivision.ReceivableAccountId = retailAccount.Id;
                    _context.CustomerDivisions.Update(customerDivision);
                    await _context.SaveChangesAsync();
                    accountId = retailAccount.Id;
                    _context.ChangeTracker.Clear();

                }

                return accountId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

    }

    public class MonthsAndYears
    {
        public int Year { get; set; }
        public long Month { get; set; }
        public double Amount { get; set; }
    }
}
