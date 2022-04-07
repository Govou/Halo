using Halobiz.Common.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;


namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class SbutoContractServiceProportionTransferDTO
    {
        public long Id { get; set; }
        public double Proportion { get; set; }
        public ProportionStatusType Status { get; set; }
        public SBUWithoutOperatingEntityTransferDTO StrategicBusinessUnit { get; set; }
        public UserProfileTransferDTO UserInvolved { get; set; }
        public long ContractServiceId { get; set; }
    }
}