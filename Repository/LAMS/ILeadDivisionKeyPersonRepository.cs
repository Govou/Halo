using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface ILeadDivisionKeyPersonRepository
    {
        Task<LeadDivisionKeyPerson> SaveLeadDivisionKeyPerson(LeadDivisionKeyPerson entity);

        Task<LeadDivisionKeyPerson> FindLeadDivisionKeyPersonById(long Id);

        Task<IEnumerable<LeadDivisionKeyPerson>> FindAllLeadDivisionKeyPerson();
        Task<IEnumerable<LeadDivisionKeyPerson>> FindAllLeadDivisionKeyPersonByLeadDivisionId(long leadDivisionId);

        Task<LeadDivisionKeyPerson> UpdateLeadDivisionKeyPerson(LeadDivisionKeyPerson entity);
        Task<bool> DeleteLeadDivisionKeyPerson(LeadDivisionKeyPerson entity);
    }
}
