using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface ILeadTypeRepository
    {
        Task<LeadType> SaveLeadType(LeadType leadType);
        Task<LeadType> FindLeadTypeById(long Id);
        Task<LeadType> FindLeadTypeByName(string name);
        Task<IEnumerable<LeadType>> FindAllLeadType();
        Task<LeadType> UpdateLeadType(LeadType leadType);
        Task<bool> DeleteLeadType(LeadType leadType);
    }
}