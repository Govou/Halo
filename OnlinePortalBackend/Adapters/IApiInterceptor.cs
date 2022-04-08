using System.Threading.Tasks;

namespace OnlinePortalBackend.Adapters
{
    public interface IApiInterceptor
    {
        Task<string> GetToken();
    }
}
