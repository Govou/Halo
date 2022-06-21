using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;

namespace OnlinePortalBackend.Helpers
{
    public interface IJwtHelper
    {
        string GenerateToken(OnlineProfile userProfile);
        (string token, string expiryTime) GenerateToken_v2(UserProfile userProfile);
        bool IsValidToken(string token);
    }
}