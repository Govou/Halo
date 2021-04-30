using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class SupplierServiceReceivingDTO 
    {
        public long SupplierId { get; set; }
        public string AveragePrice { get; set; }
        public string StandardDiscount { get; set; }
        public string UnitCostPrice { get; set; }
        public string ReferenceNumber3 { get; set; }
        public string ReferenceNumber1 { get; set; }
        public string IdentificationNumber { get; set; }
        public string SerialNumber { get; set; }
        public string ImageUrl { get; set; }
        public string ModelNumber { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string Description { get; set; }
        [Required]
        [StringLength(50)]
        public string ServiceName { get; set; }
        public string ReferenceNumber2 { get; set; }
    }
}