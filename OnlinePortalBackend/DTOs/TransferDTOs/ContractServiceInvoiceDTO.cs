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
    }
}
