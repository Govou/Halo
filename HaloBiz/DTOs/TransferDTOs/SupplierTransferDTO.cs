using HalobizMigrations.Models;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class SupplierTransferDTO
    {
        public long Id { get; set; }
        public string SupplierName { get; set; }
        public string Description { get; set; }
        public long? SupplierCategoryId { get; set; }
        public SupplierCategory SupplierCategory { get; set; }
        public string SupplierEmail { get; set; }
        public string MobileNumber { get; set; }
        public long StateId { get; set; }
        public State State { get; set; }
        public long LGAId { get; set; }
        public Lga LGA { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactMobile { get; set; }
        public string PrimaryContactGender { get; set; }
    }
}