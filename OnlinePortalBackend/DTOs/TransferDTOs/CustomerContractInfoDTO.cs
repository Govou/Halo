namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class CustomerContractInfoDTO 
    {
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int PendingChangeRequests { get; set; }
        public int CompletedChangeRequests { get; set; }
        public double InvoiceStatusInPercentage { get; set; }
        public double PaymentStatusInPercentage { get; set; }
        public double ComplaintsStatusInPercentage { get; set; }
        public double EndorsementsStatusInPercentage { get; set; }
    }
}
