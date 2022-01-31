using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IDesignationRepository
    {
        Task<Designation> SaveDesignation(Designation designation);
        Task<IEnumerable<Designation>> GetDesignations();
        Task<Designation> UpdateDesignation(Designation designation);
        Task<bool> DeleteDesignation(Designation designation);
        Task<Designation> FindDesignationById(long Id);
    }
}