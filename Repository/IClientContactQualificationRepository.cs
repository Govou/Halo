using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IClientContactQualificationRepository
    {
        Task<ClientContactQualification> SaveClientContactQualification(ClientContactQualification clientContactQualification);
        Task<IEnumerable<ClientContactQualification>> GetClientContactQualifications();
        Task<ClientContactQualification> UpdateClientContactQualification(ClientContactQualification clientContactQualification);
        Task<bool> DeleteClientContactQualification(ClientContactQualification clientContactQualification);
        Task<ClientContactQualification> FindClientContactQualificationById(long Id);
    }
}