using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.Helpers;
using HaloBiz.Repository.LAMS;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public interface IAdvancePaymentService
    {
        Task<ApiCommonResponse> AddPayment(HttpContext context, AdvancePaymentReceivingDTO payment);
        Task<ApiCommonResponse> GetCustomerPayment(long customerDivisionId);
        Task<ApiCommonResponse> GetAllPayments();
        Task<ApiCommonResponse> UpdatePayment(HttpContext context, long paymentId, AdvancePaymentReceivingDTO payment);
        Task<ApiCommonResponse> DeletePayment(HttpContext context, long paymentId);
        Task<ApiCommonResponse> GetById(long paymentId);
    }

    public class AdvancePaymentService : IAdvancePaymentService
    {
        private readonly HalobizContext _context;
        private readonly ILogger<AdvancePaymentService> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IFinancialVoucherTypeRepository _voucherRepo;


        public AdvancePaymentService(HalobizContext context, IFinancialVoucherTypeRepository voucherRepo,
            ILogger<AdvancePaymentService> logger, 
            IMapper mapper,
            IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _voucherRepo = voucherRepo;
        }

        public async Task<ApiCommonResponse> AddPayment(HttpContext context, AdvancePaymentReceivingDTO paymentDTO)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var customerDivion = await _context.CustomerDivisions.FindAsync(paymentDTO.CustomerDivisionId);

                    if (customerDivion == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "No customer division with this Id");
                    }

                    var payment = _mapper.Map<AdvancePayment>(paymentDTO);
                    payment.IsDeleted = false;
                    payment.CreatedAt = DateTime.Now;
                    var userId = context.GetLoggedInUserId();
                    payment.CreatedById = userId;

                    var payEntity = await _context.AdvancePayments.AddAsync(payment);
                    if (!await SaveChanges())
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }

                    var recordedPayment = payEntity.Entity;
                    recordedPayment.CustomerDivision = customerDivion;

                    var paymentVoucherType = await _voucherRepo.GetFinanceVoucherTypeByName("Advance Payment");
                    var branch = await _context.Branches.FirstOrDefaultAsync();
                    var office = await _context.Offices.FirstOrDefaultAsync();
                    var master = await CreateAccountMaster(recordedPayment, paymentVoucherType.Id, branch.Id, office.Id, userId);

                    //post to cash book
                    var accountDetail1 = await PostAccountDetail(recordedPayment, paymentVoucherType.Id, false, master.Id, paymentDTO.AccountId, payment.Amount, branch.Id, office.Id, userId);

                    if (accountDetail1 == null)
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not post account detail for cash book");


                    //get the liablity account for Advance Payment
                    var advancedPayccountNo = _configuration.GetSection("AccountsInformation:AdvancePaymentAccount")?.Value ?? throw new Exception("Advanced payment account property has not been created in appsetting.json");
                    if (string.IsNullOrEmpty(advancedPayccountNo))
                    {
                        throw new Exception("Advanced payment account value has not been entered in appsetting.json");
                    }

                    long advancedPayNo = long.Parse(advancedPayccountNo);
                    var controlAccount = await _context.ControlAccounts.Where(x => x.AccountNumber == advancedPayNo).FirstOrDefaultAsync();

                    //post to liability account for customer
                    var accountDetail2 = await PostAccountDetail(recordedPayment, paymentVoucherType.Id, true, master.Id, controlAccount.Id, payment.Amount, branch.Id, office.Id, userId);
                    if (accountDetail2 == null)
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not post account detail for avance payment account");

                    await transaction.CommitAsync();
                }

                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception e)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, e.Message);
            }

        }

        public async Task<ApiCommonResponse> GetCustomerPayment(long customerDivisionId)
        {
            //add IsDeleted
            var result = await _context.AdvancePayments.Where(x=>x.CustomerDivisionId==customerDivisionId && !x.IsDeleted).OrderByDescending(x=>x.Id).ToListAsync();  
            if (!result.Any())
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }
        public async Task<ApiCommonResponse> GetById(long paymentId)
        {
            var result = await _context.AdvancePayments.FindAsync(paymentId);
            if(result==null)
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "No payment with this id" );

            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }
        public async Task<ApiCommonResponse> GetAllPayments()
        {
            //add IsDeleted
            var result = await _context.AdvancePayments.Where(x=>!x.IsDeleted).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

        public async Task<ApiCommonResponse> DeletePayment(HttpContext context, long paymentId)
        {
            var paymentRecord = await _context.AdvancePayments.FindAsync(paymentId);
            if (paymentRecord == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "No payment with such id");
            }

            //check that this payment has not been used
            if (_context.AdvancePaymentUsages.Any(x => x.AdvancePaymentId == paymentId))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Cannot delete! This payment has usage record");
            }

            paymentRecord.IsDeleted = true;
            paymentRecord.UpdatedAt = DateTime.Now;


            _context.AdvancePayments.Update(paymentRecord);
            if (await SaveChanges())
                return CommonResponse.Send(ResponseCodes.SUCCESS);

            return CommonResponse.Send(ResponseCodes.FAILURE);

        }

        public async Task<ApiCommonResponse> UpdatePayment(HttpContext context, long paymentId, AdvancePaymentReceivingDTO paymentDTO)
        {
            var paymentRecord = await _context.AdvancePayments.FindAsync(paymentId);
            if(paymentRecord ==  null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE,null, "No payment with such id");
            }

            //check that this payment has not been used
            if (_context.AdvancePaymentUsages.Any(x=>x.AdvancePaymentId==paymentId))
            {
            }

            if (paymentRecord.Amount != paymentDTO.Amount)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Cannot edit! This amount cannot be changed");
            }


            var payment = _mapper.Map<AdvancePayment>(paymentDTO);

            paymentRecord.Amount  = payment.Amount;
            paymentRecord.EvidenceOfPaymentUrl = payment.EvidenceOfPaymentUrl;
            paymentRecord.Description = payment.Description;
            payment.UpdatedAt = DateTime.Now;
            
            _context.AdvancePayments.Update(paymentRecord);

            if(await SaveChanges())
                return CommonResponse.Send(ResponseCodes.SUCCESS);

            return CommonResponse.Send(ResponseCodes.FAILURE);

        }
        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        private async Task<AccountMaster> CreateAccountMaster(AdvancePayment payment,
                                                       long accountVoucherTypeId,
                                                       long branchId,
                                                       long officeId,
                                                       long userId
                                                       )
        {
            AccountMaster accountMaster = new AccountMaster()
            {
                Description = $"Posting advance payment for {payment.CustomerDivision?.DivisionName} on {payment.Id}",
                IntegrationFlag = false,
                VoucherId = accountVoucherTypeId,
                Value = payment.Amount,
                TransactionId = "No Transaction Id",
                CreatedById = userId,
                CustomerDivisionId = payment.CustomerDivisionId,
                BranchId = branchId,
                OfficeId = officeId
            };
            var savedAccountMaster = await _context.AccountMasters.AddAsync(accountMaster);
            await _context.SaveChangesAsync();
            return savedAccountMaster.Entity;
        }

        private async Task<AccountDetail> PostAccountDetail(
                                                    AdvancePayment payment,
                                                    long accountVoucherTypeId,
                                                    bool isCredit,
                                                    long accountMasterId,
                                                    long accountId,
                                                    double amount,
                                                    long branchId,
                                                    long officeId,
                                                    long userId
                                                    )
        {

            AccountDetail accountDetail = new AccountDetail()
            {
                Description = $"Advance payment for {payment.CustomerDivision?.DivisionName}",
                IntegrationFlag = false,
                VoucherId = accountVoucherTypeId,
                TransactionId = "No Transaction Id",
                TransactionDate = DateTime.Now,
                Credit = isCredit ? amount : 0,
                Debit = !isCredit ? amount : 0,
                AccountId = accountId,
                AccountMasterId = accountMasterId,
                CreatedById = userId,
                BranchId = branchId,
                OfficeId = officeId

            };

            var savedAccountDetails = await _context.AccountDetails.AddAsync(accountDetail);
            await _context.SaveChangesAsync();
            return savedAccountDetails.Entity;
        }
    }

}
