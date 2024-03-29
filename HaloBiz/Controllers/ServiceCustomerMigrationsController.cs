﻿using ClosedXML.Excel;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using GoogleMaps.LocationServices;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.Helpers;
using HaloBiz.Helpers;
using HaloBiz.MigrationsHelpers;
using HaloBiz.MyServices.LAMS;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Models.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace HaloBiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups,92)]

    public class ServiceCustomerMigrationsController : ControllerBase
    {
        private ILogger<ServiceCustomerMigrationsController> _logger;
        private readonly IWebHostEnvironment _environment;
        private HalobizContext _context;
        private ILeadConversionService _leadConversionService;
        private List<State> _states = new List<State>();
        private List<Service> _services = new List<Service>();
        private Service MigrationService = null;

        private HttpSender sender;
        List<Customero> customers = new List<Customero>();
        long userIdToUse = 0;
        List<ServiceTypes> _serviceTypes = new List<ServiceTypes>();

        public ServiceCustomerMigrationsController(ILogger<ServiceCustomerMigrationsController> logger,
            ILeadConversionService leadConversionService,
            IWebHostEnvironment environment,
            HalobizContext context)
        {
            _logger = logger;
            _context = context;
            sender = new HttpSender();
            _leadConversionService = leadConversionService;
            _environment = environment;
        }

      

        [HttpGet("RunMigration/{page}/{cutoffdate}")]
        public async Task<ApiCommonResponse> RunMigration(int page, string cutoffdate)
        {
            try
            {
                try
                {
                    DateTime.Parse(cutoffdate);
                }
                catch (Exception)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Invalid date. Use the format yyyy-MM-dd");
                }

                 setServiceTypes();
                _states = _context.States.Include(x => x.Lgas).ToList();
                _services = _context.Services.ToList();
                MigrationService = _context.Services.Where(x => x.Name == "Migrations").FirstOrDefault();

                //get the userId
                var user = await _context.UserProfiles.Where(x => x.Email.ToLower().Contains("seeder")).FirstOrDefaultAsync();
                userIdToUse = user.Id;
                
                if (MigrationService == null)
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "No service with name: Migrations");


                var cb = await sender.getServiceContract(page);
                //var problemContracts = new string[] {"11/01/361-47","20/01/017-07","20/01/016","18/05/035-02" };
                //var contracts = cb.Items.Where(x=>problemContracts.Contains(x.ContractNumber)).ToList();
               
                var contracts = cb.Items.ToList();

                if (!contracts.Any())
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"No contract fetched for page {page}");
                }

                _logger.LogInformation("MIGRATION OF CUSTOMER AND CONTRACT STARTED");
               return await saveContracts(contracts, page, cutoffdate);
            }
            catch (Exception ex)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }

        }

       
        
        private async Task<ApiCommonResponse> saveContracts(List<Contracto> contracts, int page, string cutOffDateStr)
        {
            int totalSaved = 0, previouslySaved = 0, errorLaden = 0;

            var defaultOffice = _context.Offices.FirstOrDefault();
            var group = _context.GroupTypes.Where(x => x.Caption == "Corporate").FirstOrDefault();
            var designation = _context.Designations.FirstOrDefault();

            _logger.LogInformation($"Total of {contracts.Count} contracts fetched");

            List<ServiceContractItem> allContractServiceItems = new List<ServiceContractItem>();
           

            foreach (var _contract in contracts)
            {
                try
                {
                    //var transaction = _context.Database.BeginTransaction();

                    //check if this contract exist previously and skip
                    if (_context.Contracts.Any(x => x.Caption == _contract.ContractNumber))
                    {
                        ++previouslySaved;
                        continue;
                    }                 

                    //11/01/079-01
                    var contractServiceDetails = await sender.getServiceContractDetail(_contract.ContractNumber);//
                    var contractServices = contractServiceDetails.ServiceContractItems;

                    int counter = 0;
                    List<ServiceContractItem> directsAndStandalone = new List<ServiceContractItem>();
                    List<ServiceContractItem> admins = new List<ServiceContractItem>();
                    List<ServiceContractItem> sortedOutServices = new List<ServiceContractItem>();
                    List<ServiceContractItem> finalSortedOutServices = new List<ServiceContractItem>();
                    List<ServiceContractItem> mergedAdminCasesSorted = new List<ServiceContractItem>();

                    //check if this customer exist and fetch
                    var customer = customers.Where(x => x.CustomerNumber == _contract.CustomerNumber).FirstOrDefault();
                    if (customer == null)
                    {
                        //fetch this customer from the api
                        customer = sender.getCustomerWithCustomerNumber(_contract.CustomerNumber).GetAwaiter().GetResult();
                        (long customerId, long divisionId, CustomerDivision customerDivision) = await SaveCustomer(customer, group.Id, designation.Id);
                        customer.CustomerId = customerId;
                        customer.CustomerDivisionId = divisionId;
                        customer.customerDivision = customerDivision;
                        customers.Add(customer);
                    }

                    var lastDate = _contract.StartDate;
                    while (lastDate < DateTime.Today)
                    {
                        lastDate = lastDate.AddYears(1);
                    }

                    var startDate = lastDate.AddYears(-1);
                    lastDate = lastDate.AddDays(-1);

                    //cut off position parameters
                    var cutOffDate = DateTime.Parse(cutOffDateStr);
                    var input = new AccountBalanceInput
                    {
                        SubAccount = _contract.SubAccount,
                        GLAccount = customer.GLAccount,
                        FinancialYear = cutOffDate.Year,
                        AsAtDate = cutOffDate
                    };

                    var migrationContractSaved = await setCutOffMigration(_contract.ContractNumber, customer.customerDivision, input, customer.CustomerNumber, _context, defaultOffice, cutOffDate);
                    //create contract
                    if (!migrationContractSaved)
                    {
                        continue;
                    }

                    //save the contract now
                    var contract_ = new Contract
                    {
                        CustomerDivisionId = customer.CustomerDivisionId,
                        Caption = _contract.ContractNumber,
                        GroupContractCategory = GroupContractCategory.GroupContractWithSameDetails,
                        GroupInvoiceNumber = await GenerateGroupInvoiceNumber(),
                        CreatedById = userIdToUse,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                    };

                    var entity = _context.Contracts.Add(contract_);
                    System.Threading.Thread.Sleep(5000);
                    var afftected = _context.SaveChanges();

                    if (afftected == 0) continue;

                    var contractId = contract_.Id;

                    //create location for this customer
                    await createLocation(_contract, customer.CustomerDivisionId);

                    foreach (var contractService in contractServices)
                    {
                        contractService.SerialNo = ++counter;
                        contractService.ContractId = contractId;
                        contractService.StartDate = startDate;
                        contractService.EndDate = lastDate;
                        contractService.CustomerName = customer.Name;


                        var description = contractService.Description.ToLower();
                        if (description.Contains("supervisor") && description.Contains("direct"))
                        {
                            contractService.ServiceType = "SUPDR";
                        }
                        else if (description.Contains("supervisor") && description.Contains("admin"))
                        {
                            contractService.ServiceType = "SUPAD";
                        }
                        else if (description.Contains("dog") && description.Contains("handler") && description.Contains("direct"))
                        {
                            contractService.ServiceType = "DH";
                        }
                        else if (description.Contains("dog") && description.Contains("handler") && description.Contains("admin"))
                        {
                            contractService.ServiceType = "DHA";
                        }
                        else if (description.Contains("supervisor"))
                        {
                            contractService.ServiceType = "SUPDR";
                        }

                        //get the corrresponding service
                        var apiContractService = _serviceTypes.Where(x => x.ServiceType == contractService.ServiceType).FirstOrDefault();

                        if (apiContractService == null)
                        {
                            continue;
                        }

                        //check if this contract service has id
                        if (apiContractService.ServiceId == 0)
                        {
                            apiContractService = GetServiceType(apiContractService);
                        }

                        contractService.Enum = apiContractService.Enum;
                        contractService.AdminDirectTie = apiContractService.AdminDirectTie;
                        contractService.ApiContractService = apiContractService;

                        if (contractService.Enum == ServiceRelationshipEnum.Admin) admins.Add(contractService);
                        else { directsAndStandalone.Add(contractService); }
                    }

                    var backwardTimer = 1000;
                    foreach (var _contractService in directsAndStandalone)
                    {
                        //prepare the direct details
                        if (_contractService.Enum == ServiceRelationshipEnum.Direct)
                        {
                            //get the corresponding admin
                            var adminMatch = admins.Where(x => x.Quantity == _contractService.Quantity && x.AdminDirectTie == _contractService.AdminDirectTie).FirstOrDefault();
                            if (adminMatch != null)
                            {
                                backwardTimer += backwardTimer;
                                //create the ties for the contractService
                                var adminDirectMatch = DateTime.Now.AddMilliseconds(backwardTimer).ToString("yyyyMMddHHmmss");

                                //give it the admin direct tie
                                _contractService.AdminDirectTie = adminDirectMatch;
                                adminMatch.AdminDirectTie = adminDirectMatch;

                                //add this to the list
                                sortedOutServices.Add(adminMatch);
                                //now remove this admin from the admin list
                                var index = admins.FindIndex(x => x.SerialNo == adminMatch.SerialNo);
                                if (index != -1)
                                {
                                    admins.RemoveAt(index);
                                }
                            }
                            //add the direct
                            sortedOutServices.Add(_contractService);
                        }
                        else
                        {
                            //it is standalone
                            sortedOutServices.Add(_contractService);
                        }
                    }


                    ServiceContractItem _adminMatch = null;
                    finalSortedOutServices.AddRange(sortedOutServices);

                    foreach (var item in admins)
                    {
                        var itemStr = JsonConvert.SerializeObject(item);
                        var itsDirects = sortedOutServices.Where(x => x.Enum == ServiceRelationshipEnum.Direct && x.AdminDirectTie == item.AdminDirectTie);
                        //spit this amoung the direct remaining
                        if (itsDirects.Any())
                        {
                            foreach (var direct in itsDirects)
                            {
                                backwardTimer += backwardTimer;
                                //create the ties for the contractService
                                var adminDirectTie = DateTime.Now.AddMilliseconds(backwardTimer).ToString("yyyyMMddHHmmss");

                                var adminToMatch = JsonConvert.DeserializeObject<ServiceContractItem>(itemStr);
                                adminToMatch.Quantity = direct.Quantity;
                                adminToMatch.Description = adminToMatch.Description + "--Split admin";
                                adminToMatch.AdminDirectTie = adminDirectTie;
                                finalSortedOutServices.Add(adminToMatch);

                                //change the tie of the direct in question
                                direct.AdminDirectTie = adminDirectTie;
                                //find the index
                                int index = finalSortedOutServices.FindIndex(x => x.SerialNo == direct.SerialNo);
                                if (index != -1)
                                {
                                    finalSortedOutServices[index] = direct;
                                }
                            }
                        }
                    }

                    foreach (var service in finalSortedOutServices)
                    {
                        if (service.Enum == ServiceRelationshipEnum.Direct)
                        {
                            _adminMatch = finalSortedOutServices.Where(x => x.AdminDirectTie == service.AdminDirectTie && x.Enum != ServiceRelationshipEnum.Direct).FirstOrDefault();
                            if (_adminMatch == null)
                            {
                                _adminMatch = admins.Where(x => x.Quantity == service.Quantity).FirstOrDefault();
                                if (_adminMatch != null)
                                {
                                    var adminDirectTie = DateTime.Now.AddMilliseconds(-1000).ToString("yyyyMMddHHmmss");

                                    //we have found a pair
                                    _adminMatch.AdminDirectTie = adminDirectTie;
                                    service.AdminDirectTie = adminDirectTie;
                                    mergedAdminCasesSorted.Add(_adminMatch);

                                    //remove this from the list
                                    var index = admins.FindIndex(x => x.SerialNo == _adminMatch.SerialNo);
                                    if (index != -1)
                                    {
                                        admins.RemoveAt(index);
                                    }
                                }
                                else
                                {
                                    var adminDirectTie = DateTime.Now.AddMilliseconds(-2000).ToString("yyyyMMddHHmmss");

                                    //we need to create a corresponding admin match
                                    var serviceType = _serviceTypes.Where(x => x.AdminDirectTie == service.AdminDirectTie && x.Enum == ServiceRelationshipEnum.Admin).FirstOrDefault();
                                    //get the corrresponding service
                                    var apiContractService = _serviceTypes.Where(x => x.ServiceType == serviceType.ServiceType).FirstOrDefault();
                                    //check if this contract service has id
                                    if (apiContractService.ServiceId == 0)
                                    {
                                        apiContractService = GetServiceType(apiContractService);
                                    }

                                    var adminNew = new ServiceContractItem
                                    {
                                        Enum = ServiceRelationshipEnum.Admin,
                                        Quantity = 0,
                                        Amount = 0,
                                        Description = "Generated admin: " + service.Description,
                                        AdminDirectTie = adminDirectTie,
                                        ApiContractService = apiContractService,
                                        ContractId = contractId,
                                        StartDate = service.StartDate,
                                        EndDate = service.EndDate,                                        
                                    };

                                    service.AdminDirectTie = adminDirectTie;
                                    mergedAdminCasesSorted.Add(adminNew);
                                }
                            }
                        }
                        mergedAdminCasesSorted.Add(service);
                    }

                    foreach (var item in mergedAdminCasesSorted)
                    {
                       var (success, id) = await postContractService(item, customer, defaultOffice, _context, cutOffDateStr);
                        if (!success)
                        {
                            //delete this contract service
                            continue;
                        }

                        item.ContractServiceId = id;
                    }

                    allContractServiceItems.AddRange(mergedAdminCasesSorted);

                    //save the contract services at this point
                   // transaction.Commit();
                    ++totalSaved;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.StackTrace);
                }
            }

            var obj = new
            {
                TotalContract = contracts.Count,
                TotalSaved = totalSaved,
                SkippedForDuplicates = previouslySaved,
                Errorneous = errorLaden
            };

            var report = $"Total contracts: {contracts.Count}, Saved to db: {totalSaved}; Skipped for duplicate: {previouslySaved}; With error {errorLaden}";
           
            _logger.LogInformation(report);
            return CommonResponse.Send(ResponseCodes.SUCCESS, obj, report);

        }

        private int getBillingCycle(string cycle)
        {
            switch (cycle)
            {
                case "m":  return (int) TimeCycle.Monthly;
                case "q": return (int)TimeCycle.Quarterly;
                case "b": return (int)TimeCycle.BiAnnually;
                case "y": return (int)TimeCycle.Annually;
                case "t": return (int)TimeCycle.Adhoc;
                default: return (int)TimeCycle.Monthly;
            }
        }

        private async Task<(bool,long)> postContractService(ServiceContractItem contractService, Customero customer, Office defaultOffice, HalobizContext _context, string cutoffdate)
        {
            var thisMonth = contractService.StartDate.Month;
            var invoiceSendDate = contractService.StartDate.AddDays(5);
            while (invoiceSendDate.Month - thisMonth != 0)
            {
                invoiceSendDate = invoiceSendDate.AddDays(-1);
            }

            int cycle = 0;
            try
            {
                cycle = getBillingCycle(contractService.BillingCycle?.ToLower()); //get confirmation to adjust 
            }
            catch (Exception ex)
            {
                var r = ex.ToString();
            }

            var amount = Math.Abs(contractService.Amount);
            amount = Math.Round(amount, 2);
            var service = new ContractService();
            service.AdminDirectTie = contractService.AdminDirectTie;
            service.ServiceId = contractService.ApiContractService.ServiceId;
            service.BillableAmount = amount;
            service.ActivationDate = contractService.StartDate;
            service.ContractStartDate = contractService.StartDate;
            service.ContractEndDate = contractService.EndDate;
            service.ContractId = contractService.ContractId;
            service.UnitPrice = contractService.UnitPrice;
            service.Vat = Math.Round(contractService.Taxable == 1 ? 0.075 * amount : 0, 2);
            service.Quantity = contractService.Quantity;
            service.UniqueTag = $"{contractService.Description}@{contractService.ContractId}";
            service.BranchId = defaultOffice.BranchId;
            service.OfficeId = defaultOffice.Id;
            service.CreatedById = userIdToUse;
            service.InvoicingInterval = cycle;
            service.FirstInvoiceSendDate = invoiceSendDate;
            service.WHTLoadingValue = contractService.WHTLoadingValue;

            var saveContractEntity = _context.ContractServices.Add(service);

            System.Threading.Thread.Sleep(3000);

            var affected = _context.SaveChanges();
            if (affected > 0)
            {
                var contractServiceCreated = saveContractEntity.Entity;
                var _contractService = _context.ContractServices
                        .Include(x=>x.Service)
                        .Include(x=>x.Contract)
                        .Where(x => x.Id == contractServiceCreated.Id)
                        .FirstOrDefault();

                _logger.LogInformation($"Saved for contract with ID:{_contractService.ContractId}, Service: {JsonConvert.SerializeObject(contractService)}");
               
               var success =  await _leadConversionService.onMigrationAccountsForContracts(_contractService,
                                                        customer.customerDivision,
                                                         _contractService.ContractId, userIdToUse, cutoffdate);
                if (!success)
                    return (false, 0);
                return (true,contractServiceCreated.Id);
            }

            return (true, 0);
        }

        private async Task<bool> CreditNoteEndorsement(ContractService currentContractService,
                                                        CustomerDivision customerDivision,
                                                        Service service, long loggedInUserId)
        {

            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType == "Credit Note");          

            var (success, msg) = await _leadConversionService.CreateAccounts(
                                            currentContractService,
                                            customerDivision,
                                            (long)currentContractService.BranchId,
                                            (long)currentContractService.OfficeId,
                                            service,
                                            financialVoucherType,
                                            null,
                                            loggedInUserId,
                                            true,
                                            null, true);

            return success;
        }

        private async Task<bool> setCutOffMigration(string contractNo, CustomerDivision division, AccountBalanceInput input, string customerNumber, HalobizContext _context, Office defaultOffice, DateTime cutOffDate)
        {
            //first check if a cut off migration exist for this customer previously on this contract
            var caption = customerNumber + "_Migration";
            try
            {
                if (_context.Contracts.Any(x => x.Caption == caption))
                {
                    return true;
                }

                var accounts = await sender.getCutOffPosition(input);
                var customerInput = accounts.Where(x => x.SubLedgerCode == customerNumber).FirstOrDefault();
                if (customerInput == null)
                    return true;

                if(customerInput.Amount == 0)
                {
                    //customer is not owing
                    return true;
                }

                //save the contract now
                var contract_ = new Contract
                {
                    CustomerDivisionId = division.Id,
                    Caption = caption,
                    GroupContractCategory = GroupContractCategory.IndividualContract,
                    GroupInvoiceNumber = null,
                    CreatedById = userIdToUse,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false,
                };

                var entity = _context.Contracts.Add(contract_);
              //  System.Threading.Thread.Sleep(5000);
                var afftected = _context.SaveChanges();
                long contractId = 0;
                if (afftected > 0)
                {
                    var amount = Math.Abs(customerInput.Amount);
                    amount = Math.Round(amount, 2);
                    contractId = entity.Entity.Id;
                    var saveContractEntity = _context.ContractServices.Add(new ContractService
                    {
                        ServiceId = MigrationService.Id,
                        BillableAmount = amount,
                        ActivationDate = cutOffDate,
                        ContractStartDate = cutOffDate,
                        ContractEndDate = cutOffDate,
                        ContractId = (long)contractId,
                        UnitPrice = amount,
                        Vat = 0,
                        Quantity = 1,
                        InvoicingInterval = (int)TimeCycle.OneTime,
                        UniqueTag = $"Cutoff_{caption}",
                        BranchId = defaultOffice.BranchId,
                        OfficeId = defaultOffice.Id,
                        CreatedById = userIdToUse,
                        FirstInvoiceSendDate = cutOffDate.AddDays(1),
                    });

                   // System.Threading.Thread.Sleep(3000);

                    var affected = _context.SaveChanges();
                    if (affected > 0)
                    {
                        var contractServiceCreated = saveContractEntity.Entity;
                        var _contractService = _context.ContractServices
                                .Include(x => x.Service)
                                .Include(x => x.Contract)
                                .Where(x => x.Id == contractServiceCreated.Id).FirstOrDefault();

                        if (customerInput.Amount < 0)
                        {
                            //customer is not owing
                           var result = await CreditNoteEndorsement(_contractService, division, _contractService.Service, userIdToUse);
                            if (!result)
                                throw new Exception("Credit not was not created");                            
                        }
                        else
                        {
                            await _leadConversionService.onMigrationAccountsForContracts(_contractService,
                                                                    division,
                                                                     _contractService.ContractId, userIdToUse, cutOffDate.ToString());
                        }                       
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }

            return true;
        }

        private async Task<(State, Lga)> createLocation(Contracto contract, long customerDivisionId)
        {
            string address = "";
            try
            {
                var googleKey = "AIzaSyCQuetprs2UHb_zKSXznyumyfvw95ERCDY";

                if (contract.AddressLine1.ToLower().Contains(contract.AddressLine2.ToLower()))
                {
                    address = string.Concat(contract.AddressLine1, " ", contract.AddressLine2);
                }
                else
                {
                    address = string.Concat(contract.AddressLine1, " ", contract.AddressLine2, " ", contract.AddressLine3);
                }

                //check if this location exist previously
                var location = _context.Locations.Where(x => x.CustomerDivisionId == customerDivisionId && x.Street == address).FirstOrDefault();
                if (location != null)
                {
                    var state_ = _states.Where(x => x.Id == location.StateId).FirstOrDefault();
                    var lga_ = state_.Lgas.Where(x => x.Id == location.LgaId).FirstOrDefault();
                    return (state_, lga_);
                }

                var locationService = new GoogleLocationService(googleKey);
                var point = locationService.GetLatLongFromAddress(address);
                var (state, lga) = getStateAndLga(address.ToLower());

                await _context.Locations.AddAsync(new Location
                {
                    Longitutude = point?.Longitude,
                    Latitude = point?.Longitude,
                    CustomerDivisionId = customerDivisionId,
                    CreatedById = userIdToUse,
                    Name = contract.BillTo,
                    Description = contract.Description,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    Street = address,
                    StateId = state == null ? 1 : state.Id,
                    LgaId = lga == null ? 1 : lga.Id,
                });

                await _context.SaveChangesAsync();

                return (state, lga);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private (State, Lga) getStateAndLga(string lastAddressLine)
        {
            if (lastAddressLine == "portharcourt") lastAddressLine = "port harcourt";

            State selectedState = null;
            foreach (var item in _states)
            {
                if (lastAddressLine.Contains(item.Name.ToLower()) || lastAddressLine.Contains(item.Capital.ToLower()))
                {
                    selectedState = item;
                    break;
                }
            }

            return selectedState == null ? (null, null) : (selectedState, selectedState.Lgas.FirstOrDefault());
        }

        private ServiceTypes GetServiceType(ServiceTypes apiContractService)
        {
            var names = apiContractService.ServiceTypeName; //.Split(" ");
            var systemContractService = _context.Services.Where(x => x.Name.Contains(names)).FirstOrDefault();
            if (systemContractService != null)
            {
                apiContractService.ServiceId = systemContractService.Id;
                apiContractService.ServiceTypeName = systemContractService.Name;
                apiContractService.IsVatable = (bool)systemContractService.IsVatable;
                var index = _serviceTypes.FindIndex(x => x.ServiceTypeName == apiContractService.ServiceTypeName);
                _serviceTypes[index] = apiContractService;
            }
            else
            {
                throw new Exception($"The service with name: {apiContractService.ServiceTypeName} does not exist on the system");
            }

            return apiContractService;
        }

        private async Task<(long, long, CustomerDivision)> SaveCustomer(Customero customer, long GroupTypeId, long designationId)
        {

            var dbCustomerDivision = _context.CustomerDivisions.Where(x => x.DivisionName == customer.Name || x.DTrackCustomerNumber == customer.CustomerNumber).FirstOrDefault();
            if (dbCustomerDivision != null)
            {
                if (!string.IsNullOrEmpty(customer.Contact))
                {
                    await CreatePrimaryContact(customer, dbCustomerDivision.CustomerId);
                }

                return (dbCustomerDivision.CustomerId, dbCustomerDivision.Id, dbCustomerDivision);
            }

            //create customer first
            try
            {

                var customerEntity = await _context.Customers.AddAsync(new Customer()
                {
                    GroupName = customer.Name,
                    Rcnumber = "",
                    GroupTypeId = GroupTypeId,
                    Industry = null,
                    LogoUrl = "",
                    Email = customer.EmailAddress ?? "",
                    PhoneNumber = customer.TelephoneNumber ?? "",
                    CreatedById = userIdToUse
                });

                var p = await _context.SaveChangesAsync();
                var newCustomer = customerEntity.Entity;

                var stateId = GetStateId(customer.Location);
                //creates customer division from lead division and saves the customer division
                var customerDivisionEntity = await _context.CustomerDivisions.AddAsync(new CustomerDivision()
                {
                    Industry = "Engineering",
                    Rcnumber = "",
                    DivisionName = customer.Name,
                    Email = customer.EmailAddress ?? "",
                    LogoUrl = "",
                    CustomerId = newCustomer.Id,
                    PhoneNumber = customer.TelephoneNumber ?? "",
                    Address = string.IsNullOrEmpty(customer.AddressLine1) ? customer.AddressLine2 : customer.AddressLine1,
                    StateId = stateId,
                    Lga = null,
                    Street = null,
                    CreatedById = userIdToUse,
                    DTrackCustomerNumber = customer.CustomerNumber
                });

                dbCustomerDivision = customerDivisionEntity.Entity;

                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(customer.Contact))
                {
                    await CreatePrimaryContact(customer, newCustomer.Id);
                }

                return (newCustomer.Id, customerDivisionEntity.Entity.Id, dbCustomerDivision);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> CreatePrimaryContact(Customero customer, long customerId)
        {
            //check if this contact exist previously
            try
            {
                var faxWithEmail = "";
                if (!string.IsNullOrEmpty(customer.FaxNumber))
                    if (customer.FaxNumber.Contains("@"))
                        faxWithEmail = customer.FaxNumber;

                var email = string.IsNullOrEmpty(customer.EmailAddress) ? faxWithEmail : customer.EmailAddress;
                if (string.IsNullOrEmpty(email))
                    return false;

                var contact = await _context.Contacts.Where(x => x.Email == email).FirstOrDefaultAsync();
                if (contact != null)
                {
                    return true;
                }

                var contactNew = new Contact
                {
                    Mobile = customer.TelephoneNumber ?? "",
                    Email = customer.EmailAddress,
                    DateOfBirth = DateTime.Now,
                    CreatedById = userIdToUse,
                    CreatedAt = DateTime.Now,
                    Gender = Gender.Unspecified
                };

                bool hasTitle = false;
                string[] titles = { "mr", "mrs", "ms" };
                var input = customer.Contact?.Replace(".", ""); //remove period
                var splitName = input.Split(' ');

                if (titles.Contains(splitName[0].ToLower()))
                {
                    hasTitle = true;
                    contactNew.Title = splitName[0];
                    contactNew.Gender = splitName[0].ToLower() == "mr" ? Gender.Male : Gender.Female;
                }
                else
                {
                    contactNew.Title = "";
                }

                if (splitName.Length == 3)
                {
                    if (hasTitle)
                    {
                        contactNew.FirstName = splitName[1];
                        contactNew.LastName = splitName[2];
                    }
                    else
                    {
                        contactNew.FirstName = splitName[0];
                        contactNew.MiddleName = splitName[1];
                        contactNew.LastName = splitName[2];

                    }

                }
                else if (splitName.Length == 2)
                {
                    if (hasTitle)
                        contactNew.FirstName = splitName[1];
                    else
                    {
                        contactNew.FirstName = splitName[0];
                        contactNew.LastName = splitName[1];
                    }
                }
                else
                {
                    contactNew.FirstName = customer.Contact;
                }

                var contactEntity = _context.Contacts.Add(contactNew);

                _context.SaveChanges();
                var contactId = contactEntity.Entity.Id;

                //now save this contact to the customerDivision
                _context.CustomerContacts.Add(new CustomerContact
                {
                    CustomerId = customerId,
                    ContactId = contactId,
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
            return true;
        }

        private long GetStateId(string stateCode)
        {
            var stateName = GetStateShortName(stateCode);
            var state = _context.States.Where(x => x.Name == stateName).FirstOrDefault();
            return state == null ? 1 : state.Id;
        }

        private string GetStateShortName(string stateName)
        {
            switch (stateName?.ToUpper())
            {
                case "ABA":
                    return "ABA";
                case "ABJ":
                    return "ABUJA";
                case "ABK":
                    return "ABEOKUTA";
                case "ASA":
                    return "ASABA";
                case "BCH":
                    return "ASABA";
                case "BEN":
                    return "BENIN";
                case "BKB":
                    return "BIRNIN-KEBBI";
                case "CAL":
                    return "CALABAR";
                case "ENU":
                    return "ENUGU";
                case "GUA":
                    return "GUSAU";
                case "IBA":
                    return "IBADAN";
                case "ILR":
                    return "ILORIN";
                case "JGW":
                    return "JIGAWA";
                case "JOS":
                    return "JOS";
                case "KD":
                    return "KADUNA";
                case "KN":
                    return "KANO";
                case "KST":
                    return "KASTINA";
                case "LA":
                    return "LAGOS";
                case "MDG":
                    return "MAIDUGURI";
                case "MIN":
                    return "MINNA";
                case "ONT":
                    return "ONITSHA";
                case "OSO":
                    return "OSOGBO";
                case "OWR":
                    return "OWERRI";
                case "PH":
                    return "PORTHARCOURT";
                case "SKT":
                    return "SOKOTO";
                default:
                    return "LAGOS";
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

        private void setServiceTypes()
        {
            _serviceTypes.Add(new ServiceTypes { ServiceType = "TS", ServiceTypeName = "TRAININGS" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "TEC", ServiceTypeName = "ALARM INSTALLATION SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "AGS", ServiceTypeName = "ARMED GUARDING SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "FCS", ServiceTypeName = "CYBER SECURITY SERVICES" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "DOG", ServiceTypeName = "DOG SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "ELEC", ServiceTypeName = "ELECTRONICS SERVICES" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "E", ServiceTypeName = "EVENT MANAGEMENT SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "GES", ServiceTypeName = "FLEET EQUIPMENT SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "GSS", ServiceTypeName = "FLEET MAINTENANCE SUBSCRIPTION" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "GADM", ServiceTypeName = "GUARD ADMIN SERVICE", Enum = ServiceRelationshipEnum.Admin, AdminDirectTie = "1233564" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "GD", ServiceTypeName = "GUARD DIRECT SERVICE", Enum = ServiceRelationshipEnum.Direct, AdminDirectTie = "1233564" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "GCD", ServiceTypeName = "GUARD TOUR / CLOCKING DEVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "OS", ServiceTypeName = "OUTSOURCING SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "ALRM", ServiceTypeName = "PANIC ALARM SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "PS", ServiceTypeName = "PROTOCOL /ESCORT SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "PEAC", ServiceTypeName = "PROTOCOL/ESCORT ADMIN CHARGES", Enum = ServiceRelationshipEnum.Admin, AdminDirectTie = "123344" }); //match
            _serviceTypes.Add(new ServiceTypes { ServiceType = "PEDC", ServiceTypeName = "PROTOCOL/ESCORT DIRECT CHARGES", Enum = ServiceRelationshipEnum.Direct, AdminDirectTie = "123344" });//match
            _serviceTypes.Add(new ServiceTypes { ServiceType = "RAS", ServiceTypeName = "RISK ASSESSMENT SERVICES" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "SNA", ServiceTypeName = "SCANNER SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "GDM", ServiceTypeName = "Service GDM" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "GEO", ServiceTypeName = "Service GEO" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "TES", ServiceTypeName = "TELTONIKA SUBSCRIPTION" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "VCS", ServiceTypeName = "VETTING CONSULTANCY SERVICES" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "WKT", ServiceTypeName = "WALKIE TALKIE SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "SUPAD", ServiceTypeName = "SUPERVISOR ADMIN SERVICE", Enum = ServiceRelationshipEnum.Admin, AdminDirectTie = "1235643564" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "SUPDR", ServiceTypeName = "SUPERVISOR DIRECT SERVICE", Enum = ServiceRelationshipEnum.Direct, AdminDirectTie = "1235643564" });
            // Static Guards
            _serviceTypes.Add(new ServiceTypes { ServiceType = "SD", ServiceTypeName = "STATIC GUARDS", Enum = ServiceRelationshipEnum.Standalone });
            //Dog Handlers Direct Charges
            _serviceTypes.Add(new ServiceTypes { ServiceType = "DH", ServiceTypeName = "DOG HANDLER DIRECT", Enum = ServiceRelationshipEnum.Direct, AdminDirectTie = "92828778" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "DHA", ServiceTypeName = "DOG HANDLER ADMIN", Enum = ServiceRelationshipEnum.Admin, AdminDirectTie = "92828778" });
            //Migrations
            _serviceTypes.Add(new ServiceTypes { ServiceType = "MGN", ServiceTypeName = "Migrations", Enum = ServiceRelationshipEnum.Standalone });
        }
    }
}

