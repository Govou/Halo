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
using halobiz_backend.Model.AccountsModel;

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

        public async Task<ApiResponse> AddNewRetentionContractServiceForEndorsement (HttpContext httpContext, List<ContractServiceForEndorsementReceivingDto> contractServiceForEndorsementDtos)
        { 
            try{

                var  entityToSave = _mapper.Map<List<ContractServiceForEndorsement>>(contractServiceForEndorsementDtos);
                
                foreach (var entity in entityToSave)
                {
                    if( !await ValidateContractToRenew(entity))
                    {
                        return new ApiResponse(400, "Invalid Previous contract service id or invalid new contract start date");
                    }

                    entity.CreatedById = httpContext.GetLoggedInUserId();
                }
                
                var savedEntity = await _cntServiceForEndorsemntRepo.SaveRangeContractServiceForEndorsement(entityToSave);
                return new ApiOkResponse(true);
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> AddNewContractServiceForEndorsement (HttpContext httpContext, ContractServiceForEndorsementReceivingDto contractServiceForEndorsementReceiving)
        { 
            var  entityToSave = _mapper.Map<ContractServiceForEndorsement>(contractServiceForEndorsementReceiving);
            var endorsementType = await _context.EndorsementTypes
                        .FirstOrDefaultAsync(x => x.Id == entityToSave.EndorsementTypeId);
            if(endorsementType.Caption.ToLower().Contains("renew") 
                        && !await ValidateContractToRenew(entityToSave))
            {
                return new ApiResponse(400, "Invalid Previous contract service id or invalid new contract start date");
            }
            entityToSave.CreatedById = httpContext.GetLoggedInUserId();
            var savedEntity = await _cntServiceForEndorsemntRepo.SaveContractServiceForEndorsement(entityToSave);
            if( savedEntity == null)
            {
                return new ApiResponse(500);
            } 

            var contractServiceToEndorseTransferDto = _mapper.Map<ContractServiceForEndorsementTransferDto>(savedEntity);
            return new ApiOkResponse(contractServiceToEndorseTransferDto);
        }
        

        private async Task<bool> ValidateContractToRenew(ContractServiceForEndorsement contractServiceForEndorsement)
        {
            if(contractServiceForEndorsement.PreviousContractServiceId == null || contractServiceForEndorsement.PreviousContractServiceId == 0)
            {
                return false;
            }
            var contractService = await _context.ContractServices
                        .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);
            
            if(contractService == null)
            {
                return false;
            }
            if(contractService.ContractEndDate >= contractServiceForEndorsement.ContractStartDate)
            {
                return false;
            }
            return true;
        }



        public async Task<ApiResponse> GetUnApprovedContractServiceForEndorsement()
        {
            var contractServicesForEndorsement = await _cntServiceForEndorsemntRepo.FindAllUnApprovedContractServicesForEndorsement();
            var contractServicesToEndorseTransferDto = 
                _mapper.Map<IEnumerable<ContractServiceForEndorsementTransferDto>>(contractServicesForEndorsement);
            return new ApiOkResponse(contractServicesToEndorseTransferDto);
        }

        public async Task<ApiResponse> GetAllPossibleEndorsementStartDate(long contractServiceId)
        {
            var possibleDates = await _cntServiceForEndorsemntRepo.FindAllPossibleEndorsementStartDate(contractServiceId);
            return new ApiOkResponse(possibleDates);
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

                    contractServiceForEndorsement.IsConvertedToContractService = true;
                    _context.ContractServiceForEndorsements.Update(contractServiceForEndorsement);
                    await _context.SaveChangesAsync();

                    var contractService = contractServiceEntity.Entity;

                    var contract = await _context.Contracts
                                .Include(x => x.CustomerDivision)
                                .FirstOrDefaultAsync(x => x.Id == contractService.ContractId);

                    var customerDivision = contract.CustomerDivision;

                    var service = await _context.Services
                                .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.ServiceId);

                    string endorsementType = contractServiceForEndorsement.EndorsementType.Caption;

                    if(endorsementType.ToLower().Contains("addition"))
                    {
                        await AddServiceEndorsement( contractService, contractServiceForEndorsement,  service, customerDivision);
                    
                    }else if(endorsementType.ToLower().Contains("topup")){

                        var contractServiceToRetire = await _context.ContractServices
                            .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);

                        await ServiceTopUpGoingForwardEndorsement( contractServiceToRetire, 
                                                                    contractService,
                                                                    customerDivision,
                                                                    service,
                                                                    contractServiceForEndorsement
                                                                    );
                    }else if(endorsementType.ToLower().Contains("reduction")){

                        var contractServiceToRetire = await _context.ContractServices
                            .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);

                        await ServiceReductionGoingForwardEndorsement( contractServiceToRetire, 
                                                                    contractService,
                                                                    customerDivision,
                                                                    service,
                                                                    contractServiceForEndorsement
                                                                    );
                    }else if(endorsementType.ToLower().Contains("retention"))
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

            if(string.IsNullOrWhiteSpace(contractServcieDifference.GroupInvoiceNumber))
            {
                await  UpdateInvoices(contractServcieDifference,contractServiceForEndorsement, true,  false);
            }else {
                await UpdateInvoices( contractServcieDifference, contractServiceForEndorsement, true, true);
            }
            
            var description = $"Service Topup for {service.Name} with serviceId: {service.Id} for client: {customerDivision.DivisionName}. quantity increase of {newContractService.Quantity - retiredContractService.Quantity}";
            await RetireContractService(
                                        retiredContractService,  
                                        newContractService, 
                                        description,  
                                        contractServiceForEndorsement.EndorsementTypeId
                                        );
            
            var retiredContractServiceToNegateAmmortization = _mapper.Map<ContractService>(retiredContractService);
            
            retiredContractServiceToNegateAmmortization.BillableAmount *=  -1;
            retiredContractServiceToNegateAmmortization.ContractStartDate = contractServiceForEndorsement.DateForNewContractToTakeEffect;

            //Negate amortization value starting from date of contract service update take of to contract end date
            await _leadConversionService.GenerateAmortizations(retiredContractServiceToNegateAmmortization, customerDivision);

            //Post ammortization of new contract serivce starting from date of contract service update take of to contract end date
            newContractService.ContractStartDate = contractServiceForEndorsement.DateForNewContractToTakeEffect;
            await _leadConversionService.GenerateAmortizations(newContractService, customerDivision);

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

            if(String.IsNullOrWhiteSpace(contractServcieDifference.GroupInvoiceNumber))
            {
                await  UpdateInvoices(contractServcieDifference,contractServiceForEndorsement, false,  false);
            }else {
                await UpdateInvoices( contractServcieDifference, contractServiceForEndorsement, false, true);
            }


            var description = $"Service Reduction for {service.Name} with serviceId: {service.Id} for client: {customerDivision.DivisionName}. quantity reduction of {retiredContractService.Quantity - newContractService.Quantity }";
            await RetireContractService(
                                        retiredContractService,
                                        newContractService,
                                        description,
                                        contractServiceForEndorsement.EndorsementTypeId
                                        );
            
            var retiredContractServiceToNegateAmmortization = _mapper.Map<ContractService>(retiredContractService);
            
            retiredContractServiceToNegateAmmortization.BillableAmount *=  -1;
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
            contractServiceForEndorsement.DateForNewContractToTakeEffect = newContractService.ContractStartDate;
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
            //Check if an existing invoice with group invoice number already exists
            //if yes it updates the existing invoice else it creates a new invoice 
            var invoiceExists = String.IsNullOrWhiteSpace(newContractService.GroupInvoiceNumber) ?
                    false : await _context.Invoices.AnyAsync(x => x.GroupInvoiceNumber == newContractService.GroupInvoiceNumber);

            if(invoiceExists)
            {
                 await  UpdateInvoices(newContractService, contractServiceForEndorsement, true,  true);
            }else{
                await  _leadConversionService.GenerateInvoices(newContractService,
                                                                customerDivision.Id,
                                                                service.ServiceCode,
                                                                this.loggedInUserId);
            }
            
            await _leadConversionService.GenerateAmortizations(newContractService, customerDivision);

            var description = $"Service Renewal for {service.Name} with serviceId: {service.Id} for client: {customerDivision.DivisionName}.";

            await RetireContractService(
                                        retiredContractService,  
                                        newContractService, 
                                        description,  
                                        contractServiceForEndorsement.EndorsementTypeId
                                        );
            
            if(!String.IsNullOrWhiteSpace(newContractService.GroupInvoiceNumber))
            {
                await GenerateGroupInvoiceDetails(newContractService);
            }
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
                                    .Where(x => x.GroupInvoiceNumber == contractServiceForEndorsement.GroupInvoiceNumber && x.StartDate >= contractServiceForEndorsement.DateForNewContractToTakeEffect && !x.IsDeleted)
                                    .ToListAsync()
                    :
                    await _context.Invoices
                                    .Where(x => x.ContractServiceId == contractServiceForEndorsement.PreviousContractServiceId && x.StartDate >= contractServiceForEndorsement.DateForNewContractToTakeEffect && !x.IsDeleted)
                                    .ToListAsync();

            var billbalbleForInvoicingPeriod = CalculateTotalBillableForPeriod(contractService);
            
            foreach (var invoice in invoices)
            {
                if(isTopUp)
                {
                    invoice.Value += billbalbleForInvoicingPeriod;
                }else if(!isTopUp){
                    invoice.Value -= billbalbleForInvoicingPeriod;
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

        private async Task<bool> ReverseInvoicesAndReceipts(long contractServiceId)
        {
            IEnumerable<Invoice> invoicesToBeReversed = await _context.Invoices
                        .Include(invoice => invoice.Receipts)
                        .Where(invoice => !invoice.IsDeleted && !invoice.IsReversed && invoice.IsReversalInvoice)
                        .ToListAsync();
            List<Receipt> reversalReeceipts = new List<Receipt>();
            List<Invoice> reversalInvoices = new List<Invoice>();

            foreach (var invoice in invoicesToBeReversed)
            {
                reversalInvoices.Add(GenerateReverselInvoice(invoice));
                invoice.IsReversed = true;

                foreach (var receipt in invoice.Receipts)  
                {
                    receipt.IsReversed = true;
                    reversalReeceipts.Add(GenerateReversalReceipt(receipt));
                }
            }

            await _context.Invoices.AddRangeAsync(reversalInvoices);
            await _context.SaveChangesAsync();
            await _context.Receipts.AddRangeAsync(reversalReeceipts);
            await _context.SaveChangesAsync();
            _context.UpdateRange(invoicesToBeReversed);
            await _context.SaveChangesAsync();
            return false;
        }


        private Invoice GenerateReverselInvoice(Invoice invoice)
        {
            Invoice invoiceToReverse = _mapper.Map<Invoice>(invoice);
            invoiceToReverse.Value *= -1;
            invoiceToReverse.IsInvoiceSent = true;
            invoiceToReverse.IsReversalInvoice = true;
            invoiceToReverse.CreatedAt = DateTime.Now;
            invoiceToReverse.UpdatedAt = DateTime.Now;
            invoiceToReverse.Id = 0;
            invoiceToReverse.CreatedById = this.loggedInUserId;

            return invoiceToReverse;
        }

        private  Receipt GenerateReversalReceipt(Receipt receipt)
        {
            Receipt receiptToReverse = _mapper.Map<Receipt>(receipt);
            receiptToReverse.InvoiceValue*= -1;
            receiptToReverse.InvoiceValueBalanceAfterReceipting = 0;
            receiptToReverse.InvoiceValueBalanceBeforeReceipting = 0;
            receiptToReverse.Caption = $"Reversal of {receiptToReverse.Caption}";
            receiptToReverse.DateAndTimeOfFundsReceived = DateTime.Now;
            receiptToReverse.ReceiptValue*= -1;
            receiptToReverse.UpdatedAt = DateTime.Now;
            receiptToReverse.CreatedAt = DateTime.Now;
            receiptToReverse.CreatedById = this.loggedInUserId;
            receiptToReverse.IsReversalReceipt = true;
            receiptToReverse.Id= 0;

            return receiptToReverse;

        }

        private static double GenerateWeeklyAmount(DateTime startDate, DateTime endDate, double amountPerMonth, TimeCycle timeCylce)
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

        private double CalculateTotalBillableForPeriod(ContractService contractService )
        {
            int interval = 0;
            DateTime startDate =(DateTime) contractService.ContractStartDate; 
            DateTime endDate =(DateTime) contractService.ContractEndDate;
            TimeCycle cycle =(TimeCycle) contractService.InvoicingInterval;
            double amount =(double) contractService.BillableAmount;

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

            if(cycle == TimeCycle.Weekly || cycle == TimeCycle.BiWeekly)
            {
                return GenerateWeeklyAmount( startDate, endDate,  amount,  cycle);

            }else if(cycle == TimeCycle.OneTime ){
                return amount;
            }else{
                return amount * (double) interval ;
            }
        }

    }
}