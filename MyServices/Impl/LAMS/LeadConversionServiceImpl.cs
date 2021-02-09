using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.LAMS;
using HaloBiz.Model.ManyToManyRelationship;
using HaloBiz.MyServices.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class LeadConversionServiceImpl : ILeadConversionService
    {
        private readonly DataContext _context;
        private readonly ILogger<LeadConversionServiceImpl> _logger;
        public long LoggedInUserId;
        private readonly string ReceivableControlAccount = "Receivable";
        private readonly string SALESINVOICEVOUCHER = "Sales Invoice";
        private readonly string VALUEADDEDTAX = "VALUE ADDED TAX";
        //private readonly string VALUEADDEDTAX = "VALUE ADDED TAX";

        public LeadConversionServiceImpl(
                                        DataContext context, 
                                        ILogger<LeadConversionServiceImpl> logger
                                        
                                        )
        {
            this._context = context;
            this._logger = logger;
        }
        public async Task<bool> ConvertLeadToClient(long leadId, long loggedInUserId)
        {
            this.LoggedInUserId = loggedInUserId;
            using(var transaction =  await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var lead = await _context.Leads
                        .Include(x => x.LeadDivisions)
                            .ThenInclude(x => x.Quote)
                        .FirstOrDefaultAsync(x => x.Id == leadId);
                    
                    Customer customer = await ConvertLeadToCustomer(lead, _context);
                    foreach (var leadDivision in lead.LeadDivisions)
                    {
                      CustomerDivision customerDivision = await  ConvertLeadDivisionToCustomerDivision(leadDivision, customer.Id, _context);

                      var quote = await _context.Quotes
                                            .Include(x => x.QuoteServices)
                                                .ThenInclude(x => x.SBUToQuoteServiceProportions)
                                            .FirstOrDefaultAsync(x => x.LeadDivisionId == leadDivision.Id);

                      Contract contract = await ConvertQuoteToContract( quote, customerDivision.Id,  _context);


                      foreach (var quoteService in quote.QuoteServices)
                      {
                          await ConvertQuoteServiceToContractService( quoteService,  _context, customerDivision, contract.Id, leadDivision);
                      }

                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (System.Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    return false;
                }

            }

        }



        private async Task<Customer> ConvertLeadToCustomer(Lead lead, DataContext context)
        {

            //Converts lead to customer and saves customer and update lead
            var customerEntity = await context.Customers.AddAsync( new Customer(){
                GroupName = lead.GroupName,
                RCNumber = lead.RCNumber,
                GroupTypeId = lead.GroupTypeId,
                Industry = lead.Industry,
                LogoUrl = lead.LogoUrl,
                Email = "",
                PhoneNumber = "",
                CreatedById = this.LoggedInUserId,
            });

            await context.SaveChangesAsync();

            Customer customer = customerEntity.Entity;
            lead.CustomerId = customer.Id;
            lead.LeadConversionStatus =true;
            lead.TimeConvertedToClient =(DateTime) DateTime.Now;
            
            context.Leads.Update(lead);
            await context.SaveChangesAsync();
            return customer;
 
        }

        private async Task<CustomerDivision> ConvertLeadDivisionToCustomerDivision(LeadDivision leadDivision, long customerId, DataContext context)
        {
            //creates customer division from lead division and saves the customer division
            var customerDivisionEntity =  await context.CustomerDivisions.AddAsync(new CustomerDivision(){
                Industry = leadDivision.Industry,
                RCNumber = leadDivision.RCNumber,
                DivisionName = leadDivision.DivisionName,
                Email = leadDivision.Email,
                LogoUrl = leadDivision.LogoUrl,
                CustomerId = customerId,
                PhoneNumber = leadDivision.PhoneNumber,
                Address = leadDivision.Address,
                CreatedById = this.LoggedInUserId,
            });

            await context.SaveChangesAsync();
            
            return customerDivisionEntity.Entity;
            
        }

        private async Task<Contract> ConvertQuoteToContract(Quote quote, long customerDivisionId, DataContext context)
        {

            //Create contract from quote
            var entity = await context.Contracts.AddAsync( new Contract(){
                ReferenceNo = quote.ReferenceNo,
                CustomerDivisionId = customerDivisionId,
                QuoteId = quote.Id,
                CreatedById = this.LoggedInUserId,
            });

            await context.SaveChangesAsync();
            return entity.Entity;
                
        }

        private async Task<bool> ConvertQuoteServiceToContractService(QuoteService quoteService, 
                                                                        DataContext context, 
                                                                        CustomerDivision customerDivision, 
                                                                        long contractId, 
                                                                        LeadDivision leadDivision)
        {
            var contractServiceToSave = new ContractService()
            {
                UnitPrice = quoteService.UnitPrice,
                Quantity = quoteService.Quantity,
                Discount = quoteService.Discount,
                VAT = quoteService.VAT,
                BillableAmount = quoteService.BillableAmount,
                Budget = quoteService.Budget,
                ContractStartDate = quoteService.ContractStartDate,
                ContractEndDate = quoteService.ContractEndDate,
                PaymentCycle = quoteService.PaymentCycle,
                PaymentCycleInDays = quoteService.PaymentCycleInDays,
                FirstInvoiceSendDate = quoteService.FirstInvoiceSendDate,
                InvoicingInterval = quoteService.InvoicingInterval,
                ProblemStatement = quoteService.ProblemStatement,
                ActivationDate = quoteService.ActivationDate,
                FulfillmentStartDate = quoteService.FulfillmentStartDate,
                FulfillmentEndDate = quoteService.FulfillmentEndDate,
                TentativeDateForSiteSurvey = quoteService.TentativeDateForSiteSurvey,
                PickupDateTime = quoteService.PickupDateTime,
                DropoffDateTime = quoteService.DropoffDateTime,
                PickupLocation = quoteService.PickupLocation,
                Dropofflocation = quoteService.Dropofflocation,
                BeneficiaryName = quoteService.BeneficiaryName,
                BeneficiaryIdentificationType = quoteService.BeneficiaryIdentificationType,
                BenificiaryIdentificationNumber = quoteService.BenificiaryIdentificationNumber,
                TentativeProofOfConceptStartDate = quoteService.TentativeProofOfConceptStartDate,
                TentativeProofOfConceptEndDate = quoteService.TentativeProofOfConceptEndDate,
                TentativeFulfillmentDate = quoteService.TentativeFulfillmentDate,
                ProgramCommencementDate = quoteService.ProgramCommencementDate,
                ProgramDuration = quoteService.ProgramDuration,
                ProgramEndDate = quoteService.ProgramEndDate,
                TentativeDateOfSiteVisit = quoteService.TentativeDateOfSiteVisit,
                QuoteServiceId = quoteService.Id,
                ContractId = contractId,
                CreatedById = this.LoggedInUserId,
                ServiceId = quoteService.ServiceId,
                ReferenceNo = quoteService.ReferenceNumber
            };

            var entity = await context.ContractServices.AddAsync(contractServiceToSave);
            await context.SaveChangesAsync();
            quoteService.IsConvertedToContractService = true;
            context.Update(quoteService);
            await context.SaveChangesAsync();
            var contractService = entity.Entity;

            await ConvertSBUToQuoteServicePropToSBUToContractServiceProp( quoteService.Id, contractService.Id, context);
            await ConvertQuoteServieDocumentsToClosureDocuments(quoteService.Id, contractService.Id, context);
            await CreateTaskAndDeliverables(quoteService.ServiceId, contractService ,customerDivision.Id );
            await CreateAccounts(   quoteService,
                                     contractService.Id,
                                     customerDivision,
                                     (long) leadDivision.BranchId,
                                    (long) leadDivision.OfficeId
                                    );
            if(contractService.InvoicingInterval != TimeCycle.Adhoc)
            {
                await GenerateInvoices( contractService,  customerDivision.Id, contractId, context);
                await GenerateAmortizations( contractService,  customerDivision, context);
            }

            return true;

        }


        private async Task<bool> ConvertSBUToQuoteServicePropToSBUToContractServiceProp(long quoteServiceId, long contractServiceId, DataContext context)
        {
            var sBUToContractServiceProps = new List<SBUToContractServiceProportion>();

            var sBUToQuoteServiceProps = await _context.SBUToQuoteServiceProportions
                    .Where(x => x.QuoteServiceId == quoteServiceId)
                    .ToListAsync();
            
            foreach(var sbuToQuoteServiceProp in sBUToQuoteServiceProps)
            {
                sBUToContractServiceProps.Add(new SBUToContractServiceProportion()
                {
                    ContractServiceId = contractServiceId,
                    Proportion = sbuToQuoteServiceProp.Proportion,
                    Status = sbuToQuoteServiceProp.Status,
                    StrategicBusinessUnitId = sbuToQuoteServiceProp.StrategicBusinessUnitId,
                    UserInvolvedId = sbuToQuoteServiceProp.UserInvolvedId,
                    CreatedById = this.LoggedInUserId,
                });

            }

            await context.SBUToContractServiceProportions.AddRangeAsync(sBUToContractServiceProps);
            await context.SaveChangesAsync();
            return true;
            
        }

        private async Task<bool> ConvertQuoteServieDocumentsToClosureDocuments(long quoteServiceId, long contractServiceId, DataContext context)
        {
            var closureDocuments = new List<ClosureDocument>();

            var QuoteServieDocuments = await context.QuoteServiceDocuments
                    .Where(x => x.QuoteServiceId == quoteServiceId)
                    .ToListAsync();
            
            foreach(var doc in QuoteServieDocuments)
            {
                closureDocuments.Add(new ClosureDocument()
                {
                    ContractServiceId = contractServiceId,
                    Caption = doc.Caption,
                    Description = doc.Description,
                    DocumentUrl = doc.DocumentUrl,
                    CreatedById = this.LoggedInUserId,
                });
            }

            await context.ClosureDocuments.AddRangeAsync(closureDocuments);
            await context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> GenerateInvoices(ContractService contractService, long customerDivisionId, long contractId, DataContext context)
        {
            List<Invoice> invoicesToSave = GenerateListOfInvoiceCycle(
                                    (DateTime)contractService.ContractStartDate,
                                    (DateTime) contractService.FirstInvoiceSendDate, 
                                    (DateTime) contractService.ContractEndDate, 
                                    (TimeCycle) contractService.InvoicingInterval,
                                    (double) contractService.BillableAmount,
                                     contractService, 
                                    customerDivisionId);

            await context.Invoices.AddRangeAsync(invoicesToSave);
            await context.SaveChangesAsync();
            return true;
        }


        private async  Task<bool> GenerateAmortizations(ContractService contractService, CustomerDivision customerDivision, DataContext context)
        {
            DateTime startDate = (DateTime) contractService.ContractStartDate;
            DateTime endDate = (DateTime) contractService.ContractEndDate;
            var InitialYear = startDate.Year;
            List<Amortization> amortizations = new List<Amortization>();

            for(int i = startDate.Year; i <= endDate.Year; i++)
            {
                amortizations.Add(new Amortization(){
                    Year = i,
                    ClientId = customerDivision.CustomerId,
                    DivisionId = customerDivision.Id,
                    ContractId = contractService.ContractId,
                    ContractServiceId = contractService.Id,
                    ContractValue = (double) contractService.Budget,
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

           await  context.Amortizations.AddRangeAsync(amortizations);
            return true;
        }

        public static List<Invoice> GenerateListOfInvoiceCycle(
                                            DateTime startDate, 
                                            DateTime firstInvoiceSendDate, 
                                            DateTime endDate, 
                                            TimeCycle cycle, 
                                            double amount,
                                            ContractService contractService, 
                                            long customerDivisionId)
        {
            int interval = 0;
            List<Invoice> invoices = new  List<Invoice>();

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

            var invoiceValue = 0.0;


            if(cycle == TimeCycle.Weekly || cycle == TimeCycle.BiWeekly)
            {
                var invoiceValueForWeekly = GenerateAmount( startDate, endDate,  amount,  cycle);

                while(firstInvoiceSendDate < endDate){
                         invoices.Add(GenerateInvoice(startDate,  
                            startDate.AddDays(interval) > endDate ? endDate : startDate.AddDays(interval), 
                            invoiceValueForWeekly , 
                            firstInvoiceSendDate,
                             contractService, 
                             customerDivisionId));
                    firstInvoiceSendDate = firstInvoiceSendDate.AddDays(interval);
                    startDate = startDate.AddDays(interval);
                }
            }else if(cycle == TimeCycle.OneTime ){
                
                    invoices.Add(GenerateInvoice(startDate,  endDate, amount , firstInvoiceSendDate,
                                                     contractService, customerDivisionId));

            }else{
                invoiceValue = amount * (double) interval ;
                while(firstInvoiceSendDate < endDate){
                    invoices.Add(GenerateInvoice(startDate,  
                                        startDate.AddMonths(interval) > endDate ? endDate : startDate.AddDays(interval), 
                                                invoiceValue , 
                                                firstInvoiceSendDate, 
                                                contractService, 
                                                customerDivisionId));
                    firstInvoiceSendDate = firstInvoiceSendDate.AddMonths(interval);
                    startDate = startDate.AddDays(interval);
                }
            }
            return invoices;
        }

        private static Invoice GenerateInvoice(DateTime from, DateTime to, double amount, DateTime sendDate,ContractService contractService, long customerDivisionId)
        {
            return new Invoice(){
                    InvoiceNumber = "",
                    UnitPrice = (double) contractService.UnitPrice,
                    Quantity = contractService.Quantity,
                    Discount = contractService.Discount,
                    Value  = amount,
                    DateToBeSent = sendDate,
                    IsInvoiceSent = false,
                    CustomerDivisionId = customerDivisionId,
                    ContractId = contractService.ContractId,
                    ContractServiceId = contractService.Id,
                    StartDate  = from,
                    EndDate  = to,
                    
            };
        }

        private static double GenerateAmount(DateTime startDate, DateTime endDate, double amountPerMonth, TimeCycle timeCylce)
        {
            var interval = timeCylce == TimeCycle.Weekly ? 7 : 14;
            var numberOfMonths = 0;
            var daysBetweenStartAndEndDate = (endDate.Subtract(startDate).TotalDays + 1) < interval ?
                           (int) interval : (endDate.Subtract(startDate).TotalDays + 1);
            var numberOfPayment = Math.Floor(daysBetweenStartAndEndDate / interval);
            startDate = startDate.AddMonths(1);

            while(startDate < endDate)
            {
                numberOfMonths++;
                startDate = startDate.AddMonths(1);
            }
            numberOfMonths = numberOfMonths == 0 ? 1 : numberOfMonths;
            var totalAmountPayable = numberOfMonths * amountPerMonth;
            var amountToPay = Math.Round(totalAmountPayable / numberOfPayment, 4);
            return amountToPay;
        }


        private async Task<bool> CreateTaskAndDeliverables(long serviceId, ContractService contractServcie, long customerDivisionId)
        {
            Services servcie = await _context.Services.FirstOrDefaultAsync(x => x.Id == contractServcie.ServiceId); 
            if(servcie == null)
            {
                return false;
            }

            IEnumerable<ServiceCategoryTask> serviceCategoryTasks = await _context.ServiceCategoryTasks
            .Include(x => x.ServiceTaskDeliverable)
            .Where(x => x.ServiceCategoryId == servcie.ServiceCategoryId && x.IsDeleted == false).ToListAsync();
            
            foreach (var serviceTask in serviceCategoryTasks)
            {
                await CreateTaskFulfillment(
                                     serviceTask, 
                                     contractServcie,  
                                    customerDivisionId,
                                    servcie.OperatingEntityId
                                    );
            }
            return true;
        }

        private async Task<bool> CreateTaskFulfillment(
                                    ServiceCategoryTask serviceTask, 
                                    ContractService contractServcie,  
                                    long customerDivisionId,
                                    long operatingEntityId
                                    )
        {
            var operatingEntity = await _context.OperatingEntities
                        .Include(x => x.Division)
                        .FirstOrDefaultAsync(x => x.Id == operatingEntityId);
             
            var task = await  _context.TaskFulfillments.AddAsync(new TaskFulfillment(){
                Caption = serviceTask.Caption,
                CustomerDivisionId = customerDivisionId,
                ResponsibleId = operatingEntity.HeadId,
                AccountableId = operatingEntity.HeadId,
                ConsultedId = operatingEntity.Division.HeadId,
                InformedId = operatingEntity.Division.HeadId,
                CreatedById = this.LoggedInUserId
            });

            await _context.SaveChangesAsync();

            foreach (var deliverable in serviceTask.ServiceTaskDeliverable)
            {
                await CreateDeliverableFulfillment(task.Entity.Id, deliverable);
            }
            return true;
        }
        private async Task<bool> CreateDeliverableFulfillment(
                                    long taskId, 
                                    ServiceTaskDeliverable serviceTaskDeliverable
                                    )
        {
             
            var deliverable = await  _context.DeliverableFulfillments.AddAsync(new DeliverableFulfillment(){
                Caption = serviceTaskDeliverable.Caption,
                Description = serviceTaskDeliverable.Description,
                TaskFullfillmentId = taskId,
                CreatedById = this.LoggedInUserId
            });

            await _context.SaveChangesAsync();
            return true;
        }

        
        private async Task<bool> CreateAccounts(
                                    QuoteService quoteService,
                                    long contractServiceId,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId
                                    )
        {

            await CreateCustomerReceivablAccounts(
                                     quoteService,
                                     contractServiceId,
                                     customerDivision,
                                     branchId,
                                     officeId
                                    );
            await CreateVATAccount(
                                     quoteService,
                                     contractServiceId,
                                     customerDivision,
                                     branchId,
                                     officeId
                                    );
            await CreateIncomeAccountMasterAndDetails(
                                                    quoteService,
                                                    contractServiceId,
                                                    customerDivision,
                                                    branchId,
                                                    officeId
                                                        )   ;
            
            return true;
        }
        private async Task<bool> CreateCustomerReceivablAccounts(
                                    QuoteService quoteService,
                                    long contractServiceId,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId
                                    )
        {

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
            var savedAccount = await SaveAccount(account);

            customerDivision.AccountId = savedAccount.Id;

            _context.CustomerDivisions.Update(customerDivision);
            await _context.SaveChangesAsync();

            FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                .FirstOrDefaultAsync(x => x.VoucherType == this.SALESINVOICEVOUCHER);

                 
            var savedAccountMaster = await CreateAccountMaster( quoteService,
                                                         contractServiceId,  
                                                         accountVoucherType.Id, 
                                                         savedAccount.Id,
                                                         branchId, 
                                                         officeId  ); 
            await SaveRangeSBUAccountMaster( savedAccountMaster.Id, quoteService.SBUToQuoteServiceProportions);  
            var totalContractBillable = CalculateTotalAmountForContract((double)(quoteService.BillableAmount + quoteService.VAT), 
                                                                        (DateTime)quoteService.ContractStartDate, 
                                                                        (DateTime) quoteService.ContractEndDate, 
                                                                        (TimeCycle) quoteService.InvoicingInterval);         
            await CreateAccountDetail(quoteService, 
                                    contractServiceId, 
                                    accountVoucherType.Id, 
                                    branchId, 
                                    officeId,
                                    totalContractBillable, 
                                    false,savedAccountMaster.Id);
            
            return true;
        }
        private async Task<bool> CreateVATAccount(
                                    QuoteService quoteService,
                                    long contractServiceId,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId
                                    )
        {
            var vatAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == this.VALUEADDEDTAX);
            FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                .FirstOrDefaultAsync(x => x.VoucherType == this.SALESINVOICEVOUCHER);

            var savedAccountMaster = await CreateAccountMaster( quoteService,
                                                         contractServiceId,  
                                                         accountVoucherType.Id, 
                                                         vatAccount.Id,
                                                         branchId, 
                                                         officeId  ); 
            var totalContractBillable = CalculateTotalAmountForContract((double)quoteService.VAT, 
                                                        (DateTime)quoteService.ContractStartDate, 
                                                        (DateTime) quoteService.ContractEndDate,
                                                        (TimeCycle) quoteService.InvoicingInterval); 
            await CreateAccountDetail(quoteService, 
                                        contractServiceId, 
                                        accountVoucherType.Id, 
                                        branchId, 
                                        officeId,totalContractBillable, 
                                        true,savedAccountMaster.Id);
            
            return true;
        }

        private async Task<bool> CreateIncomeAccountMasterAndDetails(
                                    QuoteService quoteService,
                                    long contractServiceId,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId
                                    )
        {
            
            var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == quoteService.ServiceId);
            FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                .FirstOrDefaultAsync(x => x.VoucherType == this.SALESINVOICEVOUCHER);

                 
            var savedAccountMaster = await CreateAccountMaster( quoteService,
                                                         contractServiceId,  
                                                         accountVoucherType.Id, 
                                                         (long) service.AccountId,
                                                         branchId,
                                                         officeId );
            var totalContractBillable = CalculateTotalAmountForContract((double) quoteService.BillableAmount, 
                                                        (DateTime)quoteService.ContractStartDate, 
                                                        (DateTime) quoteService.ContractEndDate,
                                                        (TimeCycle) quoteService.InvoicingInterval); 
            await CreateAccountDetail(quoteService, 
                                        contractServiceId, 
                                        accountVoucherType.Id, 
                                        branchId, 
                                        officeId,totalContractBillable, 
                                        true,savedAccountMaster.Id);
            
            return true;
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

        private async Task<AccountMaster> CreateAccountMaster(QuoteService quoteService,
                                                        long contractServiceId,  
                                                        long accountVoucherTypeId, 
                                                        long accountId,
                                                        long branchId, 
                                                        long officeId  )
        {
            AccountMaster accountMaster = new AccountMaster(){
                Name = $"{quoteService.Service.Name} Charges",
                Description = $"Sales of {quoteService.Service.Name}",
                IntegrationFlag = false,
                AccountMasterAlias = 0,
                VoucherId = accountVoucherTypeId,
                AccountId = accountId,
                BranchId = branchId,
                OfficeId = officeId,
                TransactionId = $"{quoteService.ReferenceNumber}{contractServiceId}",
                CreatedById = this.LoggedInUserId
            };     
            var savedAccountMaster = await _context.AccountMasters.AddAsync(accountMaster);
            await _context.SaveChangesAsync();
            return savedAccountMaster.Entity;
        }

        private async Task<bool> SaveRangeSBUAccountMaster(long accountMasterId, IEnumerable<SBUToQuoteServiceProportion> sBUToQuoteServicesProp)
        {
            List<SBUAccountMaster> listOfSBUAccountMaster = new List<SBUAccountMaster>();
            foreach (var prop in sBUToQuoteServicesProp)
            {
                listOfSBUAccountMaster.Add(new SBUAccountMaster(){
                    StrategicBusinessUnitId = prop.StrategicBusinessUnitId,
                    AccountMasterId = accountMasterId
                });
            }

            await _context.SBUAccountMasters.AddRangeAsync(listOfSBUAccountMaster);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<AccountDetail> CreateAccountDetail(
                                                    QuoteService quoteService,
                                                    long contractServiceId,  
                                                    long accountVoucherTypeId, 
                                                    long branchId, 
                                                    long officeId,
                                                    double amount,
                                                    bool isCredit,
                                                    long accountMasterId
                                                    )
        {

            AccountDetail accountDetail = new AccountDetail(){
                Name = $"{quoteService.Service.Name} Charges",
                Description = $"Sales of {quoteService.Service.Name}",
                IntegrationFlag = false,
                AccountDetailsAlias = 0,
                VoucherId = accountVoucherTypeId,
                TransactionId = $"{quoteService.ReferenceNumber}{contractServiceId}",
                TransactionDate = DateTime.Now,
                Credit = isCredit ? amount : 0,
                Debit = !isCredit ? amount : 0,
                BranchId = branchId,
                OfficeId = officeId,
                AccountMasterId = accountMasterId,
                CreatedById = this.LoggedInUserId,
            };

            var savedAccountDetails = await _context.AccountDetails.AddAsync(accountDetail);
            await _context.SaveChangesAsync();
            return savedAccountDetails.Entity;
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

        private double CalculateTotalAmountForContract(double priceOfService, DateTime contractStartDate, DateTime contractEndDate, TimeCycle timeCycle )
        {
            if(timeCycle == TimeCycle.OneTime)
            {
                return priceOfService;
            }
            int numberOfMonth = 0;
            while(contractStartDate < contractEndDate)
            {
                numberOfMonth++;
                contractStartDate = contractStartDate.AddMonths(1);
            }
            
            return priceOfService * numberOfMonth;
        }
    }
}