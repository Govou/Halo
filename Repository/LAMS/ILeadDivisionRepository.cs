using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface ILeadDivisionRepository
    {
        Task<LeadDivision> SaveLeadDivision(LeadDivision leadDivision);
        Task<LeadDivision> FindLeadDivisionById(long Id);
        Task<LeadDivision> FindLeadDivisionByName(string name);
        Task<LeadDivision> FindLeadDivisionByRCNumber(string rcNumber);
        Task<IEnumerable<LeadDivision>> FindAllLeadDivision();
        Task<LeadDivision> UpdateLeadDivision(LeadDivision leadDivision);
        Task<bool> DeleteLeadDivision(LeadDivision leadDivision);
    }
}