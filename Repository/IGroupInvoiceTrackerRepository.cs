using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IGroupInvoiceTrackerRepository
    {
        Task<string> GenerateGroupInvoiceNumber();
    }
}