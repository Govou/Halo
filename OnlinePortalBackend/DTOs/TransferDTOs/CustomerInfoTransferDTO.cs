using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{

    public class CustomerInfoTransferDTO
    {
        public int id { get; set; }
        public string industry { get; set; }
        public string rcnumber { get; set; }
        public string divisionName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string logoUrl { get; set; }
        public int customerId { get; set; }
        public string address { get; set; }
        public int receivableAccountId { get; set; }
        public int vatAccountId { get; set; }
        public object lgaid { get; set; }
        public int stateId { get; set; }
        public object sbuId { get; set; }
        public object sbu { get; set; }
        public string street { get; set; }
        public object vatAccount { get; set; }
        public bool integrationFlag { get; set; }
        public string dTrackCustomerNumber { get; set; }
        public object receivableAccount { get; set; }
        public List<ContractTransferDTO> contracts { get; set; }
        public List<InvoiceTransferDTO> invoices { get; set; }
    }
    public class InvoiceTransferDTO
    {
        public int id { get; set; }
        public string invoiceNumber { get; set; }
        public double unitPrice { get; set; }
        public int quantity { get; set; }
        public double discount { get; set; }
        public double value { get; set; }
        public DateTime dateToBeSent { get; set; }
        public bool isInvoiceSent { get; set; }
        public int customerDivisionId { get; set; }
        public int contractId { get; set; }
        public int contractServiceId { get; set; }
       
        public DateTime endDate { get; set; }
        public DateTime startDate { get; set; }
        public int isReceiptedStatus { get; set; }
        public string transactionId { get; set; }
        public int invoiceType { get; set; }
        public bool isFinalInvoice { get; set; }
        public string groupInvoiceNumber { get; set; }
        public bool isReversalInvoice { get; set; }
        public bool isReversed { get; set; }
        public bool isAccountPosted { get; set; }
        public object createdBy { get; set; }
        public List<object> receipts { get; set; } //use later
        public object groupInvoiceDetails { get; set; }
        public object adhocGroupingId { get; set; }
    }

    public class ContractServiceTransferDTO
    {
        public int id { get; set; }
        public DateTime activationDate { get; set; }
        public int quantity { get; set; }
        public object problemStatement { get; set; }
        public double discount { get; set; }
        public double vat { get; set; }
        public double billableAmount { get; set; }
        public double budget { get; set; }
        public int paymentCycle { get; set; }
        public int invoicingInterval { get; set; }
        public object tentativeFulfillmentDate { get; set; }
        public int serviceId { get; set; }
        public int quoteServiceId { get; set; }
        public int version { get; set; }
        public int contractId { get; set; }
        public object beneficiaryIdentificationType { get; set; }
        public object beneficiaryName { get; set; }
        public object benificiaryIdentificationNumber { get; set; }
        public DateTime contractEndDate { get; set; }
        public DateTime contractStartDate { get; set; }
        public object dropoffDateTime { get; set; }
        public object dropofflocation { get; set; }
        public DateTime firstInvoiceSendDate { get; set; }
        public DateTime fulfillmentEndDate { get; set; }
        public DateTime fulfillmentStartDate { get; set; }
        public object paymentCycleInDays { get; set; }
        public object pickupDateTime { get; set; }
        public object pickupLocation { get; set; }
        public object programCommencementDate { get; set; }
        public object programDuration { get; set; }
        public object programEndDate { get; set; }
        public double unitPrice { get; set; }
        public object invoiceCycleInDays { get; set; }
        public int branchId { get; set; }
        public int officeId { get; set; }
        public double adHocInvoicedAmount { get; set; }
        public object branch { get; set; }
        public object createdBy { get; set; }
        public object office { get; set; }
        public object quoteService { get; set; }
        public object service { get; set; }
        public object amortizations { get; set; }
        public List<object> approvals { get; set; }
        public List<object> closureDocuments { get; set; }
        public List<object> groupInvoiceDetails { get; set; }
        public List<InvoiceTransferDTO> invoices { get; set; }
    
        public string uniqueTag { get; set; }
        public string adminDirectTie { get; set; }
    }

    public class ContractTransferDTO
    {
        public int id { get; set; }
        public int customerDivisionId { get; set; }
        public int quoteId { get; set; }
        public int version { get; set; }
      
      
        public object caption { get; set; }
        public object quote { get; set; }
        public List<object> amortizations { get; set; }
        public List<object> approvals { get; set; }
        public List<object> contractServiceForEndorsements { get; set; }
        public List<ContractServiceTransferDTO> contractServices { get; set; }
        public List<InvoiceTransferDTO> invoices { get; set; }
        public int groupContractCategory { get; set; }
        public string groupInvoiceNumber { get; set; }
    }

   

   

   

}
