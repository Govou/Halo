using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HalobizMigrations.Data;
using HalobizMigrations.Models.Halobiz;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public interface IAdvancePaymentService
    {
        Task<ApiCommonResponse> AddPayment(HttpContext context, AdvancePayment payment);
        Task<ApiCommonResponse> GetCustomerPayment(long customerDivisionId);
        Task<ApiCommonResponse> GetAllPayments();
        Task<ApiCommonResponse> UpdatePayment(HttpContext context, long paymentId, AdvancePayment payment);
        Task<ApiCommonResponse> DeletePayment(HttpContext context, long paymentId);
        Task<ApiCommonResponse> GetById(long paymentId);
    }

    public class AdvancePaymentService : IAdvancePaymentService
    {
        private readonly HalobizContext _context;
        private readonly ILogger<AdvancePaymentService> _logger;
        private readonly IMapper _mapper;

        public AdvancePaymentService(HalobizContext context, ILogger<AdvancePaymentService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiCommonResponse> AddPayment(HttpContext context, AdvancePayment payment)
        {
            //payment.CreatedById = context.GetLoggedInUserId();
            //check if there is no such payment by same user within last 5 mins

            if (!_context.CustomerDivisions.Any(x=>x.Id==payment.CustomerDivisionId))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "No customer division with this Id");
            }

            var affected = await _context.AdvancePayments.AddAsync(payment);
            if (!await SaveChanges())
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetCustomerPayment(long customerDivisionId)
        {
            //add IsDeleted
            var result = await _context.AdvancePayments.Where(x=>x.CustomerDivisionId==customerDivisionId).ToListAsync();  
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }
        public async Task<ApiCommonResponse> GetById(long paymentId)
        {
            //add IsDeleted
            var result = await _context.AdvancePayments.FindAsync(paymentId);
            if(result==null)
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "No payment with this id" );

            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }
        public async Task<ApiCommonResponse> GetAllPayments()
        {
            //add IsDeleted
            var result = await _context.AdvancePayments.Where(x=>x.Id>0).ToListAsync();
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


            //add for descritpion also#
            _context.AdvancePayments.Remove(paymentRecord);
            if (await SaveChanges())
                return CommonResponse.Send(ResponseCodes.SUCCESS);

            return CommonResponse.Send(ResponseCodes.FAILURE);

        }

        public async Task<ApiCommonResponse> UpdatePayment(HttpContext context, long paymentId, AdvancePayment payment)
        {
            var paymentRecord = await _context.AdvancePayments.FindAsync(paymentId);
            if(paymentRecord ==  null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE,null, "No payment with such id");
            }

            //check that this payment has not been used
            if (_context.AdvancePaymentUsages.Any(x=>x.AdvancePaymentId==paymentId))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Cannot edit! This payment has usage record");
            }

            paymentRecord.Amount  = payment.Amount;
            paymentRecord.EvidenceOfPaymentUrl = payment.EvidenceOfPaymentUrl;
            
            //add for descritpion also#
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
    }

}
