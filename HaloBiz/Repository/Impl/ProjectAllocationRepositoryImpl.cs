using HaloBiz.DTOs.ProjectManagementDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Models.ProjectManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HaloBiz.Repository.Impl
{

    public class ProjectAllocationRepositoryImpl: IProjectAllocationRepositoryImpl
    {

        private readonly HalobizContext _context;
        private readonly ILogger<ProjectAllocationRepositoryImpl> _logger;
        public ProjectAllocationRepositoryImpl(HalobizContext context, ILogger<ProjectAllocationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }



        public async Task<ProjectAllocation> saveNewManager(ProjectAllocation projectAllocation)
        {
            var ProjectAllocationEntity = await _context.ProjectAllocations.AddAsync(projectAllocation);
           
            if (await SaveChanges())
            {
                return ProjectAllocationEntity.Entity;
            }
            return null;
        }


        public async Task<Workspace> createWorkspace(HttpContext context,WorkspaceDTO workspaceDTO)
        {
            var priviledgedList = new List<PrivacyAccess>();
            var projectCreatorsList = new List<ProjectCreator>();
            var statusFlowList = new List<StatusFlow>();


            var workspaceToBeSaved = new Workspace()
            {
                Alias = workspaceDTO.Alias,
                StatusFlowOption = workspaceDTO.StatusFlowOption,
                Description = workspaceDTO.Description,
                Caption = workspaceDTO.Caption,
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsPublic = workspaceDTO.IsPublic,
                CreatedById = context.GetLoggedInUserId(),
            };

            var savedWorkspace = await _context.Workspaces.AddAsync(workspaceToBeSaved);
            await _context.SaveChangesAsync();

            foreach (var item in workspaceDTO.StatusFlowDTO) 
            {

                var statusFlowToBeSaved = new StatusFlow()
                {
                    LevelCount = item.LevelCount,
                    Caption = item.Caption,
                    CreatedAt = DateTime.Now,
                    Description = item.Description,
                    CreatedById = context.GetLoggedInUserId(),
                    Panthone = item.Panthone,
                    IsDeleted = false,
                    WorkspaceId = workspaceToBeSaved.Id
                    
                };

                statusFlowList.Add(statusFlowToBeSaved);

            }

            await _context.StatusFlows.AddRangeAsync(statusFlowList);
            await _context.SaveChangesAsync();

            foreach (var item in workspaceDTO.PrivacyAccesses)
            {
                
                var privacyAccessToBeSaved = new PrivacyAccess() {
                    PrivacyAccessId = item.PrivacyAccessId,
                    IsActive = true,
                    WorkspaceId = workspaceToBeSaved.Id,
                    CreatedById = context.GetLoggedInUserId(),
                    CreatedAt = DateTime.Now
                };

                priviledgedList.Add(privacyAccessToBeSaved);

            }
            await _context.PrivacyAccesses.AddRangeAsync(priviledgedList);

            foreach (var item in workspaceDTO.ProjectCreators)
            {

                var projectCreatorsToBeSaved = new ProjectCreator()
                {
                    IsActive = true,
                    WorkspaceId = workspaceToBeSaved.Id,
                    CreatedById = context.GetLoggedInUserId(),
                    CreatedAt = DateTime.Now,
                    ProjectCreatorProfileId = item.ProjectCreatorProfileId

                };

                projectCreatorsList.Add(projectCreatorsToBeSaved);

            }
            await _context.ProjectCreators.AddRangeAsync(projectCreatorsList);

            await _context.SaveChangesAsync();



            if (workspaceToBeSaved != null)
            {
                return savedWorkspace.Entity;
            }
            return null;
        }

       


        public async Task <List<ProjectAllocation>> getAllManagerProjects(string email,int Id)
        {
            var getMyProjects = await _context.ProjectAllocations.Where(x => x.ManagerId == Id && x.ManagerEmail == email).ToListAsync();

            if (getMyProjects.Count == 0)
            {
                return null;
            }
            return getMyProjects;
        }

        public async Task<List<ProjectAllocation>> getAllProjectManager(int categoryId)
        {
            var getMyManagers = await _context.ProjectAllocations.Where(x => x.ServiceCategoryId == categoryId &&  x.IsDeleted == false).ToListAsync();

            if (getMyManagers.Count == 0)
            {
                return getMyManagers;
            }
            return getMyManagers;
        }


        public async Task<List<Workspace>> getAllWokspaces()
        {

            var myWorkspaces = await _context.Workspaces.Where(x => x.IsActive != false).ToListAsync();
            if (myWorkspaces != null)
            {
                var myPrivacyAccess = await _context.PrivacyAccesses.ToListAsync();
                var myProjects = await _context.Projects.ToListAsync();
                var myProjectCreators = await _context.ProjectCreators.ToListAsync();
                var myStatusFlow = await _context.StatusFlows.ToListAsync();

            }


            if (myWorkspaces.Count == 0)
            {
                return myWorkspaces;
            }
            return myWorkspaces;
        }


        public async Task<Workspace> getWorkSpaceById(long id)
        {
            var myWorkspace = _context.Workspaces.FirstOrDefault(x => x.Id == id);
            if(myWorkspace != null)
            {
                var myPrivacyAccess = await _context.PrivacyAccesses.Where(x=>x.WorkspaceId == id).ToListAsync();
                var myProjects = await _context.Projects.Where(x => x.WorkspaceId == id).ToListAsync();
                var myProjectCreators = await _context.ProjectCreators.Where(x => x.WorkspaceId == id).ToListAsync();
                var myStatusFlow = await _context.StatusFlows.Where(x => x.WorkspaceId == id).ToListAsync();
            }

            if (myWorkspace == null)
            {
                return myWorkspace;
            }
            return myWorkspace;
        }


        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        
    }





}