using HalobizMigrations.Models;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class SupplierServiceTransferDTO
    {
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string ModelNumber { get; set; }
        public string SerailNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public string ReferenceNumber1 { get; set; }
        public string ReferenceNumber2 { get; set; }
        public string ReferenceNumber3 { get; set; }
        public string UnitCostPrice { get; set; }
        public string StandardDiscount { get; set; }
        public string AveragePrice { get; set; }
        public long SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}