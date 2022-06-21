namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class VehicleMakeDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class VehicleModelDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long MakeId { get; set; }
    }
}
