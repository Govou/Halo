using System;
using System.Collections;
using System.Collections.Generic;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class ContractServiceDTO
    {
        public int ContractServiceId { get; set; }
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
        public int ContractId { get; set; }
        public int ServiceId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string UniqueTag { get; set; }
        public IEnumerable<ServiceEndorsement> EndorsementHistory { get; set; }
    }

    public class ServiceEndorsement
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }

    public class ContractDTO
    {
        public int Id { get; set; }
        public IEnumerable<ContractServiceDTO> ContractServices { get; set; }
    }
}
