using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.DTOs.TransferDTOs
{
    public class SendReceiptDTO
    {
        public string CustomerName { get; set; }
        public string Amount { get; set; }
        public string Date { get; set; }
        public string Email { get; set; }
        public List<SendReceiptDetailDTO> SendReceiptDetailDTOs { get; set; }

    }

    public class SendReceiptDetailDTO
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Amount { get; set; }
        public string Total { get; set; }
    }
}
