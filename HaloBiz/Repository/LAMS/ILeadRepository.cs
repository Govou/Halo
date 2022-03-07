using HalobizMigrations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HaloBiz.Repository.LAMS
{
    public interface ILeadRepository
    {
        Task<Lead> SaveLead(Lead lead);
        Task<Lead> FindLeadById(long Id);
        Task<Lead> FindLeadByReferenceNo(string refNo);
        Task<IEnumerable<Lead>> FindAllLead();
        Task<IEnumerable<Lead>> FindUserLeads(long userId);
        Task<Lead> UpdateLead(Lead lead);
        Task<bool> DeleteLead(Lead lead);
        Task<bool> SaveChanges();
        Task<IEnumerable<Lead>> FindAllUnApprovedLeads();
    }
}