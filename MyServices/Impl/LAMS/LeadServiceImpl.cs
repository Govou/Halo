using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.Impl;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class LeadServiceImpl : ILeadService
    {
        private readonly ILogger<LeadServiceImpl> _logger;
        private readonly DataContext _context;
        private readonly IReferenceNumberRepository _refNumberRepo;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ILeadRepository _leadRepo;
        private readonly IMapper _mapper;

        public bool ShouldGenerateInvoice { get; set; }
        public long LoggedInUserId { get; set; }

        public LeadServiceImpl(
                                DataContext context,
                                IReferenceNumberRepository refNumberRepo,
                                IModificationHistoryRepository historyRepo,
                                ILeadRepository leadRepo,
                                ILogger<LeadServiceImpl> logger,
                                IMapper mapper
                                )
        {
            this._mapper = mapper;
            this._context = context;
            this._refNumberRepo = refNumberRepo;
            this._historyRepo = historyRepo;
            this._leadRepo = leadRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddLead(HttpContext context, LeadReceivingDTO leadReceivingDTO)
        {
            var lead = _mapper.Map<Lead>(leadReceivingDTO);
            lead.CreatedById = context.GetLoggedInUserId();
            var referenceNumber = await _refNumberRepo.GetReferenceNumber();
            if(referenceNumber == null)
            {
                return new ApiResponse(500);
            }

            lead.ReferenceNo = referenceNumber.ReferenceNo.GenerateReferenceNumber();
            referenceNumber.ReferenceNo = referenceNumber.ReferenceNo + 1;
            var isUpdatedRefNumber = await _refNumberRepo.UpdateReferenceNumber(referenceNumber);

            if(!isUpdatedRefNumber)
            {
                return new ApiResponse(500);
            }

            var savedLead = await _leadRepo.SaveLead(lead);
            if (savedLead == null)
            {
                return new ApiResponse(500);
            }
            var leadTransferDTO = _mapper.Map<LeadTransferDTO>(savedLead);
            return new ApiOkResponse(leadTransferDTO);
        }

        public async Task<ApiResponse> GetAllLead()
        {
            var leads = await _leadRepo.FindAllLead();
            if (leads == null)
            {
                return new ApiResponse(404);
            }
            var leadTransferDTO = _mapper.Map<IEnumerable<LeadTransferDTO>>(leads);
            return new ApiOkResponse(leadTransferDTO);
        }

        public async Task<ApiResponse> GetLeadById(long id)
        {
            var lead = await _leadRepo.FindLeadById(id);
            if (lead == null)
            {
                return new ApiResponse(404);
            }
            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(lead);
            return new ApiOkResponse(leadTransferDTOs);
        }

        public async Task<ApiResponse> GetLeadByReferenceNumber(string refNumber)
        {
            var lead = await _leadRepo.FindLeadByReferenceNo(refNumber);
            if (lead == null)
            {
                return new ApiResponse(404);
            }
            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(lead);
            return new ApiOkResponse(leadTransferDTOs);
        }

        public async Task<ApiResponse> DropLead(HttpContext context, long id, DropLeadReceivingDTO dropLeadReceivingDTO)
        {

            var leadToUpdate = await _leadRepo.FindLeadById(id);
            if (leadToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            leadToUpdate.DropReasonId = dropLeadReceivingDTO.DropReasonId;
            leadToUpdate.DropLearning = dropLeadReceivingDTO.DropLearning;
            leadToUpdate.IsLeadDropped = true;
            var updatedLead = await _leadRepo.UpdateLead(leadToUpdate);

            if(updatedLead == null)
            {
                return new ApiResponse(500);
            }


            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Lead",
                ChangeSummary = $"this lead was dropped by user with id: {context.GetLoggedInUserId()}",
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLead.Id
            };

            await _historyRepo.SaveHistory(history);

            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(updatedLead);
            return new ApiOkResponse(leadTransferDTOs);
        }
        public async Task<ApiResponse> UpdateLead(HttpContext context, long id, LeadReceivingDTO leadReceivingDTO)
        {

            var leadToUpdate = await _leadRepo.FindLeadById(id);
            if (leadToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {leadToUpdate.ToString()} \n";

            leadToUpdate.LeadTypeId = leadReceivingDTO.LeadTypeId;
            leadToUpdate.LeadOriginId = leadReceivingDTO.LeadOriginId;
            leadToUpdate.Industry = leadReceivingDTO.Industry;
            leadToUpdate.RCNumber = leadReceivingDTO.RCNumber;
            leadToUpdate.GroupName = leadReceivingDTO.GroupName;
            leadToUpdate.GroupTypeId = leadReceivingDTO.GroupTypeId;
            leadToUpdate.LogoUrl = leadReceivingDTO.LogoUrl;

            var updatedLead = await _leadRepo.UpdateLead(leadToUpdate);

            if(updatedLead == null)
            {
                return new ApiResponse(500);
            }

            summary += $"Details after change, \n {updatedLead.ToString()} \n";

            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Lead",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedLead.Id
            };

            await _historyRepo.SaveHistory(history);

            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(updatedLead);
            return new ApiOkResponse(leadTransferDTOs);


        }

        public async Task<ApiResponse> UpdateLeadStagesStatus(long leadId, LeadStages stage)
        {
            var leadToUpdate = await _leadRepo.FindLeadById(leadId);
            if (leadToUpdate == null)
            {
                return new ApiResponse(404);
            }
            switch (stage)
            {
                case LeadStages.Capture:
                    leadToUpdate.LeadCaptureStatus = true;
                    break;
                case LeadStages.Qualification:
                    leadToUpdate.LeadQualificationStatus = true;
                    leadToUpdate.TimeMovedToLeadQualification = DateTime.Now;
                    break;
                case LeadStages.Opportunity:
                    leadToUpdate.LeadOpportunityStatus = true;
                    leadToUpdate.TimeMovedToLeadOpportunity = DateTime.Now;
                    break;
                case LeadStages.Closure:
                    leadToUpdate.TimeMovedToLeadClosure = DateTime.Now;
                    leadToUpdate.LeadClosureStatus = true;
                    break;
                case LeadStages.Conversion:
                    leadToUpdate.TimeConvertedToClient = DateTime.Now;
                    leadToUpdate.LeadConversionStatus = true;
                    break;
            }

            var updatedLead = await _leadRepo.UpdateLead(leadToUpdate);

            if(updatedLead == null)
            {
                return new ApiResponse(500);
            }

            var leadTransferDTOs = _mapper.Map<LeadTransferDTO>(updatedLead);
            return new ApiOkResponse(leadTransferDTOs);
        }


        public async Task<ApiResponse> ConvertLeadToClient(HttpContext context,long leadId, bool shouldGenerateInvoice)
        {
            this.ShouldGenerateInvoice = shouldGenerateInvoice;
            this.LoggedInUserId = context.GetLoggedInUserId();
            using(var transaction =  await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var lead = await _context.Leads
                        .Include(x => x.LeadDivisions)
                        .FirstOrDefaultAsync(x => x.Id == leadId);
                    
                    Customer customer = await ConvertLeadToCustomer(lead, _context);
                    foreach (var leadDivision in lead.LeadDivisions)
                    {
                      CustomerDivision customerDivision = await  ConvertLeadDivisionToCustomerDivision(leadDivision, customer.Id, _context);

                      var quote = await _context.Quotes
                                            .Include(x => x.QuoteServices)
                                            .FirstOrDefaultAsync(x => x.LeadDivisionId == leadDivision.Id);

                      Contract contract = await ConvertQuoteToContract( quote, customerDivision.Id,  _context);


                      foreach (var quoteService in quote.QuoteServices)
                      {
                          await ConvertQuoteServiceToContractService( quoteService,  _context, customerDivision, contract.Id);
                      }

                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ApiOkResponse(true);
                }
                catch (System.Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    return new ApiResponse(500);
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

        private async Task<bool> ConvertQuoteServiceToContractService(QuoteService quoteService, DataContext context, CustomerDivision customerDivision, long contractId)
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
            await GenerateAmortizations( contractService,  customerDivision, context);
            if(this.ShouldGenerateInvoice)
            {
                await GenerateInvoices( contractService,  customerDivision.Id, contractId, context);
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
            List<Invoice> invoicesToSave = new List<Invoice>();
            List<DateTime> sendDates = GenerateListOfInvoiceCycle(
                                    (DateTime) contractService.FirstInvoiceSendDate, 
                                    (DateTime) contractService.ContractEndDate, 
                                    (TimeCycle) contractService.InvoicingInterval, 
                                    (int) contractService.PaymentCycleInDays );
            
            foreach (var date in sendDates)
            {
                invoicesToSave.Add(new Invoice(){
                    InvoiceNumber = "",
                    UnitPrice = (double) contractService.UnitPrice,
                    Quantity = contractService.Quantity,
                    Discount = contractService.Discount,
                    Value  = (double) contractService.BillableAmount,
                    DateToBeSent = date,
                    IsInvoiceSent = false,
                    CustomerDivisionId = customerDivisionId,
                    ContractId = contractId,
                    ContractServiceId = contractService.Id,
                    
                });
            }

            await context.Invoices.AddRangeAsync(invoicesToSave);
            await context.SaveChangesAsync();

            return true;

        }

        private List<DateTime> GenerateListOfInvoiceCycle(DateTime firstInvoiceSendDate, DateTime endDate, TimeCycle cycle, int timeCycleInDays = 0)
        {
            int interval = 0;
            List<DateTime> sendDates = new  List<DateTime>();


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
                case TimeCycle.Others:
                    interval = timeCycleInDays;
                    break;
            }

            if(cycle == TimeCycle.Weekly || cycle == TimeCycle.Others || cycle == TimeCycle.BiWeekly )
            {
                while(firstInvoiceSendDate < endDate){
                    sendDates.Add(firstInvoiceSendDate);
                    firstInvoiceSendDate = firstInvoiceSendDate.AddDays(interval);
                }
            }else{
                while(firstInvoiceSendDate < endDate){
                    sendDates.Add(firstInvoiceSendDate);
                    firstInvoiceSendDate = firstInvoiceSendDate.AddMonths(interval);
                }
            }
            return sendDates;
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
    }
}

