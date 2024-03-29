using System.Collections.Generic;
using System;

namespace HaloBiz.DTOs.MailDTOs
{
    public class InvoiceMailDTO
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public double Total { get; set; }
        public double SubTotal { get; set; }
        public double VAT { get; set; }
        public double Discount { get; set; }
        public double UnInvoicedAmount { get; set; }
        public string InvoicingCycle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double DaysUntilDeadline { get; set; }
        public string Subject { get; set; }
        public string[] Recepients { get; set; }
        public ClientInfoMailDTO ClientInfo { get; set; }
        public IEnumerable<ContractServiceMailDTO> ContractServices { get; set; }
    }
    public class ContractServiceMailDTO
    {
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public long Quantity { get; set; }
        public double Total { get; set; }
        public double Discount { get; set; }
        public string UniqueTag { get; set; }
        public string AdminDirectTie { get; set; }
        public long Id { get; set; }
        public string StartDate { get; set; }
        public double Vat { get; set; }
        public double Amount { get; set; }
    }

    public class ClientInfoMailDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string LGA { get; set; }
        public string State { get; set; }
    }

    public class ConfirmComplaintResolutionMailDTO
    {
        public string Username { get; set; }
        public DateTime DateComplaintReported { get; set; }
        public long ComplaintId { get; set; }
        public string HandlerName { get; set; }
        public string[] ReceipentEmailAddress { get; set; }
        public string ConfirmationLink { get; set; }
        public string Subject { get; set; }
    }
}