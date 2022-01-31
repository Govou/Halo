using HalobizMigrations.Models;
using System.Threading.Tasks;


namespace HaloBiz.Repository.LAMS
{
    public interface IReferenceNumberRepository
    {
        Task<ReferenceNumber> GetReferenceNumber();
        Task<bool> UpdateReferenceNumber(ReferenceNumber refNumber);
    }
}