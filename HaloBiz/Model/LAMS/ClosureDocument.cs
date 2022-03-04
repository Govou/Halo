namespace HaloBiz.Model.LAMS
{
    public class ClosureDocument : Documents
    {
        public long ContractServiceId { get; set; }
        public ContractService ContractService { get; set; }
    }
}