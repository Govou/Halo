using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using halobiz_backend.Helpers;
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface ICustomer
    {
        Task<ApiCommonResponse> GetCustomerInfo(long id);
        Task<ApiCommonResponse> GetCustomerInfo(string email);
    }

    public class Customer : ICustomer
    {
        private readonly IMailService _mailService;
        private readonly HalobizContext _context;
        private readonly ILogger<Customer> _logger;
        private readonly IMapper _mapper;

        public Customer(IMailService mailService,
            IMapper mapper,
            ILogger<Customer> logger,
            HalobizContext context)
        {
            _mailService = mailService;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }


        public async Task<ApiCommonResponse> GetCustomerInfo(long id)
        {
            if (id == 0) return CommonResponse.Send(ResponseCodes.FAILURE, "No customer division Id");

            int completed = (int)InvoiceStatus.CompletelyReceipted;
            var customer = await _context.CustomerDivisions.Where(x => x.Id == id)
                         .Include(x=>x.Contracts)
                            .ThenInclude(x=>x.ContractServices)
                          .Include(x=>x.Invoices.Where(x=>x.IsReceiptedStatus==completed)) //check status later
                         .FirstOrDefaultAsync();

            if (customer == null)
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            var dto = _mapper.Map<CustomerInfoTransferDTO>(customer);
            return CommonResponse.Send(ResponseCodes.SUCCESS,dto);
        }

        public async Task<ApiCommonResponse> GetCustomerInfo(string email)
        {
            if (string.IsNullOrEmpty(email)) return CommonResponse.Send(ResponseCodes.FAILURE, "No customer email in request");

            int completed = (int)InvoiceStatus.CompletelyReceipted;
            var customer = await _context.CustomerDivisions.Where(x => x.Email == email)
                         .Include(x => x.Contracts)
                            .ThenInclude(x => x.ContractServices)
                          .Include(x => x.Invoices.Where(x => x.IsReceiptedStatus == completed)) //check status later
                         .FirstOrDefaultAsync();

            if (customer == null)
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null,"No customer with such email");

            var dto = _mapper.Map<CustomerInfoTransferDTO>(customer);
            return CommonResponse.Send(ResponseCodes.SUCCESS, dto);
        }
    }    
}
