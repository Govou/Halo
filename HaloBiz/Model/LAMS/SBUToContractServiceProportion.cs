namespace HaloBiz.Model.LAMS
{
    public class SBUToContractServiceProportion :  QuoteServiceProportion
    {
        public long ContractServiceId { get; set; }
        public ContractService ContractService { get; set; }
    }
}