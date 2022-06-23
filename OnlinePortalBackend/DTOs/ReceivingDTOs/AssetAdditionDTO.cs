namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class AssetAdditionDTO
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string ModelNumber { get; set; }
        public string ImageUrl { get; set; }
        public string TrackerId { get; set; }
        public bool? IsAvailable { get; set; }
        public string SerialNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public string ReferenceNumber1 { get; set; }
        public string ReferenceNumber2 { get; set; }
        public string ReferenceNumber3 { get; set; }
        public string UnitCostPrice { get; set; }
        public string AveragePrice { get; set; }
        public string StandardDiscount { get; set; }
        public long SupplierId { get; set; }
        public string FrontViewImage { get; set; }
        public string LeftViewImage { get; set; }
        public string RightViewImage { get; set; }
        public string RearViewImage { get; set; }
        public string TopViewImage { get; set; }
        public string InteriorViewImage { get; set; }
        public string PaymentGateway { get; set; }
        public string PaymentReference { get; set; }
        public string CentreId { get; set; }
        public string PaymentType { get; set; }
        public string BookingAmount { get; set; }
        public string BookingAddress { get; set; }
        public string BookingState { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public int ProfileId { get; set; }
        public string ServiceId { get; set; }
        public string ProviderId { get; set; }
    }
}
