namespace OnlinePortalBackend.Helpers
{
    public static class ClaimConstants
    {

        public const string ClaimType = "ApplicationClaim";

        public const string SuperAdmin = "SuperAdmin";
        public const string UnAssigned = "UnAssigned";
    }

    public class RoleClaimModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
