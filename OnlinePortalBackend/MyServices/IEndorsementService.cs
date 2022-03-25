using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IEndorsementService
    {
        Task<string> FetchEndorsements(HttpContext context, int limit = 10);
        Task<string> TrackEndorsement(HttpContext context, string endorsementId);
    }
}
