using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;

namespace HaloBiz.Repository
{
    public interface IProjectAllocationRepositoryImpl
    {

        Task<ProjectAllocation> saveNewManager(ProjectAllocation projectAllocation);

        Task<List<ProjectAllocation>> getAllManagerProjects(string email, int Id);

        Task<List<ProjectAllocation>> getAllProjectManager(int category);


    }
}