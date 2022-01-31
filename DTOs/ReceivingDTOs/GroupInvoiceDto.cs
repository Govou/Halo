using System;
using System.Collections.Generic;

namespace HaloBiz.DTOs.ReceivingDTOs
{
        public class GroupInvoiceDto
        {
            public double TotalBillable { get; set; }
            public double VAT { get; set; }
            public string GroupInvoiceNumber { get; set; }
            public DateTime DateToBeSent { get; set; }
            public long CustomerDivisionId { get; set; }
        }

}