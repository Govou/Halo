﻿using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.GenericResponseDTO;
using HaloBiz.DTOs.ProjectManagementDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Repository;
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
using Task = HalobizMigrations.Models.ProjectManagement.Task;

namespace HaloBiz.MyServices.Impl
{
    public class ProjectAllocationServiceImpl : IProjectAllocationServiceImpl
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ProjectAllocationServiceImpl> _logger;
        private readonly IProjectAllocationRepositoryImpl _projectAllocationRepository;
        private readonly IMapper _mapper;

        public ProjectAllocationServiceImpl(HalobizContext context, IProjectAllocationRepositoryImpl projectAllocationRepository, ILogger<ProjectAllocationServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._projectAllocationRepository = projectAllocationRepository;
            this._logger = logger;
            this._context = context;
        }


        public async Task<ApiResponse> AddNewManager(HttpContext context, ProjectAllocationRecievingDTO projectAllocationDTO)
        {

            var existingManagerInAnotherMarketArea = _context.ProjectAllocations.Where(x => x.ManagerId == projectAllocationDTO.ManagerId
                                                       && x.ManagerEmail == projectAllocationDTO.ManagerEmail
                                                       && x.OperatingEntityId != projectAllocationDTO.OperatingEntityId
                                                       ).ToList();


            if (existingManagerInAnotherMarketArea.Count == 0)
            {

                var existingManagerInTheSameGroup = _context.ProjectAllocations.Where(x => x.ManagerId == projectAllocationDTO.ManagerId
                                                                   && x.ServiceCategoryId == projectAllocationDTO.ServiceCategoryId
                                                                   && x.ServiceGroupId == projectAllocationDTO.ServiceGroupId

                                                                   && x.IsDeleted == false).ToList();

                if (existingManagerInTheSameGroup.Count == 0)
                {
                    var projectClass = new ProjectAllocation()
                    {
                        OperatingEntityId = projectAllocationDTO.OperatingEntityId,
                        DivisionId = projectAllocationDTO.DivisionId,
                        ServiceCategoryId = projectAllocationDTO.ServiceCategoryId,
                        ServiceGroupId = projectAllocationDTO.ServiceGroupId,
                        IsMoved = false,
                        IsDeleted = false,
                        ManagerEmail = projectAllocationDTO.ManagerEmail,
                        ManagerId = projectAllocationDTO.ManagerId,
                        ManagerImageUrl = projectAllocationDTO.ManagerImageUrl,
                        ManagerMobileNo = projectAllocationDTO.ManagerMobileNo,
                        ManagerName = projectAllocationDTO.ManagerName,
                    };

                    var savedProjectManager = await _projectAllocationRepository.saveNewManager(projectClass);
                    if (savedProjectManager == null)
                    {
                        return new ApiResponse(500);
                    }
                    return new ApiOkResponse(savedProjectManager);

                }

                return new ApiOkResponse(409);


            }

            return new ApiOkResponse(409);


        }


        public async Task<ApiGenericResponse<Workspace>> CreateNewWorkspace(HttpContext context, WorkspaceDTO workspaceDTO) {



            var savedWorkSpaces = await _projectAllocationRepository.createWorkspace(context, workspaceDTO);

            if (savedWorkSpaces == null)
            {
                return new ApiGenericResponse<Workspace>
                {
                    responseCode = 500,
                    responseMessage = ResponseMessage.ENTITYNOTSAVED,
                    data = null
                };
            }
            else
            {


                return new ApiGenericResponse<Workspace>
                {
                    responseCode = 202,
                    responseMessage = ResponseMessage.EntitySuccessfullySaved,
                    data = savedWorkSpaces
                };

            }

        }




        public async Task<ApiResponse> getProjectManagers(int serviceCategoryId)
        {

            var getManagers = await _projectAllocationRepository.getAllProjectManager(serviceCategoryId);

            if (getManagers == null)
            {
                return new ApiOkResponse(getManagers);
            }

            return new ApiOkResponse(getManagers);


        }

        public async Task<ApiResponse> getAllWorkspaces(HttpContext httpContext)
        {

            var workspaceArr = new List<RevampedWorkspaceDTO>();
            var works = await _context.Workspaces.Where(x => x.IsActive != false && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();


            foreach (var item in works)
            {
                var workspaces = new RevampedWorkspaceDTO();
                workspaces.Id = item.Id;
                workspaces.Alias = item.Alias;
                workspaces.IsActive = item.IsActive;
                workspaces.Caption = item.Caption;
                workspaces.CreatedAt = item.CreatedAt;
                workspaces.Description = item.Description;
                workspaces.StatusFlowOption = item.StatusFlowOption;
                workspaces.Projects = await _context.Projects.Where(x => x.IsActive != false && x.WorkspaceId == item.Id).ToListAsync();
                workspaces.PrivacyAccesses = await _context.PrivacyAccesses.Where(x => x.IsActive != false && x.WorkspaceId == item.Id).ToListAsync();
                workspaces.ProjectCreators = await _context.ProjectCreators.Where(x => x.IsActive != false && x.WorkspaceId == item.Id).ToListAsync();
                workspaces.StatusFlowDTO = await _context.StatusFlows.Where(x => x.IsDeleted == false && x.WorkspaceId == item.Id).ToListAsync();
                workspaces.ProjectCreatorsLength = (item.ProjectCreators == null ? 0 : item.ProjectCreators.Count());
                workspaces.ProjectLength = (item.Projects == null ? 0 : item.Projects.Count());
                workspaces.IsPublic = item.IsPublic == false ? "Private" : "Public";
                workspaceArr.Add(workspaces);
            }

            if (workspaceArr == null)
            {
                return new ApiOkResponse(workspaceArr);
            }

            return new ApiOkResponse(workspaceArr);

        }


        public async Task<ApiResponse> getWorkspaceById(long id)
        {

            var getWorkspace = await _projectAllocationRepository.getWorkSpaceById(id);

            if (getWorkspace == null)
            {
                return new ApiOkResponse(getWorkspace);
            }

            return new ApiOkResponse(getWorkspace);

        }


        public async Task<ApiResponse> getWorkspaceByCaption(string caption)
        {

            var result = await _context.Workspaces.FirstOrDefaultAsync(x => x.Caption == caption && x.IsActive != false);


            if (result == null)
            {
                return new ApiOkResponse(result);
            }

            return new ApiOkResponse(result);

        }


        public async Task<ApiResponse> getAllProjectManagers()
        {

            var getAllManagers = await _context.ProjectAllocations.ToListAsync();

            if (getAllManagers == null)
            {
                return new ApiOkResponse(getAllManagers);
            }

            return new ApiOkResponse(getAllManagers);

        }



        public async Task<ApiResponse> getDefaultStatus()
        {

            var getDefaultStatus = await _context.StatusFlows.Where(x => x.WorkspaceId == 84).ToListAsync();

            if (getDefaultStatus == null)
            {
                return new ApiOkResponse(getDefaultStatus);
            }

            return new ApiOkResponse(getDefaultStatus);

        }




        public async Task<ApiResponse> removeFromCategory(long id, int category, long projectId)
        {
            var activityToDelete = await _context.ProjectAllocations.FirstOrDefaultAsync(x => x.ManagerId == id
                                                                    && x.ServiceCategoryId == category
                                                                    && x.Id == projectId);
            if (activityToDelete == null)
            {
                return new ApiResponse(404);
            }
            activityToDelete.IsDeleted = true;
            _context.ProjectAllocations.Update(activityToDelete);
            await _context.SaveChangesAsync();
            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> removeFromProjectCreator(long workspaceId, long creatorId)
        {
            var creatorToDelete = await _context.ProjectCreators.FirstOrDefaultAsync(x => x.ProjectCreatorProfileId == creatorId && x.IsActive != false && x.WorkspaceId == workspaceId);
            if (creatorToDelete == null)
            {
                return new ApiResponse(404);
            }
            creatorToDelete.IsActive = false;
            _context.ProjectCreators.Update(creatorToDelete);
            await _context.SaveChangesAsync();
            var remainderUser = await _context.ProjectCreators.Where(x => x.WorkspaceId == workspaceId && x.IsActive != false).ToListAsync();
            return new ApiOkResponse(remainderUser);
        }

        public async Task<ApiResponse> updateToPublic(long workspaceId)
        {

            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == workspaceId);
            workspace.IsPublic = true;
            _context.Workspaces.UpdateRange(workspace);
            var privateUserArray = new List<PrivacyAccess>();
            var privateUser = await _context.PrivacyAccesses.Where(x => x.WorkspaceId == workspaceId && x.IsActive != false).ToListAsync();
            foreach (var item in privateUser)
            {

                if (privateUser != null)
                {
                    item.IsActive = false;
                    privateUserArray.Add(item);

                }

            }

            _context.PrivacyAccesses.UpdateRange(privateUserArray);
            await _context.SaveChangesAsync();
            var newprivateUserArr = await _context.ProjectCreators.Where(x => x.WorkspaceId == workspaceId && x.IsActive != false).ToListAsync();

            return new ApiOkResponse(newprivateUserArr);

        }


        public async Task<ApiResponse> disablePrivateUser(long workspaceId, long privateUserId)
        {


            var privateUser = await _context.PrivacyAccesses.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.IsActive != false && x.PrivacyAccessId == privateUserId);


            if (privateUser == null)
            {
                return new ApiResponse(404);
            }
            privateUser.IsActive = false;
            _context.PrivacyAccesses.Update(privateUser);
            await _context.SaveChangesAsync();
            var remainderUser = await _context.PrivacyAccesses.Where(x => x.WorkspaceId == workspaceId && x.IsActive != false).ToListAsync();
            return new ApiOkResponse(remainderUser);
        }

        public async Task<ApiResponse> disableStatus(long workspaceId, long statusId)
        {

            var status = await _context.StatusFlows.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.Id == statusId);


