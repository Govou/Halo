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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private HalobizContext _context;
        private ILeadConversionService _leadConversionService;

          private HttpSender sender;
          List<Customero> customers = new List<Customero>();
          long userIdToUse = 31;
          List<ServiceTypes> _serviceTypes = new List<ServiceTypes>();

        public ServiceCustomerMigrations(ILogger<ServiceCustomerMigrations> logger,
            ILeadConversionService leadConversionService,
            HalobizContext context)
        {
            _logger = logger;
            _context = context;
            sender = new HttpSender();
            _leadConversionService = leadConversionService;
        }

        [HttpGet]
        public async Task<ApiCommonResponse> RunMigration()
        {
            setServiceTypes();
            try
            {
                //var customerBody = await sender.getCustomers();
                //customers = customerBody.Items;
                _logger.LogInformation("MIGRATION OF CUSTOMER AND CONTRACT STARTED");
                await saveContracts();
            }
            catch (Exception ex)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }


        private async Task<bool> saveContracts()
        {
            int totalSaved=0, previouslySaved=0, errorLaden = 0;
            List<Contracto> contracts = new List<Contracto>();

            var cb = await sender.getServiceContract();
            contracts = cb.Items.Take(5).ToList();

            var group = _context.GroupTypes.Where(x => x.Caption == "Corporate").FirstOrDefault();
            var designation = _context.Designations.FirstOrDefault();

            _logger.LogInformation($"Total of {contracts.Count} contracts fetched");

            foreach (var contracto in contracts)
            {
                try
                {
                    HalobizContext _context = new HalobizContext();

                    var transaction = await _context.Database.BeginTransactionAsync();


                    //check if this contract exist previously and skip
                    if (_context.Contracts.Any(x => x.Caption == contracto.ContractNumber))
                    {
                        ++previouslySaved;
                        continue;
                    }

                    //check if this customer exist and fetch
                    var customer = customers.Where(x => x.CustomerNumber == contracto.CustomerNumber).FirstOrDefault();
                    if (customer == null)
                    {
                        //fetch this customer from the api
                        customer = sender.getCustomerWithCustomerNumber(contracto.CustomerNumber).GetAwaiter().GetResult();
                        (long customerId, long divisionId) = await SaveCustomer(customer, group.Id, designation.Id);
                        customer.CustomerId = customerId;
                        customer.CustomerDivisionId = divisionId;
                        customers.Add(customer);
                    }

                    var defaultOffice = _context.Offices.FirstOrDefault();
                    var contract = _context.Contracts.Where(x => x.CustomerDivisionId == customer.CustomerDivisionId && x.Caption == contracto.ContractNumber).FirstOrDefault();
                    if (contract == null)
                    {
                        var lastDate = contracto.StartDate;
                        while (lastDate < DateTime.Today)
                        {
                            lastDate = lastDate.AddYears(1);
                        }
                        var startDate = lastDate.AddYears(-1);

                        //save the contract now
                        var contract_ = new Contract
                        {
                            CustomerDivisionId = customer.CustomerDivisionId,
                            Caption = contracto.ContractNumber,
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
                        if (afftected > 0)
                        {
                            var contractId = contract_.Id;
                            ++totalSaved;

                            _logger.LogInformation($"Successfully saved contract with Id:{contractId}, details: {JsonConvert.SerializeObject(contracto)}");

                            //now get the contract services for this contract
                            var contractServiceDetails = sender.getServiceContractDetail(contracto.ContractNumber).GetAwaiter().GetResult();
                            var contractServices = contractServiceDetails.ServiceContractItems;

                            //now save the contract service
                            var admindirectMatchFirst = DateTime.Now.AddMilliseconds(-1000).ToString("yyyyMMddHHmmss");
                            var adminDirectMatchSecond = DateTime.Now.ToString("yyyyMMddHHmmss");


                            foreach (var contractService in contractServices)
                            {
                                string admindirectMatch = "";
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


                                //get the corrresponding service
                                var apiContractService = _serviceTypes.Where(x => x.ServiceType == contractService.ServiceType).FirstOrDefault();

                                if (apiContractService.Enum != ServiceRelationshipEnum.Standalone)
                                {
                                    if (apiContractService.AdminDirectTie == "1233564")
                                        admindirectMatch = admindirectMatchFirst;
                                    else if (apiContractService.AdminDirectTie == "1235643564")
                                        admindirectMatch = adminDirectMatchSecond;
                                    else
                                        admindirectMatch = adminDirectMatchSecond + "01";
                                }

                                //check if this contract service has id
                                if (apiContractService.ServiceId == 0)
                                {
                                    apiContractService = GetServiceType(apiContractService);
                                }

                                //now save the contract service
                                var saveContractEntity = _context.ContractServices.Add(new ContractService
                                {
                                    AdminDirectTie = admindirectMatch,
                                    ServiceId = apiContractService.ServiceId,
                                    BillableAmount = contractService.Amount,
                                    ActivationDate = contractService.StartDate,
                                    ContractStartDate = startDate,
                                    ContractEndDate = lastDate.AddDays(-1),
                                    ContractId = contractId,
                                    UnitPrice = contractService.UnitPrice,
                                    Vat = contractService.Taxable == 1 ? 0.075 * contractService.Amount : 0,
                                    Quantity = contractService.Quantity,
                                    UniqueTag = $"{contractService.Description}@{contractId}",
                                    BranchId = defaultOffice.BranchId,
                                    OfficeId = defaultOffice.Id,
                                    CreatedById = userIdToUse,
                                    InvoicingInterval = (int)TimeCycle.Monthly,
                                    FirstInvoiceSendDate = startDate.AddDays(15),
                                });

                                System.Threading.Thread.Sleep(3000);

                                var affected = _context.SaveChanges();
                                if (affected > 0)
                                {
                                    var contractServiceCreated = saveContractEntity.Entity;
                                    _logger.LogInformation($"Saved for contract with ID:{contractId}, Service: {JsonConvert.SerializeObject(contractService)}");

                                    //var honder = await _leadConversionService.onMigrationAccountsForContracts(contractServiceCreated, _context,
                                    //                                      CustomerDivision customerDivision,
                                    //                                      long contractId,
                                    //                                      LeadDivision leadDivision);
                                }
                            }
                            //create location for this contract
                        }
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    ++errorLaden;
                    _logger.LogError($"Error on contract with number {contracto.ContractNumber}, Error: {ex.InnerException?.Message ?? ex.Message}");
                }               
            }

            _logger.LogInformation($"Total contracts: {contracts.Count}, Saved to db: {totalSaved}; Skipped for duplicate: {previouslySaved}; With error {errorLaden}");

            return true;
        }

        private async Task<bool> createLocation(Contracto contract, long customerDivisionId)
        {
            string address = "";
            var googleKey = "AIzaSyCQuetprs2UHb_zKSXznyumyfvw95ERCDY";

            if (contract.AddressLine1.ToLower().Contains(contract.AddressLine2.ToLower()))
            {
                address = string.Concat(contract.AddressLine1, contract.AddressLine2);
            }
            else
            {
                address = string.Concat(contract.AddressLine1, contract.AddressLine2, contract.AddressLine3);
            }

            var locationService = new GoogleLocationService(googleKey);
            var point = locationService.GetLatLongFromAddress(address);

            await _context.Locations.AddAsync(new Location
            {
                Longitutude = point.Longitude,
                Latitude = point.Longitude,
                CustomerDivisionId = customerDivisionId,
                CreatedById = userIdToUse,
                Name = contract.BillTo,
                Description = contract.Description,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                Street = address,
            });

            await _context.SaveChangesAsync();

            return true;
        }

        private ServiceTypes GetServiceType(ServiceTypes apiContractService)
        {
            var names = apiContractService.ServiceTypeName.Split(" ");
            var systemContractService = _context.Services.Where(x => x.Name.Contains(names[0]) && x.Name.Contains(names[1])).FirstOrDefault();
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

        private async Task<(long, long)> SaveCustomer(Customero customer, long GroupTypeId, long designationId)
        {          

            var dbCustomerDivision = _context.CustomerDivisions.Where(x => x.DivisionName == customer.Name || x.DTrackCustomerNumber == customer.CustomerNumber).FirstOrDefault();
            if (dbCustomerDivision != null)
            {
                if (!string.IsNullOrEmpty(customer.Contact))
                {
                    await CreatePrimaryContact(customer, dbCustomerDivision.Id);
                }

                return (dbCustomerDivision.CustomerId, dbCustomerDivision.Id); 
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
                    Industry = "",
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
                    await CreatePrimaryContact(customer, dbCustomerDivision.Id);
                }

                return (newCustomer.Id, customerDivisionEntity.Entity.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> CreatePrimaryContact(Customero customer, long customerDivisionId)
        {
            //check if this contact exist previously
            try
            {
                var email = string.IsNullOrEmpty(customer.EmailAddress) ? customer.FaxNumber : customer.EmailAddress;
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

                string[] titles = { "mr", "mrs", "ms" };
                var input = customer.Contact?.Replace(".", ""); //remove period
                var splitName = input.Split(' ');

                if (titles.Contains(splitName[0].ToLower()) && splitName.Length == 3)
                {
                    contactNew.Title = splitName[0];
                    contactNew.FirstName = splitName[1];
                    contactNew.LastName = splitName[2];
                    contactNew.Gender = splitName[0].ToLower() == "mr" ? Gender.Male : Gender.Female;
                }
                else if (splitName.Length == 2)
                {
                    contactNew.Title = "";
                    contactNew.FirstName = splitName[0];
                    contactNew.LastName = splitName[1];
                }
                else
                {
                    contactNew.Title = "";
                    contactNew.FirstName = customer.Contact;
                    contactNew.LastName = "";
                }

                var contactEntity = _context.Contacts.Add(contactNew);

                _context.SaveChanges();
                var contactId = contactEntity.Entity.Id;

                //now save this contact to the customerDivision
                _context.CustomerDivisionContacts.Add(new CustomerDivisionContact
                {
                    CustomerDivisionId = customerDivisionId,
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

        void setServiceTypes()
        {
            _serviceTypes.Add(new ServiceTypes { ServiceType = "TEC", ServiceTypeName = "ALARM INSTALLATION SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "AGS", ServiceTypeName = "ARMED GUARDING SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "FCS", ServiceTypeName = "CYBER SECURITY SERVICES" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "DOG", ServiceTypeName = "DOG SERVICE" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "ELECT", ServiceTypeName = "ELECTRONICS SERVICES" });
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
            _serviceTypes.Add(new ServiceTypes { ServiceType = "SUPDR", ServiceTypeName = "SUPERVISOR DIRECT SERVICE", Enum = ServiceRelationshipEnum.Admin, AdminDirectTie = "1235643564" });
            // Static Guards
            _serviceTypes.Add(new ServiceTypes { ServiceType = "SD", ServiceTypeName = "STATIC GUARDS", Enum = ServiceRelationshipEnum.Standalone});
            //Dog Handlers Direct Charges
            _serviceTypes.Add(new ServiceTypes { ServiceType = "DH", ServiceTypeName = "DOG HANDLER DIRECT", Enum = ServiceRelationshipEnum.Direct, AdminDirectTie="92828778" });
            _serviceTypes.Add(new ServiceTypes { ServiceType = "DHA", ServiceTypeName = "DOG HANDLER ADMIN", Enum = ServiceRelationshipEnum.Admin, AdminDirectTie = "92828778" });


        }
    }    
}
