namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class VehicleMakeDTO
    {
        public long MakeId { get; set; }
        public string Name { get; set; }
    }

    public class VehicleModelDTO
    {
        public long ModelId { get; set; }
        public string Name { get; set; }
        public long MakeId { get; set; }
    }
}
