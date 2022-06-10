namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class SMSSupplierAccountDTO
    {
        public string SupplierName { get; set; }
        public string Description { get; set; }
        public int SupplierCategoryId { get; set; }
        public string MobileNumber { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public int LGAId { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactMobile { get; set; }
        public string PrimaryContactGender { get; set; }
        public SupplierLogin AccountLogin { get; set; }
    }

    public class SupplierLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
