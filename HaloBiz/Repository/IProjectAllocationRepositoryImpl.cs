using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.DTOs.ProjectManagementDTO;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Models.ProjectManagement;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.Repository
{
    public interface IProjectAllocationRepositoryImpl
    {

        Task<ProjectAllocation> saveNewManager(ProjectAllocation projectAllocation);

        Task<List<ProjectAllocation>> getAllManagerProjects(string email, int Id);

        Task<List<ProjectAllocation>> getAllProjectManager(int category);

        Task<Workspace> createWorkspace(HttpContext context,WorkspaceDTO workspaceDTO);

        //Task<StatusFlow> createStatusFlow(StatusFlow statusFlow);

        Task<List<Workspace>> getAllWokspaces();

        Task<Workspace> getWorkSpaceById(long id);

    }
}