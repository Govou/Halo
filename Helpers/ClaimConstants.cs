using HaloBiz.DTOs.TransferDTOs.RoleManagement;
using System.Collections.Generic;

namespace HaloBiz.Helpers
{
    public static class ClaimConstants
    {
        public const string CAN_GET_BANKS = "Can Get Banks";
        public const string CAN_UPDATE_BANK = "Can Update Bank";
        public const string CAN_DELETE_BANK = "Can Delete Bank";

        public static List<ClaimTransferDTO> ApplicationClaims = new List<ClaimTransferDTO>()
        {
            new ClaimTransferDTO { Name = CAN_GET_BANKS, Description = "Can Get Banks" },
            new ClaimTransferDTO { Name = CAN_UPDATE_BANK, Description = "Can Update Banks" },
            new ClaimTransferDTO { Name = CAN_DELETE_BANK, Description = "Can Delete Banks" },
        };

        public const string ClaimType = "ApplicationClaim";

        public const string SUPER_ADMIN = "SuperAdmin";
        public const string UN_ASSIGNED = "UnAssigned";
    }

    public class RoleClaimModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
