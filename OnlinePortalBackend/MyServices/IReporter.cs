using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IReporter
    {
        Task ReportExceptions(object Request);
    }
}
