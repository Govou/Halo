using System;
using System.Collections.Generic;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{

    public class ServiceDetailDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public bool HasAdminComponent { get; set; }
        public bool HasDirectServiceComponent { get; set; }
        public bool IsVatable { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalContractValue { get; set; }
        public IEnumerable<EndorsementHistory> EndorsementHistory { get; set; }
    }

    public class EndorsementHistory
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
