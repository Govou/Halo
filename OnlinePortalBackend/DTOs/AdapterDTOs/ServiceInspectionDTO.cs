using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnlinePortalBackend.DTOs.AdapterDTOs
{
    public class ServiceInspectionDTO
    {
        public string Status { get; set; }
        public List<InspectionDetail> Data { get; set; }
        public string ResponseCode { get; set; }
    }

    public class InspectionDetail
    {
        [JsonProperty("service_group")]
        public string ServiceGroup { get; set; }
        [JsonProperty("service_id")]
        public string ServiceId { get; set; }
        [JsonProperty("provider_id")]
        public string ProviderId { get; set; }
        [JsonProperty("service_group_id")]
        public string ServiceGroupId { get; set; }
        [JsonProperty("centre_id")]
        public string CentreId { get; set; }
        [JsonProperty("car_brand")]
        public string CarBrand { get; set; }
        [JsonProperty("service_desc")]
        public string ServiceDescription { get; set; }
        public string Amount { get; set; }
        [JsonProperty("service_charge")]
        public string ServiceCharge { get; set; }
        public string Active { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    public class ServiceInspectionRequestDTO
    {
        [JsonProperty("service_name")]
        public string ServiceName { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("group_id")]
        public string GroupId { get; set; }
    }
}

