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
        }

        public async Task<ApiResponse> AddNewContractServiceForEndorsement (ContractServiceForEndorsementReceivingDto contractServiceForEndorsementReceiving)
        { 
            var  entityToSave = _mapper.Map<ContractServiceForEndorsement>(contractServiceForEndorsementReceiving);
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
        
        public async Task<ApiResponse> ConvertContractServiceForEndorsement(long Id)
        {
            using(var transaction =  await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var contractServiceForEndorsement = await _cntServiceForEndorsemntRepo
                                                .FindContractServiceForEndorsementById(Id);
                    
                    if(contractServiceForEndorsement == null)
                    {
                        return new ApiResponse(404);
                    }

                    var contractServiceToSave = _mapper.Map<ContractService>(contractServiceForEndorsement);
                    
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

                    if(endorsementType.ToLower().Contains("Service Addition"))
                    {
                        await AddServiceEndorsement( contractService,  service, customerDivision);
                    }

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
        

        private async Task<bool> AddServiceEndorsement(ContractService contractService, Services service, CustomerDivision customerDivision)
        {
            var salesVoucherName = this._configuration.GetSection("VoucherTypes:SalesInvoiceVoucher").Value;
            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType.ToLower() == salesVoucherName.ToLower());

            await _leadConversionService.CreateTaskAndDeliverables(contractService, customerDivision.Id);

            await _leadConversionService.CreateAccounts(contractService,
                                                         customerDivision, 
                                                         (long)contractService.BranchId, 
                                                         (long)contractService.OfficeId, 
                                                         service,financialVoucherType);
            
            if(string.IsNullOrWhiteSpace(contractService.GroupInvoiceNumber))
            {
                await _leadConversionService.GenerateInvoices(contractService,customerDivision.Id, service.ServiceCode);
            }else {
                await AddServiceToGroupInvoice( contractService,  customerDivision);
            }
            await _leadConversionService.GenerateAmortizations(contractService, customerDivision);

            return true;

        }

        private async Task<bool> AddServiceToGroupInvoice(ContractService contractService, CustomerDivision customerDivision)
        {
            var groupInvoices = await _context.Invoices
                                    .Where(x => x.GroupInvoiceNumber == contractService.GroupInvoiceNumber && x.StartDate >= DateTime.Now && !x.IsDeleted)
                                    .ToListAsync();        
            var invoices = _leadConversionService.GenerateListOfInvoiceCycle(contractService, customerDivision.Id, "");
            
            groupInvoices.ForEach(invoice => {
                var generatedInvoice = invoices.FirstOrDefault(inv => inv.StartDate == invoice.StartDate);
                if(generatedInvoice != null)
                {
                    invoice.Value+= generatedInvoice.Value;
                }
            });

            _context.Invoices.UpdateRange(groupInvoices);
            await _context.SaveChangesAsync();

            await GenerateGroupInvoiceDetails(contractService);
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