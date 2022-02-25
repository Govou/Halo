using ClosedXML.Excel;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using GoogleMaps.LocationServices;
using HaloBiz.DTOs.ApiDTOs;
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
    public class ServiceCustomerMigrations : ControllerBase
    {
        private ILogger<ServiceCustomerMigrations> _logger;
        private readonly IWebHostEnvironment _environment;
        private HalobizContext _context;
        private ILeadConversionService _leadConversionService;
        private List<State> _states = new List<State>();
        private List<Service> _services = new List<Service>();
        private Service MigrationService = null;

        private HttpSender sender;
        List<Customero> customers = new List<Customero>();
        long userIdToUse = 31;
        List<ServiceTypes> _serviceTypes = new List<ServiceTypes>();

        public ServiceCustomerMigrations(ILogger<ServiceCustomerMigrations> logger,
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
                
                if (MigrationService == null)
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "No service with name: Migrations");

                var cb = await sender.getServiceContract(page);
                var contracts =  cb.Items.Take(150).ToList();//cb.Items.Where(x => x.ContractNumber == "11/01/079-01").ToList();

                if (contracts.Count == 0)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"No contract fetched for page {page}");
                }

                _logger.LogInformation("MIGRATION OF CUSTOMER AND CONTRACT STARTED");
                contracts = contracts.Where(x => x.Status == 1).ToList();
                await saveContracts(contracts, page, cutoffdate);
            }
            catch (Exception ex)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        private async Task saveToExcel(List<ServiceContractItem> services, int page)
        {
            try
            {
                string path = Path.Combine(_environment.WebRootPath, "MigrationsLog");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                string filePath = Path.Combine(path + $"/CS_{timestamp}.xlsx");

                FileStream fs = System.IO.File.Create(filePath);
                fs.Dispose();

                using var workbook = new XLWorkbook(filePath);
                var worksheet = workbook.Worksheets.Add($"CS_{page}");
                //int NumberOfLastRow = worksheet.LastRowUsed().RowNumber();

                var currentRow = 1; // + NumberOfLastRow;

                worksheet.Cell(currentRow, 1).Value = "ContractNumber";
                worksheet.Cell(currentRow, 2).Value = "Customer Name";
                worksheet.Cell(currentRow, 3).Value = "Description";
                worksheet.Cell(currentRow, 4).Value = "Quantity";
                worksheet.Cell(currentRow, 5).Value = "UnitPrice";
                worksheet.Cell(currentRow, 6).Value = "Amount";
                worksheet.Cell(currentRow, 7).Value = "ServiceType";
                worksheet.Cell(currentRow, 8).Value = "Service Name";
                worksheet.Cell(currentRow, 9).Value = "StartDate";
                worksheet.Cell(currentRow, 10).Value = "End Date (Halobiz)";
                worksheet.Cell(currentRow, 11).Value = "Contract Service Id(Halobiz)";
                worksheet.Cell(currentRow, 12).Value = "Billing Cycle";
                worksheet.Cell(currentRow, 13).Value = "Service Id (Halobiz)";
                worksheet.Cell(currentRow, 14).Value = "Type (Halobiz)";
                worksheet.Cell(currentRow, 15).Value = "Admin Direct Tie (Halobiz)";
                worksheet.Cell(currentRow, 16).Value = "Taxable";//Contract Service Id(Halobiz)
                worksheet.Cell(currentRow, 17).Value = "InvoiceItemDetail";

                foreach (var service in services)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = service.ContractNumber;
                    worksheet.Cell(currentRow, 2).Value = service.CustomerName; //contractId
                    worksheet.Cell(currentRow, 3).Value = service.Description;
                    worksheet.Cell(currentRow, 4).Value = service.Quantity;
                    worksheet.Cell(currentRow, 5).Value = service.UnitPrice;
                    worksheet.Cell(currentRow, 6).Value = service.Amount;
                    worksheet.Cell(currentRow, 7).Value = service.ServiceType;
                    worksheet.Cell(currentRow, 8).Value = service.ApiContractService.ServiceTypeName;
                    worksheet.Cell(currentRow, 9).Value = service.StartDate;
                    worksheet.Cell(currentRow, 10).Value = service.EndDate;
                    worksheet.Cell(currentRow, 11).Value = service.ContractServiceId;
                    worksheet.Cell(currentRow, 12).Value = service.BillingCycle;
                    worksheet.Cell(currentRow, 13).Value = service.ApiContractService.ServiceId;
                    worksheet.Cell(currentRow, 14).Value = service.Enum.ToString();
                    worksheet.Cell(currentRow, 15).Value = service.AdminDirectTie;
                    worksheet.Cell(currentRow, 16).Value = service.Taxable;
                    worksheet.Cell(currentRow, 17).Value = service.InvoiceItemDetail;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                System.IO.File.WriteAllBytes(filePath, content);
            }
            catch (Exception ex)
            {
                throw;
            }           
        }
        
        private async Task<bool> saveContracts(List<Contracto> contracts, int page, string cutOffDateStr)
        {
            int totalSaved = 0, previouslySaved = 0, errorLaden = 0;

            var defaultOffice = _context.Offices.FirstOrDefault();
            var group = _context.GroupTypes.Where(x => x.Caption == "Corporate").FirstOrDefault();
            var designation = _context.Designations.FirstOrDefault();

            _logger.LogInformation($"Total of {contracts.Count} contracts fetched");

            List<ServiceContractItem> allContractServiceItems = new List<ServiceContractItem>();
           
            var transaction = _context.Database.BeginTransaction();

            foreach (var _contract in contracts)
            {
                try
                {
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

                    var migrationContractSaved = await setCutOffMigration(startDate, lastDate, _contract.ContractNumber, customer.customerDivision, input, customer.CustomerNumber, _context, defaultOffice, cutOffDate);
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
                       item.ContractServiceId = await postContractService(item, customer, defaultOffice, _context, cutOffDateStr);
                    }

                    allContractServiceItems.AddRange(mergedAdminCasesSorted);

                    //save the contract services at this point
                    transaction.Commit();
                    ++totalSaved;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.StackTrace);
                }
            }

            if(allContractServiceItems.Count > 0)
                await saveToExcel(allContractServiceItems, page);

            _logger.LogInformation($"Total contracts: {contracts.Count}, Saved to db: {totalSaved}; Skipped for duplicate: {previouslySaved}; With error {errorLaden}");

            return true;
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

        private async Task<long> postContractService(ServiceContractItem contractService, Customero customer, Office defaultOffice, HalobizContext _context, string cutoffdate)
        {
            var thisMonth = contractService.StartDate.Month;
            var invoiceSendDate = contractService.StartDate.AddDays(10);
            while (invoiceSendDate.Month - thisMonth != 0)
            {
                invoiceSendDate = invoiceSendDate.AddDays(-1);
            }

            var saveContractEntity = _context.ContractServices.Add(new ContractService
            {
                AdminDirectTie = contractService.AdminDirectTie,
                ServiceId = contractService.ApiContractService.ServiceId,
                BillableAmount = contractService.Amount,
                ActivationDate = contractService.StartDate,
                ContractStartDate = contractService.StartDate,
                ContractEndDate = contractService.EndDate, 
                ContractId = contractService.ContractId,
                UnitPrice = contractService.UnitPrice,
                Vat = contractService.Taxable == 1 ? 0.075 * contractService.Amount : 0,
                Quantity = contractService.Quantity,
                UniqueTag = $"{contractService.Description}@{contractService.ContractId}",
                BranchId = defaultOffice.BranchId,
                OfficeId = defaultOffice.Id,
                CreatedById = userIdToUse,
                InvoicingInterval = getBillingCycle(contractService.BillingCycle.ToLower()), //get confirmation to adjust 
                FirstInvoiceSendDate = invoiceSendDate,
            });

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
               
                await _leadConversionService.onMigrationAccountsForContracts(_contractService,
                                                        customer.customerDivision,
                                                         _contractService.ContractId, userIdToUse, cutoffdate);
                return contractServiceCreated.Id;
            }

            return 0;
        }

        private async Task<bool> CreditNoteEndorsement(ContractService currentContractService,
                                                        CustomerDivision customerDivision,
                                                        Service service, long loggedInUserId)
        {

            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType == "Credit Note");          

            await _leadConversionService.CreateAccounts(
                                            currentContractService,
                                            customerDivision,
                                            (long)currentContractService.BranchId,
                                            (long)currentContractService.OfficeId,
                                            service,
                                            financialVoucherType,
                                            null,
                                            loggedInUserId,
                                            true,
                                            null);

            return true;
        }

        private async Task<bool> setCutOffMigration(DateTime startdate, DateTime enddate, string contractNo, CustomerDivision division, AccountBalanceInput input, string customerNumber, HalobizContext _context, Office defaultOffice, DateTime cutOffDate)
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
                    contractId = entity.Entity.Id;
                    var saveContractEntity = _context.ContractServices.Add(new ContractService
                    {
                        ServiceId = MigrationService.Id,
                        BillableAmount = Math.Abs(customerInput.Amount),
                        ActivationDate = startdate,
                        ContractStartDate = startdate,
                        ContractEndDate = enddate,
                        ContractId = (long)contractId,
                        UnitPrice = customerInput.Amount,
                        Vat = 0,
                        Quantity = 1,
                        InvoicingInterval = (int)TimeCycle.OneTime,
                        UniqueTag = $"Cutoff_{caption}",
                        BranchId = defaultOffice.BranchId,
                        OfficeId = defaultOffice.Id,
                        CreatedById = userIdToUse,
                        FirstInvoiceSendDate = cutOffDate.AddDays(15)
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
                            await CreditNoteEndorsement(_contractService, division, _contractService.Service, userIdToUse);
                            return true;
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

