using System.Collections.Generic;

namespace HaloBiz.Helpers
{
    public static class ClaimConstants
    {
        public const string CAN_GET_BANKS = "CanGetBanks";
        public const string CAN_UPDATE_BANK = "CanUpdateBank";
        public const string CAN_DELETE_BANK = "CanDeleteBank";



        public static List<RoleClaimModel> ApplicationClaims = new List<RoleClaimModel>()
        {
            new RoleClaimModel { Name = CAN_GET_BANKS, Description = "Can Get Banks" },
            new RoleClaimModel { Name = CAN_UPDATE_BANK, Description = "Can Update Banks" },
            new RoleClaimModel { Name = CAN_DELETE_BANK, Description = "Can Delete Banks" }
        };

        public const string ClaimType = "ApplicationClaim";

        public const string SUPER_ADMIN = "SuperAdmin";
    }

    public class RoleClaimModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
