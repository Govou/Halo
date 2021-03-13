using HaloBiz.Model.LAMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface ILeadDivisionContactRepository
    {
        Task<LeadDivisionContact> SaveLeadDivisionContact(LeadDivisionContact entity);

        Task<LeadDivisionContact> FindLeadDivisionContactById(long Id);

        Task<IEnumerable<LeadDivisionContact>> FindAllLeadDivisionContact();

        Task<LeadDivisionContact> UpdateLeadDivisionContact(LeadDivisionContact entity);
        Task<bool> DeleteLeadDivisionContact(LeadDivisionContact entity);
    }
}
