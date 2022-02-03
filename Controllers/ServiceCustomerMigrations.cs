using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.MigrationsHelpers;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration _config;
        private HalobizContext _context;

          private HttpSender sender;
          List<Customero> customers = new List<Customero>();
          List<Contracto> contracts = new List<Contracto>();
          long userIdToUse = 31;
          List<ServiceTypes> _serviceTypes = new List<ServiceTypes>();

        public ServiceCustomerMigrations(IConfiguration config,
            HalobizContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpGet]
        public async Task<ApiCommonResponse> RunMigration()
        {
            setServiceTypes();

            try
            {
                var transaction = await _context.Database.BeginTransactionAsync();

                var cb = await sender.getServiceContract();
                contracts = cb.Items.Take(5).ToList();

                //var customerBody = await sender.getCustomers();
                //customers = customerBody.Items;

                await saveContracts();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.StackTrace);

            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
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

        }

        private async Task saveContracts()
        {
            var group = _context.GroupTypes.Where(x => x.Caption == "Corporate").FirstOrDefault();
            var designation = _context.Designations.FirstOrDefault();

            foreach (var contracto in contracts)
            {
                //check if this customer exist and fetch
                var customer =  customers.Where(x => x.CustomerNumber == contracto.CustomerNumber).FirstOrDefault();
                if(customer == null)
                {
                    //fetch this customer from the api
                    customer = await sender.getCustomerWithCustomerNumber(contracto.CustomerNumber);
                     (long customerId, long divisionId) = await SaveCustomer(customer, group.Id, designation.Id);
                    customer.CustomerId = customerId;
                    customer.CustomerDivisionId = divisionId;
                    customers.Add(customer);
                }

                var defaultOffice = await _context.Offices.FirstOrDefaultAsync();
                var contract = await _context.Contracts.Where(x => x.CustomerDivisionId == customer.CustomerDivisionId && x.Caption == contracto.ContractNumber).FirstOrDefaultAsync();
                if (contract == null)
                {
                    //save the contract now
                    var entity = await _context.Contracts.AddAsync(new Contract
                    {
                        CustomerDivisionId = customer.CustomerDivisionId,
                        Caption = contracto.ContractNumber,
                        GroupContractCategory = GroupContractCategory.GroupContractWithSameDetails,
                        GroupInvoiceNumber = GenerateGroupInvoiceNumber(),
                        CreatedById = userIdToUse,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                    });

                    var r = await _context.SaveChangesAsync();
                    if (r > 0)
                    {
                        var contractId = entity.Entity.Id;
                        //now get the contract services for this contract
                        var contractServiceDetails = await sender.getServiceContractDetail(contracto.ContractNumber);
                        var contractServices = contractServiceDetails.ServiceContractItems;

                        //now save the contract service
                        var admindirectMatch = DateTime.Now.ToString("yyyyMMddHHmmss");

                        foreach (var contractService in contractServices)
                        {
                            //get the corrresponding service
                            var apiContractService = _serviceTypes.Where(x => x.ServiceType == contractService.ServiceType).FirstOrDefault();
                            //check if this contract service has id
                            if (apiContractService.ServiceId == 0)
                            {
                                apiContractService = GetServiceType(apiContractService);
                            }

                            //now save the contract service
                            var saveContractEntity = await _context.ContractServices.AddAsync(new ContractService
                            {
                                AdminDirectTie = admindirectMatch,
                                ServiceId = apiContractService.ServiceId,
                                BillableAmount = contractService.Amount,
                                ActivationDate = contractService.StartDate,
                                ContractStartDate = contractService.StartDate,
                                ContractEndDate = DateTime.Now.AddYears(1),
                                ContractId = contractId,
                                Vat = contractService.Taxable == 1 ? 0.075 * contractService.Amount : 0,
                                Quantity = contractService.Quantity,
                                UniqueTag = $"{contractService.Description}@{contractId}",
                                BranchId = defaultOffice.BranchId,
                                OfficeId = defaultOffice.Id
                            });

                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        private   ServiceTypes GetServiceType(ServiceTypes apiContractService)
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

          async Task<bool> saveCustomers()
        {
            //get the group all should belong
            var group = _context.GroupTypes.Where(x => x.Caption == "Corporate").FirstOrDefault();
            var designation = _context.Designations.FirstOrDefault();
            try
            {

                foreach (var customer in customers)
                {
                    customer.Name = customer.Name.Replace("  ", " ");

                    //check if the customer exist previously
                    var dbCustomerDivision = _context.CustomerDivisions.Where(x => x.DivisionName == customer.Name || x.DTrackCustomerNumber==customer.CustomerNumber).FirstOrDefault();
                   // var dbCustomerDivision = _context.CustomerDivisions.FirstOrDefault();

                    if (dbCustomerDivision == null)
                    {
                        //create the contact of this customer                       
                        //save the customer
                        //save the customerDivision
                        var (customerId, divisionId) = await SaveCustomer(customer, group.Id, designation.Id);
                        customer.CustomerId = customerId;
                        customer.CustomerDivisionId = divisionId;
                    }
                    else
                    {
                        //Console.WriteLine($"Record already exist: {dbCustomerDivision.DivisionName}");
                        //pick the customerId and division Id
                        customer.CustomerId = dbCustomerDivision.CustomerId;
                        customer.CustomerDivisionId = dbCustomerDivision.Id;
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                var p = ex.StackTrace;
                return false;
            }

            return true;
        }

        private async Task<(long, long)> SaveCustomer(Customero customer, long GroupTypeId, long designationId)
        {
            //create customer first
            long? primaryContactId = null;
            try
            {
                //create the contact person
                //if (!string.IsNullOrEmpty(customer.Contact)){
                //    primaryContactId = CreatePrimaryContact(customer, designationId);
                //}

                var customerEntity = await _context.Customers.AddAsync(new Customer()
                {
                    GroupName = customer.Name,
                    Rcnumber = "",
                    GroupTypeId = GroupTypeId,
                    Industry = null,
                    LogoUrl = "",
                    Email = customer.EmailAddress ?? "",
                    PhoneNumber = customer.TelephoneNumber ?? "",
                    CreatedById = userIdToUse,
                    PrimaryContactId = primaryContactId,
                    SecondaryContactId = null,
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
                    //PrimaryContactId = primaryContactId,
                    //SecondaryContactId = null,
                    CreatedById = userIdToUse,
                    DTrackCustomerNumber = customer.CustomerNumber
                });

                await _context.SaveChangesAsync();              

                return (newCustomer.Id, customerDivisionEntity.Entity.Id);
            }
            catch (Exception ex)
            {
                var p = ex.StackTrace;
                return (0, 0);
            }
        }

        private long CreatePrimaryContact(Customero customer, long designationId)
        {
            var contactEntity = _context.LeadDivisionContacts.Add(new LeadDivisionContact
            {
                FirstName = customer.Contact,
                LastName = "",
                MobileNumber = customer.TelephoneNumber ?? "100000001",
                Email = customer.EmailAddress,
                DateOfBirth = DateTime.Now,
                CreatedById = userIdToUse,
                CreatedAt = DateTime.Now,
                Gender = "Male",
                Title = "Mr",
                DesignationId = designationId

            });

            _context.SaveChanges();

            return contactEntity.Entity.Id;
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



        private string GenerateGroupInvoiceNumber()
        {
            try
            {
                var tracker = _context.GroupInvoiceTrackers.OrderBy(x => x.Id).FirstOrDefault();
                long newNumber = 0;
                if (tracker == null)
                {
                    newNumber = 1;
                    _context.GroupInvoiceTrackers.Add(new GroupInvoiceTracker() { Number = newNumber + 1 });
                    _context.SaveChangesAsync();
                    return $"GINV{newNumber.ToString().PadLeft(7, '0')}";
                }
                else
                {
                    newNumber = tracker.Number;
                    tracker.Number = tracker.Number + 1;
                    _context.GroupInvoiceTrackers.Update(tracker);
                    _context.SaveChangesAsync();
                    return $"GINV{newNumber.ToString().PadLeft(7, '0')}";
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }    
}
