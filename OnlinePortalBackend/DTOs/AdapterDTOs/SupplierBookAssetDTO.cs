using Newtonsoft.Json;

namespace OnlinePortalBackend.DTOs.AdapterDTOs
{
    public class SupplierBookAssetDTO
    {
        [JsonProperty("centre_id")]
        public string CentreId { get; set; }
        [JsonProperty("provider_id")]
        public string ProviderId { get; set; }
        [JsonProperty("service_id")]
        public string ServiceId { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("payType")]
        public string PayType { get; set; }
        [JsonProperty("time")]
        public string Time { get; set; }
        [JsonProperty("carModel")]
        public string CarModel { get; set; }
        [JsonProperty("carYear")]
        public string CarYear { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("ref")]
        public string PaymentReference { get; set; }
        [JsonProperty("payment_gateway")]
        public string PaymentGateway { get; set; }
    }

    public class SupplierBookAssetResponseDTO
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
