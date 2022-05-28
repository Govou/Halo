using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.Helpers;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
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
        private IConfiguration _configuration;
        private readonly ILogger<SMSContractsRepository> _logger;
        private long LoggedInUserId = 0;
        private bool? isRetail = null;

        private readonly string ReceivableControlAccount = "Receivable";
        private readonly string VatControlAccount = "VAT";

        private readonly string SALESINVOICEVOUCHER = "Sales Invoice";

        private readonly string RETAIL_RECEIVABLE_ACCOUNT = "RETAIL RECEIVABLE ACCOUNT";
        private readonly string RETAIL_VAT_ACCOUNT = "RETAIL VAT ACCOUNT";

        private readonly string RETAIL = "RETAIL";
        private IMapper _mapper;

        public SMSContractsRepository(HalobizContext context, ILogger<SMSContractsRepository> logger, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }

        public SMSContractsRepository()
        {

        }


        public async Task<(bool isSuccess, object message)> AddNewContract(SMSContractDTO contractDTO)
        {
            var groupInvoiceNumber = await GenerateGroupInvoiceNumber();
            var branch = _configuration["OnlineBranchID"] ?? _configuration.GetSection("AppSettings:OnlineBranchID").Value;
            var office = _configuration["OnlineOfficeID"] ?? _configuration.GetSection("AppSettings:OnlineOfficeID").Value;
            var userId = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var customerDivisionId = 0l; 
            var leadDivisionId = _context.LeadDivisions.FirstOrDefault(x => x.Email == contractDTO.Email).Id;

            var suspectId = _context.Suspects.FirstOrDefault(x => x.Email == contractDTO.Email).Id;
            var contactId = _context.SuspectContacts.FirstOrDefault(x => x.SuspectId == suspectId).ContactId;

            var branchId = int.Parse(branch);
            var officeId = int.Parse(office);
            var endorsementType = _context.EndorsementTypes.FirstOrDefault(x => x.Caption.ToLower() == "service addition").Id;

            using var transaction = await _context.Database.BeginTransactionAsync();

            var leadDiv = _context.LeadDivisions.FirstOrDefault(x => x.Email == contractDTO.Email);

            var contractServices = new List<ContractService>();

            if (leadDiv == null)
                return (false, "lead profile does not exist");

            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var contractId = 0;

            var contractServiceForEndorsements = new List<ContractServiceForEndorsementReceivingDto>();
            var quoteServices = new List<QuoteServiceReceivingDTO>();

            try
            {

                var customerExists = _context.CustomerDivisions.Any(x => x.Email == contractDTO.Email);
                var onlineProfileExists = _context.OnlineProfiles.Any(x => x.Email == contractDTO.Email);

                if (!customerExists)
                {
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
                        Rcnumber = leadDiv.Rcnumber ?? "NULL",
                        PhoneNumber = leadDiv.PhoneNumber,
                        LogoUrl = leadDiv.LogoUrl
                    };

                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();

                    var customerContact = new CustomerContact
                    {
                        ContactDesignation = ContactDesignation.Self,
                        ContactPriority = ContactPriority.PrimaryContact,
                        ContactQualification = ContactQualification.DecisionMaker,
                        ContactId = contactId,
                        CustomerId = customer.Id
                    };

                    _context.CustomerContacts.Add(customerContact);
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

                    customerDivisionId = (int)customerDiv.Id;
                }

                if (onlineProfileExists && !customerExists)
                {
                    var onlineProfile = _context.OnlineProfiles.FirstOrDefault(x => x.Email.ToLower() == contractDTO.Email.ToLower());
                    onlineProfile.CustomerDivisionId = customerDivisionId;

                    await _context.SaveChangesAsync();
                }

                if (onlineProfileExists && customerExists)
                {
                    customerDivisionId = _context.CustomerDivisions.FirstOrDefault(x => x.Email.ToLower() == contractDTO.Email.ToLower()).Id;
                    var onlineProfile = _context.OnlineProfiles.FirstOrDefault(x => x.Email.ToLower() == contractDTO.Email.ToLower());
                    onlineProfile.CustomerDivisionId = customerDivisionId;

                    await _context.SaveChangesAsync();
                }
              


                foreach (var item in contractDTO.SMSContractServices)
                {
                    var service = _context.Services.FirstOrDefault(x => x.Id == item.ServiceId);
                    var amountWithoutVat = service.UnitPrice * item.Quantity;
                    var amount = 0.0;
                    var vat = 0.0;

                    if (service.IsVatable.Value)
                    {
                        var amountWithVat = amountWithoutVat + (0.075 * amountWithoutVat);
                        amount = amountWithVat;
                        vat = 0.075 * amountWithoutVat;
                    }
                    else
                    {
                        amount = amountWithoutVat;
                    }
                    contractServiceForEndorsements.Add(new ContractServiceForEndorsementReceivingDto
                    {
                        ActivationDate = DateTime.UtcNow.AddHours(1),
                        AdminDirectTie = new Random().Next(100_000_000, 1000_000_000).ToString(),
                        BillableAmount = amount,
                        IsApproved = true,
                        VAT = vat,
                        BranchId = branchId,
                        OfficeId = officeId,
                        ContractStartDate = item.ServiceStartDate,
                        ContractEndDate = item.ServiceEndDate,
                        UniqueTag = new Random().Next(100_000_000, 1000_000_000).ToString(),
                        CreatedById = userId,
                        UnitPrice = service.UnitPrice,
                        Dropofflocation = item.DropLocation,
                        DropoffDateTime = item.DropoffDateTime,
                        CustomerDivisionId = (long)customerDivisionId,
                        GroupInvoiceNumber = groupInvoiceNumber,
                        ServiceId = service.Id,
                        ContractId = contractId,
                        InvoicingInterval = TimeCycle.Adhoc,
                        Quantity = item.Quantity,
                        PaymentCycle = TimeCycle.Adhoc,
                        EndorsementTypeId = endorsementType,
                        PickupLocation = item.PickupLocation,
                        PickupDateTime = item.PickupTime,
                        GroupContractCategory = GroupContractCategory.GroupContractWithSameDetails,
                        FirstInvoiceSendDate = DateTime.UtcNow.AddHours(1),
                        FulfillmentEndDate = DateTime.UtcNow.AddHours(1),
                        FulfillmentStartDate = DateTime.UtcNow.AddHours(1),
                    });

                    quoteServices.Add(new QuoteServiceReceivingDTO
                    {
                        ActivationDate = DateTime.UtcNow.AddHours(1),
                        AdminDirectTie = new Random().Next(100_000_000, 1000_000_000).ToString(),
                        BillableAmount = amount,
                        BranchId = branchId,
                        OfficeId = officeId,
                        ContractStartDate = item.ServiceStartDate,
                        ContractEndDate = item.ServiceEndDate,
                        UniqueTag = new Random().Next(100_000_000, 1000_000_000).ToString(),
                        UnitPrice = service.UnitPrice,
                        Dropofflocation = item.DropLocation,
                        DropoffDateTime = item.DropoffDateTime,
                        ServiceId = service.Id,
                        InvoicingInterval = TimeCycle.Adhoc,
                        Quantity = item.Quantity,
                        PaymentCycle = TimeCycle.Adhoc,
                        PickupLocation = item.PickupLocation,
                        PickupDateTime = item.PickupTime,
                        VAT = vat,
                        FirstInvoiceSendDate = DateTime.UtcNow.AddHours(1),
                        FulfillmentStartDate = DateTime.UtcNow.AddHours(1),
                        FulfillmentEndDate = DateTime.UtcNow.AddHours(1),
                    });
                }

                var endorsementResult = await AddNewRetentionContractServiceForEndorsement(contractServiceForEndorsements);

                if (endorsementResult.isSuccess)
                {
                    contractId = int.Parse(endorsementResult.message.ToString());
                }

                var quote = new QuoteReceivingDTO
                {
                    GroupInvoiceNumber = groupInvoiceNumber,
                    GroupQuoteCategory = GroupQuoteCategory.GroupQuoteWithSameDetails,
                    IsConvertedToContract = true,
                    Version = Halobiz.Common.DTOs.ReceivingDTOs.VersionType.Latest,
                    LeadDivisionId = leadDivisionId,
                    QuoteServices = quoteServices,
                    
                };

                var quoteResult = await AddQuote(quote);

                if (!quoteResult.isSuccess)
                    throw new Exception(quoteResult.message.ToString());
                
                await transaction.CommitAsync();

                return (true, new
                {
                    CustomerDivisionId = customerDivisionId,
                    ContractId = contractId
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                transaction.Rollback();
                return (false, "An error has occured");

            }
        }

        private async Task<string> GenerateGroupInvoiceNumber()
        {
            try
            {
                var tracker = _context.GroupInvoiceTrackers.OrderBy(x => x.Id).FirstOrDefault();
                long newNumber = 0;
                if (tracker == null)
                {
                    newNumber = 1;
                    _context.GroupInvoiceTrackers.Add(new GroupInvoiceTracker() { Number = newNumber + 1 });
                    await _context.SaveChangesAsync();
                    return $"GINV{newNumber.ToString().PadLeft(7, '0')}";
                }
                else
                {
                    newNumber = tracker.Number;
                    tracker.Number = tracker.Number + 1;
                    _context.GroupInvoiceTrackers.Update(tracker);
                    await _context.SaveChangesAsync();
                    return $"GINV{newNumber.ToString().PadLeft(7, '0')}";
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<(bool isSuccess, object message)> AddNewRetentionContractServiceForEndorsement(List<ContractServiceForEndorsementReceivingDto> contractServiceForEndorsementDtos)
        {
            long contractId = 0;
            if (!contractServiceForEndorsementDtos.Any())
            {
                return (false, "No contract service specified");
            }


            var id = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            bool createNewContract = contractServiceForEndorsementDtos.All(x => x.ContractId == 0 && x.PreviousContractServiceId == 0);
            Contract newContract = null;
            List<ContractService> newContractServices = new List<ContractService>();

            if (createNewContract)
            {
                var contractDetail = contractServiceForEndorsementDtos.FirstOrDefault();

                //check if there is a pending contract addition for this guy
                if (_context.Contracts.Any(x => x.CustomerDivisionId == contractDetail.CustomerDivisionId && !x.IsApproved))
                {
                    return (false, "You have pending contract waiting approval");
                }

                newContract = new Contract
                {
                    CreatedAt = DateTime.Now,
                    CreatedById = id,
                    CustomerDivisionId = contractDetail.CustomerDivisionId,
                    Version = (int)VersionType.Latest,
                    GroupContractCategory = contractDetail.GroupContractCategory,
                    GroupInvoiceNumber = contractDetail.GroupInvoiceNumber,
                    IsApproved = true,
                    HasAddedSBU = true,
                    Caption = contractDetail.DocumentUrl
                };

                var entity = await _context.Contracts.AddAsync(newContract);
                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                    throw;
                }
                newContract = entity.Entity;
                contractId = newContract.Id;
            }

            //validate the admin direct pairs
            var (isValid, errorMessage) = await ValidateEndorsementRequest(createNewContract, contractServiceForEndorsementDtos);
            if (!isValid)
                return (false, errorMessage);


            foreach (var item in contractServiceForEndorsementDtos)
            {

                if (item.ContractId != 0)
                {
                    ContractServiceForEndorsement alreadyExists = null;
                    if (item.PreviousContractServiceId > 0)
                    {
                        alreadyExists = await _context.ContractServiceForEndorsements
                       .Where(x => x.ContractId == item.ContractId && x.PreviousContractServiceId == item.PreviousContractServiceId
                                   && x.CustomerDivisionId == item.CustomerDivisionId && x.ServiceId == item.ServiceId
                                   && !x.IsApproved && !x.IsDeclined && x.IsConvertedToContractService != true && !x.IsDeleted).FirstOrDefaultAsync();
                    }

                    if (alreadyExists != null)
                    {
                        return (false, $"There is already an endorsement request for the contract service with id {alreadyExists.Id}");
                    }
                }

                if (item.QuoteServiceId == 0)
                {
                    item.QuoteServiceId = null;
                }

                //check if this is nenewal and the previous contract has not
                var previouslyRenewal = await _context.ContractServiceForEndorsements
                                                .Include(x => x.EndorsementType)
                                                .Where(x => x.PreviousContractServiceId == item.PreviousContractServiceId && x.EndorsementType.Caption.Contains("retention"))
                                                .FirstOrDefaultAsync();

                if (previouslyRenewal != null)
                {
                    return (false, "There has been a retention on this contract service");
                }

                item.CreatedById = id;
                if (createNewContract)
                {
                    if (item.InvoicingInterval == TimeCycle.MonthlyProrata)
                    {
                        if (item.ContractEndDate.Value.AddDays(1).Day != 1)
                        {
                            return (false, $"Contract end date must be last day of month for tag {item.UniqueTag}");
                        }
                    }

                    try
                    {
                        var contractService = _mapper.Map<ContractService>(item);
                        contractService.ContractId = newContract.Id;
                        newContractServices.Add(contractService);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                  
                }
            }

            var entityToSaveList = _mapper.Map<List<ContractServiceForEndorsement>>(contractServiceForEndorsementDtos);

            try
            {
                if (createNewContract)
                {
                    await _context.ContractServices.AddRangeAsync(newContractServices);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    foreach (var item in entityToSaveList)
                    {
                        //check if it addition endorsement
                        var endorsementType = await _context.EndorsementTypes.Where(x => x.Id == item.EndorsementTypeId).FirstOrDefaultAsync();
                        if (endorsementType.Caption.ToLower().Contains("addition"))
                        {
                            //create a new contract service at draft stage
                            var contractService = _mapper.Map<ContractService>(item);
                            contractService.Version = (int)VersionType.Latest;
                            contractService.AdHocInvoicedAmount = contractService.BillableAmount.Value;
                            var entity = _context.ContractServices.Add(contractService);
                            await _context.SaveChangesAsync();
                            item.PreviousContractServiceId = entity.Entity.Id;
                            item.IsConvertedToContractService = true;
                            item.IsRequestedForApproval = false;
                        }

                        var savedEntity = await SaveContractServiceForEndorsement(item);
                        if (savedEntity == null)
                        {
                            return (false, "Some system errors occurred");
                        }

                        //bool successful = await _approvalService.SetUpApprovalsForContractModificationEndorsement(savedEntity, httpContext);
                        //if (!successful)
                        //{
                        //    return (false, "Could not set up approvals for service endorsement.");
                        //}
                    }
                }

                await _context.SaveChangesAsync();

                if (createNewContract)
                {
                    var contract = await _context.ContractServices
                            .Where(x => x.ContractId == newContract.Id)
                           .Include(x => x.Contract)
                           .ToListAsync();
                    return (true, contractId);
                }
                return (true, contractId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return (false, "An error has occured");
            }
        }

        public async Task<ContractServiceForEndorsement> SaveContractServiceForEndorsement(ContractServiceForEndorsement entity)
        {
            try
            {
                var contractServiceForEndorsementEntity = await _context.ContractServiceForEndorsements.AddAsync(entity);
                int affected = await _context.SaveChangesAsync();
                return affected > 0 ? entity : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        private async Task<(bool, string)> ValidateEndorsementRequest(bool isToCreateNewContract, List<ContractServiceForEndorsementReceivingDto> contractServices)
        {
            if (contractServices.Count == 1)
                return (true, null);

            try
            {
                //get the list of all services
                var services = await _context.Services.AsNoTracking().ToListAsync();

                //add this to indicate the type of services
                foreach (var item in contractServices)
                {
                    var service = services.Where(x => x.Id == item.ServiceId).FirstOrDefault();
                    if (service == null)
                        return (false, $"Invalid service specified for contract service with tag '{item.UniqueTag}'");

                    item.ServiceRelationshipEnum = service.ServiceRelationshipEnum;

                    if (!isToCreateNewContract && item.EndorsementTypeId != 2)
                    {
                        //check that the previous contract Id specified is valid
                        if (item.PreviousContractServiceId == 0)
                            return (false, $"Invalid PreviousContractServiceId specified for contract service with tag '{item.UniqueTag}'");

                        //check that PreviousContractServiceId is latest
                        if (!_context.ContractServices.Any(x => x.Id == item.PreviousContractServiceId && x.Version == (int)VersionType.Latest))
                        {
                            return (false, $"PreviousContractServiceId specified for contract service with tag '{item.UniqueTag}' is not latest");
                        }
                    }
                }

                //get all the directs
                var directs = contractServices.Where(x => x.ServiceRelationshipEnum == ServiceRelationshipEnum.Direct).ToList();
                foreach (var item in directs)
                {
                    //get the admin of this guy
                    var admin = contractServices.Where(x => x.AdminDirectTie == item.AdminDirectTie && x.ServiceRelationshipEnum == ServiceRelationshipEnum.Admin).FirstOrDefault();
                    if (admin == null) return (false, $"There is no admin matching pair for contract service with tag '{item.UniqueTag}'");

                    //check qty etc
                    if (item.Quantity != admin.Quantity)
                        return (false, $"No matching quantity with admin for contract service with tag '{item.UniqueTag}'");
                    if (item.ServiceId == admin.ServiceId)
                        return (false, $"Direct and admin cannot have the same service Id for contract service with tag '{item.UniqueTag}'");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

            return (true, null);
        }

        private async Task<(bool isSuccess, object message)> CreateContract(int contractId)
        {
            var contract = await _context.Contracts.AsNoTracking()
                               .Where(x => x.Id == contractId)
                               .Include(x => x.CustomerDivision)
                               .Include(x => x.ContractServices)
                                   .ThenInclude(x => x.Service)
                                .FirstOrDefaultAsync();

            var customerDivision = contract.CustomerDivision;
            var userId = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;

            foreach (var item in contract.ContractServices)
            {
                if (item.InvoicingInterval != (int)TimeCycle.Adhoc)
                {
                    var issuccess = await AccountsForContractServices(item, customerDivision, userId);
                    if (!issuccess)
                        return (false, "Could not create accounts after approval");
                }

            }

            //update the contract
            var contractToUpdate = await _context.Contracts.Where(x => x.Id == contractId).FirstOrDefaultAsync();
            contractToUpdate.IsApproved = true;
            _context.Update(contractToUpdate);
            await _context.SaveChangesAsync();

            return (true, "success");
        }

        public async Task<bool> AccountsForContractServices(ContractService contractService, CustomerDivision customerDivision, long userId)
        {
            try
            {
                if (contractService.InvoicingInterval != (int)TimeCycle.Adhoc)
                {
                    FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                        .FirstOrDefaultAsync(x => x.VoucherType == SALESINVOICEVOUCHER);

                    var (createSuccess, createMsg) = await CreateAccounts(
                                          contractService,
                                          customerDivision,
                                          (long)contractService.BranchId,
                                         (long)contractService.OfficeId,
                                         contractService.Service,
                                         accountVoucherType,
                                         null,
                                         userId,
                                         false, null);

                    var _serviceCode = contractService.Service?.ServiceCode;
                    var (invoiceSuccess, invoiceMsg) = await GenerateInvoices(contractService, customerDivision.Id, _serviceCode, userId, "");
                    var (amoSuccess, amoMsg) = await GenerateAmortizations(contractService, customerDivision, (double)contractService.BillableAmount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return false;
            }

            return true;
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
            isRetail = false;

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

        public async Task<(bool isSuccess, object message)> AddQuote(QuoteReceivingDTO quoteReceivingDTO)
        {
            Quote savedQuote = null;

            foreach (var quoteService in quoteReceivingDTO.QuoteServices)
            {
                if (quoteService.ContractStartDate == null || quoteService.ContractEndDate == null
                    || quoteService.InvoicingInterval == null || quoteService.PaymentCycle == null || quoteService.FirstInvoiceSendDate == null
                    || quoteService.FulfillmentStartDate == null || quoteService.FulfillmentEndDate == null)
                {
                    return (false, "Missing Quote Service Parameters");
                }
            }



                try
                {
                    var createdById = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;

                    var quote = _mapper.Map<Quote>(quoteReceivingDTO);
                    var quoteService = quote.QuoteServices;

                    quote.QuoteServices = null;
                    quote.CreatedById = createdById;
                    savedQuote = await SaveQuote(quote);

                    if (savedQuote == null)
                    {
                        return (false, "Some system errors occurred");
                    }

                    foreach (var item in quoteService)
                    {
                        item.QuoteId = savedQuote.Id;
                        item.CreatedById = createdById;

                        if (item.InvoicingInterval == (int)TimeCycle.MonthlyProrata)
                        {
                            if (item.ContractEndDate.Value.AddDays(1).Day != 1)
                            {
                                return (false, $"Contract end date must be last day of month for tag {item.UniqueTag}");
                            }
                        }
                    }

                    var savedSuccessfully = await SaveQuoteServiceRange(quoteService);
                    if (!savedSuccessfully)
                    {
                        return (false, "Some system errors occurred");
                    }

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return (false, "Some system errors occurred");
                }
            
            return (true, "success");
        }

        private async Task<bool> SaveQuoteServiceRange(IEnumerable<QuoteService> quoteServices)
        {
            await _context.QuoteServices.AddRangeAsync(quoteServices);
            await _context.SaveChangesAsync();
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

        private async Task<Quote> SaveQuote(Quote quote)
        {
            var quoteEntity = await _context.Quotes.AddAsync(quote);
             await _context.SaveChangesAsync();
            return quote;
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

        public async Task<(bool isSuccess, string message, List<InvoiceResult> invoiceResults)> GenerateInvoiceForContract(SMSCreateInvoiceDTO request)
        {

         using (var transaction = await _context.Database.BeginTransactionAsync())
                {

                    try
                    {
                        var contractServices = _context.ContractServices.Where(x => x.ContractId == request.ContractId);
                        var customerDivisionId = _context.OnlineProfiles.FirstOrDefault(x => x.Id == request.ProfileId).CustomerDivisionId;
                        var invoicesIds = new List<long>();
                        foreach (var item in contractServices)
                        {
                            var invoice = new InvoiceReceivingDTO
                            {
                                BillableAmount = item.BillableAmount.Value,
                                VAT = item.Vat.Value,
                                ContractServiceId = item.Id,
                                DateToBeSent = DateTime.UtcNow.AddHours(1),
                                CustomerDivisionId = customerDivisionId.Value,
                                UnitPrice = item.UnitPrice.Value,
                                EndDate = item.ContractEndDate.Value,
                                StartDate = item.ContractStartDate.Value,
                                Quantity = item.Quantity,
                            };
                            var response = await AddInvoice(invoice);
                            if (response.isSuccess)
                                invoicesIds.Add(response.result);
                        }

                       var result = await ConvertInvoiceToFinalInvoice(invoicesIds);
                      await transaction.CommitAsync();

                    return result;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);
                       await transaction.RollbackAsync();
                    return (false, "An error occured", null);
                    }

                }
           
        }

        public async Task<(bool isSuccess, string message, long result)> AddInvoice(InvoiceReceivingDTO invoiceReceivingDTO)
        {

                try
                {
                    var invoice = _mapper.Map<Invoice>(invoiceReceivingDTO);
                    var contractService = await _context.ContractServices
                                .Where(x => x.Id == invoice.ContractServiceId)
                                .Include(x => x.Contract)
                                .FirstOrDefaultAsync();

                    if (invoice.Value + contractService.AdHocInvoicedAmount > contractService.BillableAmount)
                    {
                        return (false, $"Total Invoiced value cannot be greater than the billable amount for contract with id: {contractService.Id}", 0);
                    }

                    var service = await _context.Services
                                    .FirstOrDefaultAsync(x => x.Id == contractService.ServiceId);

                    if (contractService.Contract.GroupContractCategory == GroupContractCategory.GroupContractWithSameDetails
                        || contractService.Contract.GroupContractCategory == GroupContractCategory.GroupContractWithIndividualDetails)
                    {
                        var invNo = contractService.Contract?.GroupInvoiceNumber;
                        //check if there is a previous invoice from with the group invoice number
                        var allThisGroupedInvoices = await _context.Invoices.Where(x => x.GroupInvoiceNumber == invNo).ToListAsync();
                        invoice.InvoiceNumber = $"{invNo}/{allThisGroupedInvoices.Count + 1}";

                    }
                    else
                    {
                        invoice.InvoiceNumber = $"INV{contractService.Id.ToString().PadLeft(8, '0')}";
                    }
                this.LoggedInUserId = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
                    invoice.ContractId = contractService.ContractId;
                    invoice.CreatedById = this.LoggedInUserId;
                    invoice.InvoiceType = (int)InvoiceType.New;
                    invoice.GroupInvoiceNumber = contractService.Contract?.GroupInvoiceNumber;
                    invoice.IsFinalInvoice = false;
                    invoice.TransactionId = GenerateTransactionNumber(service.ServiceCode, contractService);

                    var savedInvoice = await _context.Invoices.AddAsync(invoice);
                    await _context.SaveChangesAsync();
                    
                    return (true, "success", invoice.Id);
                }
                //catch (InvalidAdHocBillableAmount e)
                //{
                //    _logger.LogError(e.Message);
                //    _logger.LogError(e.StackTrace);
                //    await transaction.RollbackAsync();
                //    return CommonResponse.Send(ResponseCodes.FAILURE, null, e.Message);
                //}
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    throw;
                }
            
        }

        private string GenerateTransactionNumber(string serviceCode, ContractService contractService)
        {
            return String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber) ? $"{serviceCode}/{contractService.Id}"
            : $"{contractService.Contract?.GroupInvoiceNumber.Replace("GINV", "TRS")}/{contractService.Id}";

        }

        private async Task<IEnumerable<Invoice>> GetProformaInvoiceByContractServiceId(long contractServiceId)
        {

            List<Invoice> invoices = new List<Invoice>();

            try
            {
                var contractService = await _context.ContractServices
                    .Where(x => x.Id == contractServiceId && !x.IsDeleted)
                    .Include(x => x.Contract)
                    .FirstOrDefaultAsync();

                if (contractService == null)
                {
                    return new List<Invoice>();
                }

                if (String.IsNullOrWhiteSpace(contractService.Contract?.GroupInvoiceNumber))
                {
                    invoices = await _context.Invoices
                    .Include(x => x.Receipts)
                        .Where(x => x.ContractServiceId == contractServiceId
                                    && (bool)x.IsFinalInvoice == false && x.IsDeleted == false)
                        .OrderBy(x => x.StartDate)
                        .ToListAsync();
                }
                else
                {
                    invoices = await _context.Invoices
                    .Include(x => x.Receipts)
                        .Where(x => x.GroupInvoiceNumber == contractService.Contract.GroupInvoiceNumber
                                                                && (bool)x.IsFinalInvoice == false && !x.IsDeleted)
                        .OrderBy(x => x.StartDate)
                        .ToListAsync();

                    return invoices;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex);
                return new List<Invoice>();
            }

            return invoices;
        }

        public async Task<(bool isSuccess, string message, List<InvoiceResult> invoiceResults)> ConvertInvoiceToFinalInvoice(List<long> invoicesIds)
        {
            var invoiceResults = new List<InvoiceResult>();
                try
                {

                    //get the hightest 
                    var hightestGroupingId = await _context.Invoices.MaxAsync(x => x.AdhocGroupingId);
                    var adhocGroupingId = 1 + hightestGroupingId ?? 0;

                    foreach (var invoiceId in invoicesIds)
                    {
                        var invoice = await _context.Invoices
                                       .Where(x => x.Id == invoiceId)
                                       .Include(x => x.ContractService)
                                        .ThenInclude(x => x.QuoteService)
                                        .FirstOrDefaultAsync();

                        var contractService = invoice.ContractService;

                        contractService.AdHocInvoicedAmount += invoice.Value;
                        if (contractService.AdHocInvoicedAmount > contractService.BillableAmount)
                        {
                            return (false, "Total invoices amount exceeds contract service billable amount", null);
                        }

                        _context.ContractServices.Update(contractService);
                        await _context.SaveChangesAsync();

                        var customerDivision = await _context.CustomerDivisions
                                            .Where(x => x.Id == invoice.CustomerDivisionId)
                                            .Include(x => x.Customer)
                                                .ThenInclude(x => x.GroupType)
                                            .FirstOrDefaultAsync();

                        this.isRetail = customerDivision.Customer.GroupName == RETAIL;

                        FinanceVoucherType accountVoucherType = await _context.FinanceVoucherTypes
                                .FirstOrDefaultAsync(x => x.VoucherType == this.SALESINVOICEVOUCHER);

                        var service = await _context.Services
                                        .FirstOrDefaultAsync(x => x.Id == contractService.ServiceId);

                    invoice.Value = contractService.AdHocInvoicedAmount;

                    var (success, msg) = await CreateAccounts(contractService, customerDivision, (long)contractService?.BranchId, (long)contractService?.OfficeId,
                        service, accountVoucherType, null, LoggedInUserId, false, invoice);
                    if (!success)
                        return (false, $"Account posting issue. {msg}", null);


                    invoice.IsFinalInvoice = true;
                    invoice.AdhocGroupingId = adhocGroupingId;
                    invoice.IsInvoiceSent = true;

                    _context.Invoices.Update(invoice);

                    var invoiceRes = new InvoiceResult
                    {
                        InvoiceId = invoice.Id,
                        InvoiceNumber = invoice.InvoiceNumber,
                        InvoiceValue = invoice.Value
                    };

                    invoiceResults.Add(invoiceRes);
                    await _context.SaveChangesAsync();
                    var invoiceTransferDTO = _mapper.Map<InvoiceTransferDTO>(invoice);

                    await GenerateAmortizations(contractService, customerDivision,
                                    invoice.Value);
                    }

                    await _context.SaveChangesAsync();

                    return (true, "success", invoiceResults );
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                     throw;
                }
            
        }

        public async Task<(bool isSuccess, object message)> AddServiceToContract(SMSContractServiceDTO contractDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var branch = _configuration["OnlineBranchID"] ?? _configuration.GetSection("AppSettings:OnlineBranchID").Value;
                var office = _configuration["OnlineOfficeID"] ?? _configuration.GetSection("AppSettings:OnlineOfficeID").Value;
               

                var userId = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
                var customerDivisionId = _context.Contracts.FirstOrDefault(x => x.Id == contractDTO.ContractId).CustomerDivisionId;
                var contractServiceForEndorsements = new List<ContractServiceForEndorsementReceivingDto>();
                var quoteServices = new List<QuoteServiceReceivingDTO>();
                var email = _context.CustomerDivisions.FirstOrDefault(x => x.Id == customerDivisionId).Email;
                var leadDivisionId = _context.LeadDivisions.FirstOrDefault(x => x.Email == email).Id;

                var branchId = int.Parse(branch);
                var officeId = int.Parse(office);
                var endorsementType = _context.EndorsementTypes.FirstOrDefault(x => x.Caption.ToLower() == "service addition").Id;
                var groupInvoiceNumber = _context.Contracts.FirstOrDefault(x => x.Id == contractDTO.ContractId).GroupInvoiceNumber;
                foreach (var item in contractDTO.SMSContractServices)
                {
                    var service = _context.Services.FirstOrDefault(x => x.Id == item.ServiceId);
                    var amountWithoutVat = service.UnitPrice * item.Quantity;
                    var amount = 0.0;
                    var vat = 0.0;

                    if (service.IsVatable.Value)
                    {
                        var amountWithVat = amountWithoutVat + (0.075 * amountWithoutVat);
                        amount = amountWithVat;
                        vat = 0.075 * amountWithoutVat;
                    }
                    else
                    {
                        amount = amountWithoutVat;
                    }
                    contractServiceForEndorsements.Add(new ContractServiceForEndorsementReceivingDto
                    {
                        ActivationDate = DateTime.UtcNow.AddHours(1),
                        AdminDirectTie = new Random().Next(100_000_000, 1000_000_000).ToString(),
                        BillableAmount = amount,
                        IsApproved = true,
                        VAT = vat,
                        BranchId = branchId,
                        OfficeId = officeId,
                        ContractStartDate = item.ServiceStartDate,
                        ContractEndDate = item.ServiceEndDate,
                        UniqueTag = new Random().Next(100_000_000, 1000_000_000).ToString(),
                        CreatedById = userId,
                        UnitPrice = service.UnitPrice,
                        Dropofflocation = item.DropLocation,
                        DropoffDateTime = item.DropoffDateTime,
                        CustomerDivisionId = (long)customerDivisionId,
                        GroupInvoiceNumber = groupInvoiceNumber,
                        ServiceId = service.Id,
                        ContractId = contractDTO.ContractId,
                        InvoicingInterval = TimeCycle.Adhoc,
                        Quantity = item.Quantity,
                        PaymentCycle = TimeCycle.Adhoc,
                        EndorsementTypeId = endorsementType,
                        PickupLocation = item.PickupLocation,
                        PickupDateTime = item.PickupTime,
                        GroupContractCategory = GroupContractCategory.GroupContractWithSameDetails,
                        FirstInvoiceSendDate = DateTime.UtcNow.AddHours(1),
                        FulfillmentEndDate = DateTime.UtcNow.AddHours(1),
                        FulfillmentStartDate = DateTime.UtcNow.AddHours(1),
                    });


                    quoteServices.Add(new QuoteServiceReceivingDTO
                    {
                        ActivationDate = DateTime.UtcNow.AddHours(1),
                        AdminDirectTie = new Random().Next(100_000_000, 1000_000_000).ToString(),
                        BillableAmount = amount,
                        BranchId = branchId,
                        OfficeId = officeId,
                        ContractStartDate = item.ServiceStartDate,
                        ContractEndDate = item.ServiceEndDate,
                        UniqueTag = new Random().Next(100_000_000, 1000_000_000).ToString(),
                        UnitPrice = service.UnitPrice,
                        Dropofflocation = item.DropLocation,
                        DropoffDateTime = item.DropoffDateTime,
                        ServiceId = service.Id,
                        InvoicingInterval = TimeCycle.Adhoc,
                        Quantity = item.Quantity,
                        PaymentCycle = TimeCycle.Adhoc,
                        PickupLocation = item.PickupLocation,
                        PickupDateTime = item.PickupTime,
                        VAT = vat,
                        FirstInvoiceSendDate = DateTime.UtcNow.AddHours(1),
                        FulfillmentStartDate = DateTime.UtcNow.AddHours(1),
                        FulfillmentEndDate = DateTime.UtcNow.AddHours(1),
                    });
                }

                var endorsementResult = await AddNewRetentionContractServiceForEndorsement(contractServiceForEndorsements);

               
                var quote = new QuoteReceivingDTO
                {
                    GroupInvoiceNumber = groupInvoiceNumber,
                    GroupQuoteCategory = GroupQuoteCategory.GroupQuoteWithSameDetails,
                    IsConvertedToContract = true,
                    Version = Halobiz.Common.DTOs.ReceivingDTOs.VersionType.Latest,
                    LeadDivisionId = leadDivisionId,
                    QuoteServices = quoteServices,

                };

                var quoteResult = await AddQuote(quote);

                if (!quoteResult.isSuccess)
                    throw new Exception(quoteResult.message.ToString());

                await transaction.CommitAsync();

                var contractServiceId = _context.ContractServices.Where(x => x.ContractId == contractDTO.ContractId).OrderByDescending(x => x.Id).FirstOrDefault().Id;

                return (true, new
                {
                    CustomerDivisionId = customerDivisionId,
                    ContractServiceId = contractServiceId
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                transaction.Rollback();
                return (false, "An error has occured");

            }
        }
    }

    public enum VersionType
    {
        Latest, Previous, Draft
    }

    public class MonthsAndYears
    {
        public int Year { get; set; }
        public long Month { get; set; }
        public double Amount { get; set; }
    }

    public class InvoiceResult
    {
        public string InvoiceNumber { get; set; }
        public double InvoiceValue { get; set; }
        public long InvoiceId { get; set; }
    }
}
