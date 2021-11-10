using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using halobiz_backend.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using HaloBiz.Adapters;
using Newtonsoft.Json;
using HaloBiz.MyServices.LAMS;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class LeadConversionServiceImplV2 : ILeadConversionService
    {
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;
        private readonly IMailAdapter _mailAdapter;
        private readonly ILogger<LeadConversionServiceImplV2> _logger;
        public long LoggedInUserId;

        private readonly string ReceivableControlAccount = "Receivable";
        private readonly string VatControlAccount = "VAT";
        
        private readonly string SALESINVOICEVOUCHER = "Sales Invoice";

        private readonly string RETAIL_RECEIVABLE_ACCOUNT = "RETAIL RECEIVABLE ACCOUNT";
        private readonly string RETAIL_VAT_ACCOUNT = "RETAIL VAT ACCOUNT";

        private readonly string RETAIL = "RETAIL";
        
        private readonly string retailLogo = "https://firebasestorage.googleapis.com/v0/b/halo-biz.appspot.com/o/LeadLogo%2FRetail.png?alt=media&token=c07dd3f9-a25e-4b4b-bf23-991a6f09ee58";
        
        private bool isRetail = false;

        private readonly List<string> groupInvoiceNumbers = new List<string>();

        public LeadConversionServiceImplV2(
                                        HalobizContext context,
                                        ILogger<LeadConversionServiceImplV2> logger,
                                        IMapper mapper,
                                        IMailAdapter mailAdapter
                                        )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _mailAdapter = mailAdapter;
        }

        public async Task<(bool, string)> ConvertLeadToClient(long leadId, long loggedInUserId)
        {
            LoggedInUserId = loggedInUserId;

            _context.ChangeTracker.Clear();


            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var lead = await _context.Leads.AsNoTracking()
                    .Where(x => x.Id == leadId)
                    .Include(x => x.LeadKeyPeople)
                    .Include(x => x.LeadDivisions)
                        .ThenInclude(x => x.Quote)
                    .Include(x => x.LeadDivisions)
                        .ThenInclude(x => x.LeadDivisionKeyPeople)
                    .Include(x => x.GroupType)
                    .FirstOrDefaultAsync();

                if (lead.LeadConversionStatus) return (false, "Lead has already been converted to client");

                Customer customer = await ConvertLeadToCustomer(lead, _context);
                foreach (var leadDivision in lead.LeadDivisions)
                {
                    CustomerDivision customerDivision = await ConvertLeadDivisionToCustomerDivision(leadDivision, customer.Id, _context);

                    var quote = await _context.Quotes
                                          .Include(x => x.QuoteServices.Where(x => !x.IsDeleted))
                                              .ThenInclude(x => x.SbutoQuoteServiceProportions)
                                          .FirstOrDefaultAsync(x => x.LeadDivisionId == leadDivision.Id);

                    Contract contract = await ConvertQuoteToContract(quote, customerDivision.Id, _context);


                    foreach (var quoteService in quote.QuoteServices)
                    {
                        await ConvertQuoteServiceToContractService(quoteService, _context, customerDivision, contract.Id, leadDivision);
                    }

                }

                // _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Accounts ON;");
                //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ControlAccounts ON;");
                 _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Divisions ON;");
                //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.FinanceVoucherTypes ON;");
                //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.GroupType ON;");


                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return (true, "Success");
            }
            catch (Exception e)
            {
                //await transaction.RollbackAsync(); //not needed. Automatic rollback happens
                _logger.LogError("Error converting lead to client");
                _logger.LogError(e.StackTrace);
                return (false, e.Message);
            }
            finally
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Accounts OFF;");
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ControlAccounts OFF;");
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Divisions OFF;");
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.FinanceVoucherTypes OFF;");
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.GroupType OFF;");
            }
        }

        private async Task<Customer> ConvertLeadToCustomer(Lead lead, HalobizContext context)
        {
            try
            {
                Customer customer;

                if (lead.GroupType.Caption.ToLower().Trim() == "individual" || lead.GroupType.Caption.ToLower().Trim() == "sme")
                {
                    isRetail = true;
                    customer = await GetRetailCustomer(lead, _context);
                }
                else
                {
                    customer = await GetOtherCustomer(lead, _context);
                }

                lead.CustomerId = customer.Id;
                lead.LeadConversionStatus = true;
                lead.TimeConvertedToClient = DateTime.Now;

                context.Leads.Update(lead);
                await context.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        private async Task<Customer> GetRetailCustomer(Lead lead, HalobizContext context)
        {
            try
            {
                var retailCustomer = await context.Customers
               .FirstOrDefaultAsync(x => x.GroupName == RETAIL);

                if (retailCustomer != null)
                {
                    return retailCustomer;
                }

                var customerEntity = await context.Customers.AddAsync(new Customer()
                {
                    GroupName = "Retail",
                    Rcnumber = "",
                    GroupTypeId = lead.GroupTypeId,
                    Industry = "Retail",
                    LogoUrl = retailLogo,
                    Email = "",
                    PhoneNumber = "",
                    CreatedById = LoggedInUserId,
                });

                await context.SaveChangesAsync();
                return customerEntity.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetRetailCustomer: {ex.StackTrace}");
                return null;
            }
        }

        private async Task<Customer> GetOtherCustomer(Lead lead, HalobizContext context)
        {
            try
            {
                var customer = await context.Customers
               .FirstOrDefaultAsync(x => x.GroupName == lead.GroupName || x.Rcnumber == lead.Rcnumber);

                if (customer != null)
                {
                    return customer;
                }

                var customerEntity = await context.Customers.AddAsync(new Customer()
                {
                    GroupName = lead.GroupName,
                    Rcnumber = lead.Rcnumber ?? "",
                    GroupTypeId = lead.GroupTypeId,
                    Industry = lead.Industry,
                    LogoUrl = lead.LogoUrl,
                    Email = "",
                    PhoneNumber = "",
                    CreatedById = LoggedInUserId,
                    PrimaryContactId = lead.PrimaryContactId,
                    SecondaryContactId = lead.SecondaryContactId
                });

                await context.SaveChangesAsync();

                foreach (var keyPerson in lead?.LeadKeyPeople)
                {
                    keyPerson.CustomerId = customerEntity.Entity.Id;
                }

                _context.LeadKeyPeople.UpdateRange(lead.LeadKeyPeople);
                await context.SaveChangesAsync();
                return customerEntity.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        private async Task<CustomerDivision> ConvertLeadDivisionToCustomerDivision(LeadDivision leadDivision, long customerId, HalobizContext context)
        {
            var customerDivision = await context.CustomerDivisions
                    .FirstOrDefaultAsync(x => x.DivisionName == leadDivision.DivisionName && x.Rcnumber == leadDivision.Rcnumber);

            if (customerDivision != null)
            {
                return customerDivision;
            }



            try
            {
                //creates customer division from lead division and saves the customer division
                var customerDivisionEntity = await context.CustomerDivisions.AddAsync(new CustomerDivision()
                {
                    Industry = leadDivision.Industry,
                    Rcnumber = leadDivision?.Rcnumber,
                    DivisionName = leadDivision.DivisionName,
                    Email = leadDivision?.Email,
                    LogoUrl = leadDivision?.LogoUrl,
                    CustomerId = customerId,
                    PhoneNumber = leadDivision?.PhoneNumber,
                    Address = leadDivision.Address,
                    State = leadDivision.State,
                    Lga = leadDivision.Lga,
                    Street = leadDivision.Street,
                    PrimaryContactId = leadDivision?.PrimaryContactId,
                    SecondaryContactId = leadDivision?.SecondaryContactId,
                    CreatedById = LoggedInUserId,
                    DTrackCustomerNumber = isRetail ? null : await GetDtrackCustomerNumber(leadDivision)
                });

                await context.SaveChangesAsync();

                foreach (var keyPerson in leadDivision?.LeadDivisionKeyPeople)
                {
                    keyPerson.CustomerDivisionId = customerDivisionEntity.Entity.Id;
                }

                _context.LeadDivisionKeyPeople.UpdateRange(leadDivision.LeadDivisionKeyPeople);
                await context.SaveChangesAsync();

                return customerDivisionEntity.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.ToString()}");
                throw;
            }

        }

        private async Task<string> GetDtrackCustomerNumber(LeadDivision leadDivision)
        {
            var dTrackCustomerNumber = $"{GenerateClientAlias(leadDivision.DivisionName)}_{await GenerateNextCustomerNumberSequence()}";
            return dTrackCustomerNumber;
        }

        private async Task<Contract> ConvertQuoteToContract(Quote quote, long customerDivisionId, HalobizContext context)
        {
            try
            {
                //Create contract from quote
                var contract = _mapper.Map<Contract>(quote);
                contract.CustomerDivisionId = customerDivisionId;

                var entity = await context.Contracts.AddAsync(contract);

                var affected = await context.SaveChangesAsync();
                return entity.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in converting", ex);
            }

            return null;
        }

        private async Task<bool> ConvertQuoteServiceToContractService(QuoteService quoteService,
                                                                        HalobizContext context,
                                                                        CustomerDivision customerDivision,
                                                                        long contractId,
                                                                        LeadDivision leadDivision)
        {
            try
            {

                //var contractServiceToSave = _mapper.Map<ContractService>(quoteService);

                var contractServiceToSave = new ContractService()
                {
                    AdminDirectTie = quoteService.AdminDirectTie,
                    BillableAmount = quoteService.BillableAmount,
                    BranchId = quoteService.BranchId,
                    ContractId = contractId,
                    ContractEndDate = quoteService.ContractEndDate,
                    CreatedAt = DateTime.Now,
                    CreatedById = quoteService.CreatedById,
                    InvoiceCycleInDays = quoteService.InvoiceCycleInDays,
                    Discount = quoteService.Discount,
                    FirstInvoiceSendDate = quoteService.FirstInvoiceSendDate,
                    FulfillmentEndDate = quoteService.FulfillmentEndDate,
                    ContractStartDate = quoteService.ContractStartDate,
                    PaymentCycle = quoteService.PaymentCycle,
                    InvoicingInterval = quoteService.InvoicingInterval,
                    QuoteServiceId = quoteService.Id,
                    UniqueTag = quoteService.UniqueTag,
                    UnitPrice = quoteService.UnitPrice,
                    Quantity = quoteService.Quantity,
                    Vat = quoteService.Vat,
                    Version = quoteService.Version,
                    ServiceId = quoteService.ServiceId,
                    ActivationDate = quoteService.ActivationDate
                };

                contractServiceToSave.ContractId = contractId;
                contractServiceToSave.Id = 0;

                var entity = await context.ContractServices.AddAsync(contractServiceToSave);
                await context.SaveChangesAsync();

                quoteService.IsConvertedToContractService = true;
                context.Update(quoteService);
                await context.SaveChangesAsync();
                var contractService = entity.Entity;

                await ConvertSBUToQuoteServicePropToSBUToContractServiceProp(quoteService.Id, contractService.Id, context);
                await ConvertQuoteServiceDocumentsToClosureDocuments(quoteService.Id, contractService.Id, context);
                await CreateTaskAndDeliverables(contractService, customerDivision.Id, "New");

                if (contractService.InvoicingInterval != (int)TimeCycle.Adhoc)
                {
                    FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                        .FirstOrDefaultAsync(x => x.VoucherType == SALESINVOICEVOUCHER);

                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Divisions ON;");


                   var (createSuccess, createMsg) =  await CreateAccounts(
                                         contractService,
                                         customerDivision,
                                         (long)leadDivision.BranchId,
                                        (long)leadDivision.OfficeId,
                                        quoteService.Service,
                                        accountVoucherType,
                                        quoteService,
                                        LoggedInUserId,
                                        false);

                 var (invoiceSuccess, invoiceMsg) =   await GenerateInvoices(contractService, customerDivision.Id, quoteService.Service.ServiceCode, LoggedInUserId);
                 var (amoSuccess, amoMsg) = await GenerateAmortizations(contractService, customerDivision);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error converting", ex);
                return false;
            }

            return true;
        }

        private async Task<bool> ConvertSBUToQuoteServicePropToSBUToContractServiceProp(long quoteServiceId, long contractServiceId, HalobizContext context)
        {
            try
            {
                var sBUToContractServiceProps = new List<SbutoContractServiceProportion>();

                var sBUToQuoteServiceProps = await _context.SbutoQuoteServiceProportions.AsNoTracking()
                        .Where(x => x.QuoteServiceId == quoteServiceId)
                        .ToListAsync();

                foreach (var sbuToQuoteServiceProp in sBUToQuoteServiceProps)
                {
                    sBUToContractServiceProps.Add(new SbutoContractServiceProportion()
                    {
                        ContractServiceId = contractServiceId,
                        Proportion = sbuToQuoteServiceProp.Proportion,
                        Status = sbuToQuoteServiceProp.Status,
                        StrategicBusinessUnitId = sbuToQuoteServiceProp.StrategicBusinessUnitId,
                        UserInvolvedId = sbuToQuoteServiceProp.UserInvolvedId,
                        CreatedById = LoggedInUserId,
                    });

                }

                await context.SbutoContractServiceProportions.AddRangeAsync(sBUToContractServiceProps);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogDebug("SbutoContractServiceProportion", ex);
            }
            return true;
        }

        private async Task<bool> ConvertQuoteServiceDocumentsToClosureDocuments(long quoteServiceId, long contractServiceId, HalobizContext context)
        {
            var closureDocuments = new List<ClosureDocument>();

            var QuoteServiceDocuments = await context.QuoteServiceDocuments.AsNoTracking()
                    .Where(x => x.QuoteServiceId == quoteServiceId)
                    .ToListAsync();

            foreach (var doc in QuoteServiceDocuments)
            {
                closureDocuments.Add(new ClosureDocument()
                {
                    ContractServiceId = contractServiceId,
                    Caption = doc.Caption,
                    Description = doc.Description,
                    DocumentUrl = doc.DocumentUrl,
                    CreatedById = LoggedInUserId,
                });
            }

            await context.ClosureDocuments.AddRangeAsync(closureDocuments);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool, string)> GenerateInvoices(ContractService contractService, long customerDivisionId, string serviceCode, long loggedInUserId)
        {
            List<Invoice> invoicesToSave = GenerateListOfInvoiceCycle(
                                                                        contractService,
                                                                        customerDivisionId,
                                                                        serviceCode,
                                                                        loggedInUserId);
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

        public List<Invoice> GenerateListOfInvoiceCycle(
                                            ContractService contractService,
                                            long customerDivisionId,
                                            string serviceCode,
                                            long loggedInUserId
                                            )
        {
            try
            {
                int interval = 0;
                int invoiceNumber = 1;
                DateTime startDate = (DateTime)contractService.ContractStartDate;
                DateTime firstInvoiceSendDate = (DateTime)contractService.FirstInvoiceSendDate;
                DateTime endDate = (DateTime)contractService.ContractEndDate;
                TimeCycle cycle = (TimeCycle)contractService.InvoicingInterval;
                double amount = (double)contractService.BillableAmount;

                List<Invoice> invoices = new List<Invoice>();

                switch (cycle)
                {
                    case TimeCycle.Weekly:
                        interval = 7;
                        break;
                    case TimeCycle.BiWeekly:
                        interval = 14;
                        break;
                    case TimeCycle.Monthly:
                        interval = 1;
                        break;
                    case TimeCycle.BiMonthly:
                        interval = 2;
                        break;
                    case TimeCycle.Quarterly:
                        interval = 4;
                        break;
                    case TimeCycle.SemiAnnually:
                        interval = 6;
                        break;
                    case TimeCycle.Annually:
                        interval = 12;
                        break;
                    case TimeCycle.BiAnnually:
                        interval = 24;
                        break;
                }

                var billableForInvoicingPeriod = CalculateTotalBillableForPeriod(contractService, false);
                var totalContractValue = CalculateTotalAmountForContract(
                                                                        (double)contractService.BillableAmount,
                                                                         (DateTime)contractService.ContractStartDate,
                                                                         (DateTime)contractService.ContractEndDate,
                                                                         (TimeCycle)contractService.InvoicingInterval
                                                                        );

                if (cycle == TimeCycle.Weekly || cycle == TimeCycle.BiWeekly)
                {

                    while (firstInvoiceSendDate < endDate)
                    {

                        //to cater for edge cases where the last invoicing cycle isn't complete
                        var invoiceValueToPost = billableForInvoicingPeriod <= totalContractValue ?
                                            billableForInvoicingPeriod : totalContractValue;

                        //to cater for edge cases where the calculated enddate false beyond the contract end date
                        var invoiceEndDateToPost = startDate.AddDays(interval) > endDate ? endDate : startDate.AddMonths(interval);

                        invoices.Add(
                            GenerateInvoice(startDate,
                                           invoiceEndDateToPost,
                                           invoiceValueToPost,
                                           firstInvoiceSendDate,
                                           contractService,
                                           customerDivisionId,
                                            serviceCode,
                                            invoiceNumber,
                                            loggedInUserId)
                            );
                        firstInvoiceSendDate = firstInvoiceSendDate.AddDays(interval);
                        startDate = startDate.AddDays(interval);
                        invoiceNumber++;
                        totalContractValue -= billableForInvoicingPeriod;
                    }
                }
                else if (cycle == TimeCycle.OneTime)
                {

                    invoices.Add(GenerateInvoice(startDate, endDate, amount, firstInvoiceSendDate,
                                                     contractService, customerDivisionId, serviceCode, invoiceNumber, loggedInUserId));

                }
                else
                {

                    while (firstInvoiceSendDate < endDate)
                    {

                        //to cater for edge cases where the last invoicing cycle isn't complete
                        var invoiceValueToPost = billableForInvoicingPeriod <= totalContractValue ?
                                            billableForInvoicingPeriod : totalContractValue;
                        //to cater for edge cases where the calculated enddate false beyond the contract end date
                        var invoiceEndDateToPost = startDate.AddMonths(interval) > endDate ? endDate : startDate.AddMonths(interval);

                        invoices.Add(GenerateInvoice(startDate,
                                                    invoiceEndDateToPost,
                                                    invoiceValueToPost,
                                                    firstInvoiceSendDate,
                                                    contractService,
                                                    customerDivisionId,
                                                    serviceCode,
                                                    invoiceNumber,
                                                    loggedInUserId));
                        firstInvoiceSendDate = firstInvoiceSendDate.AddMonths(interval);
                        startDate = startDate.AddMonths(interval);
                        invoiceNumber++;
                        totalContractValue -= billableForInvoicingPeriod;
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

                /*var unitPrice = String.IsNullOrWhiteSpace(contractService.GroupInvoiceNumber)
                                    ? contractService.UnitPrice : 0;*/

                /*var quantity = String.IsNullOrWhiteSpace(contractService.GroupInvoiceNumber) ?
                                    contractService.Quantity : 0;*/

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

        public async Task<(bool, string)> GenerateAmortizations(ContractService contractService, CustomerDivision customerDivision)
        {
            try
            {
                DateTime startDate = (DateTime)contractService.ContractStartDate;
                DateTime endDate = (DateTime)contractService.ContractEndDate;
                var InitialYear = startDate.Year;
                List<Amortization> amortizations = new List<Amortization>();

                var totalContractBillable = CalculateTotalAmountForContract((double)contractService.BillableAmount,
                                                                            (DateTime)startDate,
                                                                            (DateTime)endDate,
                                                                            (TimeCycle)contractService.InvoicingInterval);

                for (int i = startDate.Year; i <= endDate.Year; i++)
                {
                    amortizations.Add(new Amortization()
                    {
                        Year = i,
                        ClientId = customerDivision.CustomerId,
                        DivisionId = customerDivision.Id,
                        ContractId = contractService.ContractId,
                        ContractServiceId = contractService.Id,
                        ContractValue = (double)totalContractBillable,
                        GroupInvoiceNumber = string.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) ? null : contractService.Contract?.GroupInvoiceNumber,
                        January = DateTime.Parse($"{i}/01/31") > startDate && DateTime.Parse($"{i}/01/31") <= endDate ? (double)contractService.BillableAmount : 0,
                        February = DateTime.Parse($"{i}/02/28") > startDate && DateTime.Parse($"{i}/02/28") <= endDate ? (double)contractService.BillableAmount : 0,
                        March = DateTime.Parse($"{i}/03/31") > startDate && DateTime.Parse($"{i}/03/31") <= endDate ? (double)contractService.BillableAmount : 0,
                        April = DateTime.Parse($"{i}/04/30") > startDate && DateTime.Parse($"{i}/04/30") <= endDate ? (double)contractService.BillableAmount : 0,
                        May = DateTime.Parse($"{i}/05/31") > startDate && DateTime.Parse($"{i}/05/31") <= endDate ? (double)contractService.BillableAmount : 0,
                        June = DateTime.Parse($"{i}/06/30") > startDate && DateTime.Parse($"{i}/06/30") <= endDate ? (double)contractService.BillableAmount : 0,
                        July = DateTime.Parse($"{i}/07/31") > startDate && DateTime.Parse($"{i}/07/31") <= endDate ? (double)contractService.BillableAmount : 0,
                        August = DateTime.Parse($"{i}/08/31") > startDate && DateTime.Parse($"{i}/08/31") <= endDate ? (double)contractService.BillableAmount : 0,
                        September = DateTime.Parse($"{i}/09/30") > startDate && DateTime.Parse($"{i}/09/30") <= endDate ? (double)contractService.BillableAmount : 0,
                        October = DateTime.Parse($"{i}/10/31") > startDate && DateTime.Parse($"{i}/10/31") <= endDate ? (double)contractService.BillableAmount : 0,
                        November = DateTime.Parse($"{i}/11/30") > startDate && DateTime.Parse($"{i}/11/30") <= endDate ? (double)contractService.BillableAmount : 0,
                        December = DateTime.Parse($"{i}/12/31") > startDate && DateTime.Parse($"{i}/12/31") <= endDate ? (double)contractService.BillableAmount : 0,

                    });
                }

                await _context.Amortizations.AddRangeAsync(amortizations);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex);
                return (false, ex.Message);
            }

            return (true, "success");
        }

        public async Task<(bool, string)> CreateTaskAndDeliverables(ContractService contractServcie, long customerDivisionId, string endorsementType, long? loggedInUserId = null)
        {
            var createdById = loggedInUserId ?? this.LoggedInUserId;
            Service service = await _context.Services.FirstOrDefaultAsync(x => x.Id == contractServcie.ServiceId);
            if (service == null)
            {
                return (false, $"There is no such success with id {contractServcie.ServiceId}");
            }

            try
            {
                IEnumerable<ServiceCategoryTask> serviceCategoryTasks = await _context.ServiceCategoryTasks
            .Include(x => x.ServiceTaskDeliverables)
            .Where(x => x.ServiceCategoryId == service.ServiceCategoryId && x.EndorsementType.Caption == endorsementType && x.IsDeleted == false).ToListAsync();

                foreach (var serviceTask in serviceCategoryTasks)
                {
                    await CreateTaskFulfillment(
                                         serviceTask,
                                         contractServcie,
                                        customerDivisionId,
                                        service,
                                        createdById
                                        );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex);
                return (false, ex.Message);
            }

            return (true, "success");
        }

        private async Task<(bool, string)> CreateTaskFulfillment(
                                    ServiceCategoryTask serviceTask,
                                    ContractService contractServcie,
                                    long customerDivisionId,
                                    Service service,
                                    long loggedInUserId
                                    )
        {
            try
            {
                var operatingEntity = await _context.OperatingEntities
                        .Include(x => x.Division)
                        .FirstOrDefaultAsync(x => x.Id == service.OperatingEntityId);

                var task = await _context.TaskFulfillments.AddAsync(new TaskFulfillment()
                {
                    Caption = serviceTask.Caption,
                    Description = $"{serviceTask.Caption} for {service.Name}",
                    CustomerDivisionId = customerDivisionId,
                    ResponsibleId = operatingEntity.HeadId,
                    AccountableId = operatingEntity.HeadId,
                    ConsultedId = operatingEntity.Division.HeadId,
                    InformedId = operatingEntity.Division.HeadId,
                    CreatedById = loggedInUserId,
                    ContractServiceId = contractServcie.Id,
                    EndDate = contractServcie.FulfillmentEndDate,
                    StartDate = contractServcie.FulfillmentStartDate,
                    ServiceCode = service.ServiceCode,
                    Budget = contractServcie.Budget,
                    ProjectDeliveryDate = contractServcie.ActivationDate ?? contractServcie.FulfillmentEndDate,
                });

                await _context.SaveChangesAsync();

                var taskFulfilment = await _context.TaskFulfillments.AsNoTracking().
                    Where(x => x.Id == task.Entity.Id)
                    .SingleOrDefaultAsync();

                await SendNewTaskAssignedMail(taskFulfilment, contractServcie.Service.OperatingEntity.Name);

                foreach (var deliverable in serviceTask.ServiceTaskDeliverables)
                {
                    await CreateDeliverableFulfillment(task.Entity.Id, deliverable, service.ServiceCode, loggedInUserId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex.Message);
                return (false, ex.Message);
            }
            return (true,"success");
        }

        private async Task<(bool, string)> CreateDeliverableFulfillment(
                                    long taskId,
                                    ServiceTaskDeliverable serviceTaskDeliverable,
                                    string serviceCode,
                                    long loggedInUserId
                                    )
        {

            try
            {
                var deliverable = await _context.DeliverableFulfillments.AddAsync(new DeliverableFulfillment()
                {
                    Caption = serviceTaskDeliverable.Caption,
                    Description = serviceTaskDeliverable.Description,
                    TaskFullfillmentId = taskId,
                    CreatedById = loggedInUserId,
                    ServiceCode = serviceCode
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex);
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
                                    bool isReversal
                                    )
        {
            var totalContractBillable = CalculateTotalBillableForPeriod(contractService, false);
            var totalVAT = CalculateTotalBillableForPeriod(contractService, true);

            var totalAfterTax = totalContractBillable - totalVAT;
            var savedAccountMasterId = await CreateAccountMaster(service,
                                                         contractService,
                                                         accountVoucherType,
                                                         branchId,
                                                         officeId,
                                                         totalContractBillable,
                                                         customerDivision,
                                                         loggedInUserId);

            if (quoteService != null)
            {
                // this breaks the conversion.
              bool succeded =  await SaveRangeSbuaccountMaster(savedAccountMasterId, quoteService.SbutoQuoteServiceProportions);  
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
                return (false, ex.Message);
            }

            return (true,"success");
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
                if (isRetail)
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
                        Name = $"{customerDivision.DivisionName} Receivable with tag: {contractService?.UniqueTag}",
                        Description = $"Receivable Account of {customerDivision.DivisionName} with tag: {contractService?.UniqueTag}",
                        Alias = customerDivision.DTrackCustomerNumber,
                        IsDebitBalance = true,
                        ControlAccountId = controlAccount.Id,
                        CreatedById = createdById,
                        CreatedAt = DateTime.Now
                    };

                    _logger.LogInformation($"Data in account: {JsonConvert.SerializeObject(account)}");

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
                return (false, ex.Message);
            }

            return (true, "success");
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

        private async Task<long> GetRetailVATAccount(CustomerDivision customerDivision)
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
                customerDivision.VatAccountId = vatAccount.Id;
                _context.CustomerDivisions.Update(customerDivision);
                await _context.SaveChangesAsync();
                accountId = vatAccount.Id;
            }

            return accountId;

        }

        private async Task<long> GetServiceIncomeAccountForClient(CustomerDivision customerDivision, Service service)
        {
            string serviceClientIncomeAccountName = $"{service.Name} Income for {customerDivision.DivisionName}";

            Account serviceClientIncomeAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.ControlAccountId == (long)service.ControlAccountId && x.Name == serviceClientIncomeAccountName);

            long accountId = 0;
            if(serviceClientIncomeAccount == null)
            {
                Account account = new Account()
                {
                    Name = serviceClientIncomeAccountName,
                    Description = $"{service.Name} Income Account for {customerDivision.DivisionName}",
                    Alias = customerDivision.DTrackCustomerNumber,
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
                if (isRetail)
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

                    var savedAccountId = await SaveAccount(account);

                    customerDivision.VatAccountId = savedAccountId;
                    accountId = savedAccountId;

                    _context.CustomerDivisions.Update(customerDivision);
                    var affected = await _context.SaveChangesAsync();

                }

              var (isPosted, details) =  await PostAccountDetail(service,
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
                return (false, ex.Message);
            }

            return (true, "success");
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
            
            if (isRetail)
            {
                accountId = await GetServiceIncomeAccountForRetailClient(service);
            }
            else
            {
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


        private async Task<long> SaveAccount(Account account)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts ON");
                await _context.SaveChangesAsync();


                var lastSavedAccount = await _context.Accounts.Where(x => x.ControlAccountId == account.ControlAccountId)
                    .OrderBy(x => x.Id).LastOrDefaultAsync();
                if (lastSavedAccount == null || lastSavedAccount.Id < 1000000000)
                {
                    account.Id = (long)account.ControlAccountId + 1;
                }
                else
                {
                    account.Id = lastSavedAccount.Id + 1;
                }

                _logger.LogInformation($"Data account: {JsonConvert.SerializeObject(account)}");

                _context.ChangeTracker.Clear();
                var savedAccount = await _context.Accounts.AddAsync(account);
                var id = account.Id;
                await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
            finally
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts OFF");
                await _context.SaveChangesAsync();
            }

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
                string transactionId = String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) ?
                             $"TRS{service.ServiceCode}/{contractService.Id}"
                                    : $"{contractService.Contract?.GroupInvoiceNumber?.Replace("GINV", "TRS")}/{contractService.Id}";
               
                AccountMaster  accountMaster = new AccountMaster()
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
            var fvt = financeVoucherType.VoucherType.ToLower();

            var description = $"Sales of {service.Name} to {customerDivision.DivisionName} with tag '{contractService?.UniqueTag}'";

            if(fvt == "debit note")
            {
                description = $"Debit Note on {service.Name} to {customerDivision.DivisionName}";
            }
            else if(fvt == "credit note")
            {
                description = $"Credit Note on {service.Name} to {customerDivision.DivisionName}";
            }

            return description;
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
                AccountDetail accountDetail = new AccountDetail()
                {
                    Description = GetAccountMasterDescription(accountVoucherType, service, customerDivision, contractService),
                    IntegrationFlag = false,
                    VoucherId = accountVoucherType.Id,
                    TransactionId = $"{service.ServiceCode}/{contractService.Id}",
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
                return  affected > 0 ? (true, savedAccountDetails.Entity) : (false, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex.Message);
                return (false, null);
            }
        }


        private string GenerateClientAlias(string divisionName)
        {
            string[] names = divisionName.Trim().Split(" ");
            string initial = "";
            foreach (var name in names)
            {
                initial += name.Substring(0, 1).ToUpper();
            }
            return initial;
        }

        private async Task<string> GenerateNextCustomerNumberSequence()
        {
            var sequenceTracker = await _context.SequenceTrackers.SingleOrDefaultAsync();
            if(sequenceTracker == null)
            {
                sequenceTracker = new SequenceTracker { CustomerNumber = 1 };
                await _context.SequenceTrackers.AddAsync(sequenceTracker);
                await _context.SaveChangesAsync();
                return sequenceTracker.CustomerNumber.ToString().PadLeft(3, '0');
            }
            else
            {
                long nextSequence = (sequenceTracker.CustomerNumber + 1) > 999 ? 1 : (sequenceTracker.CustomerNumber + 1);
                sequenceTracker.CustomerNumber = nextSequence;
                _context.SequenceTrackers.Update(sequenceTracker);
                await _context.SaveChangesAsync();
                return sequenceTracker.CustomerNumber.ToString().PadLeft(3, '0');
            }
        }

        private double CalculateTotalAmountForContract(double priceOfService, DateTime contractStartDate, DateTime contractEndDate, TimeCycle timeCycle)
        {
            int numberOfMonth = 0;

            if (timeCycle == TimeCycle.OneTime)
            {
                return priceOfService;
            }

            while (contractStartDate <= contractEndDate)
            {
                numberOfMonth++;
                contractStartDate = contractStartDate.AddMonths(1);
            }

            return priceOfService * numberOfMonth;
        }

        private async Task SendNewTaskAssignedMail(TaskFulfillment taskFulfillment, string operatingEntityName)
        {
            taskFulfillment.Responsible = await _context.UserProfiles.AsNoTracking().Where(x => x.Id == taskFulfillment.ResponsibleId).SingleOrDefaultAsync();
            taskFulfillment.CustomerDivision = await _context.CustomerDivisions.AsNoTracking().Where(x => x.Id == taskFulfillment.CustomerDivisionId).SingleOrDefaultAsync();

            var serializedTask = JsonConvert.SerializeObject(taskFulfillment, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Action action = async () =>
            {
                await _mailAdapter.SendNewTaskAssigned(serializedTask, operatingEntityName);
            };
            action.RunAsTask();
        }

        private static double GenerateWeeklyAmount(DateTime startDate, DateTime endDate, double amountPerMonth, TimeCycle timeCylce)
        {
            var interval = timeCylce == TimeCycle.Weekly ? 7 : 14;
            var numberOfMonths = 0;
            var daysBetweenStartAndEndDate = (endDate.Subtract(startDate).TotalDays + 1) < interval ?
                           (int)interval : (endDate.Subtract(startDate).TotalDays + 1);
            var numberOfPayment = Math.Floor(daysBetweenStartAndEndDate / interval);
            startDate = startDate.AddMonths(1);

            while (startDate < endDate)
            {
                numberOfMonths++;
                startDate = startDate.AddMonths(1);
            }
            numberOfMonths = numberOfMonths == 0 ? 1 : numberOfMonths;
            var totalAmountPayable = numberOfMonths * amountPerMonth;
            var amountToPay = Math.Round(totalAmountPayable / numberOfPayment, 4);
            return amountToPay;
        }

        private double CalculateTotalBillableForPeriod(ContractService contractService, bool isVAT)
        {
            int interval = 0;
            DateTime startDate = (DateTime)contractService.ContractStartDate;
            DateTime endDate = (DateTime)contractService.ContractEndDate;
            TimeCycle cycle = (TimeCycle)contractService.InvoicingInterval;
            double amount = isVAT ? (double)contractService.Vat : (double)contractService.BillableAmount;

            switch (cycle)
            {
                case TimeCycle.Weekly:
                    interval = 7;
                    break;
                case TimeCycle.BiWeekly:
                    interval = 14;
                    break;
                case TimeCycle.Monthly:
                    interval = 1;
                    break;
                case TimeCycle.BiMonthly:
                    interval = 2;
                    break;
                case TimeCycle.Quarterly:
                    interval = 4;
                    break;
                case TimeCycle.SemiAnnually:
                    interval = 6;
                    break;
                case TimeCycle.Annually:
                    interval = 12;
                    break;
                case TimeCycle.BiAnnually:
                    interval = 24;
                    break;
            }

            if (cycle == TimeCycle.Weekly || cycle == TimeCycle.BiWeekly)
            {
                return GenerateWeeklyAmount(startDate, endDate, amount, cycle);

            }
            else if (cycle == TimeCycle.OneTime)
            {
                return amount;
            }
            else
            {
                return amount * (double)interval;
            }
        }
    }
}