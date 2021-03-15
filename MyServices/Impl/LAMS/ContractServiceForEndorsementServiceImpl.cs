using System.Threading.Tasks;
using HaloBiz.Data;
using AutoMapper;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.Model.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Repository.LAMS;
using HaloBiz.DTOs.ApiDTOs;
using System.Collections.Generic;
using HaloBiz.MyServices.LAMS;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HaloBiz.Model;
using System.Linq;
using System;
using HaloBiz.Model.AccountsModel;
using Microsoft.AspNetCore.Http;
using HaloBiz.Helpers;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class ContractServiceForEndorsementServiceImpl : IContractServiceForEndorsementService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILeadConversionService _leadConversionService;
        private readonly IContractServiceForEndorsementRepository  _cntServiceForEndorsemntRepo;
        private readonly ILogger<ContractServiceForEndorsementServiceImpl> _logger;
        private readonly IConfiguration _configuration;
        private  long loggedInUserId;

        public ContractServiceForEndorsementServiceImpl( IContractServiceForEndorsementRepository  cntServiceForEndorsemntRepo, 
                                            DataContext context, 
                                            IMapper mapper, 
                                            ILeadConversionService leadConversionService,
                                            ILogger<ContractServiceForEndorsementServiceImpl> logger,
                                            IConfiguration configuration)
        {
            this._context = context;
            this._mapper = mapper;
            this._leadConversionService = leadConversionService;
            this._cntServiceForEndorsemntRepo = cntServiceForEndorsemntRepo;
            this._configuration = configuration;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddNewContractServiceForEndorsement (HttpContext httpContext, ContractServiceForEndorsementReceivingDto contractServiceForEndorsementReceiving)
        { 
            var  entityToSave = _mapper.Map<ContractServiceForEndorsement>(contractServiceForEndorsementReceiving);
            entityToSave.CreatedById = httpContext.GetLoggedInUserId();
            var savedEntity = await _cntServiceForEndorsemntRepo.SaveContractServiceForEndorsement(entityToSave);
            if( savedEntity == null)
            {
                return new ApiResponse(500);
            } 

            var contractServiceToEndorseTransferDto = _mapper.Map<ContractServiceForEndorsementTransferDto>(savedEntity);
            return new ApiOkResponse(contractServiceToEndorseTransferDto);
        }

        public async Task<ApiResponse> GetUnApprovedContractServiceForEndorsement()
        {
            var contractServicesForEndorsement = await _cntServiceForEndorsemntRepo.FindAllUnApprovedContractServicesForEndorsement();
            var contractServicesToEndorseTransferDto = _mapper.Map<IEnumerable<ContractServiceForEndorsementTransferDto>>(contractServicesForEndorsement);
            return new ApiOkResponse(contractServicesToEndorseTransferDto);
        }

        public async Task<ApiResponse> ApproveContractServiceForEndorsement(long Id, bool isApproved)
        {
            var entityToApprove = await _cntServiceForEndorsemntRepo.FindContractServiceForEndorsementById(Id);
            if(entityToApprove == null)
            {
                return new ApiResponse(404);
            }
            entityToApprove.IsApproved = isApproved;
            var approvedEntity = await _cntServiceForEndorsemntRepo.UpdateContractServiceForEndorsement(entityToApprove);
            if(approvedEntity == null)
            {
                return new ApiResponse(500);
            }
            var contractServicesToEndorseTransferDto =  _mapper.Map<ContractServiceForEndorsementTransferDto>(approvedEntity);
            return new ApiOkResponse(contractServicesToEndorseTransferDto);
        }
        
        public async Task<ApiResponse> ConvertContractServiceForEndorsement(HttpContext httpContext, long Id)
        {
            using(var transaction =  await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    this.loggedInUserId = httpContext.GetLoggedInUserId();
                    var contractServiceForEndorsement = await _cntServiceForEndorsemntRepo
                                                .FindContractServiceForEndorsementById(Id);
                    
                    if(contractServiceForEndorsement == null)
                    {
                        return new ApiResponse(404);
                    }

                    var contractServiceToSave = _mapper.Map<ContractService>(contractServiceForEndorsement);
                    contractServiceToSave.Id = 0;
                    
                    var contractServiceEntity = await _context.ContractServices.AddAsync(contractServiceToSave);
                    await _context.SaveChangesAsync();

                    var contractService = contractServiceEntity.Entity;

                    var contract = await _context.Contracts
                                .Include(x => x.CustomerDivision)
                                .FirstOrDefaultAsync(x => x.Id == contractService.ContractId);

                    var customerDivision = contract.CustomerDivision;

                    var service = await _context.Services
                                .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.ServiceId);

                    string endorsementType = contractServiceForEndorsement.EndorsementType.Caption;

                    if(endorsementType.ToLower().Contains("service addition"))
                    {
                        await AddServiceEndorsement( contractService, contractServiceForEndorsement,  service, customerDivision);
                    }else if(endorsementType.ToLower().Contains("sales topup")){
                        var contractServiceToRetire = await _context.ContractServices
                            .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);
                        await ServiceTopUpGoingForwardEndorsement( contractServiceToRetire, 
                                                                    contractService,
                                                                    customerDivision,
                                                                    service,
                                                                    contractServiceForEndorsement
                                                                    );
                    }else if(endorsementType.ToLower().Contains("sales reduction")){
                        var contractServiceToRetire = await _context.ContractServices
                            .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);
                        await ServiceReductionGoingForwardEndorsement( contractServiceToRetire, 
                                                                    contractService,
                                                                    customerDivision,
                                                                    service,
                                                                    contractServiceForEndorsement
                                                                    );
                    }else if(endorsementType.ToLower().Contains("sales retention"))
                    {
                        var contractServiceToRetire = await _context.ContractServices
                            .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);

                        await ServiceRenewalEndorsement(
                                                        contractServiceToRetire, 
                                                        contractService, 
                                                        contractServiceForEndorsement, 
                                                        service, 
                                                        customerDivision);
                    }

                    else{
                        throw new Exception("Invalid Endorsement Type");
                    }

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
        

        private async Task<bool> AddServiceEndorsement(ContractService contractService,ContractServiceForEndorsement contractServiceForEndorsement, Services service, CustomerDivision customerDivision)
        {
            var salesVoucherName = this._configuration.GetSection("VoucherTypes:SalesInvoiceVoucher").Value;
            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType.ToLower() == salesVoucherName.ToLower());

            await _leadConversionService.CreateTaskAndDeliverables(contractService, customerDivision.Id, this.loggedInUserId);

            await _leadConversionService.CreateAccounts(contractService,
                                                         customerDivision, 
                                                         (long)contractService.BranchId, 
                                                         (long)contractService.OfficeId, 
                                                         service,financialVoucherType, 
                                                         null, 
                                                         this.loggedInUserId,
                                                         false);
            
            if(string.IsNullOrWhiteSpace(contractService.GroupInvoiceNumber))
            {
                await  _leadConversionService.GenerateInvoices(contractService,customerDivision.Id, service.ServiceCode, this.loggedInUserId);
            }else {
                await  UpdateInvoices( contractService, contractServiceForEndorsement, true, true);
            }
            await _leadConversionService.GenerateAmortizations(contractService, customerDivision);

            return true;

        }

        private async Task<bool> ServiceTopUpGoingForwardEndorsement(ContractService retiredContractService, 
                                                                    ContractService newContractService, 
                                                                    CustomerDivision customerDivision, 
                                                                    Services service,
                                                                    ContractServiceForEndorsement contractServiceForEndorsement
                                                                    )
        {
            var contractServcieDifference = GetContractServiceDifference(retiredContractService, newContractService);

            var salesTopUpVoucher = this._configuration.GetSection("VoucherTypes:SalesTopupVoucher").Value;

            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType.ToLower() == salesTopUpVoucher.ToLower());

            await _leadConversionService.CreateTaskAndDeliverables(newContractService, customerDivision.Id, this.loggedInUserId);

            await _leadConversionService.CreateAccounts(contractServcieDifference,
                                                        customerDivision, 
                                                        (long)contractServcieDifference.BranchId, 
                                                        (long)contractServcieDifference.OfficeId, 
                                                        service,
                                                        financialVoucherType,
                                                        null, 
                                                        this.loggedInUserId,
                                                        false);

            if(string.IsNullOrWhiteSpace(contractServcieDifference.GroupInvoiceNumber))
            {
                await  UpdateInvoices(contractServcieDifference,contractServiceForEndorsement, true,  false);
            }else {
                await UpdateInvoices( contractServcieDifference, contractServiceForEndorsement, true, true);
            }

            await _leadConversionService.GenerateAmortizations(contractServcieDifference, customerDivision);

            await _leadConversionService.GenerateAmortizations(newContractService, customerDivision);
            
            var description = $"Service Topup for {service.Name} with serviceId: {service.Id} for client: {customerDivision.DivisionName}. quantity increase of {newContractService.Quantity - retiredContractService.Quantity}";
            await RetireContractService( 
                                        retiredContractService,  
                                        newContractService, 
                                        description,  
                                        contractServiceForEndorsement.EndorsementTypeId
                                        );
            
            return true;
        }

        private async Task<bool> ServiceReductionGoingForwardEndorsement(ContractService retiredContractService, 
                                                                    ContractService newContractService, 
                                                                    CustomerDivision customerDivision, 
                                                                    Services service,
                                                                    ContractServiceForEndorsement contractServiceForEndorsement
                                                                    )
        {
            var contractServcieDifference = GetContractServiceDifference(newContractService, retiredContractService);

            var salesReductionVoucher = this._configuration.GetSection("VoucherTypes:SalesReductionVoucher").Value;

            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType.ToLower() == salesReductionVoucher.ToLower());

            await _leadConversionService.CreateAccounts(contractServcieDifference,
                                                        customerDivision, 
                                                        (long)contractServcieDifference.BranchId, 
                                                        (long)contractServcieDifference.OfficeId, 
                                                        service,
                                                        financialVoucherType,
                                                        null, 
                                                        this.loggedInUserId,
                                                        true);

            if(string.IsNullOrWhiteSpace(contractServcieDifference.GroupInvoiceNumber))
            {
                await  UpdateInvoices(contractServcieDifference,contractServiceForEndorsement, false,  false);
            }else {
                await UpdateInvoices( contractServcieDifference, contractServiceForEndorsement, false, true);
            }

            await _leadConversionService.GenerateAmortizations(contractServcieDifference, customerDivision);

            var description = $"Service Reduction for {service.Name} with serviceId: {service.Id} for client: {customerDivision.DivisionName}. quantity reduction of {retiredContractService.Quantity - newContractService.Quantity }";
            await RetireContractService( 
                                        retiredContractService,  
                                        newContractService, 
                                        description,  
                                        contractServiceForEndorsement.EndorsementTypeId
                                        );
            
            var retiredContractServiceToNegateAmmortization = _mapper.Map<ContractService>(retiredContractService);
            
            retiredContractServiceToNegateAmmortization.BillableAmount = retiredContractServiceToNegateAmmortization.BillableAmount * -1;
            retiredContractServiceToNegateAmmortization.ContractStartDate = contractServiceForEndorsement.DateForNewContractToTakeEffect;

            //Negate amortization value starting from date of contract service update take of to contract end date
            await _leadConversionService.GenerateAmortizations(retiredContractServiceToNegateAmmortization, customerDivision);

            //Post ammortization of new contract serivce starting from date of contract service update take of to contract end date
            newContractService.ContractStartDate = contractServiceForEndorsement.DateForNewContractToTakeEffect;
            await _leadConversionService.GenerateAmortizations(newContractService, customerDivision);
            return true;
        }

        private async Task<bool> ServiceRenewalEndorsement(
                                                            ContractService retiredContractService, 
                                                            ContractService newContractService, 
                                                            ContractServiceForEndorsement contractServiceForEndorsement, 
                                                            Services service, CustomerDivision customerDivision)
        {
            var renewalVoucherName = this._configuration.GetSection("VoucherTypes:SalesRetentionVoucher").Value;
            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType.ToLower() == renewalVoucherName.ToLower());

            await _leadConversionService.CreateTaskAndDeliverables(newContractService, customerDivision.Id, this.loggedInUserId);

            await _leadConversionService.CreateAccounts(newContractService,
                                                         customerDivision,
                                                         (long)newContractService.BranchId,
                                                         (long)newContractService.OfficeId,
                                                         service,financialVoucherType,
                                                         null,
                                                         this.loggedInUserId,
                                                         false);
            
            await  _leadConversionService.GenerateInvoices(newContractService,
                                                            customerDivision.Id,
                                                            service.ServiceCode,
                                                            this.loggedInUserId);
           
            await _leadConversionService.GenerateAmortizations(newContractService, customerDivision);

            var description = $"Service Renewal for {service.Name} with serviceId: {service.Id} for client: {customerDivision.DivisionName}.";

            await RetireContractService(
                                        retiredContractService,  
                                        newContractService, 
                                        description,  
                                        contractServiceForEndorsement.EndorsementTypeId
                                        );
            return true;
        }


        private ContractService GetContractServiceDifference(ContractService retiredContractService, ContractService newContractService)
        {
            var contractServcie = _mapper.Map<ContractService>(newContractService);
            contractServcie.VAT = contractServcie.VAT - retiredContractService.VAT;
            contractServcie.Quantity = contractServcie.Quantity - retiredContractService.Quantity;
            contractServcie.BillableAmount = contractServcie.BillableAmount - retiredContractService.BillableAmount;
            contractServcie.Budget = contractServcie.Budget = retiredContractService.Budget;
            return contractServcie;
        }

        private async Task<bool> RetireContractService(ContractService retiredContractService, ContractService newContractService, string description, long endorsementTypeId)
        {
            EndorsementTypeTracker tracker = new EndorsementTypeTracker()
            {
                PreviousContractServiceId = retiredContractService.Id,
                NewContractServiceId = newContractService.Id,
                DescriptionOfChange = description,
                ApprovedById = this.loggedInUserId,
                EndorsementTypeId = endorsementTypeId,
            };

            await _context.EndorsementTypeTrackers.AddAsync(tracker);
            retiredContractService.Version = VersionType.Previous;
            _context.ContractServices.Update(retiredContractService);
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<bool> UpdateInvoices(ContractService contractService, ContractServiceForEndorsement contractServiceForEndorsement, bool isTopUp, bool isGroupInvoice)
        {
            IEnumerable<Invoice> invoices = null;
            
            invoices = isGroupInvoice?  await _context.Invoices
                                    .Where(x => x.GroupInvoiceNumber == contractService.GroupInvoiceNumber && x.StartDate >= contractServiceForEndorsement.DateForNewContractToTakeEffect && !x.IsDeleted)
                                    .ToListAsync()
                    :
                    await _context.Invoices
                                    .Where(x => x.ContractServiceId == contractService.Id && x.StartDate >= contractServiceForEndorsement.DateForNewContractToTakeEffect && !x.IsDeleted)
                                    .ToListAsync();
            
            foreach (var invoice in invoices)
            {
                if(isTopUp)
                {
                    invoice.Value += (double) contractService.BillableAmount;
                }else if(!isTopUp){
                    invoice.Value -= (double) contractService.BillableAmount;
                }

                if(!isGroupInvoice)
                {
                    invoice.Quantity = invoice.Quantity + contractService.Quantity;
                }

                invoice.ContractServiceId = contractService.Id;
            }

            _context.Invoices.UpdateRange(invoices);
            await _context.SaveChangesAsync();
            
            if(isGroupInvoice)
            {
                await GenerateGroupInvoiceDetails(contractService);
            }

            return true;
        }


        private async Task<bool> GenerateGroupInvoiceDetails(ContractService contractService)
        {
            GroupInvoiceDetails groupInvoiceDetails = new GroupInvoiceDetails()
                {
                    InvoiceNumber = contractService.GroupInvoiceNumber,
                    Description = $"Invoice details for Group Invoice {contractService.GroupInvoiceNumber}",
                    UnitPrice = (double) contractService.UnitPrice,
                    Quantity =(int) contractService.Quantity,
                    VAT  = (double) contractService.VAT,
                    Value = (double) (contractService.BillableAmount - contractService.VAT),
                    BillableAmount = (double) contractService.BillableAmount,
                    ContractServiceId = contractService.Id
                };

            await _context.GroupInvoiceDetails.AddAsync(groupInvoiceDetails);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}