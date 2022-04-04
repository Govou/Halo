namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class ContractServiceDTO
    {
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceType { get; set; }
        public string ServiceDescription { get; set; }
        public string ImageUrl { get; set; }
        public string ServiceCategory { get; set; }
        public bool HasAdminComponent { get; set; }
        public bool HasDirectComponent { get; set; }
        public bool Isvatable { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalContractValue { get; set; }
    }
}
