using System.Threading.Tasks;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadConversionService
    {
        Task<bool> ConvertLeadToClient(long leadId, long loggedInUserId);
    }
}