namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class StateDTO
    {
        public string StateName { get; set; }
        public int StateId { get; set; }
    }

    public class LocalGovtAreaDTO
    {
        public string LGAName { get; set; }
        public int LGAId { get; set; }
        public int StateId { get; set; }
    }

    public class CommonResposeDTO
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
