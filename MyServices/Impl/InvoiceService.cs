using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ILogger<InvoiceService> _logger;
        private readonly IContractServiceRepository _contractServiceRepo;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IAccountMasterRepository _accountMasterRepo;
        private readonly IAccountDetailsRepository _accountDetailsRepo;

        public InvoiceService(
                                IContractServiceRepository contractServiceRepo, 
                                IModificationHistoryRepository historyRepo, 
                                IInvoiceRepository invoiceRepo, 
                                ILogger<InvoiceService> logger, 
                                IMapper mapper, DataContext context,
                                IAccountMasterRepository accountMasterRepo,
                                IAccountDetailsRepository accountDetailsRepo
                                )
        {
            this._mapper = mapper;
            this._context = context;
            this._contractServiceRepo = contractServiceRepo;
            this._historyRepo = historyRepo;
            this._invoiceRepo = invoiceRepo;
            this._logger = logger;
            this._accountMasterRepo = accountMasterRepo;
            this._accountDetailsRepo = accountDetailsRepo;
        }
        public async  Task<ApiResponse> AddInvoice(HttpContext context, InvoiceReceivingDTO invoiceReceivingDTO)
        {
            using(var transaction  = await _context.Database.BeginTransactionAsync())
            {
                try{
                    var invoice = _mapper.Map<Invoice>(invoiceReceivingDTO);
                    var contractService = await  _contractServiceRepo.FindContractServiceById(invoice.ContractServiceId);
                    invoice.ContractId = contractService.ContractId;
                    var savedInvoice = await _invoiceRepo.SaveInvoice(invoice);
                    
                    var invoiceTransferDTO = _mapper.Map<InvoiceTransferDTO>(invoice);
                    await transaction.CommitAsync();
                    return new ApiOkResponse(invoiceTransferDTO);
                }catch(Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await transaction.RollbackAsync();
                    return new ApiResponse(500);
                }
            }
           
        }

        // public AccountMaster CreateAccountMaster(Invoice invoice)
        // {
        //     // var invoice = new Invoice(){
        //     //     Description = in
        //     // }
        //     /* 
        // public string Description { get; set; }
        // public bool IntegrationFlag { get; set; }
        // public double Value { get; set; }
        // public long VoucherId { get; set; }
        // public FinanceVoucherType Voucher { get; set; }
        // public string TransactionId { get; set; }
        // public long? BranchId { get; set; }
        // public virtual Branch Branch { get; set; }
        // public long? OfficeId { get; set; }
        // public virtual Office Office { get; set; }
        // public long? CustomerDivisionId { get; set; }
        //     */
        // }



        public async Task<ApiResponse> DeleteInvoice(long id)
        {
            var invoiceToDelete = await _invoiceRepo.FindInvoiceById(id);
            if(invoiceToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _invoiceRepo.DeleteInvoice(invoiceToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllInvoice()
        {
            var invoice = await _invoiceRepo.GetInvoice();
            if (invoice == null)
            {
                return new ApiResponse(404);
            }
            var invoiceTransferDTO = _mapper.Map<IEnumerable<InvoiceTransferDTO>>(invoice);
            return new ApiOkResponse(invoiceTransferDTO);
        }
        public async Task<ApiResponse> GetAllInvoicesByContactserviceId(long contractServiceId)
        {
            var invoice = await _invoiceRepo.GetInvoiceByContractServiceId(contractServiceId);
            if (invoice == null)
            {
                return new ApiResponse(404);
            }
            var invoiceTransferDTO = _mapper.Map<IEnumerable<InvoiceTransferDTO>>(invoice);
            return new ApiOkResponse(invoiceTransferDTO);
        }

        public async Task<ApiResponse> GetAllInvoicesById(long id)
        {
            var invoice = await _invoiceRepo.FindInvoiceById(id);
            if (invoice == null)
            {
                return new ApiResponse(404);
            }
            var invoiceTransferDTO = _mapper.Map<InvoiceTransferDTO>(invoice);
            return new ApiOkResponse(invoiceTransferDTO);
        }

        public  async Task<ApiResponse> UpdateInvoice(HttpContext context, long id, InvoiceReceivingDTO invoiceReceivingDTO)
        {
            var invoiceToUpdate = await _invoiceRepo.FindInvoiceById(id);
            if (invoiceToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {invoiceToUpdate.ToString()} \n" ;

            invoiceToUpdate.TransactionId = invoiceReceivingDTO.TransactionId;
            invoiceToUpdate.UnitPrice = invoiceReceivingDTO.UnitPrice;
            invoiceToUpdate.Quantity = invoiceReceivingDTO.Quantity;
            invoiceToUpdate.Discount = invoiceReceivingDTO.Discount;
            invoiceToUpdate.Value = invoiceReceivingDTO.Value;
            invoiceToUpdate.DateToBeSent = invoiceReceivingDTO.DateToBeSent;
            invoiceToUpdate.StartDate = invoiceReceivingDTO.StartDate;
            invoiceToUpdate.EndDate = invoiceReceivingDTO.EndDate;
            invoiceToUpdate.CustomerDivisionId = invoiceReceivingDTO.CustomerDivisionId;
            invoiceToUpdate.ContractServiceId = invoiceReceivingDTO.ContractServiceId;

            var updatedInvoice = await _invoiceRepo.UpdateInvoice(invoiceToUpdate);

            summary += $"Details after change, \n {updatedInvoice.ToString()} \n";

            if (updatedInvoice == null)
            {
                return new ApiResponse(500);
            }

            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Invoice",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedInvoice.Id
            };

            await _historyRepo.SaveHistory(history);

            var invoiceTransferDTOs = _mapper.Map<InvoiceTransferDTO>(updatedInvoice);
            return new ApiOkResponse(invoiceTransferDTOs);
        }
    }
}