            if (status == null)
            {
                return new ApiResponse(404);
            }
            status.IsDeleted = true;
            _context.StatusFlows.Update(status);
            await _context.SaveChangesAsync();
            var allActveStatus = await _context.StatusFlows.Where(x => x.WorkspaceId == workspaceId && x.IsDeleted == false).ToListAsync();
            return new ApiOkResponse(allActveStatus);
        }


        public async Task<ApiResponse> disableWorkspace(long id)
        {
            var activityToDelete = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == id);
            if (activityToDelete == null)
            {
                return new ApiResponse(404);
            }
            activityToDelete.IsActive = false;
            _context.Workspaces.Update(activityToDelete);
            await _context.SaveChangesAsync();
            return new ApiOkResponse(true);
        }

        public async Task<ApiGenericResponse<Workspace>> updateWorkspace(HttpContext httpContext, long id, UpdateWorkspaceDTO workspaceDTO)
        {
            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
            if (workspace != null)
            {
                var duplicateCaption = await _context.Workspaces.FirstOrDefaultAsync(x => x.Caption == workspaceDTO.Caption && x.IsActive == true);
               
                if(workspace.Caption == workspaceDTO.Caption || duplicateCaption != null)
                {
                    if (workspaceDTO.Description == null)
                        workspaceDTO.Description = workspace.Description;
                    if (workspaceDTO.Alias == null)
                        workspaceDTO.Alias = workspace.Alias;
                    workspace.Alias = workspaceDTO.Alias;
                    workspace.Description = workspaceDTO.Description;
                    _context.Workspaces.Update(workspace);
                    await _context.SaveChangesAsync();
                    var updatedWorkspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);

                    return new ApiGenericResponse<Workspace>
                    {
                        responseCode = 200,
                        responseMessage = "The caption could not be updated because it already exist,but every other value provided where successfully updated.",
                        data = updatedWorkspace
                    };


                }
                else
                {

                    if (workspaceDTO.Caption == null)
                        workspaceDTO.Caption = workspace.Caption;
                    if (workspaceDTO.Description == null)
                        workspaceDTO.Description = workspace.Description;
                    if (workspaceDTO.Alias == null)
                        workspaceDTO.Alias = workspace.Alias;
                    
                    
                    workspace.Alias = workspaceDTO.Alias;
                    workspace.Caption = workspaceDTO.Caption;
                    workspace.Description = workspaceDTO.Description;
                    _context.Workspaces.Update(workspace);
                    await _context.SaveChangesAsync();
                    var updatedWorkspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
                    return new ApiGenericResponse<Workspace>
                    {
                        responseCode = 200,
                        responseMessage = "Workspace successfuly updated",
                        data = updatedWorkspace
                    };

                }



            }
            else
            {
                return new ApiGenericResponse<Workspace>
                {
                    responseCode = 404,
                    responseMessage = "Workspace with Id " + id + " does not exist",
                    data = null
                };

            }
        }



     
    


        public async Task<ApiResponse> addMoreProjectCreators(HttpContext httpContext, long id, List<AddMoreUserDto> projectCreatorDtos)
        {
            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == id);
            if (workspace == null)
            {
                return new ApiResponse(404);
            }

            var projectCreatorsList = new List<ProjectCreator>();

          

            foreach (var item in projectCreatorDtos)
            {
                if (await _context.ProjectCreators.FirstOrDefaultAsync(x => x.ProjectCreatorProfileId == item.usersId && x.IsActive != false && x.WorkspaceId == id) == null)
                {

                    var projectCreatorsToBeSaved = new ProjectCreator()
                    {
                        IsActive = true,
                        WorkspaceId = id,
                        CreatedById = httpContext.GetLoggedInUserId(),
                        UpdatedAt = DateTime.Now,
                        ProjectCreatorProfileId = item.usersId

                    };

                    projectCreatorsList.Add(projectCreatorsToBeSaved);
                }
                

            }
            await _context.ProjectCreators.AddRangeAsync(projectCreatorsList);
            await _context.SaveChangesAsync();
            var remainderUser = await _context.ProjectCreators.Where(x => x.WorkspaceId == id && x.IsActive != false).ToListAsync();
            return new ApiOkResponse(remainderUser);
        }


        public async Task<ApiGenericResponse<Project>> updateProject(HttpContext httpContext, long projectId , ProjectDTO projectDTO)
        {
            var projectFound = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true);
            
            if(projectFound != null)
            {
                var getProjectByCaption = await _context.Projects.FirstOrDefaultAsync(x => x.IsActive == true && x.Caption == projectDTO.Caption);

                if(projectDTO.Caption == projectFound.Caption || getProjectByCaption != null)
                {
                    if (projectDTO.Alias == null)
                        projectDTO.Alias = projectFound.Alias;
                    if (projectDTO.ProjectImage == null)
                        projectDTO.ProjectImage = projectFound.ProjectImage;
                    if (projectDTO.Description == null)
                        projectDTO.Description = projectFound.Description;

                    projectFound.Alias = projectDTO.Alias;
                    projectFound.Description = projectDTO.Description;
                    projectFound.ProjectImage = projectDTO.ProjectImage;
                    _context.Projects.Update(projectFound);
                    await _context.SaveChangesAsync();
                    var updatedProject = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId && x.IsActive == true);
                    return new ApiGenericResponse<Project>
                    {
                        responseCode = 200,
                        responseMessage = "The caption could not be updated because it already exist,but every other values provided where successfully updated.",
                        data = updatedProject
                    };

                }
                else
                {
                    if (projectDTO.Alias == null)
                        projectDTO.Alias = projectFound.Alias;
                    if (projectDTO.Caption == null)
                        projectDTO.Caption = projectFound.Caption;
                    if (projectDTO.ProjectImage == null)
                        projectDTO.ProjectImage = projectFound.ProjectImage;
                    if (projectDTO.Description == null)
                        projectDTO.Description = projectFound.Description;

                    projectFound.Alias = projectDTO.Alias;
                    projectFound.Caption = projectDTO.Caption;
                    projectFound.Description = projectDTO.Description;
                    projectFound.ProjectImage = projectDTO.ProjectImage;
                    _context.Projects.Update(projectFound);
                    await _context.SaveChangesAsync();
                    var updatedProject = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId && x.IsActive == true);
                    return new ApiGenericResponse<Project>
                    {
                        responseCode = 200,
                        responseMessage = "Project was updated successfully",
                        data = updatedProject
                    };
                }

            }
            else
            {
                return new ApiGenericResponse<Project>
                {
                    responseCode = 404,
                    responseMessage = "Project with Id " + projectId + " does not exist",
                    data = null
                };
            }
        }


        public async Task<ApiGenericResponse<TaskSummaryDTO>> updateTask(HttpContext httpContext, long TaskId, TaskDTO taskDTO)
        {
            var tasksfound = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == TaskId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true);

            if (tasksfound != null)
            {
                var taskFoundByCaption = await _context.Tasks.FirstOrDefaultAsync(x => x.Caption == taskDTO.Caption && x.IsActive == true);

                if (taskDTO.Caption == tasksfound.Caption || taskFoundByCaption != null)
                {
                    if (taskDTO.Alias == null)
                        taskDTO.Alias = tasksfound.Alias;


                    if (taskDTO.TaskEndDate == DateTime.MinValue)
                        taskDTO.TaskEndDate = tasksfound.TaskEndDate;
                    if (taskDTO.TaskStartDate == DateTime.MinValue)
                        taskDTO.TaskStartDate = tasksfound.TaskStartDate;
                    if (taskDTO.WorkingManHours == 0)
                        taskDTO.WorkingManHours = tasksfound.WorkingManHours;
                    if (taskDTO.DueTime == DateTime.MinValue)
                        taskDTO.DueTime = tasksfound.DueTime;

                    if (taskDTO.Description == null)
                        taskDTO.Description = tasksfound.Description;

                    tasksfound.Alias = taskDTO.Alias;
                    tasksfound.Description = taskDTO.Description;
                    tasksfound.DueTime = taskDTO.DueTime;
                    tasksfound.TaskEndDate = taskDTO.TaskEndDate;
                    tasksfound.WorkingManHours = taskDTO.WorkingManHours;
                    tasksfound.TaskStartDate = taskDTO.TaskStartDate; ;
                    _context.Tasks.Update(tasksfound);
                    await _context.SaveChangesAsync();
                    var updatedTask = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == TaskId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
                    var taskSummary = new TaskSummaryDTO();
                    taskSummary.Alias = updatedTask.Alias;
                    taskSummary.DueTime = updatedTask.DueTime;
                    taskSummary.WorkingManHours = updatedTask.WorkingManHours;
                    taskSummary.Caption = updatedTask.Caption;
                    taskSummary.CreatedAt = updatedTask.CreatedAt;
                    taskSummary.CreatedById = updatedTask.CreatedById;
                    taskSummary.Description = updatedTask.Description;
                    taskSummary.IsAssigned = updatedTask.IsAssigned;
                    taskSummary.IsMilestone = updatedTask.IsMilestone;
                    taskSummary.IsReassigned = updatedTask.IsReassigned;
                    taskSummary.IsWorkbenched = updatedTask.IsWorkbenched;
                    taskSummary.ProjectId = updatedTask.ProjectId;
                    taskSummary.TaskEndDate = updatedTask.TaskEndDate;
                    taskSummary.TaskStartDate = updatedTask.TaskStartDate;
                    taskSummary.Project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == updatedTask.ProjectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
                    taskSummary.TaskAssignees = await _context.TaskAssignees.Where(X => X.TaskId == updatedTask.Id && X.IsActive == true && X.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    taskSummary.workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == taskSummary.Project.WorkspaceId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());

                    return new ApiGenericResponse<TaskSummaryDTO>
                    {
                        responseCode = 200,
                        responseMessage = "The caption could not be updated because it already exist,but every other values provided where successfully updated.",
                        data = taskSummary,
                    };

                }
                else
                {
                    if (taskDTO.Caption == null)
                        taskDTO.Caption = tasksfound.Caption;
                    if (taskDTO.Alias == null)
                        taskDTO.Alias = tasksfound.Alias;

                    if (taskDTO.TaskEndDate == DateTime.MinValue)
                        taskDTO.TaskEndDate = tasksfound.TaskEndDate;
                    if (taskDTO.TaskStartDate == DateTime.MinValue)
                        taskDTO.TaskStartDate = tasksfound.TaskStartDate;
                    if (taskDTO.WorkingManHours == 0)
                        taskDTO.WorkingManHours = tasksfound.WorkingManHours;
                    if (taskDTO.DueTime == DateTime.MinValue)
                        taskDTO.DueTime = tasksfound.DueTime;

                    if (taskDTO.Description == null)
                        taskDTO.Description = tasksfound.Description;
                    tasksfound.Caption = taskDTO.Caption;
                    tasksfound.Alias = taskDTO.Alias;
                    tasksfound.Description = taskDTO.Description;
                    tasksfound.DueTime = taskDTO.DueTime;
                    tasksfound.TaskEndDate = taskDTO.TaskEndDate;
                    tasksfound.WorkingManHours = taskDTO.WorkingManHours;
                    tasksfound.TaskStartDate = taskDTO.TaskStartDate; 
                    _context.Tasks.Update(tasksfound);
                    await _context.SaveChangesAsync();
                    var updatedTask = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == TaskId && x.IsActive == true);
                    var taskSummary = new TaskSummaryDTO();
                    taskSummary.Alias = updatedTask.Alias;
                    taskSummary.Caption = updatedTask.Caption;
                    taskSummary.CreatedAt = updatedTask.CreatedAt;
                    taskSummary.CreatedById = updatedTask.CreatedById;
                    taskSummary.Description = updatedTask.Description;
                    taskSummary.DueTime = updatedTask.DueTime;
                    taskSummary.WorkingManHours = updatedTask.WorkingManHours;
                    taskSummary.IsAssigned = updatedTask.IsAssigned;
                    taskSummary.IsMilestone = updatedTask.IsMilestone;
                    taskSummary.IsReassigned = updatedTask.IsReassigned;
                    taskSummary.IsWorkbenched = updatedTask.IsWorkbenched;
                    taskSummary.ProjectId = updatedTask.ProjectId;
                    taskSummary.TaskEndDate = updatedTask.TaskEndDate;
                    taskSummary.TaskStartDate = updatedTask.TaskStartDate;
                    taskSummary.Project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == updatedTask.ProjectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
                    taskSummary.TaskAssignees = await _context.TaskAssignees.Where(X => X.TaskId == updatedTask.Id && X.IsActive == true && X.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    taskSummary.workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == taskSummary.Project.WorkspaceId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());

                    return new ApiGenericResponse<TaskSummaryDTO>
                    {
                        responseCode = 200,
                        responseMessage = "Task was updated successfully",
                        data = taskSummary,
                    };
                }

            }
            else
            {
                return new ApiGenericResponse<TaskSummaryDTO>
                {
                    responseCode = 404,
                    responseMessage = "Task with Id " + TaskId + " does not exist",
                    data = null
                };
            }
        }




        public async Task<ApiResponse> addMorePrivateUser(HttpContext httpContext, long workspaceId, List<AddMoreUserDto> privateUserid)
        {
            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == workspaceId);
            if (workspace == null)
            {
                return new ApiResponse(404);
            }

            workspace.IsPublic = false;
            _context.Workspaces.Update(workspace);



            var privacyList = new List<PrivacyAccess>();

            foreach (var item in privateUserid)
            {
                if (await _context.PrivacyAccesses.FirstOrDefaultAsync(x => x.PrivacyAccessId == item.usersId && x.IsActive != false && x.WorkspaceId == workspaceId) == null)
                {

                    var privateToBeSaved = new PrivacyAccess()
                    {
                        IsActive = true,
                        WorkspaceId = workspaceId,
                        CreatedById = httpContext.GetLoggedInUserId(),
                        UpdatedAt = DateTime.Now,
                        PrivacyAccessId = item.usersId

                    };
                    privacyList.Add(privateToBeSaved);

                }


            }
            await _context.PrivacyAccesses.AddRangeAsync(privacyList);
            await _context.SaveChangesAsync();
            var remainderUser = await _context.PrivacyAccesses.Where(x => x.WorkspaceId == workspaceId && x.IsActive != false).ToListAsync();
            return new ApiOkResponse(remainderUser);

        }

        public async Task<ApiResponse> updateStatus(HttpContext httpContext, long workspaceId, long statusFlowId,StatusFlowDTO statusFlowDTO)
        {
            var gottenStatusFlow = await _context.StatusFlows.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.Id == statusFlowId);
            if (gottenStatusFlow == null)
            {
                return new ApiResponse(404);
            }

            gottenStatusFlow.LevelCount = statusFlowDTO.LevelCount;
            gottenStatusFlow.Caption = statusFlowDTO.Caption;
            gottenStatusFlow.CreatedAt = DateTime.Now;
            gottenStatusFlow.Description = statusFlowDTO.Description;
            gottenStatusFlow.CreatedById = httpContext.GetLoggedInUserId();
            gottenStatusFlow.Panthone = statusFlowDTO.Panthone;
            gottenStatusFlow.WorkspaceId = workspaceId;

            var StatusArray = await _context.StatusFlows.Where(x => x.WorkspaceId == workspaceId && x.IsDeleted == false).ToListAsync();

            _context.StatusFlows.Update(gottenStatusFlow);
            await _context.SaveChangesAsync();
            return new ApiOkResponse(gottenStatusFlow);

        }

        public async Task<ApiGenericResponse<List<Watcher>>> addmoreWatchers(HttpContext httpContext, long projectId, List<WatchersDTO> watchersDTOs)
        {
            var gottenProject = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
           
            if (gottenProject == null)
            {
                return new ApiGenericResponse<List<Watcher>>
                {
                    responseCode = 404,
                    responseMessage = "Watcher(s) with id "+ projectId + "was not found.",
                    data = null
                };
            }
            else
            {
                var watchersArray = new List<Watcher>();
                var getCurrentWatchers = await _context.Watchers.Where(x => x.ProjectId == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                if (getCurrentWatchers.Count() == 0)
                {

                    foreach (var item in watchersDTOs)
                    {
                        var watchersInstance = new Watcher();
                        watchersInstance.CreatedById = httpContext.GetLoggedInUserId();
                        watchersInstance.ProjectWatcherId = item.ProjectWatcherId;
                        watchersInstance.ProjectId = item.ProjectId;
                        watchersInstance.IsActive = true;
                        watchersInstance.CreatedAt = DateTime.Now;

                        watchersArray.Add(watchersInstance);

                    }

                }
                else
                {
                    
                        foreach (var item in watchersDTOs)
                        {

                        var getExistingWatcher = await _context.Watchers.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId() && x.ProjectWatcherId == item.ProjectWatcherId);

                            if (getExistingWatcher  == null)
                            {

                                var watchersInstance = new Watcher();
                                watchersInstance.CreatedById = httpContext.GetLoggedInUserId();
                                watchersInstance.ProjectWatcherId = item.ProjectWatcherId;
                                watchersInstance.ProjectId = item.ProjectId;
                                watchersInstance.IsActive = true;
                                watchersInstance.CreatedAt = DateTime.Now;

                                watchersArray.Add(watchersInstance);
                            }

                        }
                    
                }

                await _context.Watchers.AddRangeAsync(watchersArray);
                await _context.SaveChangesAsync();
                var getUpdatedWatchers = await _context.Watchers.Where(x => x.ProjectId == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

                return new ApiGenericResponse<List<Watcher>>
                {
                    responseCode = 200,
                    responseMessage = "Watcher(s) were successfully added.",
                    data = getUpdatedWatchers
                };
            }


        }

        


        public async Task<ApiGenericResponse<List<Watcher>>> removeWatcher(HttpContext httpContext, long projectId, long projectWatcherId)
        {
            var ifWatcherExist = await _context.Watchers.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.ProjectWatcherId == projectWatcherId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
            if(ifWatcherExist == null)
            {
                return new ApiGenericResponse<List<Watcher>>
                {
                    responseCode = 404,
                    responseMessage = "Watcher(s) with id " + projectId + "was not found.",
                    data = null
                };
            }
            else
            {
                ifWatcherExist.IsActive = false;
                _context.Watchers.Update(ifWatcherExist);
                await _context.SaveChangesAsync();
                var getUpdatedWatchers =await _context.Watchers.Where(x => x.CreatedById == httpContext.GetLoggedInUserId() && x.ProjectId == projectId && x.IsActive == true).ToListAsync();
                return new ApiGenericResponse<List<Watcher>>
                {
                    responseCode = 200,
                    responseMessage = "Watcher wwas successfully removed",
                    data = getUpdatedWatchers
                };
            }
        }





        public async Task<ApiResponse> addmoreStatus(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO)
        {
            

            var statusArray = new List<StatusFlow>();
            foreach (var item in statusFlowDTO)
            {
                var gottenStatusFlow = new StatusFlow();
                gottenStatusFlow.LevelCount = item.LevelCount;
                gottenStatusFlow.Caption = item.Caption;
                gottenStatusFlow.CreatedAt = DateTime.Now;
                gottenStatusFlow.Description = item.Description;
                gottenStatusFlow.CreatedById = httpContext.GetLoggedInUserId();
                gottenStatusFlow.Panthone = item.Panthone;
                gottenStatusFlow.WorkspaceId = workspaceId;

                statusArray.Add(gottenStatusFlow);

            }


            await _context.StatusFlows.AddRangeAsync(statusArray);
            await _context.SaveChangesAsync();
            var currentStatus = await _context.StatusFlows.Where(x => x.WorkspaceId == workspaceId  &&x.CreatedById == httpContext.GetLoggedInUserId() && x.IsDeleted == false).ToListAsync();
            return new ApiOkResponse(currentStatus);

        }


        public async Task<ApiResponse> moveStatusSequenec(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO)
        {


            foreach (var item in statusFlowDTO)
            {
                var statusToDisable = await _context.StatusFlows.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.Caption == item.Caption.Trim());
                if (statusToDisable != null)
                {
                    _context.Remove(statusToDisable);
                    await _context.SaveChangesAsync();
                }

            }

            var statusArray = new List<StatusFlow>();
            foreach (var item in statusFlowDTO)
            {
                var gottenStatusFlow = new StatusFlow();
                gottenStatusFlow.LevelCount = item.LevelCount;
                gottenStatusFlow.Caption = item.Caption;
                gottenStatusFlow.CreatedAt = DateTime.Now;
                gottenStatusFlow.Description = item.Description;
                gottenStatusFlow.CreatedById = httpContext.GetLoggedInUserId();
                gottenStatusFlow.Panthone = item.Panthone;
                gottenStatusFlow.WorkspaceId = workspaceId;

                statusArray.Add(gottenStatusFlow);

            }


            _context.StatusFlows.AddRange(statusArray);
            await _context.SaveChangesAsync();
            var currentStatus = await _context.StatusFlows.Where(x => x.WorkspaceId == workspaceId && x.IsDeleted == false).ToListAsync();
            return new ApiOkResponse(statusArray);

        }

        

        public async Task<ApiResponse> createDefaultStatus(HttpContext httpContext, List<DefaultStatusDTO> defaultStatusFlows)
        {

            var defaultStatusArray = new List<DefaultStatusFlow>();

            foreach(var item in defaultStatusFlows)
            {
                var defaultStatusInstance = new DefaultStatusFlow();
                defaultStatusInstance.Caption = item.Caption;

                defaultStatusInstance.CreatedAt = DateTime.Now;
                defaultStatusInstance.CreatedById = httpContext.GetLoggedInUserId();
                defaultStatusInstance.Description = item.Description;
                defaultStatusInstance.LevelCount = item.LevelCount;
                defaultStatusInstance.Panthone = item.Panthone;
                defaultStatusInstance.IsDeleted = false;
                

                defaultStatusArray.Add(defaultStatusInstance);
            }

            await _context.DefaultStatusFlows.AddRangeAsync(defaultStatusArray);
            await _context.SaveChangesAsync();
            var currentStatus = await _context.StatusFlows.Where(x=>x.IsDeleted == false).ToListAsync();
            return new ApiOkResponse(defaultStatusArray);

        }


        public async Task<ApiResponse> createProject(HttpContext httpContext, ProjectDTO projectDTO)
        {

            var projectTobeSaved = new Project()
            {
                Caption = projectDTO.Caption,
                Description = projectDTO.Description,
                Alias = projectDTO.Alias,
                ProjectImage = projectDTO.ProjectImage,
                IsActive = true,
                WorkspaceId = projectDTO.WorkspaceId,
                CreatedById = httpContext.GetLoggedInUserId(),
                CreatedAt = DateTime.Now,
            };

              _context.Projects.Add(projectTobeSaved);
               await _context.SaveChangesAsync();


            foreach (var item in projectDTO.Watchers)
            {
                var watchersToBeSaved = new Watcher()
                {
                    IsActive = true,
                    ProjectWatcherId = item.ProjectWatcherId,
                    ProjectId = projectTobeSaved.Id,
                    CreatedById = httpContext.GetLoggedInUserId(),
                    CreatedAt = DateTime.Now
                };

                await _context.Watchers.AddAsync(watchersToBeSaved);
            }

                await _context.SaveChangesAsync();
                var getAllProjects = await _context.Projects.Where(x => x.WorkspaceId == projectDTO.WorkspaceId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true).ToListAsync();
                return new ApiOkResponse(getAllProjects);

        }


        public async Task<ApiResponse> updateStatusFlowOpton(HttpContext httpContext,long workspaceId,string statusOption, List<StatusFlowDTO> statusFlowDTOs)
        {


            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == workspaceId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive);
            if (workspace != null)
            {
                workspace.StatusFlowOption = statusOption;
                _context.Workspaces.Update(workspace);
                //await _context.SaveChangesAsync();
                
            }

           var statusToDisable = await _context.StatusFlows.Where(x => x.WorkspaceId == workspaceId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsDeleted != true).ToListAsync();
           if(statusToDisable != null)
            {
                var statustoUpdate = new List<StatusFlow>();
                foreach (var item in statusToDisable)
                {
                    item.IsDeleted = true;
                    statustoUpdate.Add(item);
                }
                _context.StatusFlows.UpdateRange(statustoUpdate);
                await _context.SaveChangesAsync();
            }

           var statusArray = new List<StatusFlow>();
           foreach(var item in statusFlowDTOs)
           {
                var statusInstance = new StatusFlow();
                statusInstance.Caption = item.Caption;
                statusInstance.CreatedAt = DateTime.Now;
                statusInstance.CreatedById = httpContext.GetLoggedInUserId();
                statusInstance.Description = item.Description;
                statusInstance.IsDeleted = false;
                statusInstance.LevelCount = item.LevelCount;
                statusInstance.Panthone = item.Panthone;
                statusInstance.WorkspaceId = workspaceId;

                statusArray.Add(statusInstance);
                
              
           }
            await _context.StatusFlows.AddRangeAsync(statusArray);
            await _context.SaveChangesAsync();
            return new ApiOkResponse(statusArray);
        }


        public async Task<ApiResponse> getAllDefaultStatus()

        {

            var allDefaultStatus = await _context.DefaultStatusFlows.Where(x => x.IsDeleted == false).ToListAsync();

            if (allDefaultStatus == null)
            {
                return new ApiResponse(400);
            }

            return new ApiOkResponse(allDefaultStatus);


        }


        public async Task<ApiResponse> getAllProjects(HttpContext httpContext)

        {

            var getAllProjects = await _context.Projects.Where(x => x.IsActive == true &&  x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

            if (getAllProjects == null)
            {
                return new ApiResponse(400);
            }

            return new ApiOkResponse(getAllProjects);


        }


        public async Task<ApiGenericResponse<List<WorkspaceDTO>>> getAllProjectCreatorsWorkspace(HttpContext httpContext)

        {

            var getAllWorkspaces = await _context.Workspaces.Where(x => x.IsActive == true).ToListAsync();
            if(getAllWorkspaces.Count() == 0)
            {
                return new ApiGenericResponse<List<WorkspaceDTO>>
                {
                    responseCode = 404,
                    responseMessage = "No workspace was found",
                    data = null,
                };
            }

            else
            {

                var workspaceArray = new List<WorkspaceDTO>();
                foreach (var workspace in getAllWorkspaces)
                {
                    foreach (var projectCreator in workspace.ProjectCreators)
                    {
                        if (projectCreator.ProjectCreatorProfileId == httpContext.GetLoggedInUserId())
                        {
                            var workspaceInstance = new WorkspaceDTO();
                            workspaceInstance.Alias = workspace.Alias;
                            workspaceInstance.Caption = workspace.Caption;
                            workspaceInstance.Description = workspace.Description;
                            workspaceInstance.IsPublic = workspace.IsPublic;
                            workspaceInstance.StatusFlowOption = workspace.StatusFlowOption;
                            workspaceInstance.UpdatedAt = workspace.UpdatedAt;
                            workspaceInstance.IsActive = workspace.IsActive;
                            workspaceInstance.Id = workspace.Id;

                            workspaceArray.Add(workspaceInstance);
                        }
                    }
                }

                return new ApiGenericResponse<List<WorkspaceDTO>>
                {
                    responseCode = 200,
                    responseMessage = "workspace successfully retrieved",
                    data = workspaceArray,
                };


            }
          
        }


        public async Task<ApiGenericResponse<List<Deliverable>>> getAllDeliverables(HttpContext httpContext)

        {

            var getAllDeliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

            if (getAllDeliverables.Count() == 0)
            {
                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 404,
                    responseMessage = "No Deliverables was found",
                    data = null,
                };
            }
            else
            {

                var deliverableToDisplayArray = new List<Deliverable>();

                foreach (var item in getAllDeliverables)
                {
                    var deliverableToDisplayInstance = new Deliverable();
                    deliverableToDisplayInstance.Alias = item.Alias;
                    deliverableToDisplayInstance.Caption = item.Caption;
                    deliverableToDisplayInstance.Description = item.Description;
                    deliverableToDisplayInstance.Balances = await _context.Balances.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    deliverableToDisplayInstance.CreatedById = httpContext.GetLoggedInUserId();
                    deliverableToDisplayInstance.DatePicked = item.DatePicked;
                    deliverableToDisplayInstance.DeliverableAssignees = await _context.DeliverableAssignees.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true).ToListAsync();
                    deliverableToDisplayInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    deliverableToDisplayInstance.DependentType = item.DependentType;
                    deliverableToDisplayInstance.EndDate = item.EndDate;
                    deliverableToDisplayInstance.Id = item.Id;
                    deliverableToDisplayInstance.TimeEstimate = item.TimeEstimate;
                    deliverableToDisplayInstance.Videos = await _context.Videos.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    deliverableToDisplayInstance.Task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == item.TaskId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true);
                    deliverableToDisplayInstance.StartDate = item.StartDate;
                    deliverableToDisplayInstance.Requirements = await _context.PMRequirements.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == false).ToListAsync();
                    deliverableToDisplayInstance.Pictures = await _context.Pictures.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId() ).ToListAsync();
                    deliverableToDisplayInstance.IsActive = item.IsActive;
                    deliverableToDisplayInstance.Documents = await _context.Documents.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();


                    deliverableToDisplayArray.Add(deliverableToDisplayInstance);
                }

                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 200,
                    responseMessage = "Successfull retrieved all deliverables",
                    data = deliverableToDisplayArray,
                };
            }

            


        }


        public async Task<ApiGenericResponse<List<Deliverable>>> getAllDeliverablesByTaskId(HttpContext httpContext,long taskId)

        {

            var getAllDeliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == taskId && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

            if (getAllDeliverables == null || getAllDeliverables.Count() == 0)
            {
                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 404,
                    responseMessage = "No Deliverables was found",
                    data = null,
                };
            }
            else
            {

                var deliverableToDisplayArray = new List<Deliverable>();

                
                
                
                
                foreach (var item in getAllDeliverables)
                {
                    var deliverableToDisplayInstance = new Deliverable();
                    deliverableToDisplayInstance.Alias = item.Alias;
                    deliverableToDisplayInstance.Caption = item.Caption;
                    deliverableToDisplayInstance.Description = item.Description;
                    deliverableToDisplayInstance.Balances = await _context.Balances.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    deliverableToDisplayInstance.CreatedById = httpContext.GetLoggedInUserId();
                    deliverableToDisplayInstance.DatePicked = item.DatePicked;
                    deliverableToDisplayInstance.DeliverableAssignees = await _context.DeliverableAssignees.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true).ToListAsync();
                    deliverableToDisplayInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    deliverableToDisplayInstance.DependentType = item.DependentType;
                    deliverableToDisplayInstance.EndDate = item.EndDate;
                    deliverableToDisplayInstance.Id = item.Id;
                    deliverableToDisplayInstance.TimeEstimate = item.TimeEstimate;
                    deliverableToDisplayInstance.Videos = await _context.Videos.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    deliverableToDisplayInstance.Task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == item.TaskId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true);
                    deliverableToDisplayInstance.StartDate = item.StartDate;
                    deliverableToDisplayInstance.Requirements = await _context.PMRequirements.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == false).ToListAsync();
                    deliverableToDisplayInstance.Pictures = await _context.Pictures.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    deliverableToDisplayInstance.IsActive = item.IsActive;
                    deliverableToDisplayInstance.Documents = await _context.Documents.Where(x => x.DeliverableId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

                    deliverableToDisplayArray.Add(deliverableToDisplayInstance);
                }

                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 200,
                    responseMessage = "Successfull retrieved all deliverables",
                    data = deliverableToDisplayArray,
                };
            }


        }



        public async Task<ApiGenericResponse<Deliverable>> getDeliverablesById(HttpContext httpContext, long id)

        {

            var getAllDeliverables = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == id && x.CreatedById == httpContext.GetLoggedInUserId());

            if (getAllDeliverables == null)
            {
                return new ApiGenericResponse<Deliverable>
                {
                    responseCode = 404,
                    responseMessage = "No Deliverables was found",
                    data = null,
                };
            }
            else
            {

                    var deliverableToDisplayInstance = new Deliverable();
                    deliverableToDisplayInstance.Alias = getAllDeliverables.Alias;
                    deliverableToDisplayInstance.Caption = getAllDeliverables.Caption;
                    deliverableToDisplayInstance.Description = getAllDeliverables.Description;
                    deliverableToDisplayInstance.Balances = await _context.Balances.Where(x => x.DeliverableId == getAllDeliverables.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    deliverableToDisplayInstance.CreatedById = httpContext.GetLoggedInUserId();
                    deliverableToDisplayInstance.DatePicked = getAllDeliverables.DatePicked;
                    deliverableToDisplayInstance.DeliverableAssignees = await _context.DeliverableAssignees.Where(x => x.DeliverableId == getAllDeliverables.Id && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true).ToListAsync();
                    deliverableToDisplayInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == getAllDeliverables.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    deliverableToDisplayInstance.DependentType = getAllDeliverables.DependentType;
                    deliverableToDisplayInstance.EndDate = getAllDeliverables.EndDate;
                    deliverableToDisplayInstance.Id = getAllDeliverables.Id;
                    deliverableToDisplayInstance.TimeEstimate = getAllDeliverables.TimeEstimate;
                    deliverableToDisplayInstance.Videos = await _context.Videos.Where(x => x.DeliverableId == getAllDeliverables.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    deliverableToDisplayInstance.Task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == getAllDeliverables.TaskId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true);
                    deliverableToDisplayInstance.StartDate = getAllDeliverables.StartDate;
                    deliverableToDisplayInstance.Requirements = await _context.PMRequirements.Where(x => x.DeliverableId == getAllDeliverables.Id && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == false).ToListAsync();
                    deliverableToDisplayInstance.Pictures = await _context.Pictures.Where(x => x.DeliverableId == getAllDeliverables.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    deliverableToDisplayInstance.IsActive = getAllDeliverables.IsActive;
                    deliverableToDisplayInstance.Documents = await _context.Documents.Where(x => x.DeliverableId == getAllDeliverables.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();


                return new ApiGenericResponse<Deliverable>
                {
                    responseCode = 200,
                    responseMessage = "Successfull retrieved all deliverables",
                    data = deliverableToDisplayInstance,
                };
            }


        }


        public async Task<ApiResponse> getWorkByProjectCreatorId(HttpContext httpContext)

        {

            var projectCreatorList = new List<RevampedWorkspaceDTO>();
            var id = httpContext.GetLoggedInUserId();
            Console.WriteLine("Here is my id ", id);
            var getAllProjectCreatorsById = await _context.ProjectCreators.Where(x => x.IsActive == true && x.ProjectCreatorProfileId == httpContext.GetLoggedInUserId()).ToListAsync();

            if(getAllProjectCreatorsById != null)
            {
                

                foreach (var item in getAllProjectCreatorsById)
                {
                    var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == item.WorkspaceId);
                    if(workspace != null)
                    {
                        var assignedWorkspace = new RevampedWorkspaceDTO()
                        {
                            Caption = workspace.Caption,
                            Id = workspace.Id,
                            Alias = workspace.Alias,
                            CreatedAt = workspace.CreatedAt,
                            Description = workspace.Description,
                            IsActive = workspace.IsActive,
                            StatusFlowOption = workspace.StatusFlowOption,
                            Projects = await _context.Projects.Where(x => x.IsActive != false && x.CreatedById == httpContext.GetLoggedInUserId() && x.WorkspaceId == item.WorkspaceId).ToListAsync(),
                            ProjectCreators = await _context.ProjectCreators.Where(x => x.IsActive != false  && x.WorkspaceId == item.WorkspaceId).ToListAsync(),
                            PrivacyAccesses = await _context.PrivacyAccesses.Where(x => x.IsActive != false  && x.WorkspaceId == item.WorkspaceId).ToListAsync(),
                            StatusFlowDTO = await _context.StatusFlows.Where(x => x.IsDeleted == false  && x.WorkspaceId == item.WorkspaceId).ToListAsync(),
                            ProjectCreatorsLength = workspace.ProjectCreators == null ? 0 : workspace.ProjectCreators.Count(),
                            ProjectLength = workspace.Projects == null ? 0 : workspace.Projects.Count(),
                            IsPublic = workspace.IsPublic == false ? "Private" : "Public",
                        };

                        projectCreatorList.Add(assignedWorkspace);

                    }
                   
                    
                }

                if (projectCreatorList != null)
                {
                    return new ApiOkResponse(projectCreatorList);
                }

                return new ApiOkResponse(projectCreatorList);



            }

            return new ApiOkResponse(projectCreatorList);


        }


        public async Task<ApiResponse> getWatchersByProjectId(HttpContext httpContext, long projectId)

        {

            var getWatchersByProjectId = await _context.Watchers.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId() && x.ProjectId == projectId).ToListAsync();

            if (getWatchersByProjectId == null)
            {
                return new ApiOkResponse(getWatchersByProjectId);
            }

            return new ApiOkResponse(getWatchersByProjectId);


        }


        public async Task<ApiResponse> getProjectByProjectName(HttpContext httpContext,string projectName)

        {

            var getProjectByCaption = await _context.Projects.FirstOrDefaultAsync(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId() && x.Caption == projectName);

            if (getProjectByCaption == null)
            {
                return new ApiOkResponse(getProjectByCaption);
            }

            return new ApiOkResponse(getProjectByCaption);


        }



        public async Task<ApiGenericResponse<List<Deliverable>>> createNewDeliverable(HttpContext httpContext, long TaskId,DeliverableDTO deliverableDTO)

        {

            var getAvailableTask = await _context.Tasks.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId() && x.Id == TaskId).ToListAsync();
           
            if (getAvailableTask == null)
            {
                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 404,
                    responseMessage = "Task with Id" + TaskId + "could not be found",
                    data = null,
                };
            }

            else
            {
               
                var deliverableToBeSaved = new Deliverable();
                deliverableToBeSaved.Caption = deliverableDTO.Caption;
                deliverableToBeSaved.Alias = deliverableDTO.Alias;
                deliverableToBeSaved.Description = deliverableDTO.Description;
                deliverableToBeSaved.Budget = deliverableDTO.Budget;
                deliverableToBeSaved.StartDate = deliverableDTO.StartDate;
                deliverableToBeSaved.EndDate = deliverableDTO.EndDate;
                deliverableToBeSaved.TimeEstimate = deliverableDTO.TimeEstimate;
                deliverableToBeSaved.DependentType = deliverableDTO.DependentType;
                deliverableToBeSaved.CreatedAt = DateTime.Now;
                deliverableToBeSaved.CreatedById = httpContext.GetLoggedInUserId();
                deliverableToBeSaved.TaskId = TaskId;
                deliverableToBeSaved.IsActive = true;

                await _context.Deliverables.AddAsync(deliverableToBeSaved);
                await _context.SaveChangesAsync();

                if(deliverableDTO.Requirements.Count()>0)
                {
                    var requirementArray = new List<PMRequirement>();
                    foreach (var item in deliverableDTO.Requirements)
                    {
                        var requirementInstance = new PMRequirement();
                        requirementInstance.CreatedAt = DateTime.Now;
                        requirementInstance.CreatedById = httpContext.GetLoggedInUserId();
                        requirementInstance.DeliverableId = deliverableToBeSaved.Id;
                        requirementInstance.IsActive = true;
                        requirementInstance.Caption = item.Caption;
                        requirementInstance.Alias = item.Alias;
                        requirementInstance.Descrption = item.Descrption;
                        requirementInstance.FileExtention = item.FileExtention;
                        requirementArray.Add(requirementInstance);
                    }

                   _context.PMRequirements.AddRange(requirementArray);

                }

                if(deliverableDTO.Dependencies.Count() > 0)
                {
                    var dependyArray = new List<Dependency>();
                    foreach(var item in deliverableDTO.Dependencies)
                    {
                        var dependencyInstance = new Dependency();
                        dependencyInstance.CreatedAt = DateTime.Now;
                        dependencyInstance.CreatedById = httpContext.GetLoggedInUserId();
                        dependencyInstance.DependencyDeliverableId = deliverableToBeSaved.Id;
                        dependencyInstance.DependencyProfileId = item.DependencyProfileId;
                        dependencyInstance.DeliverableDependentOnId = item.DeliverableDependentOnId;
                        dependyArray.Add(dependencyInstance);
                    }

                     _context.Dependencies.AddRange(dependyArray);
                    await _context.SaveChangesAsync();
                }

                var updatedResult = await _context.Deliverables.Where(x => x.TaskId == TaskId && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 200,
                    responseMessage = "Deliverable successfully saved",
                    data = updatedResult,
                };
            }


        }


        public async Task<ApiGenericResponse<List<TaskSummaryDTO>>> createNewTask(HttpContext context,long projectId ,TaskDTO taskDTO)
        {
            var taskSummaryList = new List<TaskSummaryDTO>();
            var ProjectExist = await _context.Projects.FirstOrDefaultAsync(x => x.IsActive == true && x.CreatedById == context.GetLoggedInUserId() && x.Id == projectId);
            if(ProjectExist == null)
            {
                return new ApiGenericResponse<List<TaskSummaryDTO>>
                {
                    responseCode = 404,
                    responseMessage = "Project with Id" + projectId + "could not be found",
                    data = null,
                };
            }

            else
            {
                var TaskToBeSaved = new Task()
                {
                    Alias = taskDTO.Alias,
                    Caption = taskDTO.Caption,
                    Description = taskDTO.Description,
                    IsAssigned = taskDTO.TaskAssignees.Count > 0 ? true : false,
                    IsReassigned = false,
                    WorkingManHours = taskDTO.WorkingManHours,
                    DueTime = taskDTO.DueTime,
                    IsMilestone = taskDTO.IsMilestone,
                    IsWorkbenched = taskDTO.IsWorkbenched,
                    ProjectId = projectId,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    TaskEndDate = taskDTO.TaskEndDate,
                    TaskStartDate = taskDTO.TaskStartDate,
                    CreatedById = context.GetLoggedInUserId()
                    
                };
                await _context.Tasks.AddAsync(TaskToBeSaved);
                await _context.SaveChangesAsync();

                if ( taskDTO.TaskAssignees.Count > 0)
                {
                    var taskAssigneeArray = new List<TaskAssignee>();

                   foreach(var item in taskDTO.TaskAssignees)
                    {
                        var taskAssigneeInstance = new TaskAssignee()
                        {
                            TaskAssigneeId = item.TaskAssigneeId,
                            Name = item.Name,
                            TaskId = TaskToBeSaved.Id,
                            CreatedById = context.GetLoggedInUserId(),
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        };

                        taskAssigneeArray.Add(taskAssigneeInstance);
                    }

                    await _context.TaskAssignees.AddRangeAsync(taskAssigneeArray);
                    await _context.SaveChangesAsync();
                    var getUpdatedList = await _context.Tasks.Where(x => x.ProjectId == projectId && x.CreatedById == context.GetLoggedInUserId()).ToListAsync();
                    

                    foreach (var item in getUpdatedList)
                    {
                        var taskSummary = new TaskSummaryDTO();
                        taskSummary.Alias = item.Alias;
                        taskSummary.Caption = item.Caption;
                        taskSummary.CreatedAt = item.CreatedAt;
                        taskSummary.CreatedById = item.CreatedById;
                        taskSummary.Description = item.Description;
                        taskSummary.IsAssigned = item.IsAssigned;
                        taskSummary.IsMilestone = item.IsMilestone;
                        taskSummary.IsReassigned = item.IsReassigned;
                        taskSummary.IsWorkbenched = item.IsWorkbenched;
                        taskSummary.ProjectId = item.ProjectId;
                        taskSummary.TaskEndDate = item.TaskEndDate;
                        taskSummary.TaskStartDate = item.TaskStartDate;
                        taskSummary.Project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == item.ProjectId && x.IsActive == true);
                        taskSummary.TaskAssignees = await _context.TaskAssignees.Where(X => X.TaskId == item.Id && X.IsActive == true).ToListAsync();
                        taskSummary.workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == taskSummary.Project.WorkspaceId && x.IsActive == true);

                        taskSummaryList.Add(taskSummary);

                    }

                }
                return new ApiGenericResponse<List<TaskSummaryDTO>>
                {
                    responseCode = 200,
                    responseMessage = "Task was successfully saved",
                    data = taskSummaryList,
                };

            }
           

        }

        public async Task<ApiGenericResponse<List<TaskSummaryDTO>>> getAllTask(HttpContext httpContext)
        {
            var taskSummaryList = new List<TaskSummaryDTO>();
            var getAllTask = await _context.Tasks.Where(x => x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
           if(getAllTask == null)
            {
                return new ApiGenericResponse<List<TaskSummaryDTO>>
                {
                    responseCode = 404,
                    responseMessage = "No task was found.",
                    data = null,
                };
            }

            else
            {
                foreach (var item in getAllTask)
                {
                    var taskSummary = new TaskSummaryDTO();
                    taskSummary.Alias = item.Alias;
                    taskSummary.Caption = item.Caption;
                    taskSummary.CreatedAt = item.CreatedAt;
                    taskSummary.CreatedById = item.CreatedById;
                    taskSummary.Description = item.Description;
                    taskSummary.IsAssigned = item.IsAssigned;
                    taskSummary.IsMilestone = item.IsMilestone;
                    taskSummary.IsReassigned = item.IsReassigned;
                    taskSummary.IsWorkbenched = item.IsWorkbenched;
                    taskSummary.ProjectId = item.ProjectId;
                    taskSummary.TaskEndDate = item.TaskEndDate;
                    taskSummary.TaskStartDate = item.TaskStartDate;
                    taskSummary.Deliverables = await _context.Deliverables.Where(X => X.TaskId == item.Id && X.IsActive == true && X.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    taskSummary.Project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == item.ProjectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
                    taskSummary.TaskAssignees = await _context.TaskAssignees.Where(X => X.TaskId == item.Id && X.IsActive == true && X.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    taskSummary.workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == taskSummary.Project.WorkspaceId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());

                    taskSummaryList.Add(taskSummary);

                }


                return new ApiGenericResponse<List<TaskSummaryDTO>>
                {
                    responseCode = 200,
                    responseMessage = "All Task were successfully retrieved",
                    data = taskSummaryList,
                };



            }
          
        }


        public async Task<ApiGenericResponse<List<ProjectSummaryDTO>>> getProjectByWorkspaceId(HttpContext httpContext, long workspaceId)
        {
            var getProjectsByWorkspaceId = await _context.Projects.Where(x => x.WorkspaceId == workspaceId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true).ToListAsync();
            var ProjectArr = new List<ProjectSummaryDTO>();
            if (getProjectsByWorkspaceId.Count() == 0)
            {

                return new ApiGenericResponse<List<ProjectSummaryDTO>>
                {
                    responseCode = 404,
                    responseMessage = "No Project was with workspaceId " + workspaceId + " was found",
                    data = null,
                };

            }

            else
            {
                foreach(var item in getProjectsByWorkspaceId)
                {
                    var projectInstance = new ProjectSummaryDTO();

                    projectInstance.Alias = item.Alias;
                    projectInstance.Caption = item.Caption;
                    projectInstance.CreatedById = item.CreatedById;
                    projectInstance.Description = item.Description;
                    projectInstance.Id = item.Id;
                    projectInstance.IsActive = item.IsActive;
                    projectInstance.ProjectImage = item.ProjectImage;
                    projectInstance.CreatedAt = item.CreatedAt;
                    projectInstance.Tasks = await _context.Tasks.Where(x => x.ProjectId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                    projectInstance.Watchers = await _context.Watchers.Where(x => x.ProjectId == item.Id && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true).ToListAsync();
                    projectInstance.Workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == workspaceId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
                    ProjectArr.Add(projectInstance);

                };

                return new ApiGenericResponse<List<ProjectSummaryDTO>>
                {
                    responseCode = 200,
                    responseMessage = "Projects were successfully retrieved",
                    data = ProjectArr,
                };
            }
        }











        //public async Task<ApiGenericResponse<List<ProjectTree>>> getProjectForTreeTable(HttpContext httpContext)
        //{
        //    var getAllAssignedToAssignee = await _context.TaskAssignees.Where(x => x.IsActive == true && x.TaskAssigneeId == httpContext.GetLoggedInUserId() && x.IsActive == true).ToListAsync();
        //    if(getAllAssignedToAssignee.Count()  != 0)
        //    {

        //        var projectArray = new List<ProjectTree>();
        //        var getAllProjects = await _context.Projects.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
        //        if(getAllProjects.Count() != 0)
        //        {
        //            var taskArray = new List<TaskRevampDTO>();

        //            foreach(var project in getAllProjects)
        //            {
        //                var projectInstance = new ProjectTree();
        //                var getAllTask = await _context.Tasks.Where(x => x.IsActive == true && x.ProjectId == project.Id).ToListAsync();
        //                projectInstance.Alias = project.Alias;
        //                projectInstance.Caption = project.Caption;
        //                projectInstance.CreatedAt = project.CreatedAt;
        //                projectInstance.CreatedById = project.CreatedById;
        //                projectInstance.Description = project.Description;
        //                projectInstance.Id = project.Id;
        //                projectInstance.IsActive = project.IsActive;
        //                projectInstance.ProjectImage = project.ProjectImage;
        //                projectInstance.UpdatedAt = project.UpdatedAt;
        //                projectInstance.WorkspaceId = project.WorkspaceId;
        //                if(getAllTask.Count() != 0)
        //                {
        //                    foreach(var task in getAllTask)
        //                    {
        //                        var taskInstance = new TaskRevampDTO();
        //                        taskInstance.Alias = task.Alias;
        //                        taskInstance.Caption = task.Caption;
        //                        taskInstance.CreatedAt = task.CreatedAt;
        //                        taskInstance.CreatedById = task.CreatedById;
        //                        taskInstance.Deliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == task.Id).ToListAsync();
        //                        taskInstance.Description = task.Description;
        //                        taskInstance.DueTime = task.DueTime;
        //                        taskInstance.IsAssigned = task.IsAssigned;
        //                        taskInstance.IsMilestone = task.IsMilestone;
        //                        taskInstance.IsReassigned = task.IsReassigned;
        //                        taskInstance.IsWorkbenched = task.IsWorkbenched;
        //                        taskInstance.ProjectId = task.ProjectId;
        //                        taskInstance.project = await _context.Projects.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == task.ProjectId);
        //                        taskInstance.TaskAssignees = await getAssignees(task.Id,httpContext);
        //                        taskInstance.TaskEndDate = task.TaskEndDate;
        //                        taskInstance.TaskStartDate = task.TaskStartDate;
        //                        taskInstance.UpdatedAt = task.UpdatedAt;
        //                        task.WorkingManHours = task.WorkingManHours;

        //                        taskArray.Add(taskInstance);

        //                    }
        //                }

        //                projectInstance.Tasks = taskArray;
        //                projectArray.Add(projectInstance);

        //            }

        //            return new ApiGenericResponse<List<ProjectTree>>
        //            {
        //                responseCode = 200,
        //                responseMessage = "ProjectTree was successfully retrieved",
        //                data = projectArray,
        //            };

        //        }
        //        else
        //        {
        //            return new ApiGenericResponse<List<ProjectTree>>
        //            {
        //                responseCode = 404,
        //                responseMessage = "No Project  was found",
        //                data = null,
        //            };
        //        }

        //    }
        //    else
        //    {

        //        return new ApiGenericResponse<List<ProjectTree>>
        //        {
        //            responseCode = 404,
        //            responseMessage = "No Assignee  was found",
        //            data = null,
        //        };

        //    }

        //}







        //public async Task<ApiGenericResponse<List<ProjectTree>>> getProjectForTreeTable(HttpContext httpContext)
        //{
        //    var getTaskAssigneeDetails = await _context.TaskAssignees.Where(x => x.IsActive == true).ToListAsync();
        //    if (getTaskAssigneeDetails.Count() == 0 || getTaskAssigneeDetails == null)
        //    {
        //        return new ApiGenericResponse<List<ProjectTree>>
        //        {
        //            responseCode = 404,
        //            responseMessage = "No Assignee details  was found",
        //            data = null,
        //        };

        //    }
        //    else
        //    {
        //        foreach(var assignee in getTaskAssigneeDetails)
        //        {
        //            if(assignee.TaskAssigneeId == httpContext.GetLoggedInUserId())
        //            {
        //                var getTask = await _context.Tasks.Where(x => x.IsActive == true && x.Id == assignee.TaskId).ToA
        //            }
        //        }
        //    }



        //}


        public async Task<ApiGenericResponse<List<Task>>> pickUptask(long taskId,HttpContext httpContext)
        {
            var taskToBePicked = await _context.Tasks.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == taskId);
            if (taskToBePicked == null)
            {
                return new ApiGenericResponse<List<Task>>
                {
                    responseCode = 404,
                    responseMessage = "No Assignee details  was found",
                    data = null,
                };

            }
            else
            {

                var taskOwner = new TaskOwnership()
                {
                    TimePicked = DateTime.Now,
                    TaskOwnerId = httpContext.GetLoggedInUserId(),
                    CreatedAt = DateTime.Now,
                    DatePicked = DateTime.Now,
                    CreatedById = httpContext.GetLoggedInUserId(),
                    IsDeleted = false,
                };

                await _context.TaskOwnerships.AddAsync(taskOwner);
                await _context.SaveChangesAsync();

                taskToBePicked.IsPickedUp = true;
                taskToBePicked.TaskOwnershipId = taskOwner.Id;
                _context.Tasks.Update(taskToBePicked);
                await _context.SaveChangesAsync();

                var getUpdatedTaskownerShip = await _context.TaskOwnerships.Where(x => x.IsDeleted == false && x.TaskOwnerId == httpContext.GetLoggedInUserId()).ToListAsync();

                var taskArray = new List<Task>();
                if (getUpdatedTaskownerShip != null || getUpdatedTaskownerShip.Count() > 0)
                {
                    
                    foreach (var item in getUpdatedTaskownerShip)
                    {

                        var taskGotten = await _context.Tasks.Where(x => x.IsActive == true && x.TaskOwnershipId == item.Id).ToListAsync();

                        taskArray.AddRange(taskGotten);

                    }

                }

                return new ApiGenericResponse<List<Task>>
                {
                    responseCode = 200,
                    responseMessage = " TaskOwner details  was found",
                    data = taskArray,
                };

            }

        }

        public async Task<ApiGenericResponse<List<Task>>> getAllPickedTask(HttpContext httpContext)
        {
            var getUpdatedTaskownerShip = await _context.TaskOwnerships.Where(x => x.IsDeleted == false && x.TaskOwnerId == httpContext.GetLoggedInUserId()).ToListAsync();

            if (getUpdatedTaskownerShip == null || getUpdatedTaskownerShip.Count() == 0)
            {
                return new ApiGenericResponse<List<Task>>
                {
                    responseCode = 404,
                    responseMessage = "No Assignee details  was found",
                    data = null,
                };

            }
            else
            {

                   var taskArray = new List<Task>();
                   foreach (var item in getUpdatedTaskownerShip)
                    {
                        
                        var taskGotten = await _context.Tasks.Where(x => x.IsActive == true && x.TaskOwnershipId == item.Id).ToListAsync();

                        taskArray.AddRange(taskGotten);

                    }

                

                return new ApiGenericResponse<List<Task>>
                {
                    responseCode = 200,
                    responseMessage = " TaskOwner details  was found",
                    data = taskArray,
                };

            }

        }

        

        public async Task<ApiGenericResponse<List<Task>>> dropTask(long taskId,long taskOwnershipId, HttpContext httpContext)
        {
            var taskToBePicked = await _context.Tasks.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == taskId);
            var taskOwnerShip = await _context.TaskOwnerships.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == taskOwnershipId && x.TaskOwnerId == httpContext.GetLoggedInUserId());
            if (taskToBePicked == null || taskOwnerShip == null)
            {
                return new ApiGenericResponse<List<Task>>
                {
                    responseCode = 404,
                    responseMessage = "No Assignee details  was found",
                    data = null,
                };

            }
            else
            {

                taskToBePicked.IsPickedUp = false;
                _context.Tasks.Update(taskToBePicked);
                taskOwnerShip.IsDeleted = true;
                //taskOwnerShip.TaskOwnerId = 0;
                
                 _context.TaskOwnerships.Update(taskOwnerShip);
                await _context.SaveChangesAsync();

                var getUpdatedTaskownerShip = await _context.TaskOwnerships.Where(x => x.IsDeleted == false && x.TaskOwnerId == httpContext.GetLoggedInUserId()).ToListAsync();

                var taskArray = new List<Task>();
                if (getUpdatedTaskownerShip != null || getUpdatedTaskownerShip.Count() > 0)
                {
                    
                    foreach (var item in getUpdatedTaskownerShip)
                    {

                        var taskGotten = await _context.Tasks.Where(x => x.IsActive == true && x.TaskOwnershipId == item.Id).ToListAsync();

                        taskArray.AddRange(taskGotten);

                    }


                }

                return new ApiGenericResponse<List<Task>>
                {
                    responseCode = 200,
                    responseMessage = " TaskOwner details  was found",
                    data = taskArray,
                };

            }


        }



        public async Task<ApiGenericResponse<List<TaskRevampDTO>>> getAssignedTask(HttpContext httpContext)
        {
            var getAllAssigneeId = await _context.TaskAssignees.Where(x => x.IsActive == true && x.TaskAssigneeId == httpContext.GetLoggedInUserId()).ToListAsync();
            if(getAllAssigneeId == null || getAllAssigneeId.Count() == 0)
            {
                return new ApiGenericResponse<List<TaskRevampDTO>>
                {
                    responseCode = 404,
                    responseMessage = "No Assignee details  was found",
                    data = null,
                };

            }

            else
            {
                var taskArray = new List<TaskRevampDTO>();
                foreach (var item in getAllAssigneeId)
                {
                    var getTaskAssigned = await _context.Tasks.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == item.TaskId);
                    if (getTaskAssigned != null)
                    {
                        var taskAssigneeInstance = new TaskRevampDTO();
                        taskAssigneeInstance.Alias = getTaskAssigned.Alias;
                        taskAssigneeInstance.Id = getTaskAssigned.Id;
                        taskAssigneeInstance.Caption = getTaskAssigned.Caption;
                        taskAssigneeInstance.CreatedAt = getTaskAssigned.CreatedAt;
                        taskAssigneeInstance.CreatedById = getTaskAssigned.CreatedById;
                        taskAssigneeInstance.Deliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == getTaskAssigned.Id).ToListAsync();
                        taskAssigneeInstance.Description = getTaskAssigned.Description;
                        taskAssigneeInstance.DueTime = getTaskAssigned.DueTime;
                        taskAssigneeInstance.IsAssigned = getTaskAssigned.IsAssigned;
                        taskAssigneeInstance.IsMilestone = getTaskAssigned.IsReassigned;
                        taskAssigneeInstance.IsReassigned = getTaskAssigned.IsReassigned;
                        taskAssigneeInstance.IsWorkbenched = getTaskAssigned.IsWorkbenched;
                        taskAssigneeInstance.IsPickedUp = getTaskAssigned.IsPickedUp;
                        taskAssigneeInstance.project = await _context.Projects.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == getTaskAssigned.ProjectId);
                        taskAssigneeInstance.ProjectId = getTaskAssigned.ProjectId;
                        taskAssigneeInstance.TaskAssignees = await getAssignees(getTaskAssigned.Id, httpContext);
                        taskAssigneeInstance.TaskEndDate = getTaskAssigned.TaskEndDate;
                        taskAssigneeInstance.TaskStartDate = getTaskAssigned.TaskStartDate;
                        taskAssigneeInstance.UpdatedAt = getTaskAssigned.UpdatedAt;
                        taskAssigneeInstance.WorkingManHours = getTaskAssigned.WorkingManHours;
                        taskArray.Add(taskAssigneeInstance);
                    }

                }

                return new ApiGenericResponse<List<TaskRevampDTO>>
                {
                    responseCode = 200,
                    responseMessage = "Assignee details  was suucessfully  found",
                    data = taskArray,
                };
            }


        }



            public async Task<List<TaskRevampDTO>> getTasks(long projectId, HttpContext httpContext)
        {
            var taskList = new List<TaskRevampDTO>();

            if (projectId != 0)
            {


                var getAllTask = await _context.Tasks.Where(x => x.IsActive == true && x.ProjectId == projectId && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

                foreach (var task in getAllTask)
                {
                    var taskInstance = new TaskRevampDTO();
                    taskInstance.Alias = task.Alias;
                    taskInstance.Caption = task.Caption;
                    taskInstance.CreatedAt = task.CreatedAt;
                    taskInstance.Description = task.Description;
                    taskInstance.DueTime = task.DueTime;
                    taskInstance.IsAssigned = task.IsAssigned;
                    taskInstance.IsMilestone = task.IsMilestone;
                    taskInstance.IsReassigned = task.IsReassigned;
                    taskInstance.IsWorkbenched = task.IsWorkbenched;
                    taskInstance.ProjectId = task.ProjectId;
                    taskInstance.TaskAssignees = await getAssignees(task.Id, httpContext);
                    taskInstance.TaskEndDate = task.TaskEndDate;
                    taskInstance.TaskStartDate = task.TaskStartDate;
                    taskInstance.UpdatedAt = task.UpdatedAt;
                    taskInstance.WorkingManHours = task.WorkingManHours;
                    taskList.Add(taskInstance);
                }



            }

            return taskList;

        }


        public async Task<List<TaskAssigneeDTO>> getAssignees(long taskId,HttpContext httpContext)
        {
            var taskAssigneeList = new List<TaskAssigneeDTO>();

            if (taskId != 0)
            {

                
                    var getTaskAssignees = await _context.TaskAssignees.Where(x => x.IsActive == true && x.TaskId == taskId && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                   
                    foreach(var taskAssignees in getTaskAssignees)
                    {
                        var taskAssignee = new TaskAssigneeDTO();
                        taskAssignee.CreatedAt = taskAssignees.CreatedAt;
                        taskAssignee.CreatedById = taskAssignees.CreatedById;
                        taskAssignee.IsActive = taskAssignees.IsActive;
                        taskAssignee.Name = taskAssignees.Name;
                        taskAssignee.TaskAssigneeId = taskAssignees.TaskAssigneeId;
                        taskAssignee.ProfileImage = _context.UserProfiles.FirstOrDefault(x => x.Id == taskAssignee.TaskAssigneeId).ImageUrl;
                        taskAssignee.TaskId = taskAssignees.TaskId;
                        taskAssignee.UpdatedAt = taskAssignees.UpdatedAt;

                        taskAssigneeList.Add(taskAssignee);
                    }
                

                
            }

            return taskAssigneeList;

        }

        public async Task<ApiGenericResponse<TaskSummaryDTO>> getTaskByCaption(HttpContext httpContext,string caption)
        {
            var checkIfTaskExistByCaption = await _context.Tasks.FirstOrDefaultAsync(x => x.Caption == caption.Trim() && x.CreatedById == httpContext.GetLoggedInUserId());
            if(checkIfTaskExistByCaption == null)
            {
                return new ApiGenericResponse<TaskSummaryDTO>
                {
                    responseCode = 404,
                    responseMessage = "Task with caption " + caption + "was not found.",
                    data = null,
                };
            }
            else
            {
                var taskSummary = new TaskSummaryDTO();
                taskSummary.id = checkIfTaskExistByCaption.Id;
                taskSummary.Alias = checkIfTaskExistByCaption.Alias;
                taskSummary.Caption = checkIfTaskExistByCaption.Caption;
                taskSummary.CreatedAt = checkIfTaskExistByCaption.CreatedAt;
                taskSummary.CreatedById = checkIfTaskExistByCaption.CreatedById;
                taskSummary.Description = checkIfTaskExistByCaption.Description;
                taskSummary.IsAssigned = checkIfTaskExistByCaption.IsAssigned;
                taskSummary.IsMilestone = checkIfTaskExistByCaption.IsMilestone;
                taskSummary.IsReassigned = checkIfTaskExistByCaption.IsReassigned;
                taskSummary.DueTime = checkIfTaskExistByCaption.DueTime;
                taskSummary.WorkingManHours = checkIfTaskExistByCaption.WorkingManHours;
                taskSummary.IsWorkbenched = checkIfTaskExistByCaption.IsWorkbenched;
                taskSummary.ProjectId = checkIfTaskExistByCaption.ProjectId;
                taskSummary.TaskEndDate = checkIfTaskExistByCaption.TaskEndDate;
                taskSummary.TaskStartDate = checkIfTaskExistByCaption.TaskStartDate;
                taskSummary.Project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == checkIfTaskExistByCaption.ProjectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
                taskSummary.TaskAssignees = await _context.TaskAssignees.Where(X => X.TaskId == checkIfTaskExistByCaption.Id && X.IsActive == true && X.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                taskSummary.workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == taskSummary.Project.WorkspaceId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
                return new ApiGenericResponse<TaskSummaryDTO>
                {
                    responseCode = 200,
                    responseMessage = "Task with caption " + caption + "exists.",
                    data = taskSummary,
                };
            }

        }


        public async Task<ApiGenericResponse<List<PrivacyAccess>>> getAllPrivacyAccessByWorkspaceId(HttpContext httpContext, long workspaceId)
        {
            var privacyAccesses = await _context.PrivacyAccesses.Where(x => x.WorkspaceId == workspaceId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
            if(privacyAccesses == null || privacyAccesses.Count < 0)
            {
                return new ApiGenericResponse<List<PrivacyAccess>>
                {
                    responseCode = 404,
                    responseMessage = "PrivacyAccess with workspaceId  " + workspaceId + "was not found.",
                    data = null,
                };

            }
            else
            {
                return new ApiGenericResponse<List<PrivacyAccess>>
                {
                    responseCode = 200,
                    responseMessage = "PrivacyAccesses successfully retrieved",
                    data = privacyAccesses,
                };
            }
            

        }


        public async Task<ApiGenericResponse<TaskSummaryDTO>> getTaskById(HttpContext httpContext, long taskId)
        {
            var checkIfTaskExistByCaption = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId && x.CreatedById == httpContext.GetLoggedInUserId());
            if (checkIfTaskExistByCaption == null)
            {
                return new ApiGenericResponse<TaskSummaryDTO>
                {
                    responseCode = 404,
                    responseMessage = "Task with taskId " + taskId + "was not found.",
                    data = null,
                };
            }
            else
            {
                var taskSummary = new TaskSummaryDTO();
                taskSummary.id = checkIfTaskExistByCaption.Id;
                taskSummary.Alias = checkIfTaskExistByCaption.Alias;
                taskSummary.Caption = checkIfTaskExistByCaption.Caption;
                taskSummary.CreatedAt = checkIfTaskExistByCaption.CreatedAt;
                taskSummary.CreatedById = checkIfTaskExistByCaption.CreatedById;
                taskSummary.Description = checkIfTaskExistByCaption.Description;
                taskSummary.IsAssigned = checkIfTaskExistByCaption.IsAssigned;
                taskSummary.DueTime = checkIfTaskExistByCaption.DueTime;
                taskSummary.WorkingManHours = checkIfTaskExistByCaption.WorkingManHours;
                taskSummary.IsMilestone = checkIfTaskExistByCaption.IsMilestone;
                taskSummary.IsReassigned = checkIfTaskExistByCaption.IsReassigned;
                taskSummary.IsWorkbenched = checkIfTaskExistByCaption.IsWorkbenched;
                taskSummary.ProjectId = checkIfTaskExistByCaption.ProjectId;
                taskSummary.TaskEndDate = checkIfTaskExistByCaption.TaskEndDate;
                taskSummary.TaskStartDate = checkIfTaskExistByCaption.TaskStartDate;
                taskSummary.Deliverables = await _context.Deliverables.Where(X => X.TaskId == checkIfTaskExistByCaption.Id && X.IsActive == true && X.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                taskSummary.Project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == checkIfTaskExistByCaption.ProjectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
                taskSummary.TaskAssignees = await _context.TaskAssignees.Where(X => X.TaskId == checkIfTaskExistByCaption.Id && X.IsActive == true && X.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                taskSummary.workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == taskSummary.Project.WorkspaceId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
                return new ApiGenericResponse<TaskSummaryDTO>
                {
                    responseCode = 200,
                    responseMessage = "Task with taskId " + taskId + " was successfully found.",
                    data = taskSummary,
                };
            }

        }

        public async Task<ApiGenericResponse<List<TaskSummaryDTO>>> getTaskByProjectId(HttpContext httpContext, long  projectId)
        {
            var checkIfTaskExistById = await _context.Tasks.Where(x => x.ProjectId == projectId && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
            if (checkIfTaskExistById == null)
            {
                return new ApiGenericResponse<List<TaskSummaryDTO>>
                {
                    responseCode = 404,
                    responseMessage = "Tasks with ProjectId " + projectId + " was not found.",
                    data = null,
                };
            }
            else
            {
                var taskSummaryList = new List<TaskSummaryDTO>();

                foreach(var item in checkIfTaskExistById)
                {
                    var taskSummary = new TaskSummaryDTO();
                    taskSummary.id = item.Id;
                    taskSummary.Alias = item.Alias;
                    taskSummary.Caption = item.Caption;
                    taskSummary.CreatedAt = item.CreatedAt;
                    taskSummary.CreatedById = item.CreatedById;
                    taskSummary.Description = item.Description;
                    taskSummary.IsAssigned = item.IsAssigned;
                    taskSummary.DueTime = item.DueTime;
                    taskSummary.WorkingManHours = item.WorkingManHours;
                    taskSummary.IsMilestone = item.IsMilestone;
                    taskSummary.IsReassigned = item.IsReassigned;
                    taskSummary.IsWorkbenched = item.IsWorkbenched;
                    taskSummary.ProjectId = item.ProjectId;
                    taskSummary.TaskEndDate = item.TaskEndDate;
                    taskSummary.TaskStartDate = item.TaskStartDate;
                    taskSummary.Deliverables = await _context.Deliverables.Where(X => X.TaskId == item.Id && X.IsActive == true).ToListAsync();
                    taskSummary.Project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == item.ProjectId && x.IsActive == true);
                    taskSummary.TaskAssignees = await _context.TaskAssignees.Where(X => X.TaskId == item.Id && X.IsActive == true).ToListAsync();
                    taskSummary.AssigneeLength = taskSummary.TaskAssignees.Count();
                    taskSummary.workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == taskSummary.Project.WorkspaceId && x.IsActive == true);

                    taskSummaryList.Add(taskSummary);
                        
                }

                return new ApiGenericResponse<List<TaskSummaryDTO>>
                {
                    responseCode = 200,
                    responseMessage = "Task(s) with ProjectId " + projectId + " were  found.",
                    data = taskSummaryList,
                };
            }

        }




        public async Task<ApiResponse> getManagersProjects(string email,int emailId)
       
        {

            var getProjects = await _projectAllocationRepository.getAllManagerProjects(email,emailId);

            if (getProjects == null)
            {
                return new ApiResponse(400);
            }

            return new ApiOkResponse(getProjects);


        }

        public async Task<ApiGenericResponse<List<PMRequirement>>> getRequirementsByDeliverableId(HttpContext httpContext, long deliverableId)
        {
            var gottenRequirements = await _context.PMRequirements.Where(x => x.DeliverableId == deliverableId && x.CreatedById == httpContext.GetLoggedInUserId() & x.IsActive == true).ToListAsync();
            if (gottenRequirements == null)
            {
                return new ApiGenericResponse<List<PMRequirement>>
                {
                    responseCode = 404,
                    responseMessage = "Requirements with deliverableId " + deliverableId + " were not found.",
                    data = null,
                };
            }

            else
            {
                return new ApiGenericResponse<List<PMRequirement>>
                {
                    responseCode = 200,
                    responseMessage = "Requirements with deliverableId " + deliverableId + " were  found.",
                    data = gottenRequirements,
                };
            }

            

        }
    }
}
