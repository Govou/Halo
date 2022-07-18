using System.Net.Http;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class Reporter : IReporter
    {
        private readonly IHttpClientFactory _httpClient;
        public Reporter(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task ReportExceptions(object Request)
        {
            try
            {
                using var client = _httpClient.CreateClient();
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
