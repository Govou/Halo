using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.ReceivingDTOs
{
    public class SMSContractDTO
    {
        public int profileId { get; set; }
        public string Email { get; set; }

        public List<SMSContractService> SMSContractServices { get; set; }
    }

    public class SMSContractServiceDTO
    {
        public int ContractId { get; set; }
        public List<SMSContractService> SMSContractServices { get; set; }
    }

    public class SMSContractService
    {
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public DateTime ServiceStartDate { get; set; }
        public DateTime ServiceEndDate { get; set; }
        public string DropLocation { get; set; }
        public DateTime DropoffDateTime { get; set; }
        public string PickupLocation { get; set; }
        public DateTime PickupTime { get; set; }
    }

    public class SMSCreateInvoiceDTO
    {
        public int ProfileId { get; set; }
        public int ContractId { get; set; }
    }
}
