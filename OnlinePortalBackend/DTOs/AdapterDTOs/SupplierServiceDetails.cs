using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnlinePortalBackend.DTOs.AdapterDTOs
{
    public class SupplierServiceDetails
    {
        public string Status { get; set; }
        public IEnumerable<ServiceData> Data { get; set; }
    }

    public class ServiceData
    {
        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }
    }
}
