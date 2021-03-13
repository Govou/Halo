using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface IReferenceNumberRepository
    {
        Task<ReferenceNumber> GetReferenceNumber();
        Task<bool> UpdateReferenceNumber(ReferenceNumber refNumber);
    }
}