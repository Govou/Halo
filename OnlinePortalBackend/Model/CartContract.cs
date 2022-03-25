using System;

namespace OnlinePortalBackend.Model
{
    public class CartContract
    {
        public long Id { get; set; }
        public long CustomerDivisionId { get; set; }
        public long CreatedById { get; set; }
        public int GroupContractCategory { get; set; }
        public string GroupInvoiceNumber { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
