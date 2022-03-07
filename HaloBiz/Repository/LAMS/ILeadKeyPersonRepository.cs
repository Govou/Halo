using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface ILeadKeyPersonRepository
    {
        Task<LeadKeyPerson> SaveLeadKeyPerson(LeadKeyPerson entity) ;

        Task<LeadKeyPerson> FindLeadKeyPersonById(long Id) ;

        Task<IEnumerable<LeadKeyPerson>> FindAllLeadKeyPerson(); 

        Task<LeadKeyPerson> UpdateLeadKeyPerson(LeadKeyPerson entity);
        Task<bool> DeleteLeadKeyPerson(LeadKeyPerson entity);
        Task<IEnumerable<LeadKeyPerson>> FindAllLeadKeyPersonByLeadId(long leadId);
    }
}