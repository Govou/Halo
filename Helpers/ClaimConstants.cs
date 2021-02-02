using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Helpers
{
    public static class ClaimConstants
    {
        public const string CAN_GET_BANKS = "CanGetBanks";
        public const string CAN_UPDATE_BANK = "CanUpdateBank";
        public const string CAN_DELETE_BANK = "CanDeleteBank";

        public static List<string> ApplicationClaims = new List<string>() 
        {
            CAN_GET_BANKS, CAN_UPDATE_BANK, CAN_DELETE_BANK
        };

        public const string ClaimType = "ApplicationClaim";

        public const string SUPER_ADMIN = "SuperAdmin";
    }
}
