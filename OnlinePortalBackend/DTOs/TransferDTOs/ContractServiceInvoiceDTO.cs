using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class ContractServiceInvoiceDTO
    {
        public int PaymentsDue { get; set; }
        public int PaymentsOverDue { get; set; }
        public IEnumerable<InvoiceDTO> Invoices { get; set; }
        public double TotalPayments { get; set; }
        public string Status { get; set; }
        public int ContractId { get; set; }
    }

    public class IdividualInvoiceDTO
    {
        public IEnumerable<InvoiceDTO> Invoices { get; set; }
    }
    public class ContractServiceIdividualInvoiceDTO
    {

        public List<IdividualInvoiceDTO> IndividualInvoices { get; set; }
        public int ContractId { get; set; }
        public int PaymentsDue { get; set; }
        public int PaymentsOverDue { get; set; }
    }

    public class ContractInvoiceDTO
    {
        public IEnumerable<ContractServiceInvoiceDTO> ContractServiceInvoices { get; set; }
        public IEnumerable<ContractServiceIdividualInvoiceDTO> IndividualContractServiceInvoices { get; set; }
    }

    public class InvoiceDTO
    {
        public int Id { get; set; }
        public DateTime InvoiceStartDate { get; set; }
        public DateTime InvoiceEndDate { get; set; }
        public string InvoiceNumber { get; set; }
        public double InvoiceValue { get; set; }
        public double VAT { get; set; }

        [JsonIgnore]
        public double? Payment { get; set; }
        [JsonIgnore]
        public int IsReceiptedStatus { get; set; }
        [JsonIgnore]
        public bool IsFinalInvoice { get; set; }
        [JsonIgnore]
        public DateTime DateToBeSent { get; set; }
        [JsonIgnore]
        public double? InvoiceValueBalanceAfterReceipt { get; set; }
        public int ContractId { get; set; }
        public int ContractServiceId { get; set; }
    }

    public class InvoiceDetailDTO
    {
        public DateTime InvoiceStart { get; set; }
        public DateTime InvoiceEnd { get; set; }
        public DateTime InvoiceDue { get; set; }
        public string InvoiceNumber { get; set; }
        public double InvoiceValue { get; set; }
        public double InvoiceBalanceBeforeReceipting { get; set; }

        public List<InvoiceDetailInfo> InvoiceDetailsInfos { get; set; }
    }

    public class InvoiceDetailInfo
    {
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public double Discount { get; set; }
        public int ContractServiceId { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
