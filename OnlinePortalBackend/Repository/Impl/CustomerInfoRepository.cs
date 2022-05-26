using HalobizMigrations.Data;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class CustomerInfoRepository : ICustomerInfoRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<CustomerInfoRepository> _logger;
        public CustomerInfoRepository(HalobizContext context, ILogger<CustomerInfoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<CustomerContractInfoDTO> GetCotractInfos(int customerDiv)
        {
            var pendingOrders = 0;
            var completedOrders = 0;
            var pendingChangeRequests = 0;
            var completedChangeRequests = 0;
            var invoiceStatusInPercentage = 0d;
            var paymentStatusInPercentage = 0d;
            var complaintsStatusInPercentage = 0d;
            var endorsementsStatusInPercentage = 0d;

            try
            {
                var cust = _context.CustomerDivisions.FirstOrDefault(x => x.Id == customerDiv);

                if (cust == null)
                    return null;

                var pendingContracts = _context.Contracts.Where(x => x.IsApproved == false && x.CustomerDivisionId == customerDiv && x.IsDeleted == false).ToList();

                foreach (var item in pendingContracts)
                {
                    var conServices = _context.ContractServices.Where(x => x.ContractId == item.Id).Count();
                    pendingOrders += conServices;
                }

                var approvedContracts = _context.Contracts.Where(x => x.IsApproved == true && x.CustomerDivisionId == customerDiv && x.IsDeleted == false).ToList();

                foreach (var item in pendingContracts)
                {
                    var conServices = _context.ContractServices.Where(x => x.ContractId == item.Id).Count();
                    completedOrders += conServices;
                }

                pendingChangeRequests = _context.ContractServiceForEndorsements.Where(x => x.IsApproved == false && x.CustomerDivisionId == customerDiv && x.IsDeleted == false).Count();

                completedChangeRequests = _context.ContractServiceForEndorsements.Where(x => x.IsApproved == true && x.CustomerDivisionId == customerDiv && x.IsDeleted == false).Count();

                var invoiceCount = _context.Invoices.Where(x => x.CustomerDivisionId == customerDiv && x.IsDeleted == false).Count();
                var invoiceNotPaidCount = _context.Invoices.Where(x => x.CustomerDivisionId == customerDiv && x.IsReceiptedStatus == (int)InvoiceStatus.NotReceipted && x.IsDeleted == false).Count();

                if (invoiceCount > 0)
                {
                    invoiceStatusInPercentage = (invoiceNotPaidCount / invoiceCount) * 100;
                }

                var invoicePaidCount = _context.Invoices.Where(x => x.CustomerDivisionId == customerDiv && x.IsReceiptedStatus == (int)InvoiceStatus.CompletelyReceipted && x.IsDeleted == false).Count();

                if(invoicePaidCount > 0)
                {
                    paymentStatusInPercentage = (invoicePaidCount / invoiceCount) * 100;
                }

                var complaintsCount = _context.Complaints.Where(x => x.ComplainantId == customerDiv && x.IsDeleted == false).Count();
                var complaintClosedCount = _context.Complaints.Where(x => x.ComplainantId != customerDiv && x.IsClosed == true && x.IsDeleted == false).Count();

                if (complaintsCount > 0)
                {
                    complaintsStatusInPercentage = (complaintClosedCount / complaintsCount) * 100;
                }

                if ((completedChangeRequests + pendingChangeRequests) > 0)
                {
                    endorsementsStatusInPercentage = completedChangeRequests / (completedChangeRequests + pendingChangeRequests) * 100;
                }


                return new CustomerContractInfoDTO
                {
                    ComplaintsStatusInPercentage = complaintsStatusInPercentage,
                    CompletedChangeRequests = completedChangeRequests,
                    CompletedOrders = completedOrders,
                    EndorsementsStatusInPercentage = endorsementsStatusInPercentage,
                    InvoiceStatusInPercentage = invoiceStatusInPercentage,
                    PaymentStatusInPercentage = paymentStatusInPercentage,
                    PendingChangeRequests = pendingChangeRequests,
                    PendingOrders = pendingOrders,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
            
        }
    }
}
