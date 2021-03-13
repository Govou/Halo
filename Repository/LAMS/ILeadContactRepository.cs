using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface ILeadContactRepository
    {
        Task<LeadContact> SaveLeadContact(LeadContact entity) ;

        Task<LeadContact> FindLeadContactById(long Id) ;

        Task<IEnumerable<LeadContact>> FindAllLeadContact(); 

        LeadContact UpdateLeadContact(LeadContact entity);
        Task<bool> DeleteLeadContact(LeadContact entity);
        Task<bool> SaveChanges();
    }
}