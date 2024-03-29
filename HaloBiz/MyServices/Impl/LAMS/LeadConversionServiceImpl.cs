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
    public class LeadConversionServiceImpl /*: ILeadConversionService*/
    {
        //private readonly HalobizContext _context;
        //private readonly IMapper _mapper;
        //private readonly IMailAdapter _mailAdapter;
        //private readonly ILogger<LeadConversionServiceImpl> _logger;
        //public long LoggedInUserId;
        //private readonly string ReceivableControlAccount = "Receivable";
        //private readonly string SALESINVOICEVOUCHER = "Sales Invoice";
        //private readonly string VALUEADDEDTAX = "VALUE ADDED TAX";
        //private readonly string RETAIL_INCOME_ACCOUNT = "RETAIL INCOME ACCOUNT";
        //private readonly string RETAIL = "RETAIL";
        //private readonly string retailLogo = "https://firebasestorage.googleapis.com/v0/b/halo-biz.appspot.com/o/LeadLogo%2FRetail.png?alt=media&token=c07dd3f9-a25e-4b4b-bf23-991a6f09ee58";
        //private bool isRetail = false;
        
        //private readonly List<string> groupInvoiceNumbers = new List<string>();

        //public LeadConversionServiceImpl(
        //                                HalobizContext context, 
        //                                ILogger<LeadConversionServiceImpl> logger,
        //                                IMapper mapper,
        //                                IMailAdapter mailAdapter
        //                                )
        //{
        //    this._context = context;
        //    this._logger = logger;
        //    this._mapper = mapper;
        //    _mailAdapter = mailAdapter;
        //}
        //public async Task<bool> ConvertLeadToClient(long leadId, long loggedInUserId)
        //{
        //    this.LoggedInUserId = loggedInUserId;
        //    using(var transaction =  await _context.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            var lead = await _context.Leads
        //                .Include(x => x.LeadKeyPeople)
        //                .Include(x => x.LeadDivisions)
        //                    .ThenInclude(x => x.Quote)
        //                .Include(x => x.LeadDivisions)
        //                    .ThenInclude(x => x.LeadDivisionKeyPeople)
        //                .Include(x => x.GroupType)
        //                .FirstOrDefaultAsync(x => x.Id == leadId);

        //            if (lead.LeadConversionStatus) return false;
                    
        //            Customer customer = await ConvertLeadToCustomer(lead, _context);
        //            foreach (var leadDivision in lead.LeadDivisions)
        //            {
        //              CustomerDivision customerDivision = await  ConvertLeadDivisionToCustomerDivision(leadDivision, customer.Id, _context);

        //              var quote = await _context.Quotes
        //                                    .Include(x => x.QuoteServices.Where(x => !x.IsDeleted))
        //                                        .ThenInclude(x => x.SbutoQuoteServiceProportions)
        //                                    .FirstOrDefaultAsync(x => x.LeadDivisionId == leadDivision.Id);

        //              Contract contract = await ConvertQuoteToContract( quote, customerDivision.Id,  _context);


        //              foreach (var quoteService in quote.QuoteServices)
        //              {
        //                  await ConvertQuoteServiceToContractService( quoteService,  _context, customerDivision, contract.Id, leadDivision);
        //              }

        //            }

        //            await _context.SaveChangesAsync();

        //            foreach (var groupInvoiceNumber in groupInvoiceNumbers)
        //            {
        //                await GenerateAndSaveGroupInvoiceDetails(groupInvoiceNumber);
        //            }
                    
        //            await transaction.CommitAsync();
        //            return true;
        //        }
        //        catch (System.Exception e)
        //        {
        //            await transaction.RollbackAsync();
        //            _logger.LogError(e.Message);
        //            _logger.LogError(e.StackTrace);
        //            return false;
        //        }

        //    }

        //}



        //private async Task<Customer> ConvertLeadToCustomer(Lead lead, HalobizContext context)
        //{
        //    Customer customer = null;

        //    if(lead.GroupType.Caption.ToLower().Trim() == "individual" || lead.GroupType.Caption.ToLower().Trim() == "sme")
        //    {
        //        this.isRetail = true;
        //        customer = await GetRetailCustomer( lead,  _context);
        //    }else{
        //        customer = await GetOtherCustomer( lead,  _context);
        //    }

        //    lead.CustomerId = customer.Id;
        //    lead.LeadConversionStatus =true;
        //    lead.TimeConvertedToClient =(DateTime) DateTime.Now;
            
        //    context.Leads.Update(lead);
        //    await context.SaveChangesAsync();
        //    return customer;
 
        //}

        //private async Task<Customer> GetRetailCustomer(Lead lead, HalobizContext context)
        //{
        //    var retailCustomer = await context.Customers
        //        .FirstOrDefaultAsync(x => x.GroupName == this.RETAIL);

        //    if(retailCustomer != null)
        //    {
        //        return retailCustomer;
        //    }

        //    var customerEntity = await context.Customers.AddAsync( new Customer(){
        //        GroupName = "Retail",
        //        Rcnumber = "",
        //        GroupTypeId = lead.GroupTypeId,
        //        Industry = "Retail",
        //        LogoUrl = this.retailLogo,
        //        Email = "",
        //        PhoneNumber = "",
        //        CreatedById = this.LoggedInUserId,
        //    });
        //    await context.SaveChangesAsync();
        //    return customerEntity.Entity;
        //}
        //private async Task<Customer> GetOtherCustomer(Lead lead, HalobizContext context)
        //{
        //    var retailCustomer = await context.Customers
        //        .FirstOrDefaultAsync(x => x.GroupName == lead.GroupName || x.Rcnumber == lead.Rcnumber);

        //    if(retailCustomer != null)
        //    {
        //        return retailCustomer;
        //    }

        //    var customerEntity = await context.Customers.AddAsync( new Customer(){
        //        GroupName = lead.GroupName,
        //        Rcnumber = lead.Rcnumber,
        //        GroupTypeId = lead.GroupTypeId,
        //        Industry = lead.Industry,
        //        LogoUrl = lead.LogoUrl,
        //        Email = "",
        //        PhoneNumber = "",
        //        CreatedById = this.LoggedInUserId,
        //        PrimaryContactId = lead.PrimaryContactId,
        //        SecondaryContactId = lead.SecondaryContactId
        //    });
        //    await context.SaveChangesAsync();

        //    foreach (var keyPerson in lead.LeadKeyPeople)
        //    {
        //        keyPerson.CustomerId = customerEntity.Entity.Id;
        //    }

        //    _context.LeadKeyPeople.UpdateRange(lead.LeadKeyPeople);
        //    await context.SaveChangesAsync();
        //    return customerEntity.Entity;
        //}


        //private async Task<CustomerDivision> ConvertLeadDivisionToCustomerDivision(LeadDivision leadDivision, long customerId, HalobizContext context)
        //{
        //    var customerDivision = await context.CustomerDivisions
        //            .FirstOrDefaultAsync(x => x.DivisionName == leadDivision.DivisionName && x.Rcnumber == leadDivision.Rcnumber);
            
        //    if(customerDivision != null)
        //    {
        //        return customerDivision;
        //    }
        //    //creates customer division from lead division and saves the customer division
        //    var customerDivisionEntity =  await context.CustomerDivisions.AddAsync(new CustomerDivision(){
        //        Industry = leadDivision.Industry,
        //        Rcnumber = leadDivision.Rcnumber,
        //        DivisionName = leadDivision.DivisionName,
        //        Email = leadDivision.Email,
        //        LogoUrl = leadDivision.LogoUrl,
        //        CustomerId = customerId,
        //        PhoneNumber = leadDivision.PhoneNumber,
        //        Address = leadDivision.Address,
        //        State = leadDivision.State,
        //        Lga = leadDivision.Lga,
        //        Street = leadDivision.Street,
        //        PrimaryContactId = leadDivision.PrimaryContactId,
        //        SecondaryContactId = leadDivision.SecondaryContactId,
        //        CreatedById = this.LoggedInUserId,
        //    });

        //    await context.SaveChangesAsync();

        //    foreach (var keyPerson in leadDivision.LeadDivisionKeyPeople)
        //    {
        //        keyPerson.CustomerDivisionId = customerDivisionEntity.Entity.Id;
        //    }

        //    _context.LeadDivisionKeyPeople.UpdateRange(leadDivision.LeadDivisionKeyPeople);
        //    await context.SaveChangesAsync();
            
        //    return customerDivisionEntity.Entity;
            
        //}

        //private async Task<Contract> ConvertQuoteToContract(Quote quote, long customerDivisionId, HalobizContext context)
        //{

        //    //todo: Create contract from quote
        //    try
        //    {
        //        var entity = await context.Contracts.AddAsync(new Contract()
        //        {
        //            // ReferenceNo = quote.ReferenceNo,
        //            CustomerDivisionId = customerDivisionId,
        //            QuoteId = quote.Id,
        //            CreatedById = this.LoggedInUserId,
        //        });

        //        await context.SaveChangesAsync();
        //        return entity.Entity;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error converting quote to contract", ex);
        //        throw;
        //    }
                
        //}

        //private async Task<bool> ConvertQuoteServiceToContractService(QuoteService quoteService, 
        //                                                                HalobizContext context, 
        //                                                                CustomerDivision customerDivision, 
        //                                                                long contractId, 
        //                                                                LeadDivision leadDivision)
        //{
        //    var contractServiceToSave = new ContractService()
        //    {
        //        UnitPrice = quoteService.UnitPrice,
        //        Quantity = quoteService.Quantity,
        //        Discount = quoteService.Discount,
        //        Vat = quoteService.Vat,
        //        BillableAmount = quoteService.BillableAmount,
        //        Budget = quoteService.Budget,
        //        ContractStartDate = quoteService.ContractStartDate,
        //        ContractEndDate = quoteService.ContractEndDate,
        //        PaymentCycle = quoteService.PaymentCycle,
        //        PaymentCycleInDays = quoteService.PaymentCycleInDays,
        //        FirstInvoiceSendDate = quoteService.FirstInvoiceSendDate,
        //        InvoicingInterval = quoteService.InvoicingInterval,
        //        ProblemStatement = quoteService.ProblemStatement,
        //        ActivationDate = quoteService.ActivationDate,
        //        FulfillmentStartDate = quoteService.FulfillmentStartDate,
        //        FulfillmentEndDate = quoteService.FulfillmentEndDate,
        //        TentativeDateForSiteSurvey = quoteService.TentativeDateForSiteSurvey,
        //        PickupDateTime = quoteService.PickupDateTime,
        //        DropoffDateTime = quoteService.DropoffDateTime,
        //        PickupLocation = quoteService.PickupLocation,
        //        Dropofflocation = quoteService.Dropofflocation,
        //        BeneficiaryName = quoteService.BeneficiaryName,
        //        BeneficiaryIdentificationType = quoteService.BeneficiaryIdentificationType,
        //        BenificiaryIdentificationNumber = quoteService.BenificiaryIdentificationNumber,
        //        TentativeProofOfConceptStartDate = quoteService.TentativeProofOfConceptStartDate,
        //        TentativeProofOfConceptEndDate = quoteService.TentativeProofOfConceptEndDate,
        //        TentativeFulfillmentDate = quoteService.TentativeFulfillmentDate,
        //        ProgramCommencementDate = quoteService.ProgramCommencementDate,
        //        ProgramDuration = quoteService.ProgramDuration,
        //        ProgramEndDate = quoteService.ProgramEndDate,
        //        TentativeDateOfSiteVisit = quoteService.TentativeDateOfSiteVisit,
        //        QuoteServiceId = quoteService.Id,
        //        ContractId = contractId,
        //        CreatedById = this.LoggedInUserId,
        //        ServiceId = quoteService.ServiceId,
        //        OfficeId = leadDivision.OfficeId,
        //        BranchId = quoteService.BranchId,
        //        AdminDirectTie = quoteService.AdminDirectTie,
        //        UniqueTag = quoteService.UniqueTag
        //    };

        //    var entity = await context.ContractServices.AddAsync(contractServiceToSave);
        //    await context.SaveChangesAsync();
        //    quoteService.IsConvertedToContractService = true;
        //    context.Update(quoteService);
        //    await context.SaveChangesAsync();
        //    var contractService = entity.Entity;

        //    await ConvertSBUToQuoteServicePropToSBUToContractServiceProp( quoteService.Id, contractService.Id, context);
        //    await ConvertQuoteServiceDocumentsToClosureDocuments(quoteService.Id, contractService.Id, context);
        //    await CreateTaskAndDeliverables(contractService, customerDivision.Id, "New");
            
        //    if(contractService.InvoicingInterval != (int)TimeCycle.Adhoc)
        //    { 

        //        //Check if the contract service is part of a part of the bulk invoice and if also the invoice has been processed
        //        //if it has been processed, it skips it
        //        if(!String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) && groupInvoiceNumbers.Contains(contractService.Contract?.GroupInvoiceNumber))
        //        {
        //            return true;
        //        }
        //        //If the invoice is bulk and has not been processed, it sends it for processing.
        //        if(!String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) &&  !groupInvoiceNumbers.Contains(contractService.Contract?.GroupInvoiceNumber))
        //        {
        //            //returns an accumulation of all the contractService in the bulk invoice and 
        //            //invoices at once
        //            contractService = await GenerateBulkContractService(contractService);
        //        }

        //        FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
        //            .FirstOrDefaultAsync(x => x.VoucherType == this.SALESINVOICEVOUCHER);

                  
        //        await CreateAccounts(
        //                             contractService,
        //                             customerDivision,
        //                             (long) leadDivision.BranchId,
        //                            (long) leadDivision.OfficeId,
        //                            quoteService.Service,
        //                            accountVoucherType,
        //                            quoteService,
        //                            this.LoggedInUserId,
        //                            false
        //                            );
        //        await GenerateInvoices( contractService,  customerDivision.Id, quoteService.Service.ServiceCode, this.LoggedInUserId);
        //        await GenerateAmortizations( contractService,  customerDivision);
        //    }
        //    return true;

        //}

        //public async Task<ContractService> GenerateBulkContractService(ContractService contractService)
        //{
        //    //todo refactor
        //    double totalVAT = 0.0, totalBillable = 0.0;

        //    try
        //    {
        //        var groupContractService = await _context.QuoteServices
        //       .Include(x => x.Quote)
        //       .Where(x => x.Quote.GroupInvoiceNumber == contractService.Contract.GroupInvoiceNumber && !x.IsDeleted)
        //           .ToListAsync();
        //        foreach (var quoteService in groupContractService)
        //        {
        //            totalBillable += (double)quoteService.BillableAmount;
        //            totalVAT += (double)quoteService.Vat;
        //        }

        //        this.groupInvoiceNumbers.Add(contractService.Contract?.GroupInvoiceNumber);
        //        var newContractService = _mapper.Map<ContractService>(contractService);
        //        newContractService.BillableAmount = totalBillable;
        //        newContractService.Vat = totalVAT;

        //        return newContractService;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error converting lead to client", ex);
        //        return null;
        //    }
            
        //}

        //public async Task<bool> GenerateAndSaveGroupInvoiceDetails(string groupInvoiceNumber)
        //{
        //    var contractServices = await _context.ContractServices.Where(x => x.Contract.GroupInvoiceNumber == groupInvoiceNumber && !x.IsDeleted).ToListAsync();
        //    var groupInvoiceDetails = new List<GroupInvoiceDetail>();
        //    foreach (var contractService in contractServices)
        //    {
        //        groupInvoiceDetails.Add(
        //            new GroupInvoiceDetail()
        //        {
        //            InvoiceNumber = contractService.Contract?.GroupInvoiceNumber,
        //            Description = $"Invoice details for Group Invoice {contractService.Contract?.GroupInvoiceNumber}",
        //            UnitPrice = (double) contractService.UnitPrice,
        //            Quantity =(int) contractService.Quantity,
        //            Vat  = (double) contractService.Vat,
        //            Value = (double) (contractService.BillableAmount - contractService.Vat),
        //            BillableAmount = (double) contractService.BillableAmount,
        //            ContractServiceId = contractService.Id
        //        }
        //        );
        //    }

        //    await _context.GroupInvoiceDetails.AddRangeAsync(groupInvoiceDetails);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}


        //private async Task<bool> ConvertSBUToQuoteServicePropToSBUToContractServiceProp(long quoteServiceId, long contractServiceId, HalobizContext context)
        //{
        //    var sBUToContractServiceProps = new List<SbutoContractServiceProportion>();

        //    var sBUToQuoteServiceProps = await _context.SbutoQuoteServiceProportions
        //            .Where(x => x.QuoteServiceId == quoteServiceId)
        //            .ToListAsync();
            
        //    foreach(var sbuToQuoteServiceProp in sBUToQuoteServiceProps)
        //    {
        //        sBUToContractServiceProps.Add(new SbutoContractServiceProportion()
        //        {
        //            ContractServiceId = contractServiceId,
        //            Proportion = sbuToQuoteServiceProp.Proportion,
        //            Status = sbuToQuoteServiceProp.Status,
        //            StrategicBusinessUnitId = sbuToQuoteServiceProp.StrategicBusinessUnitId,
        //            UserInvolvedId = sbuToQuoteServiceProp.UserInvolvedId,
        //            CreatedById = this.LoggedInUserId,
        //        });

        //    }

        //    await context.SbutoContractServiceProportions.AddRangeAsync(sBUToContractServiceProps);
        //    await context.SaveChangesAsync();
        //    return true;
            
        //}

        //private async Task<bool> ConvertQuoteServiceDocumentsToClosureDocuments(long quoteServiceId, long contractServiceId, HalobizContext context)
        //{
        //    var closureDocuments = new List<ClosureDocument>();

        //    var QuoteServiceDocuments = await context.QuoteServiceDocuments
        //            .Where(x => x.QuoteServiceId == quoteServiceId)
        //            .ToListAsync();
            
        //    foreach(var doc in QuoteServiceDocuments)
        //    {
        //        closureDocuments.Add(new ClosureDocument()
        //        {
        //            ContractServiceId = contractServiceId,
        //            Caption = doc.Caption,
        //            Description = doc.Description,
        //            DocumentUrl = doc.DocumentUrl,
        //            CreatedById = this.LoggedInUserId,
        //        });
        //    }

        //    await context.ClosureDocuments.AddRangeAsync(closureDocuments);
        //    await context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> GenerateInvoices(ContractService contractService, long customerDivisionId, string serviceCode, long loggedInUserId)
        //{
        //    List<Invoice> invoicesToSave = GenerateListOfInvoiceCycle(
        //                                                                contractService, 
        //                                                                customerDivisionId,
        //                                                                serviceCode,
        //                                                                loggedInUserId);

        //    await _context.Invoices.AddRangeAsync(invoicesToSave);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public List<Invoice> GenerateListOfInvoiceCycle(
        //                                    ContractService contractService, 
        //                                    long customerDivisionId,
        //                                    string serviceCode,
        //                                    long loggedInUserId
        //                                    )
        //{
        //    int interval = 0;
        //    int invoiceNumber = 1;
        //    DateTime startDate =(DateTime) contractService.ContractStartDate; 
        //    DateTime firstInvoiceSendDate =(DateTime) contractService.FirstInvoiceSendDate; 
        //    DateTime endDate =(DateTime) contractService.ContractEndDate;
        //    TimeCycle cycle =(TimeCycle) contractService.InvoicingInterval;
        //    double amount =(double) contractService.BillableAmount;

        //    List<Invoice> invoices = new  List<Invoice>();

        //    switch (cycle)
        //    {
        //        case TimeCycle.Weekly:
        //            interval = 7;
        //            break;
        //        case TimeCycle.BiWeekly:
        //            interval = 14;
        //            break;
        //        case TimeCycle.Monthly:
        //            interval = 1;
        //            break;
        //        case TimeCycle.BiMonthly:
        //            interval = 2;
        //            break;
        //        case TimeCycle.Quarterly:
        //            interval = 4;
        //            break;
        //        case TimeCycle.SemiAnnually:
        //            interval = 6;
        //            break;
        //        case TimeCycle.Annually:
        //            interval = 12;
        //            break;
        //        case TimeCycle.BiAnnually:
        //            interval = 24;
        //            break;
        //    }

        //    var billableForInvoicingPeriod = CalculateTotalBillableForPeriod( contractService, false);
        //    var totalContractValue = CalculateTotalAmountForContract(
        //                                                            (double)contractService.BillableAmount,
        //                                                             (DateTime)contractService.ContractStartDate,
        //                                                             (DateTime)contractService.ContractEndDate,
        //                                                             (TimeCycle)contractService.InvoicingInterval
        //                                                            );

        //    if(cycle == TimeCycle.Weekly || cycle == TimeCycle.BiWeekly)
        //    {

        //        while(firstInvoiceSendDate < endDate){
                    
        //            //to cater for edge cases where the last invoicing cycle isn't complete
        //            var invoiceValueToPost = billableForInvoicingPeriod <= totalContractValue ?
        //                                billableForInvoicingPeriod : totalContractValue;

        //            //to cater for edge cases where the calculated enddate false beyond the contract end date
        //            var invoiceEndDateToPost = startDate.AddDays(interval) > endDate ? endDate : startDate.AddMonths(interval);

        //                 invoices.Add(
        //                     GenerateInvoice(startDate,  
        //                                    invoiceEndDateToPost, 
        //                                    invoiceValueToPost, 
        //                                    firstInvoiceSendDate,
        //                                    contractService, 
        //                                    customerDivisionId,
        //                                     serviceCode,
        //                                     invoiceNumber,
        //                                     loggedInUserId)
        //                     );
        //            firstInvoiceSendDate = firstInvoiceSendDate.AddDays(interval);
        //            startDate = startDate.AddDays(interval);
        //            invoiceNumber++;
        //            totalContractValue -= billableForInvoicingPeriod;
        //        }
        //    }else if(cycle == TimeCycle.OneTime ){
                
        //            invoices.Add(GenerateInvoice(startDate,  endDate, amount , firstInvoiceSendDate,
        //                                             contractService, customerDivisionId, serviceCode, invoiceNumber, loggedInUserId));

        //    }else{
                
        //        while(firstInvoiceSendDate < endDate){

        //            //to cater for edge cases where the last invoicing cycle isn't complete
        //            var invoiceValueToPost = billableForInvoicingPeriod <= totalContractValue ?
        //                                billableForInvoicingPeriod : totalContractValue;
        //            //to cater for edge cases where the calculated enddate false beyond the contract end date
        //            var invoiceEndDateToPost = startDate.AddMonths(interval) > endDate ? endDate : startDate.AddMonths(interval);

        //            invoices.Add(GenerateInvoice(startDate,  
        //                                        invoiceEndDateToPost, 
        //                                        invoiceValueToPost , 
        //                                        firstInvoiceSendDate, 
        //                                        contractService, 
        //                                        customerDivisionId,
        //                                        serviceCode,
        //                                        invoiceNumber,
        //                                        loggedInUserId));
        //            firstInvoiceSendDate = firstInvoiceSendDate.AddMonths(interval);
        //            startDate = startDate.AddMonths(interval);
        //            invoiceNumber++;
        //            totalContractValue -= billableForInvoicingPeriod;
        //        }
        //    }
        //    return invoices;
        //}

        //private Invoice GenerateInvoice(
        //                        DateTime from, DateTime to, 
        //                        double amount, DateTime sendDate,
        //                        ContractService contractService, 
        //                        long customerDivisionId,
        //                        string serviceCode,
        //                        int invoiceIndex,
        //                        long loggedInUserId)
        //{
        //    string invoiceNumber = String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) ?  
        //                    $"INV{contractService.Id.ToString().PadLeft(8, '0')}"
        //                            : contractService.Contract?.GroupInvoiceNumber ;
        //    string transactionId = String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) ?  
        //                     $"TRS{serviceCode}/{contractService.Id}"
        //                            :  $"{contractService.Contract?.GroupInvoiceNumber?.Replace("GINV", "TRS")}/{contractService.Id}" ;

        //    var unitPrice = String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) 
        //                        ? contractService.UnitPrice : 0;

        //    var quantity = String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) ?
        //                        contractService.Quantity : 0;

        //    return new Invoice(){
        //            InvoiceNumber = $"{invoiceNumber}/{invoiceIndex}",
        //            UnitPrice =   (double) unitPrice,
        //            Quantity = quantity,
        //            Discount = contractService.Discount,
        //            Value  = amount,
        //            TransactionId = transactionId,
        //            DateToBeSent = sendDate,
        //            IsInvoiceSent = false,
        //            CustomerDivisionId = customerDivisionId,
        //            ContractId = contractService.ContractId,
        //            ContractServiceId = contractService.Id,
        //            StartDate  = from,
        //            EndDate  = to,
        //            IsReceiptedStatus = (int)InvoiceStatus.NotReceipted,
        //            IsFinalInvoice = true,
        //            InvoiceType = (int)InvoiceType.New,
        //            CreatedById =  loggedInUserId,
        //            GroupInvoiceNumber = String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber)? null :  invoiceNumber,
        //            //First Accounts for first invoices are posted when a lead is converted to client
        //            IsAccountPosted = invoiceIndex == 1
        //    };
        //}

        //public async  Task<bool> GenerateAmortizations(ContractService contractService, CustomerDivision customerDivision)
        //{
        //    DateTime startDate = (DateTime) contractService.ContractStartDate;
        //    DateTime endDate = (DateTime) contractService.ContractEndDate;
        //    var InitialYear = startDate.Year;
        //    List<Amortization> amortizations = new List<Amortization>();

        //    var totalContractBillable = CalculateTotalAmountForContract((double)contractService.BillableAmount, 
        //                                                                (DateTime)startDate, 
        //                                                                (DateTime) endDate, 
        //                                                                (TimeCycle) contractService.InvoicingInterval);

        //    for(int i = startDate.Year; i <= endDate.Year; i++)
        //    {
        //        amortizations.Add(new Amortization(){
        //            Year = i,
        //            ClientId = customerDivision.CustomerId,
        //            DivisionId = customerDivision.Id,
        //            ContractId = contractService.ContractId,
        //            ContractServiceId = contractService.Id,
        //            ContractValue = (double) totalContractBillable,
        //            January = DateTime.Parse($"{i}/01/31") > startDate &&  DateTime.Parse($"{i}/01/31") <= endDate ? (double)contractService.BillableAmount : 0,
        //            February = DateTime.Parse($"{i}/02/28") > startDate && DateTime.Parse($"{i}/02/28") <= endDate ? (double)contractService.BillableAmount : 0,
        //            March =  DateTime.Parse($"{i}/03/31") > startDate && DateTime.Parse($"{i}/03/31") <= endDate ? (double)contractService.BillableAmount : 0,
        //            April = DateTime.Parse($"{i}/04/30") > startDate && DateTime.Parse($"{i}/04/30") <= endDate ? (double)contractService.BillableAmount : 0,
        //            May = DateTime.Parse($"{i}/05/31") > startDate && DateTime.Parse($"{i}/05/31") <= endDate ? (double)contractService.BillableAmount : 0,
        //            June = DateTime.Parse($"{i}/06/30") > startDate && DateTime.Parse($"{i}/06/30") <= endDate ? (double)contractService.BillableAmount : 0,
        //            July = DateTime.Parse($"{i}/07/31") > startDate && DateTime.Parse($"{i}/07/31") <= endDate ? (double)contractService.BillableAmount : 0,
        //            August = DateTime.Parse($"{i}/08/31") > startDate && DateTime.Parse($"{i}/08/31") <= endDate ? (double)contractService.BillableAmount : 0,
        //            September = DateTime.Parse($"{i}/09/30") > startDate && DateTime.Parse($"{i}/09/30") <= endDate ? (double)contractService.BillableAmount : 0,
        //            October = DateTime.Parse($"{i}/10/31") > startDate && DateTime.Parse($"{i}/10/31") <= endDate ?  (double)contractService.BillableAmount : 0,
        //            November = DateTime.Parse($"{i}/11/30") > startDate && DateTime.Parse($"{i}/11/30") <= endDate ? (double)contractService.BillableAmount : 0,
        //            December = DateTime.Parse($"{i}/12/31") > startDate && DateTime.Parse($"{i}/12/31") <= endDate ? (double)contractService.BillableAmount : 0,
                    
        //        });
        //    }

        //   await  _context.Amortizations.AddRangeAsync(amortizations);
        //   await _context.SaveChangesAsync();
        //    return true;
        //}


        //private static double GenerateAmount(DateTime startDate, DateTime endDate, double amountPerMonth, TimeCycle timeCylce)
        //{
        //    var interval = timeCylce == TimeCycle.Weekly ? 7 : 14;
        //    var numberOfMonths = 0;
        //    var daysBetweenStartAndEndDate = (endDate.Subtract(startDate).TotalDays + 1) < interval ?
        //                   (int) interval : (endDate.Subtract(startDate).TotalDays + 1);
        //    var numberOfPayment = Math.Floor(daysBetweenStartAndEndDate / interval);
        //    startDate = startDate.AddMonths(1);

        //    while(startDate < endDate)
        //    {
        //        numberOfMonths++;
        //        startDate = startDate.AddMonths(1);
        //    }
        //    numberOfMonths = numberOfMonths == 0 ? 1 : numberOfMonths;
        //    var totalAmountPayable = numberOfMonths * amountPerMonth;
        //    var amountToPay = Math.Round(totalAmountPayable / numberOfPayment, 4);
        //    return amountToPay;
        //}


        //public async Task<bool> CreateTaskAndDeliverables(ContractService contractServcie, long customerDivisionId, string endorsementType, long? loggedInUserId = null)
        //{
        //    var createdById =  loggedInUserId?? this.LoggedInUserId; 
        //    Service service = await _context.Services.FirstOrDefaultAsync(x => x.Id == contractServcie.ServiceId); 
        //    if(service == null)
        //    {
        //        return false;
        //    }

        //    IEnumerable<ServiceCategoryTask> serviceCategoryTasks = await _context.ServiceCategoryTasks
        //    .Include(x => x.ServiceTaskDeliverables)
        //    .Where(x => x.ServiceCategoryId == service.ServiceCategoryId && x.EndorsementType.Caption == endorsementType && x.IsDeleted == false).ToListAsync();
            
        //    foreach (var serviceTask in serviceCategoryTasks)
        //    {
        //        await CreateTaskFulfillment(
        //                             serviceTask, 
        //                             contractServcie,  
        //                            customerDivisionId,
        //                            service,
        //                            createdById
        //                            );
        //    }
        //    return true;
        //}

        //private async Task<bool> CreateTaskFulfillment(
        //                            ServiceCategoryTask serviceTask, 
        //                            ContractService contractServcie,  
        //                            long customerDivisionId,
        //                            Service service,
        //                            long loggedInUserId
        //                            )
        //{
        //    var operatingEntity = await _context.OperatingEntities
        //                .Include(x => x.Division)
        //                .FirstOrDefaultAsync(x => x.Id == service.OperatingEntityId);
             
        //    var task = await  _context.TaskFulfillments.AddAsync(new TaskFulfillment(){
        //        Caption = serviceTask.Caption,
        //        Description = $"{serviceTask.Caption} for {service.Name}",
        //        CustomerDivisionId = customerDivisionId,
        //        ResponsibleId = operatingEntity.HeadId,
        //        AccountableId = operatingEntity.HeadId,
        //        ConsultedId = operatingEntity.Division.HeadId,
        //        InformedId = operatingEntity.Division.HeadId,
        //        CreatedById = loggedInUserId,
        //        ContractServiceId = contractServcie.Id,
        //        EndDate = contractServcie.FulfillmentEndDate,
        //        StartDate = contractServcie.FulfillmentStartDate,
        //        ServiceCode = service.ServiceCode,
        //        Budget = contractServcie.Budget,
        //        ProjectDeliveryDate = contractServcie.ActivationDate?? contractServcie.FulfillmentEndDate, 
        //    });

        //    await _context.SaveChangesAsync();

        //    var taskFulfilment = await _context.TaskFulfillments.AsNoTracking().
        //        Where(x => x.Id == task.Entity.Id)
        //        .SingleOrDefaultAsync();

        //    await SendNewTaskAssignedMail(taskFulfilment, contractServcie.Service.OperatingEntity.Name);
            
        //    foreach (var deliverable in serviceTask.ServiceTaskDeliverables)
        //    {
        //        await CreateDeliverableFulfillment(task.Entity.Id, deliverable, service.ServiceCode, loggedInUserId);
        //    }
        //    return true;
        //}

        //private async Task<bool> CreateDeliverableFulfillment(
        //                            long taskId, 
        //                            ServiceTaskDeliverable serviceTaskDeliverable,
        //                            string serviceCode,
        //                            long loggedInUserId
        //                            )
        //{
             
        //    var deliverable = await  _context.DeliverableFulfillments.AddAsync(new DeliverableFulfillment(){
        //        Caption = serviceTaskDeliverable.Caption,
        //        Description = serviceTaskDeliverable.Description,
        //        TaskFullfillmentId = taskId,
        //        CreatedById = loggedInUserId,
        //        ServiceCode = serviceCode
        //    });

        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        
        //public async Task<bool> CreateAccounts(
        //                            ContractService contractService,
        //                            CustomerDivision customerDivision,
        //                            long branchId,
        //                            long officeId,
        //                            Service service,
        //                            FinanceVoucherType accountVoucherType,
        //                            QuoteService quoteService,
        //                            long loggedInUserId,
        //                            bool isReversal
        //                            )
        //{
        //    var totalContractBillable = CalculateTotalBillableForPeriod(contractService, false);         
        //    var totalVAT = CalculateTotalBillableForPeriod(contractService, true); 

        //    var totalAfterTax = totalContractBillable - totalVAT;
        //    var savedAccountMaster = await CreateAccountMaster(service,
        //                                                 contractService,  
        //                                                 accountVoucherType.Id, 
        //                                                 branchId, 
        //                                                 officeId,
        //                                                 totalContractBillable,
        //                                                 customerDivision, 
        //                                                 loggedInUserId); 
                                                            
        //    if(quoteService != null)
        //    {
        //        //await SaveRangeSbuaccountMaster( savedAccountMaster.Id, quoteService.SbutoQuoteServiceProportions);  
        //    }

        //    await PostCustomerReceivablAccounts(
        //                             service,
        //                             contractService.Id,
        //                             customerDivision,
        //                             branchId,
        //                             officeId,
        //                             accountVoucherType.Id,
        //                             totalContractBillable,
        //                             savedAccountMaster.Id,
        //                             loggedInUserId,
        //                             isReversal
        //                            );

        //    await PostVATAccountDetails(
        //                             service,
        //                             contractService.Id,
        //                             customerDivision,
        //                             branchId,
        //                             officeId,
        //                             accountVoucherType.Id,
        //                             savedAccountMaster.Id,
        //                             totalVAT,
        //                            loggedInUserId,
        //                            !isReversal
        //                            );
        //    await PostIncomeAccount(
        //                            service,
        //                            contractService.Id,
        //                            customerDivision,
        //                            branchId,
        //                            officeId,
        //                            accountVoucherType.Id,
        //                            savedAccountMaster.Id,
        //                            totalAfterTax,
        //                            loggedInUserId,
        //                            !isReversal
        //                                    )   ;
            
        //    return true;
        //}
        //public async Task<bool> PostCustomerReceivablAccounts(
        //                            Service service,
        //                            long contractServiceId,
        //                            CustomerDivision customerDivision,
        //                            long branchId,
        //                            long officeId,
        //                            long accountVoucherTypeId,
        //                            double totalContractBillable,
        //                            long accountMasterId,
        //                            long createdById,
        //                            bool isCredit
        //                            )
        //{
        //    long accountId = 0;
        //    if(this.isRetail)
        //    {
        //        accountId = await GetRetailAccount(customerDivision);
        //    }else if(customerDivision.ReceivableAccountId > 0){
        //        accountId = (long) customerDivision.ReceivableAccountId;
        //    }else{
        //        //Create Customer Account, Account master and account details
        //        ControlAccount controlAccount = await _context.ControlAccounts
        //                .FirstOrDefaultAsync(x => x.Caption == this.ReceivableControlAccount);

        //        Account account = new Account(){
        //            Name = $"{customerDivision.DivisionName} Receivable",
        //            Description = $"Receivable Account of {customerDivision.DivisionName}",
        //            Alias = GenerateClientAlias(customerDivision.DivisionName),
        //            IsDebitBalance = true,
        //            ControlAccountId = controlAccount.Id,
        //            CreatedById = createdById
        //        };
        //        var savedAccount = await SaveAccount(account);

        //        customerDivision.ReceivableAccountId = savedAccount.Id;
        //        accountId = savedAccount.Id;

        //        _context.CustomerDivisions.Update(customerDivision);
        //        await _context.SaveChangesAsync();

        //    }


        //    await PostAccountDetail(service, 
        //                            contractServiceId, 
        //                            accountVoucherTypeId, 
        //                            branchId, 
        //                            officeId,
        //                            totalContractBillable, 
        //                            isCredit,
        //                            accountMasterId,
        //                            accountId,
        //                            customerDivision,
        //                            createdById
        //                            );
            
        //    return true;
        //}

        //private async Task<long> GetRetailAccount(CustomerDivision customerDivision ){
            
        //    Account retailAccount  = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == this.RETAIL_INCOME_ACCOUNT);
        //    long accountId = 0;
        //    if(retailAccount == null)
        //    {
        //        ControlAccount controlAccount = await _context.ControlAccounts
        //                .FirstOrDefaultAsync(x => x.Caption == this.ReceivableControlAccount);

        //        Account account = new Account(){
        //            Name = this.RETAIL_INCOME_ACCOUNT,
        //            Description = $"Income Receivable Account of Retail Clients",
        //            Alias = "RIA",
        //            IsDebitBalance = true,
        //            ControlAccountId = controlAccount.Id,
        //            CreatedById = this.LoggedInUserId
        //        };
        //        var savedAccount = await SaveAccount(account);

        //        customerDivision.ReceivableAccountId = savedAccount.Id;
        //        _context.CustomerDivisions.Update(customerDivision);
        //        await _context.SaveChangesAsync();
        //        accountId = savedAccount.Id;
        //    }else{
        //        accountId = retailAccount.Id;
        //    }

        //    return accountId;

        //}

        
        //public async Task<bool> PostVATAccountDetails(
        //                            Service service,
        //                            long contractServiceId,
        //                            CustomerDivision customerDivision,
        //                            long branchId,
        //                            long officeId,
        //                            long accountVoucherTypeId,
        //                            long accountMasterId,
        //                            double totalVAT,
        //                            long loggedInUserId,
        //                            bool isCredit
        //                            )
        //{
        //    var vatAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == this.VALUEADDEDTAX);
 
        //    await PostAccountDetail(service, 
        //                            contractServiceId, 
        //                            accountVoucherTypeId, 
        //                            branchId, 
        //                            officeId,
        //                            totalVAT, 
        //                            isCredit,
        //                            accountMasterId,
        //                            vatAccount.Id,
        //                            customerDivision, 
        //                            loggedInUserId
        //                            );
            
        //    return true;
        //}

        //public async Task<bool> PostIncomeAccount(
        //                            Service service,
        //                            long contractServiceId,
        //                            CustomerDivision customerDivision,
        //                            long branchId,
        //                            long officeId,
        //                            long accountVoucherTypeId,
        //                            long accountMasterId,
        //                            double totalBillableAfterTax,
        //                            long loggedInUserId,
        //                            bool isCredit
        //                            )
        //{
        //    await PostAccountDetail(service, 
        //                            contractServiceId, 
        //                            accountVoucherTypeId, 
        //                            branchId, 
        //                            officeId,
        //                            totalBillableAfterTax, 
        //                            isCredit,
        //                            accountMasterId,
        //                            (long) service.AccountId,
        //                            customerDivision,
        //                            loggedInUserId
        //                            );
            
        //    return true;
        //}



        //private async Task<Account> SaveAccount(Account account)
        //{
        //    try{
        //        await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts ON");

        //        var  lastSavedAccount = await _context.Accounts.Where(x => x.ControlAccountId == account.ControlAccountId)
        //            .OrderBy(x => x.Id).LastOrDefaultAsync();
        //        if(lastSavedAccount == null || lastSavedAccount.Id < 1000000000)
        //        {
        //            account.Id = (long) account.ControlAccountId + 1;
        //        }else{
        //            account.Id = lastSavedAccount.Id + 1;
        //        }
        //        var savedAccount = await _context.Accounts.AddAsync(account);
        //        await _context.SaveChangesAsync();
        //        return savedAccount.Entity;
        //    }catch(Exception)
        //    {
        //        throw;
        //    }finally{
        //        await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts OFF");
        //    }
           
        //}

        //public async Task<AccountMaster> CreateAccountMaster(Service service,
        //                                                ContractService contractService,  
        //                                                long accountVoucherTypeId, 
        //                                                long branchId, 
        //                                                long officeId,
        //                                                double amount,
        //                                                CustomerDivision customerDivision ,
        //                                                long createdById )
        //{
        //    string transactionId = String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) ?  
        //                     $"TRS{service.ServiceCode}/{contractService.Id}"
        //                            :  $"{contractService.Contract?.GroupInvoiceNumber?.Replace("GINV", "TRS")}/{contractService.Id}" ;
            
        //    AccountMaster accountMaster = new AccountMaster(){
        //        Description = $"Sales of {service.Name} with service ID: {service.Id} to {customerDivision.DivisionName}",
        //        IntegrationFlag = false,
        //        VoucherId = accountVoucherTypeId,
        //        BranchId = branchId,
        //        OfficeId = officeId,
        //        Value = amount,
        //        TransactionId = transactionId,
        //        CreatedById = createdById,
        //        CustomerDivisionId = customerDivision.Id
        //    };     
        //    var savedAccountMaster = await _context.AccountMasters.AddAsync(accountMaster);
        //    await _context.SaveChangesAsync();
        //    return savedAccountMaster.Entity;
        //}

        //private async Task<bool> SaveRangeSbuaccountMaster(long accountMasterId, IEnumerable<SbutoQuoteServiceProportion> sBUToQuoteServiceProp)
        //{
        //    List<SbuaccountMaster> listOfSbuaccountMaster = new List<SbuaccountMaster>();
        //    foreach (var prop in sBUToQuoteServiceProp)
        //    {
        //        listOfSbuaccountMaster.Add(new SbuaccountMaster()
        //        {
        //            StrategicBusinessUnitId = prop.StrategicBusinessUnitId,
        //            AccountMasterId = accountMasterId
        //        });
        //    }

        //    await _context.SbuaccountMasters.AddRangeAsync(listOfSbuaccountMaster);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //private async Task<AccountDetail> PostAccountDetail(
        //                                            Service service,
        //                                            long contractServiceId,  
        //                                            long accountVoucherTypeId, 
        //                                            long branchId, 
        //                                            long officeId,
        //                                            double amount,
        //                                            bool isCredit,
        //                                            long accountMasterId,
        //                                            long accountId,
        //                                            CustomerDivision customerDivision,
        //                                            long loggedInUserId
        //                                            )
        //{

        //    AccountDetail accountDetail = new AccountDetail(){
        //        Description = $"Sales of {service.Name}  with service ID: {service.Id} to {customerDivision.DivisionName}",
        //        IntegrationFlag = false,
        //        VoucherId = accountVoucherTypeId,
        //        TransactionId = $"{service.ServiceCode}/{contractServiceId}",
        //        TransactionDate = DateTime.Now,
        //        Credit = isCredit ? amount : 0,
        //        Debit = !isCredit ? amount : 0,
        //        AccountId = accountId,
        //        BranchId = branchId,
        //        OfficeId = officeId,
        //        AccountMasterId = accountMasterId,
        //        CreatedById = loggedInUserId,
        //    };

        //    var savedAccountDetails = await _context.AccountDetails.AddAsync(accountDetail);
        //    await _context.SaveChangesAsync();
        //    return savedAccountDetails.Entity;
        //}



        //private string GenerateClientAlias(string divisionName){
        //    string[] names = divisionName.Trim().Split(" ");
        //    string initial = "";
        //    foreach (var name in names)
        //    {
        //        initial+= name.Substring(0,1).ToUpper();
        //    }
        //    return initial;
        //}

        //private double CalculateTotalAmountForContract(double priceOfService, DateTime contractStartDate, DateTime contractEndDate, TimeCycle timeCycle )
        //{
        //    int numberOfMonth = 0;

        //    if(timeCycle == TimeCycle.OneTime)
        //    {
        //        return priceOfService;
        //    }

        //    while(contractStartDate <= contractEndDate)
        //    {
        //        numberOfMonth++;
        //        contractStartDate = contractStartDate.AddMonths(1);
        //    }
            
        //    return priceOfService * numberOfMonth;
        //}

        //private async Task SendNewTaskAssignedMail(TaskFulfillment taskFulfillment, string operatingEntityName)
        //{
        //    taskFulfillment.Responsible = await _context.UserProfiles.AsNoTracking().Where(x => x.Id == taskFulfillment.ResponsibleId).SingleOrDefaultAsync();
        //    taskFulfillment.CustomerDivision = await _context.CustomerDivisions.AsNoTracking().Where(x => x.Id == taskFulfillment.CustomerDivisionId).SingleOrDefaultAsync();

        //    var serializedTask = JsonConvert.SerializeObject(taskFulfillment, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        //    Action action = async () =>
        //    {
        //        await _mailAdapter.SendNewTaskAssigned(serializedTask, operatingEntityName);
        //    };
        //    action.RunAsTask();
        //}

        //private static double GenerateWeeklyAmount(DateTime startDate, DateTime endDate, double amountPerMonth, TimeCycle timeCylce)
        //{
        //    var interval = timeCylce == TimeCycle.Weekly ? 7 : 14;
        //    var numberOfMonths = 0;
        //    var daysBetweenStartAndEndDate = (endDate.Subtract(startDate).TotalDays + 1) < interval ?
        //                   (int) interval : (endDate.Subtract(startDate).TotalDays + 1);
        //    var numberOfPayment = Math.Floor(daysBetweenStartAndEndDate / interval);
        //    startDate = startDate.AddMonths(1);

        //    while(startDate < endDate)
        //    {
        //        numberOfMonths++;
        //        startDate = startDate.AddMonths(1);
        //    }
        //    numberOfMonths = numberOfMonths == 0 ? 1 : numberOfMonths;
        //    var totalAmountPayable = numberOfMonths * amountPerMonth;
        //    var amountToPay = Math.Round(totalAmountPayable / numberOfPayment, 4);
        //    return amountToPay;
        //}

        //private double CalculateTotalBillableForPeriod(ContractService contractService, bool isVAT )
        //{
        //    int interval = 0;
        //    DateTime startDate =(DateTime) contractService.ContractStartDate; 
        //    DateTime endDate =(DateTime) contractService.ContractEndDate;
        //    TimeCycle cycle =(TimeCycle) contractService.InvoicingInterval;
        //    double amount = isVAT ? (double) contractService.Vat : (double) contractService.BillableAmount;

        //    switch (cycle)
        //    {
        //        case TimeCycle.Weekly:
        //            interval = 7;
        //            break;
        //        case TimeCycle.BiWeekly:
        //            interval = 14;
        //            break;
        //        case TimeCycle.Monthly:
        //            interval = 1;
        //            break;
        //        case TimeCycle.BiMonthly:
        //            interval = 2;
        //            break;
        //        case TimeCycle.Quarterly:
        //            interval = 4;
        //            break;
        //        case TimeCycle.SemiAnnually:
        //            interval = 6;
        //            break;
        //        case TimeCycle.Annually:
        //            interval = 12;
        //            break;
        //        case TimeCycle.BiAnnually:
        //            interval = 24;
        //            break;
        //    }

        //    if(cycle == TimeCycle.Weekly || cycle == TimeCycle.BiWeekly)
        //    {
        //        return GenerateWeeklyAmount( startDate, endDate,  amount,  cycle);

        //    }else if(cycle == TimeCycle.OneTime ){
        //        return amount;
        //    }else{
        //        return amount * (double) interval ;
        //    }
        //}
    }
}
