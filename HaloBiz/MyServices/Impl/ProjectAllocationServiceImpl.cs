using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
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


        public async Task<ApiCommonResponse> AddNewManager(HttpContext context, ProjectAllocationRecievingDTO projectAllocationDTO)
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
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                    return CommonResponse.Send(ResponseCodes.SUCCESS,savedProjectManager);

                }

                return CommonResponse.Send(ResponseCodes.SUCCESS,409);


            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,409);


        }


        public async Task<ApiCommonResponse> CreateNewWorkspace(HttpContext context, WorkspaceDTO workspaceDTO) {



                var savedWorkSpaces = await _projectAllocationRepository.createWorkspace(context,workspaceDTO);
                
                if (savedWorkSpaces == null)
                {

                    return CommonResponse.Send
                    (
                        ResponseCodes.FAILURE,
                        null,
                        ResponseMessage.ENTITYNOTSAVED
                    );
                }
                else
                {
                    return CommonResponse.Send
                    (
                        ResponseCodes.SUCCESS,
                        savedWorkSpaces,
                        ResponseMessage.ENTITYNOTSAVED
                    );
                }
                      
        }




        public async Task<ApiCommonResponse> getProjectManagers(int serviceCategoryId)
        {

            var getManagers = await _projectAllocationRepository.getAllProjectManager(serviceCategoryId);

            if (getManagers == null)
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS,getManagers);
            }

            return  CommonResponse.Send(ResponseCodes.SUCCESS, getManagers);


        }





        public async Task<ApiCommonResponse> getAllWorkspacesRevamped(HttpContext httpContext)
        {

            var workspaceQuery = await _context.Workspaces.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId())
                                                          .Include(x => x.Projects.Where(x => x.IsActive == true))
                                                          .Include(x => x.PrivacyAccesses.Where(x => x.IsActive == true))
                                                          .Include(x => x.ProjectCreators.Where(x => x.IsActive == true))
                                                          .Include(x => x.StatusFlows.Where(x => x.IsDeleted == false))
                                                          .Include(x => x.Deliverables)
                                                          .ToListAsync();
            if(workspaceQuery.Count == 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null,ResponseMessage.EntityNotFound);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, workspaceQuery);

        }


        public async Task<ApiCommonResponse> getAllDataForWorkspaceSideBar(HttpContext httpContext)
        {
            var getAllWorkspaceQuery = await _context.Workspaces.Where(x => x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true).ToListAsync();

            var getAllProjectQuery = await _context.Projects.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

            var getAllTaskQuery = await _context.Tasks.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

            var getAllDeliverablesQuery = await _context.Deliverables.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

            var getAllQuery = new SidebarClassWorkspace
            {
                workspace = getAllWorkspaceQuery,
                project = getAllProjectQuery,
                task = getAllTaskQuery,
                deliverables = getAllDeliverablesQuery
            };

            return CommonResponse.Send(ResponseCodes.SUCCESS, getAllQuery, "Successfully retrieved all sidebar numbers");

        }



        //public async Task<ApiCommonResponse> getAllDeliverablesRevamped(HttpContext httpContext)
        //{

        //    var workspaceQuery = await _context.Deliverables.
        //    if (workspaceQuery.Count == 0)
        //    {
        //        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.EntityNotFound);
        //    }

        //    return CommonResponse.Send(ResponseCodes.SUCCESS, workspaceQuery);

        //}


        public async Task<ApiCommonResponse> getAllWorkspaces(HttpContext httpContext)
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
                return CommonResponse.Send(ResponseCodes.SUCCESS,workspaceArr);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,workspaceArr);

        }


        public async Task<ApiCommonResponse> getWorkspaceById(long id)
        {

            var getWorkspace = await _projectAllocationRepository.getWorkSpaceById(id);

            if (getWorkspace == null)
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS,getWorkspace);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,getWorkspace);

        }


        public async Task<ApiCommonResponse> getWorkspaceByCaption(string caption)
        {

            var result = await _context.Workspaces.FirstOrDefaultAsync(x => x.Caption == caption && x.IsActive != false);


            if (result == null)
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS,result);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,result);

        }


        public async Task<ApiCommonResponse> getAllProjectManagers()
        {

            var getAllManagers = await _context.ProjectAllocations.ToListAsync();

            if (getAllManagers == null)
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS,getAllManagers);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,getAllManagers);

        }



        public async Task<ApiCommonResponse> getDefaultStatus()
        {

            var getDefaultStatus = await _context.StatusFlows.Where(x => x.WorkspaceId == 84).ToListAsync();

            if (getDefaultStatus == null)
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS,getDefaultStatus);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,getDefaultStatus);

        }

        public async Task<ApiCommonResponse> removeFromCategory(long id,int category,long projectId)
        {
            var activityToDelete = await _context.ProjectAllocations.FirstOrDefaultAsync(x => x.ManagerId == id
                                                                    && x.ServiceCategoryId == category
                                                                    && x.Id == projectId);
            if (activityToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            activityToDelete.IsDeleted = true;
            _context.ProjectAllocations.Update(activityToDelete);
            await _context.SaveChangesAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> removeFromProjectCreator(long workspaceId,long creatorId)
        {
            var creatorToDelete = await _context.ProjectCreators.Where(x => x.ProjectCreatorProfileId == creatorId && x.IsActive != false && x.WorkspaceId == workspaceId).FirstOrDefaultAsync();
            if (creatorToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            creatorToDelete.IsActive = false;
            _context.ProjectCreators.Update(creatorToDelete);
            await _context.SaveChangesAsync();
            var remainderUser = await _context.ProjectCreators.Where(x => x.WorkspaceId == workspaceId && x.IsActive != false).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS,remainderUser);
        }


        public async Task<ApiCommonResponse> getWorkspaceById(HttpContext httpContext,long workspaceId)
        {
            var workspaceByIdQuery = await _context.Workspaces.Where(x => x.Id == workspaceId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId())
                                                               .FirstOrDefaultAsync();
            var workspaceArray = new List<Workspace>();
            if(workspaceByIdQuery != null)
            {

                   var workspaceInstance = new Workspace
                    {
                        Alias = workspaceByIdQuery.Alias,
                        Caption = workspaceByIdQuery.Caption,
                        Description = workspaceByIdQuery.Description,
                        CreatedAt = workspaceByIdQuery.CreatedAt,
                        IsActive = workspaceByIdQuery.IsActive,
                        Id = workspaceByIdQuery.Id,
                        PrivacyAccesses = await _context.PrivacyAccesses.Where(x => x.IsActive == true && x.WorkspaceId == workspaceId).ToListAsync(),
                        ProjectCreators = await _context.ProjectCreators.Where(x=>x.IsActive == true && x.WorkspaceId == workspaceId).ToListAsync(),
                        Projects = await _context.Projects.Where(x=>x.IsActive == true && x.WorkspaceId == workspaceId).ToListAsync(),
                        IsPublic = workspaceByIdQuery.IsPublic,
                        CreatedById = workspaceByIdQuery.CreatedById,
                        CreatedBy = await _context.UserProfiles.FirstOrDefaultAsync(x=>x.IsDeleted == false && x.Id == workspaceByIdQuery.CreatedById),
                        StatusFlows = await _context.StatusFlows.Where(x=>x.IsDeleted == false && x.WorkspaceId == workspaceId).ToListAsync(),
                        StatusFlowOption = workspaceByIdQuery.StatusFlowOption,
                        Deliverables = await _context.Deliverables.Where(x=>x.IsActive == true && x.WorkspaceId == workspaceId).ToListAsync(),
                        UpdatedAt = workspaceByIdQuery.UpdatedAt,
                    };

                return CommonResponse.Send(ResponseCodes.SUCCESS, workspaceInstance, "Workspace successfully retrieved");
            }
            else
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Workspace is currently empty");
            }

        }

        public async Task<ApiCommonResponse> updateToPublic(long workspaceId)
        {

            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == workspaceId && x.IsActive == true);
            
            
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

            workspace.IsPublic = true;
            _context.Workspaces.UpdateRange(workspace);
            _context.PrivacyAccesses.UpdateRange(privateUserArray);
            await _context.SaveChangesAsync();
            var newprivateUserArr = await _context.ProjectCreators.Where(x => x.WorkspaceId == workspaceId && x.IsActive != false).ToListAsync();

            return CommonResponse.Send(ResponseCodes.SUCCESS,newprivateUserArr);
        }


        public async Task<ApiCommonResponse> disablePrivateUser(long workspaceId, long privateUserId)
        {


            var privateUser = await _context.PrivacyAccesses.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.IsActive != false && x.PrivacyAccessId == privateUserId);


            if (privateUser == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            privateUser.IsActive = false;
            _context.PrivacyAccesses.Update(privateUser);
            await _context.SaveChangesAsync();
            var remainderUser = await _context.PrivacyAccesses.Where(x => x.WorkspaceId == workspaceId && x.IsActive != false).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS,remainderUser);
        }

        public async Task<ApiCommonResponse> disableStatus(long workspaceId, long statusId)
        {

            var status = await _context.StatusFlows.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.Id == statusId);


            if (status == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            status.IsDeleted = true;
            _context.StatusFlows.Update(status);
            await _context.SaveChangesAsync();
            var allActveStatus = await _context.StatusFlows.Where(x => x.WorkspaceId == workspaceId && x.IsDeleted == false).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS,allActveStatus);
        }


        public async Task<ApiCommonResponse> disableWorkspace(long id)
        {
            var activityToDelete = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == id);
            if (activityToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            activityToDelete.IsActive = false;
            _context.Workspaces.Update(activityToDelete);
            await _context.SaveChangesAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> updateWorkspace(HttpContext httpContext, long id,UpdateWorkspaceDTO workspaceDTO)
        {
            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
            if (workspace == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            workspace.Alias = workspaceDTO.Alias;
            workspace.Caption = workspaceDTO.Caption;
            workspace.Description = workspaceDTO.Description;
            _context.Workspaces.Update(workspace);
            await _context.SaveChangesAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS, workspace);
        }


        public async Task<ApiCommonResponse> addMoreProjectCreators(HttpContext httpContext, long id, List<AddMoreUserDto> projectCreatorDtos)
        {
            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
            if (workspace == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            var projectCreatorsList = new List<ProjectCreator>();
            foreach(var projectCreator in projectCreatorDtos)
            {
                var verifyIfUserExist = await _context.ProjectCreators.FirstOrDefaultAsync(x => x.ProjectCreatorProfileId == projectCreator.usersId && x.IsActive == true && x.WorkspaceId == id);
                if (verifyIfUserExist != null)
                {
                    continue;
                }
                else
                {
                    

                    var projectCreatorsToBeSaved = new ProjectCreator()
                    {
                        IsActive = true,
                        WorkspaceId = id,
                        CreatedById = httpContext.GetLoggedInUserId(),
                        UpdatedAt = DateTime.Now,
                        ProjectCreatorProfileId = projectCreator.usersId
                    };
                    projectCreatorsList.Add(projectCreatorsToBeSaved);
                       
                    
                }
            }

            if (projectCreatorsList.Count == 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not add any project creators because one or more already exist");
            }
            else
            {
                await _context.ProjectCreators.AddRangeAsync(projectCreatorsList);
                await _context.SaveChangesAsync();
                var remainderUser = await _context.ProjectCreators.Where(x => x.WorkspaceId == id && x.IsActive != false).ToListAsync();
                return CommonResponse.Send(ResponseCodes.SUCCESS, remainderUser);
            }

        }


        //public async Task<ApiCommonResponse> addMorePrivateUser(HttpContext httpContext, long workspaceId, List<AddMoreUserDto> privateUserid)
        //{
        //    var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == workspaceId);
        //    if (workspace == null)
        //    {
        //        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
        //    }

        //    workspace.IsPublic = false;
        //    _context.Workspaces.Update(workspace);



        //    var privacyList = new List<PrivacyAccess>();

        //    foreach (var item in privateUserid)
        //    {
        //        if (await _context.PrivacyAccesses.FirstOrDefaultAsync(x => x.PrivacyAccessId == item.usersId && x.IsActive != false && x.WorkspaceId == workspaceId) == null)
        //        {

        //            var privateToBeSaved = new PrivacyAccess()
        //            {
        //                IsActive = true,
        //                WorkspaceId = workspaceId,
        //                CreatedById = httpContext.GetLoggedInUserId(),
        //                UpdatedAt = DateTime.Now,
        //                PrivacyAccessId = item.usersId

        //            };
        //            privacyList.Add(privateToBeSaved);

        //        }


        //    }
        //    await _context.PrivacyAccesses.AddRangeAsync(privacyList);
        //    await _context.SaveChangesAsync();
        //    var remainderUser = await _context.PrivacyAccesses.Where(x => x.WorkspaceId == workspaceId && x.IsActive != false).ToListAsync();
        //    return CommonResponse.Send(ResponseCodes.SUCCESS,remainderUser);

        //}

        public async Task<ApiCommonResponse> addMorePrivateUser(HttpContext httpContext, long workspaceId, List<AddMoreUserDto> privateUserid)
        {
            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == workspaceId && x.IsActive == true);
            if (workspace == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

           

            var privateUserList = new List<PrivacyAccess>();
            foreach (var privateUser in privateUserid)
            {
                var verifyIfUserExist = await _context.PrivacyAccesses.FirstOrDefaultAsync(x => x.PrivacyAccessId == privateUser.usersId && x.IsActive != false && x.WorkspaceId == workspaceId);
                if (verifyIfUserExist != null)
                {
                    continue;
                }
                else
                {
                    workspace.IsPublic = false;
                    _context.Workspaces.Update(workspace);

                    var privateUsersToBeSaved = new PrivacyAccess()
                    {
                        IsActive = true,
                        WorkspaceId = workspaceId,
                        CreatedById = httpContext.GetLoggedInUserId(),
                        UpdatedAt = DateTime.Now,
                        PrivacyAccessId = privateUser.usersId
                    };
                    privateUserList.Add(privateUsersToBeSaved);


                }
            }

            if (privateUserList.Count == 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not add any Private user because the user(s) already  exist");
            }
            else
            {
                await _context.PrivacyAccesses.AddRangeAsync(privateUserList);
                await _context.SaveChangesAsync();
                var remainderUser = await _context.PrivacyAccesses.Where(x => x.WorkspaceId == workspaceId && x.IsActive != false).ToListAsync();
                return CommonResponse.Send(ResponseCodes.SUCCESS, remainderUser);
            }

        }




        public async Task<ApiCommonResponse> updateStatus(HttpContext httpContext, long workspaceId, long statusFlowId,StatusFlowDTO statusFlowDTO)
        {
            var gottenStatusFlow = await _context.StatusFlows.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.Id == statusFlowId);
            if (gottenStatusFlow == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,gottenStatusFlow);

        }


        //public async Task<ApiCommonResponse> addmoreStatus(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO)
        //{
            

        //    var statusArray = new List<StatusFlow>();
        //    foreach (var item in statusFlowDTO)
        //    {

        //        return new ApiGenericResponse<List<Watcher>>
        //        {
        //            responseCode = 404,
        //            responseMessage = "Watcher(s) with id "+ projectId + "was not found.",
        //            data = null
        //        };
        //    }
        //    else
        //    {
        //        var watchersArray = new List<Watcher>();
        //        var getCurrentWatchers = await _context.Watchers.Where(x => x.ProjectId == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
        //        if (getCurrentWatchers.Count() == 0)
        //        {

        //            foreach (var item in watchersDTOs)
        //            {
        //                var watchersInstance = new Watcher();
        //                watchersInstance.CreatedById = httpContext.GetLoggedInUserId();
        //                watchersInstance.ProjectWatcherId = item.ProjectWatcherId;
        //                watchersInstance.ProjectId = item.ProjectId;
        //                watchersInstance.IsActive = true;
        //                watchersInstance.CreatedAt = DateTime.Now;

        //                watchersArray.Add(watchersInstance);

        //            }

        //        }
        //        else
        //        {
                    
        //                foreach (var item in watchersDTOs)
        //                {

        //                var getExistingWatcher = await _context.Watchers.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId() && x.ProjectWatcherId == item.ProjectWatcherId);

        //                    if (getExistingWatcher  == null)
        //                    {

        //                        var watchersInstance = new Watcher();
        //                        watchersInstance.CreatedById = httpContext.GetLoggedInUserId();
        //                        watchersInstance.ProjectWatcherId = item.ProjectWatcherId;
        //                        watchersInstance.ProjectId = item.ProjectId;
        //                        watchersInstance.IsActive = true;
        //                        watchersInstance.CreatedAt = DateTime.Now;

        //                        watchersArray.Add(watchersInstance);
        //                    }

        //                }
                    
        //        }

        //        await _context.Watchers.AddRangeAsync(watchersArray);
        //        await _context.SaveChangesAsync();
        //        var getUpdatedWatchers = await _context.Watchers.Where(x => x.ProjectId == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

        //        return new ApiGenericResponse<List<Watcher>>
        //        {
        //            responseCode = 200,
        //            responseMessage = "Watcher(s) were successfully added.",
        //            data = getUpdatedWatchers
        //        };
        //    }


        //}

        


        //public async Task<ApiGenericResponse<List<Watcher>>> removeWatcher(HttpContext httpContext, long projectId, long projectWatcherId)
        //{
        //    var ifWatcherExist = await _context.Watchers.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.ProjectWatcherId == projectWatcherId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
        //    if(ifWatcherExist == null)
        //    {
        //        return new ApiGenericResponse<List<Watcher>>
        //        {
        //            responseCode = 404,
        //            responseMessage = "Watcher(s) with id " + projectId + "was not found.",
        //            data = null
        //        };
        //    }
        //    else
        //    {
        //        ifWatcherExist.IsActive = false;
        //        _context.Watchers.Update(ifWatcherExist);
        //        await _context.SaveChangesAsync();
        //        var getUpdatedWatchers =await _context.Watchers.Where(x => x.CreatedById == httpContext.GetLoggedInUserId() && x.ProjectId == projectId && x.IsActive == true).ToListAsync();
        //        return new ApiGenericResponse<List<Watcher>>
        //        {
        //            responseCode = 200,
        //            responseMessage = "Watcher wwas successfully removed",
        //            data = getUpdatedWatchers
        //        };
        //    }
        //}


        public async Task<ApiCommonResponse> getBarChartDetails(HttpContext httpContext,long taskId)
        {
            var getDeliverableWorkloads = await _context.Deliverables.Where(x => x.TaskId == taskId && x.IsActive == true).ToListAsync();

            var userArray = new List<DeliverableUser>();
            var finalUserArray = new List<DeliverableUser>();
            var workLoad = new WorkLoadDTO();
            foreach (var deliverable in getDeliverableWorkloads)
            {
                var userInstance = new DeliverableUser();
                var assignTask = await _context.AssignTasks.FirstOrDefaultAsync(x => x.DeliverableId == deliverable.Id && x.IsActive == true);
                if (assignTask != null)
                {
                    var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == assignTask.DeliverableAssigneeId);
                    userInstance.userId = user.Id;
                    userInstance.email = user.Email;
                    userInstance.imageUrl = user.ImageUrl;
                    userInstance.fullname = user.FirstName + " " + user.LastName;
                    userArray.Add(userInstance);
                }
            }

            var dic = new Dictionary<long, int>();
            var dataSetInstance = new DataSetDTO();
            foreach (var element in userArray)
            {
                if (dic.ContainsKey(element.userId))
                    dic[element.userId]++;
                else
                    dic[element.userId] = 1;
            }
            var assignDuration = new List<long>();
            var pickedList = new List<long>();
            foreach (var element in dic)
            {
                var userInstance = new DeliverableUser();
                var assignValue = new long();
                var pickedValue = new long();

                Console.WriteLine(element.Key + " appears " + element.Value + " time(s)");
                var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == element.Key);
                userInstance.userId = user.Id;
                userInstance.email = user.Email;
                userInstance.imageUrl = user.ImageUrl;
                userInstance.fullname = user.FirstName + " " + user.LastName;
                assignValue = element.Value;
                pickedValue = await getPickedRate(httpContext, element.Key,taskId);
                pickedList.Add(pickedValue);
                assignDuration.Add(assignValue);
                finalUserArray.Add(userInstance);
            }


            workLoad.DeliverableUser = finalUserArray;
            workLoad.assignedRate = assignDuration;
            workLoad.pickedRate = pickedList;
           
            return CommonResponse.Send
                (

                ResponseCodes.SUCCESS,
                workLoad,
                ResponseMessage.EntitySuccessfullyFound
                );

        }



        public async Task<int> getPickedRate(HttpContext httpContext,long userId,long taskId)
        {
            var getPickedRate = await _context.AssignTasks.Where(x => x.IsActive == true && x.DeliverableAssigneeId == userId)
                                .Include(x => x.Deliverable).ToListAsync();

            int ratePicked = 0;
            foreach(var pickRate in getPickedRate) {

                if(pickRate.Deliverable.IsPicked == true && pickRate.Deliverable.TaskId == taskId)
                {
                    ratePicked = ratePicked + 1;
                }
            }

            return ratePicked;

        }


        public async Task<ApiCommonResponse> getWorkspaceWithProjectWatcher(HttpContext httpContext)
        {

            var getWatcherQuery = await _context.Watchers.Where(x => x.IsActive == true && x.ProjectWatcherId == httpContext.GetLoggedInUserId()).ToListAsync();

            if(getWatcherQuery.Count == 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null,"You have not been selected as a watcher on any project");
            }

            else
            {
                var projectArray = new List<Project>();
                
                foreach(var watcher in getWatcherQuery)
                {
                    var getProjectQuery = await _context.Projects.Where(x => x.IsActive == true && x.Id == watcher.ProjectId).FirstOrDefaultAsync();
                    var projectInstance = new Project();
                    if(getProjectQuery != null)
                    {
                        projectInstance.Caption = getProjectQuery.Caption;
                        projectInstance.Alias = getProjectQuery.Alias;
                        projectInstance.CreatedAt = getProjectQuery.CreatedAt;
                        projectInstance.CreatedById = getProjectQuery.CreatedById;
                        projectInstance.Description = getProjectQuery.Description;
                        projectInstance.Id = getProjectQuery.Id;
                        projectInstance.IsActive = getProjectQuery.IsActive;
                        projectInstance.ProjectImage = getProjectQuery.ProjectImage;
                        projectInstance.Tasks = await _context.Tasks.Where(x => x.IsActive == true && x.ProjectId == getProjectQuery.Id).ToListAsync();
                        projectInstance.Watchers = await _context.Watchers.Where(x => x.IsActive == true && x.ProjectId == getProjectQuery.Id).ToListAsync();
                        projectInstance.Workspace = await _context.Workspaces.Where(x => x.IsActive == true && x.IsActive && x.Id == getProjectQuery.WorkspaceId).FirstOrDefaultAsync();
                        projectInstance.WorkspaceId = getProjectQuery.WorkspaceId;

                    }
                   
                    projectArray.Add(getProjectQuery);

                }

                if(projectArray.Count == 0)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "You have not been selected as a watcher on any project");
                }
                else
                {
                    var watcherCount = 0;
                    var workspaceArray = new List<Workspace>();
                    foreach(var project in projectArray)
                    {
                        if(project != null)
                        {
                            var workspace = project.Workspace;
                            workspaceArray.Add(workspace);
                            watcherCount = watcherCount + 1;
                        }

                    }

                    var workspaceResult = workspaceArray.GroupBy(p => p.Id)
                           .Select(result => result.First())
                           .ToArray();
                    var workspaceWithWatcher = new WorkspaceWithWatchersDTO();
                    workspaceWithWatcher.workspaces = workspaceResult;
                    workspaceWithWatcher.WatcherCount = watcherCount;

                    return CommonResponse.Send(ResponseCodes.SUCCESS, workspaceWithWatcher, "Successfully fetched records");
                }

            }




           // var testQuery = await _context.Watchers.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId())
           //                                         .Include(x => x.Project)
           //                                         .ThenInclude(x => x.Workspace)
           //                                         .ToListAsync();

           // var counter = 0;

           //var workspaces = new List<Workspace>();
           // foreach(var watcher in testQuery)
           // {
           //      if(watcher.Project.IsActive == true)
           //     {
           //         workspaces.Add(watcher.Project.Workspace);
           //         counter = counter + 1;
           //     }
           // }

           // var workspaceResult = workspaces.GroupBy(p => p.Id)
           //                .Select(result => result.First())
           //                .ToArray();

           // var workspaceWithWatcher = new WorkspaceWithWatchersDTO();
           // workspaceWithWatcher.workspaces = workspaceResult;
           // workspaceWithWatcher.WatcherCount = counter;


           // return CommonResponse.Send(ResponseCodes.SUCCESS, workspaceWithWatcher);

        }

        public async Task<ApiCommonResponse> getAllTaskFromProject(HttpContext httpContext, long projectId)
        {
            var getTaskQuery = await _context.Tasks.Where(x => x.IsActive == true && x.ProjectId == projectId).ToListAsync();

            if(getTaskQuery.Count == 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, "No Task was found");
            }

            else
            {
                var taskArray = new List<TaskDTO>();
                foreach(var task in getTaskQuery)
                {
                    var taskInstance = new TaskDTO();
                    taskInstance.Alias = task.Alias;
                    taskInstance.Caption = task.Caption;
                    taskInstance.CreatedAt = task.CreatedAt;
                    taskInstance.CreatedById = task.CreatedById;
                    taskInstance.DeliverableStatusDTOs = await getDeliverables(task.Id);
                    taskInstance.Description = task.Description;
                    taskInstance.DueTime = task.DueTime;
                    taskInstance.IsAssigned = task.IsAssigned;
                    taskInstance.IsMilestone = task.IsMilestone;
                    taskInstance.IsReassigned = task.IsReassigned;
                    taskInstance.IsWorkbenched = task.IsWorkbenched;
                    taskInstance.ProjectId = task.ProjectId;
                    taskInstance.project = task.Project;
                    taskInstance.TaskEndDate = task.TaskEndDate;
                    taskInstance.TaskStartDate = task.TaskStartDate;
                    taskInstance.WorkingManHours = task.WorkingManHours;
                    taskArray.Add(taskInstance);
                }
                return CommonResponse.Send(ResponseCodes.SUCCESS,taskArray, "List of Task from project successfully provided");
            }
        }




        //public async Task<ApiCommonResponse> getAllTaskFromProjectRevamped(HttpContext httpContext, long projectId)
        //{

        //    var getTaskQuery = await _context.Tasks.Where(x => x.IsActive == true && x.ProjectId == projectId)
        //                                                 .Include(x => x.Deliverables.Where(x => x.IsActive == true))
        //                                                 .ThenInclude(x => x.AssignTask)
        //                                                 .Include(x=>x.TaskOwnership)
        //                                                        .ThenInclude(x=>x.TaskOwner)
        //                                           .ToListAsync();




        //    return CommonResponse.Send(ResponseCodes.SUCCESS, getTaskQuery);

        //}

        public async Task<ApiCommonResponse> getProjectCountBarChart(HttpContext httpContext)
        {
            var projectQuery = await _context.Watchers.Where(x => x.IsActive == true && x.ProjectWatcherId == httpContext.GetLoggedInUserId())
                                                      .Include(x => x.Project)
                                                      .ThenInclude(x => x.Tasks)
                                                      .ToListAsync();
            var projectArray = new List<Project>();
            foreach(var watcher in projectQuery)
            {
                if(watcher.Project.IsActive == true)
                {
                    projectArray.Add(watcher.Project);
                }
            }

            //var projectBarChart = new ProjectBarChartDTO();
            //if(projectArray.Count > 0)
            //{
            //    foreach (var project in projectArray)
            //    {
            //        projectBarChart.ProjectNames.Add(project.Caption);
            //        projectBarChart.TaskCount.Add(project.Tasks.Count);

            //    }
            //}
            

            return CommonResponse.Send(ResponseCodes.SUCCESS, projectArray);
        }

        public async Task<ApiCommonResponse> getWorkspaceCountBarChart(HttpContext httpContext)
        {
            var watcherQuery = await _context.Watchers.Where(x => x.IsActive == true && x.ProjectWatcherId == httpContext.GetLoggedInUserId())
                                                       .Include(x => x.Project)
                                                       .ThenInclude(x => x.Workspace)
                                                          .ThenInclude(x => x.Projects.Where(x => x.IsActive == true))
                                                                      .ThenInclude(x => x.Tasks)
                                                       .ToListAsync();
                                                       

            var workspaceArray = new List<Workspace>();
            foreach(var watcher in watcherQuery)
            {
                workspaceArray.Add(watcher.Project.Workspace);
            }

            //var workspaceBarchart = new WorkspaceBarchartDTO();
            //if (workspaceArray.Count > 0)
            //{

            //    foreach (var workspace in workspaceArray)
            //    {
            //        workspaceBarchart.WorkspaceName.Add(workspace.Caption);
            //        workspaceBarchart.TaskCount.Add(_context.Projects.Where(x => x.IsActive == true && x.WorkspaceId == workspace.Id)
            //                                    .Include(x => x.Tasks.Where(x => x.IsActive == true))
            //                                    .ToList().Count);

            //        foreach (var project in workspace.Projects)
            //        {
            //            workspaceBarchart.TaskCount.Add(project.Tasks.Count);
            //        }


            //    }

            //}

            var workspaceResult = workspaceArray.GroupBy(p => p.Id)
                           .Select(result => result.First())
                           .ToArray();


            return CommonResponse.Send(ResponseCodes.SUCCESS, workspaceResult);

        }

        
        public async Task<ApiCommonResponse> getAllMilestoneTaskForWatcher(HttpContext httpContext)
        {
            var milestoneQuery = await _context.Watchers.Where(x => x.IsActive == true && x.ProjectWatcherId == httpContext.GetLoggedInUserId())
                                                        .Include(x => x.Project)
                                                                .ThenInclude(x => x.Tasks.Where(x => x.IsActive == true && x.IsMilestone == true))
                                                                      .ThenInclude(x=>x.Deliverables.Where(x=>x.IsActive == true))
                                                        .ToListAsync();
            var taskList = new List<Task>();
            foreach(var watcher in milestoneQuery)
            {
                if(watcher.Project.Tasks != null || watcher.Project.Tasks.Count != 0)
                {
                    taskList.AddRange(watcher.Project.Tasks);
                }
                
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, taskList);
        }

        public async Task<ApiCommonResponse> getAllTaskToDueToday(HttpContext httpContext)
        {
            var taskDuetoday = await _context.Watchers.Where(x => x.IsActive == true && x.ProjectWatcherId == httpContext.GetLoggedInUserId())
                                                       .Include(x => x.Project)
                                                               .ThenInclude(x => x.Tasks.Where(x => x.IsActive == true && x.IsMilestone == true))
                                                                            .ThenInclude(x => x.Deliverables.Where(x => x.IsActive == true))
                                                       .ToListAsync();

            var taskList = new List<Task>();
            foreach (var watcher in taskDuetoday)
            {
                var today = DateTime.Now;
                var getTaskDuetoday = watcher.Project.Tasks.Where(x => x.IsActive && x.TaskEndDate == today).ToList();
                if(getTaskDuetoday.Count != 0)
                {
                    taskList.AddRange(getTaskDuetoday);
                }
                 
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, taskList);
        }

        public async Task<ApiCommonResponse> getTaskOwnershipDTO(HttpContext httpContext)
        {

            var taskOwnersQuery = await _context.Watchers.Where(x => x.IsActive == true && x.ProjectWatcherId == httpContext.GetLoggedInUserId())
                                                         .Include(x => x.Project)
                                                                .ThenInclude(x => x.Tasks.Where(x => x.IsActive == true))
                                                                   .ThenInclude(x => x.TaskOwnership)
                                                                               .ThenInclude(x=>x.Tasks.Where(x=>x.IsActive == true))
                                                                               .ToListAsync();

            var taskArray = new List<Task>();
            if(taskOwnersQuery.Count > 0)
            {
                var taskBar = new TaskBarChartDTO();
                foreach(var watchers in taskOwnersQuery)
                {
                    taskArray.AddRange(watchers.Project.Tasks);
                }

            }


            var taskOwnerDTO = new List<TaskBarChartDTO>();
            if (taskArray.Count > 0)
            {

                foreach (var task in taskArray)
                {
                    var taskOwner = new TaskBarChartDTO();
                    var owner = await _context.UserProfiles.Where(x => x.IsDeleted == false && x.Id == task.TaskOwnership.TaskOwnerId).FirstOrDefaultAsync();
                    taskOwner.TaskOwnerName = owner.FirstName + " " + owner.LastName;
                    
                    taskOwner.TaskCount = _context.Tasks.Where(x => x.IsActive == true && x.TaskOwnershipId == task.TaskOwnershipId)
                                                              .ToList().Count();
                    taskOwnerDTO.Add(taskOwner);
                }



            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, taskOwnerDTO);

        }




        public async Task<ApiCommonResponse> getTaskPieChartData(HttpContext httpContext)
        {
            var watcherQuery = await _context.Watchers.Where(x => x.IsActive == true && x.ProjectWatcherId == httpContext.GetLoggedInUserId())
                                                       .Include(x => x.Project)
                                                               .ThenInclude(x => x.Tasks.Where(x => x.IsActive == true))
                                                               .ThenInclude(x=>x.Deliverables.Where(x=>x.IsActive == true))
                                                        .ToListAsync();


            var taskArray = new List<Task>();
            foreach(var watcher in watcherQuery)
            {


                foreach(var task in watcher.Project.Tasks)
                {
                    if(task.Deliverables.Count > 0)
                    {

                    taskArray.Add(task);

                    }
                }

            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, taskArray);
        }

        public async Task<ApiCommonResponse> getAllTaskFromProjectRevamped(HttpContext httpContext, long projectId)
        {

            var getTaskQuery = await _context.Tasks.Where(x => x.IsActive == true && x.ProjectId == projectId)
                                                         .Include(x => x.Deliverables.Where(x => x.IsActive == true))
                                                         .ThenInclude(x => x.UploadedRequirements.Where(x=>x.IsActive == true))
                                                         .ToListAsync();


              

           
            foreach(var task in getTaskQuery)
            {
                var taskArray = new List<Task>();
                var deliverableArray = new List<Deliverable>();
                foreach (var deliverable in task.Deliverables)
                {
                    deliverable.AssignTask = await _context.AssignTasks.Where(x => x.IsActive == true && x.DeliverableId == deliverable.Id)
                                                                                 .Include(x => x.DeliverableAssignee).FirstOrDefaultAsync();
                    deliverable.Balances = await _context.Balances.Where(x => x.IsActive == true && x.DeliverableId == deliverable.Id).ToListAsync();
                    deliverable.Status = await _context.StatusFlows.Where(x => x.IsDeleted == false && x.Id == deliverable.StatusId).FirstOrDefaultAsync();
                    deliverable.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == deliverable.Id).ToListAsync();
                    deliverableArray.Add(deliverable);
                }

                task.Deliverables = deliverableArray;
                taskArray.Add(task);

            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, getTaskQuery);

        }



        public async Task<List<DeliverableWithStatusDTO>> getDeliverables(long taskId)
        {
            var deliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == taskId).ToListAsync();
            var deliverableArray = new List<DeliverableWithStatusDTO>();
            if (deliverables.Count != 0)
            {
                
                foreach (var deliverable in deliverables)
                {
                    var deliverableInstance = new DeliverableWithStatusDTO();
                    deliverableInstance.Caption = deliverable.Caption;
                    deliverableInstance.Alias = deliverable.Alias;
                    deliverableInstance.Description = deliverable.Description;
                    deliverableInstance.AssignTask = await _context.AssignTasks.Where(x => x.IsActive == true && x.DeliverableId == deliverable.Id).FirstOrDefaultAsync();
                    deliverableInstance.AssignTaskId = deliverableInstance.AssignTask == null ? 0 : deliverableInstance.AssignTask.DeliverableAssigneeId;
                    deliverableInstance.Balances = await _context.Balances.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
                    deliverableInstance.Budget = deliverable.Budget;
                    deliverableInstance.IsPicked = deliverable.IsPicked;
                    deliverableInstance.IsApproved = deliverable.IsApproved;
                    deliverableInstance.IsDeclined = deliverable.IsDeclined;
                    deliverableInstance.CreatedAt = deliverable.CreatedAt;
                    //deliverableInstance.CreatedBy = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == deliverable.CreatedById);
                    deliverableInstance.CreatedById = deliverable.CreatedById;
                    deliverableInstance.DatePicked = deliverable.DatePicked;
                    //deliverableInstance.DeliverableAssignees = await _context.DeliverableAssignees.Where(x => x.IsActive == true && x.DeliverableId == deliverable.Id).ToListAsync();
                    //deliverableInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == deliverable.Id).ToListAsync();
                    deliverableInstance.DependentType = deliverable.DependentType;
                    //deliverableInstance.Documents = await _context.Documents.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
                    deliverableInstance.EndDate = deliverable.EndDate;
                    deliverableInstance.Id = deliverable.Id;
                    deliverableInstance.IsPushedForApproval = deliverable.IsPushedForApproval;
                    deliverableInstance.DatePushedForApproval = deliverable.DatePushedForApproval;
                    deliverableInstance.DeclineReason = deliverable.DeclineReason;
                    deliverableInstance.IsActive = deliverable.IsActive;
                    //deliverableInstance.Notes = await _context.PMNotes.Where(x => x.DeliverableId == deliverable.Id && x.IsActive == true).ToListAsync();
                    //deliverableInstance.PMIllustrations = await _context.PMIllustrations.Where(x => x.IsActive == true && x.TaskOrDeliverableId == deliverable.Id).ToListAsync();
                    //deliverableInstance.Pictures = await _context.Pictures.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
                    //deliverableInstance.Requirements = await _context.PMRequirements.Where(x => x.DeliverableId == deliverable.Id && x.IsActive == true).ToListAsync();
                    deliverableInstance.StartDate = deliverable.StartDate;
                    deliverableInstance.StatusFlow = await _context.StatusFlows.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == deliverable.StatusId);
                    deliverableInstance.Task = await _context.Tasks.Where(x => x.IsActive == true && x.Id == deliverable.TaskId).Include(x => x.Project).FirstOrDefaultAsync();
                    deliverableInstance.TaskId = deliverable.TaskId;
                    deliverableInstance.TimeEstimate = deliverable.TimeEstimate;
                    deliverableInstance.UpdatedAt = deliverable.UpdatedAt;
                    //deliverableInstance.UploadedRequirement = await _context.PMUploadedRequirements.Where(x => x.IsActive == true && x.DeliverableId == deliverableInstance.Id).ToListAsync();
                    //deliverableInstance.Videos = await _context.Videos.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
                    deliverableInstance.AssignedTo = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == deliverableInstance.AssignTaskId);
                    if(deliverableInstance.AssignTask != null)
                    {
                        deliverableArray.Add(deliverableInstance);
                    }
                    
                }

                
            }
            return deliverableArray;
        }

        public async Task<ApiCommonResponse> getWorkspaceWithStatus(HttpContext httpContext)
        {


            var testQuery = await _context.AssignTasks.Where(x => x.DeliverableAssigneeId == httpContext.GetLoggedInUserId() && x.IsActive == true)
                                                        .Include(x => x.Deliverable)
                                                        .ThenInclude(x => x.Task)
                                                        .ThenInclude(x => x.Project)
                                                        .ThenInclude(x => x.Workspace)
                                                            .ThenInclude(x => x.StatusFlows)
                                                        .ToListAsync();



            var workspaceArray = new List<Workspace>();
            foreach (var assignTask in testQuery)
            {
                if(assignTask.Deliverable.IsActive == true && assignTask.Deliverable.IsApproved == false)
                {
                    var relatedWorkspace = assignTask.Deliverable.Task.Project.Workspace;
                    workspaceArray.Add(relatedWorkspace);
                }

                    
            }

            var workspaceResult = workspaceArray.GroupBy(p => p.Id)
                           .Select(result => result.First())
                           .ToArray();


           // var finalResult = await getWorkspaceWithUser(httpContext, workspaceResult.ToList());

            return CommonResponse.Send(ResponseCodes.SUCCESS, workspaceResult);


            
        }



        public async Task<ApiCommonResponse> pickDeliverable(HttpContext httpContext,long deliverableId)
        {
            
            var getDeliverableToUpdate = await _context.Deliverables.Where(x => x.IsActive == true && x.Id == deliverableId).FirstOrDefaultAsync();
            getDeliverableToUpdate.IsPicked = true;
            _context.Deliverables.Update(getDeliverableToUpdate);
            _context.SaveChanges();

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, "Deliverable successfully picked");
        }



        public async Task<ApiCommonResponse> selectStatus(HttpContext httpContext, long statusId, long deliverableId)
        {

            var getDeliverableToUpdate = await _context.Deliverables.Where(x => x.IsActive == true && x.Id == deliverableId).FirstOrDefaultAsync();
            getDeliverableToUpdate.StatusId = statusId;
            getDeliverableToUpdate.IsPicked = true;
            _context.Deliverables.Update(getDeliverableToUpdate);
            _context.SaveChanges();

            var getCurrentStatus = await _context.StatusFlows.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == statusId);

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, "successfully updated status to " + getCurrentStatus.Caption);
        }

        

        public async Task<ApiCommonResponse> moveToAnotherStatus(HttpContext httpContext, List<StatusFlow> statuses,long statusId,long deliverableId,int statusCode)
        {
            if (statuses.LastOrDefault().Id == statusId && statusCode == +1)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "This action cannot be performed because you are at the last status,therefore your deliverable is pending approval");
            }
            else if(statuses.LastOrDefault().Id == statusId && statusCode == -1)
            {
                var getCurrentStatusIndex = statuses.FindIndex(x => x.Id == statusId);
                var getNewIndex = getCurrentStatusIndex - 1;
                var getNewStatusId = statuses.ElementAt(getNewIndex).Id;
                var getDeliverableToUpdate = await _context.Deliverables.Where(x => x.IsActive == true && x.Id == deliverableId).FirstOrDefaultAsync();
                getDeliverableToUpdate.StatusId = getNewStatusId;
                getDeliverableToUpdate.IsPicked = true;
                _context.Deliverables.Update(getDeliverableToUpdate);
                _context.SaveChanges();

                var getCurrentStatus = await _context.StatusFlows.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == getNewStatusId);

                return CommonResponse.Send(ResponseCodes.SUCCESS, null, "Status successfully moved to " + getCurrentStatus.Caption);

            }
            else if(statuses.FirstOrDefault().Id == statusId && statusCode == -1)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "This action cannot be performed because you are at the first status,therefore you cannot move backward");
            }
            else if(statuses.FirstOrDefault().Id == statusId && statusCode == +1)
            {
                var getCurrentStatusIndex = statuses.FindIndex(x => x.Id == statusId);
                var getNewIndex = getCurrentStatusIndex + 1;
                var getNewStatusId = statuses.ElementAt(getNewIndex).Id;
                var getDeliverableToUpdate = await _context.Deliverables.Where(x => x.IsActive == true && x.Id == deliverableId).FirstOrDefaultAsync();
                getDeliverableToUpdate.StatusId = getNewStatusId;
                getDeliverableToUpdate.IsPicked = true;
                _context.Deliverables.Update(getDeliverableToUpdate);
                _context.SaveChanges();

                var getCurrentStatus = await _context.StatusFlows.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == getNewStatusId);

                return CommonResponse.Send(ResponseCodes.SUCCESS, null, "Status successfully moved to " + getCurrentStatus.Caption);

            }
            else if(statuses.FirstOrDefault().Id != statusId || statuses.LastOrDefault().Id == statusId)
            {
                var getCurrentStatusIndex = statuses.FindIndex(x => x.Id == statusId);
                var getNewIndex = getCurrentStatusIndex + statusCode;
                var getNewStatusId = statuses.ElementAt(getNewIndex).Id;
                var getDeliverableToUpdate = await _context.Deliverables.Where(x => x.IsActive == true && x.Id == deliverableId).FirstOrDefaultAsync();
                getDeliverableToUpdate.StatusId = getNewStatusId;
                getDeliverableToUpdate.IsPicked = true;
                _context.Deliverables.Update(getDeliverableToUpdate);
                _context.SaveChanges();

                var getCurrentStatus = await _context.StatusFlows.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == getNewStatusId);

                return CommonResponse.Send(ResponseCodes.SUCCESS, null, "Status successfully moved to " + getCurrentStatus.Caption);
            }

                return CommonResponse.Send(ResponseCodes.FAILURE, null, "This operation was not successfull");

        }



        public async Task<List<WorkspaceRoot>> getWorkspaceWithUser(HttpContext httpContext,List<Workspace> workspaces)
        {
            var workspaceArray = new List<WorkspaceRoot>();
            foreach(var workspace in workspaces)
            {
                var workspaceInstance = new WorkspaceRoot();
                workspaceInstance.Caption = workspace.Caption;
                workspaceInstance.Alias = workspace.Alias;
                workspaceInstance.Description = workspace.Description;
                workspaceInstance.Id = workspace.Id;
                workspaceInstance.IsActive = workspace.IsActive;
                workspaceInstance.IsPublic = workspace.IsPublic;
                workspaceInstance.CreatedById = workspace.CreatedById;
                workspaceInstance.StatusFlowOption = workspace.StatusFlowOption;
                workspaceInstance.StatusFlows = workspace.StatusFlows;
                workspaceInstance.CreatedAt = workspace.CreatedAt;
                workspaceInstance.userprofile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == workspaceInstance.CreatedById);
                workspaceArray.Add(workspaceInstance);
            }

            return workspaceArray;
            
        }

        public async Task<ApiCommonResponse> disableComment(HttpContext httpContext, long commentId,long deliverableId)
        {
            var getComment = await _context.PMNotes.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == commentId);
            if(getComment != null)
            {
                 getComment.IsActive = false;
                _context.PMNotes.Update(getComment);
                _context.SaveChanges();
            }

            var getAllNotesByDeliverable = await _context.PMNotes.Where(x => x.IsActive == true && x.DeliverableId == deliverableId).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS, getAllNotesByDeliverable, "Entity successfully removed");
        }

        public async Task<ApiCommonResponse> saveAmountSpent(HttpContext httpContext,decimal amount, long deliverableId)
        {
            var getBalance = await _context.Balances.Where(x => x.IsActive == true && x.DeliverableId == deliverableId).ToListAsync();
            var getDeliverableById = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == deliverableId);
            var totalSpentOndeliverable = getBalance.Sum(x => x.AmountSpent);

            if (getBalance.Count > 0)
            {
               if(amount + totalSpentOndeliverable > getDeliverableById.Budget)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "You can't spend more than the budgeted amount for this deliverable");
                }
                else
                {
                    var balanceInstance = new Balance();
                    balanceInstance.AmountSpent = amount;
                    balanceInstance.CreatedAt = DateTime.Now;
                    balanceInstance.CreatedById = httpContext.GetLoggedInUserId();
                    balanceInstance.DeliverableId = deliverableId;
                    balanceInstance.IsActive = true;
                    balanceInstance.TotalAmountSpent = amount + totalSpentOndeliverable;
                    balanceInstance.PM_Balance = getDeliverableById.Budget - balanceInstance.TotalAmountSpent;
                    await _context.Balances.AddAsync(balanceInstance);
                    await _context.SaveChangesAsync();
                }

            }
            else
            {
                if (amount  > getDeliverableById.Budget)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "You can't spend more than the budgeted amount for this deliverable");
                }
                else
                {
                    var balanceInstance = new Balance();
                    balanceInstance.AmountSpent = amount;
                    balanceInstance.CreatedAt = DateTime.Now;
                    balanceInstance.CreatedById = httpContext.GetLoggedInUserId();
                    balanceInstance.DeliverableId = deliverableId;
                    balanceInstance.IsActive = true;
                    balanceInstance.PM_Balance = getDeliverableById.Budget - amount;
                    balanceInstance.TotalAmountSpent = amount;
                    await _context.Balances.AddAsync(balanceInstance);
                    await _context.SaveChangesAsync();
                }

            }
            

            var getAllBalance = await _context.Balances.Where(x => x.IsActive == true && x.DeliverableId == deliverableId).OrderByDescending(x=>x.TotalAmountSpent).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS, getAllBalance, "Entity successfully saved");
        }


        //public async Task<ApiCommonResponse> disableAmountSpent(HttpContext httpContext, decimal amount, long deliverableId)
        //{
        //    var getBalance = await _context.Balances.Where(x => x.IsActive == true && x.DeliverableId == deliverableId).ToListAsync();
        //    var getDeliverableById = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == deliverableId);
        //    var totalSpentOndeliverable = getBalance.Sum(x => x.AmountSpent);

        //    if (getBalance.Count > 0)
        //    {
        //        if (amount > totalSpentOndeliverable)
        //        {
        //            return CommonResponse.Send(ResponseCodes.FAILURE, null, "You can't deduct more than the total amount spent on this deliverable");
        //        }
        //        else if (amount > getDeliverableById.Budget)
        //        {
        //            return CommonResponse.Send(ResponseCodes.FAILURE, null, "You can't deduct more than the budgeted amount for this deliverable");
        //        }
        //        else
        //        {
        //            var balanceInstance = new Balance();
        //            balanceInstance.AmountSpent = amount;
        //            balanceInstance.CreatedAt = DateTime.Now;
        //            balanceInstance.CreatedById = httpContext.GetLoggedInUserId();
        //            balanceInstance.DeliverableId = deliverableId;
        //            balanceInstance.IsActive = true;
        //            balanceInstance.PM_Balance = getDeliverableById.Budget - amount + totalSpentOndeliverable;
        //            balanceInstance.TotalAmountSpent = amount + totalSpentOndeliverable;
        //            await _context.Balances.AddAsync(balanceInstance);
        //            await _context.SaveChangesAsync();
        //        }

        //    }



        //    var getAllBalance = await _context.Balances.Where(x => x.IsActive == true && x.DeliverableId == deliverableId).FirstOrDefaultAsync();
        //    return CommonResponse.Send(ResponseCodes.SUCCESS, getAllBalance, "Entity successfully saved");
        //}
        public async Task<ApiCommonResponse> ApproveDeliverable(HttpContext httpContext, long deliverableId)
        {

            var getDeliverableToApprove = await _context.Deliverables.Where(x => x.IsActive == true && x.Id == deliverableId).FirstOrDefaultAsync();

            getDeliverableToApprove.IsApproved = true;
            getDeliverableToApprove.IsDeclined = false;
            _context.Deliverables.Update(getDeliverableToApprove);
            _context.SaveChanges();

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, "Deliverable " + getDeliverableToApprove.Caption +  " was approved successfuly");


        }


        public async Task<ApiCommonResponse> DeclineDeliverable(HttpContext httpContext, long deliverableId,string declineReason)
        {

            var getDeliverableToDecline = await _context.Deliverables.Where(x => x.IsActive == true && x.Id == deliverableId).FirstOrDefaultAsync();
            getDeliverableToDecline.IsDeclined = true;
            getDeliverableToDecline.DeclineReason = declineReason;
            _context.Deliverables.Update(getDeliverableToDecline);
            _context.SaveChanges();

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, "Deliverabel " + getDeliverableToDecline.Caption + " was declined");


        }




        public async Task<ApiCommonResponse> pushForApproval(HttpContext httpContext,long deliverableId) {

            var getDeliverable = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == deliverableId);
            var requirementCount = 0;
            if (getDeliverable != null)
            {


                var getRequirementsForThisDeliverable = await _context.PMRequirements.Where(x => x.IsActive == true && x.DeliverableId == deliverableId).ToListAsync();

                
                if(getRequirementsForThisDeliverable.Count > 0)
                {
                    foreach (var required in getRequirementsForThisDeliverable)
                    {
                        var resolvedRequirements = await _context.PMUploadedRequirements.Where(x => x.IsActive == true && x.RequirementId == required.Id).FirstOrDefaultAsync();
                        if(resolvedRequirements != null)
                        {
                            if (required.FileExtention.ToLower().Trim() == resolvedRequirements.Extension.Substring(resolvedRequirements.Extension.IndexOf('/') + 1).ToLower().Trim())
                            {
                                requirementCount = requirementCount + 1;
                            }
                            else
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, "The Uploaded file " + resolvedRequirements.Caption + " must be of file extension " + required.FileExtention);
                            }
                        }
                            else
                            {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, "The requirement " + required.Caption + " is missing an upload,therefore it cannot be pushed for approval");
                            }
                       

                    }
                }


            }

            getDeliverable.IsPushedForApproval = true;
            getDeliverable.DatePushedForApproval = DateTime.Now;
            _context.Deliverables.Update(getDeliverable);
            _context.SaveChanges();
            return CommonResponse.Send(ResponseCodes.SUCCESS, getDeliverable, "All "+ requirementCount + " requirement(s) where met,therefore " + getDeliverable.Caption + "has been successfully pushed for Approval");
        }


        public async Task<ApiCommonResponse> reverseApproval(HttpContext httpContext, long deliverableId)
        {

            var getDeliverable = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == deliverableId);
            if (getDeliverable != null)
            {
                getDeliverable.IsPushedForApproval = false;
            }

            _context.Deliverables.Update(getDeliverable);
            _context.SaveChanges();

            return CommonResponse.Send(ResponseCodes.SUCCESS, getDeliverable, "Deliverable successfully pushed for approval");
        }



        public async Task<ApiCommonResponse> getDeliverableApprovalList(HttpContext httpContext)
        {

            var deliverableQuery = await _context.Deliverables.Where(x => x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true && x.IsPushedForApproval == true && x.IsApproved == false && x.IsDeclined == false)
                                    .Include(x => x.Task)
                                    .ThenInclude(x => x.Project)
                                    .ThenInclude(x => x.Workspace)
                                    .ThenInclude(x => x.StatusFlows.Where(x => x.IsDeleted == false))
                                    .ToListAsync();

            var counter = 0;
            var newProjectArray = new List<Project>();
            foreach (var deliverable in deliverableQuery)
            {
                newProjectArray.Add(deliverable.Task.Project);
                counter = counter + 1;

            }

            var ProjectListResult = newProjectArray.GroupBy(p => p.Id)
                          .Select(result => result.First())
                          .ToArray();

            var assignedDeliverable = new AssigneDeliverableDTO();

            assignedDeliverable.Project = ProjectListResult;
            assignedDeliverable.DeliverableCount = counter;


            return CommonResponse.Send(ResponseCodes.SUCCESS, assignedDeliverable, "Entity successfully FOUND");
        }


        public async Task<ApiCommonResponse> getprojectForWatchers(HttpContext httpContext)
        {

            var projectQuery = await _context.Watchers.Where(x => x.IsActive == true && x.ProjectWatcherId == httpContext.GetLoggedInUserId())
                                        .Include(x => x.Project)
                                        .ThenInclude(x => x.Workspace)
                                        .ToListAsync();

            var counter = 0;
            var workspaceArray = new List<Workspace>();
            foreach (var projectWatcher in projectQuery)
            {
                workspaceArray.Add(projectWatcher.Project.Workspace);
                counter = counter + 1;

            }

            var ProjectListResult = workspaceArray.GroupBy(p => p.Id)
                          .Select(result => result.First())
                          .ToArray();

            var watcherProjects = new ProjectWatcherDashboardDTO();

            watcherProjects.Workspace = ProjectListResult;
            watcherProjects.ProjectCount = counter;


            return CommonResponse.Send(ResponseCodes.SUCCESS, watcherProjects, "Entity successfully FOUND");
        }

        public async Task<ApiCommonResponse> getDeliverableApproved(HttpContext httpContext)
        {

            var deliverableQuery = await _context.Deliverables.Where(x => x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true && x.IsPushedForApproval == true && x.IsApproved == true)
                                    .Include(x => x.Task)
                                    .ThenInclude(x => x.Project)
                                    .ThenInclude(x => x.Workspace)
                                    .ThenInclude(x => x.StatusFlows.Where(x => x.IsDeleted == false))
                                    .ToListAsync();

            var counter = 0;
            var newProjectArray = new List<Project>();
            foreach (var deliverable in deliverableQuery)
            {
                newProjectArray.Add(deliverable.Task.Project);
                counter = counter + 1;

            }

            var ProjectListResult = newProjectArray.GroupBy(p => p.Id)
                          .Select(result => result.First())
                          .ToArray();

            var assignedDeliverable = new AssigneDeliverableDTO();

            assignedDeliverable.Project = ProjectListResult;
            assignedDeliverable.DeliverableCount = counter;


            return CommonResponse.Send(ResponseCodes.SUCCESS, assignedDeliverable, "Entity successfully FOUND");
        }


        public async Task<ApiCommonResponse> getDeliverableIsDeclined(HttpContext httpContext)
        {

            var deliverableQuery = await _context.Deliverables.Where(x => x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true && x.IsPushedForApproval == true && x.IsDeclined == true)
                                    .Include(x => x.Task)
                                    .ThenInclude(x => x.Project)
                                    .ThenInclude(x => x.Workspace)
                                    .ThenInclude(x => x.StatusFlows.Where(x => x.IsDeleted == false))
                                    .ToListAsync();

            var counter = 0;
            var newProjectArray = new List<Project>();
            foreach (var deliverable in deliverableQuery)
            {
                newProjectArray.Add(deliverable.Task.Project);
                counter = counter + 1;

            }

            var ProjectListResult = newProjectArray.GroupBy(p => p.Id)
                          .Select(result => result.First())
                          .ToArray();

            var assignedDeliverable = new AssigneDeliverableDTO();

            assignedDeliverable.Project = ProjectListResult;
            assignedDeliverable.DeliverableCount = counter;


            return CommonResponse.Send(ResponseCodes.SUCCESS, assignedDeliverable, "Entity successfully FOUND");
        }


        public async Task<ApiCommonResponse> disableRequirementUpload(HttpContext httpContext, long uploadedRequirementId)
        {

            var getUploadedRequirement = await _context.PMUploadedRequirements.FirstOrDefaultAsync(x => x.Id == uploadedRequirementId && x.IsActive == true);


            if(getUploadedRequirement != null)
            {
                getUploadedRequirement.IsActive = false;
                _context.Update(getUploadedRequirement);
                _context.SaveChanges();
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, getUploadedRequirement, "Entity successfully deleted");
        }


        public async Task<ApiCommonResponse> addComments(HttpContext httpContext,long deliverableId,CommentsDTO comments)
        {

            var noteToBeSaved = new PMNote
            {
                Caption = comments.caption,
                Description = comments.description,
                DeliverableId = deliverableId,
                CreatedById = httpContext.GetLoggedInUserId(),
                IsActive = true,
                //DeliverableAssigneeId = deliverableAssigneeId,
                CreatedAt = DateTime.Now
            };

            await _context.PMNotes.AddAsync(noteToBeSaved);
            await _context.SaveChangesAsync();

            var getAllNotesByDeliverableId = await _context.PMNotes.Where(x => x.DeliverableId == deliverableId && x.IsActive == true).ToListAsync();

            return CommonResponse.Send(ResponseCodes.SUCCESS, getAllNotesByDeliverableId, "Comment successfully created");
        }


        public async Task<ApiCommonResponse> createUploadedRequirement(HttpContext httpContext, UploadedRequirement uploadedRequirement )
        {
            var checkIfDeliverableExist = await _context.Deliverables.FirstOrDefaultAsync(x => x.Id == uploadedRequirement.DeliverableId && x.IsActive == true);
            if(checkIfDeliverableExist != null) {

                var checkUploadedFileExistence = await _context.PMUploadedRequirements.FirstOrDefaultAsync(x => x.IsActive == true && x.RequirementId == uploadedRequirement.RequirementId);

                if(checkUploadedFileExistence != null)
                {
                    checkUploadedFileExistence.Alias = uploadedRequirement.Alias;
                    checkUploadedFileExistence.RequirementId = uploadedRequirement.RequirementId;
                    checkUploadedFileExistence.Extension = uploadedRequirement.Extension;
                    checkUploadedFileExistence.Caption = uploadedRequirement.Caption;
                    checkUploadedFileExistence.CreatedAt = DateTime.Now;
                    checkUploadedFileExistence.DeliverableId = uploadedRequirement.DeliverableId;
                    checkUploadedFileExistence.Description = uploadedRequirement.Description;
                    checkUploadedFileExistence.DocUrl = uploadedRequirement.DocUrl;
                    _context.PMUploadedRequirements.Update(checkUploadedFileExistence);
                    _context.SaveChanges();
                }
                else
                {
                    var UploadedFilesInfo = new PMUploadedRequirement();


                    UploadedFilesInfo.Alias = uploadedRequirement.Alias;
                    UploadedFilesInfo.RequirementId = uploadedRequirement.RequirementId;
                    UploadedFilesInfo.Extension = uploadedRequirement.Extension;
                    UploadedFilesInfo.Caption = uploadedRequirement.Caption;
                    UploadedFilesInfo.CreatedAt = DateTime.Now;
                    UploadedFilesInfo.DeliverableId = uploadedRequirement.DeliverableId;
                    UploadedFilesInfo.Description = uploadedRequirement.Description;
                    UploadedFilesInfo.DocUrl = uploadedRequirement.DocUrl;
                    UploadedFilesInfo.IsActive = true;
                    UploadedFilesInfo.CreatedById = httpContext.GetLoggedInUserId();
                    UploadedFilesInfo.IsDeleted = false;
                    await _context.PMUploadedRequirements.AddAsync(UploadedFilesInfo);
                    await _context.SaveChangesAsync();
                }

                var getDeliverablefFile = await _context.PMUploadedRequirements.Where(x => x.IsActive == true && x.DeliverableId == uploadedRequirement.DeliverableId).ToListAsync();

                return CommonResponse.Send(ResponseCodes.SUCCESS, getDeliverablefFile, ResponseMessage.EntitySuccessfullyFound);

            }


            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.EntityNotFound) ;


        }



        public async Task<ApiCommonResponse> getDeliverablesByTaskId(HttpContext httpContext, long taskId)
        {
            //var deliverableQuery = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == taskId && x.IsApproved == false && x.AssignTask.DeliverableAssigneeId == httpContext.GetLoggedInUserId())
            //                                                   //.Include(x => x.Dependencies)
            //                                                   //.Include(x => x.Balances.Where(x => x.IsActive == true))
            //.Include(x => x.CreatedBy)
            //.Include(x => x.Notes.Where(x => x.IsActive == true))
            //.Include(x => x.Requirements.Where(x => x.IsActive == true))
            //.Include(x => x.Task)
            //        .ThenInclude(x=>x.Project)
            //.Include(x => x.Status)
            //.Include(x => x.UploadedRequirements.Where(x => x.IsActive == true))
            //.ToListAsync();

            var assigneeDeliverable = await _context.AssignTasks.Where(x => x.IsActive == true && x.DeliverableAssigneeId == httpContext.GetLoggedInUserId())
                                              .Include(x => x.DeliverableAssignee)
                                              .Include(x => x.Deliverable)
                                                      .ThenInclude(x => x.Status)
                                              .ToListAsync();

            var deliverableArray = new List<Deliverable>();
            foreach(var assignee in assigneeDeliverable)
            {
                if (assignee.Deliverable.IsActive == true && assignee.Deliverable.TaskId == taskId && assignee.Deliverable.IsApproved == false)
                {
                    assignee.Deliverable.AssignTask = await _context.AssignTasks.Where(x => x.IsActive == true && x.DeliverableAssigneeId == assignee.DeliverableAssigneeId).FirstOrDefaultAsync();
                    assignee.Deliverable.Task = await _context.Tasks.Where(x => x.IsActive == true && x.Id == taskId)
                                                                    .Include(x => x.Project)
                                                                  .FirstOrDefaultAsync();

                    //assignee.Deliverable.UploadedRequirements = await _context.PMUploadedRequirements.Where(x => x.IsActive == true && x.DeliverableId == assignee.Deliverable.Id)
                    //                                                                                  .ToListAsync();

                    assignee.Deliverable.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == assignee.Deliverable.Id).ToListAsync();

                    assignee.Deliverable.Balances = await _context.Balances.Where(x => x.IsActive == true && x.DeliverableId == assignee.Deliverable.Id).ToListAsync();
                    assignee.Deliverable.Workspace = await _context.Workspaces.Where(x => x.IsActive == true && x.Id == assignee.Deliverable.WorkspaceId)
                                                                               .Include(x => x.StatusFlows.Where(x => x.WorkspaceId == assignee.Deliverable.WorkspaceId && x.IsDeleted == false))
                                                                               .FirstOrDefaultAsync();


                    deliverableArray.Add(assignee.Deliverable);
                }
                
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, deliverableArray, ResponseMessage.EntitySuccessfullyFound);


        }


        public async Task<ApiCommonResponse> getCurrentDeliverableStatus(HttpContext httpContext,long deliverableId)
        {


            //PMUploadedRequirement();

                var deliverableArray = new List<DeliverableWithStatusDTO>();

            //var deliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.WorkspaceId == workspaceId).ToListAsync();
              var deliverables = await _context.Deliverables.Where(x => x.IsActive == true  && x.Id == deliverableId ).ToListAsync();


            if (deliverables.Count > 0)
                {
                    foreach (var deliverable in deliverables)
                    {
 
                        var deliverableInstance = new DeliverableWithStatusDTO();
                        deliverableInstance.Caption = deliverable.Caption;
                        deliverableInstance.Alias = deliverable.Alias;
                        deliverableInstance.Description = deliverable.Description;
                        deliverableInstance.AssignTask = await _context.AssignTasks.FirstOrDefaultAsync(x => x.IsActive == true && x.DeliverableId == deliverable.Id);
                        deliverableInstance.AssignTaskId = deliverable.AssignTaskId;
                        deliverableInstance.Balances = await _context.Balances.Where(x => x.DeliverableId == deliverable.Id && x.IsActive == true).ToListAsync();
                        deliverableInstance.Budget = deliverable.Budget;
                        deliverableInstance.IsPicked = deliverable.IsPicked;
                        deliverableInstance.IsApproved = deliverable.IsApproved;
                        deliverableInstance.IsDeclined = deliverable.IsDeclined;
                        deliverableInstance.CreatedAt = deliverable.CreatedAt;
                        deliverableInstance.CreatedById = deliverable.CreatedById;
                        deliverableInstance.DatePicked = deliverable.DatePicked;
                        deliverableInstance.DeliverableAssignees = await _context.DeliverableAssignees.Where(x => x.IsActive == true && x.DeliverableId == deliverable.Id).ToListAsync();
                        deliverableInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == deliverable.Id).ToListAsync();
                        deliverableInstance.DependentType = deliverable.DependentType;
                        deliverableInstance.Documents = await _context.Documents.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
                        deliverableInstance.EndDate = deliverable.EndDate;
                        deliverableInstance.Id = deliverable.Id;
                        deliverableInstance.IsPushedForApproval = deliverable.IsPushedForApproval;
                        deliverableInstance.DatePushedForApproval = deliverable.DatePushedForApproval;
                        deliverableInstance.DeclineReason = deliverable.DeclineReason;
                        deliverableInstance.IsActive = deliverable.IsActive;
                        deliverableInstance.Notes = await _context.PMNotes.Where(x =>x.DeliverableId == deliverable.Id).ToListAsync();
                        deliverableInstance.PMIllustrations = await _context.PMIllustrations.Where(x => x.IsActive == true && x.TaskOrDeliverableId == deliverable.Id).ToListAsync();
                        deliverableInstance.Pictures = await _context.Pictures.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
                        deliverableInstance.Requirements = await _context.PMRequirements.Where(x => x.DeliverableId == deliverable.Id && x.IsActive == true).ToListAsync();
                        deliverableInstance.StartDate = deliverable.StartDate;
                        deliverableInstance.StatusFlow = await _context.StatusFlows.FirstOrDefaultAsync(x=>x.Id == deliverable.StatusId && x.IsDeleted == false);
                        deliverableInstance.Task = await _context.Tasks.Where(x => x.IsActive == true && x.Id == deliverable.TaskId).Include(x => x.Project).FirstOrDefaultAsync();
                        deliverableInstance.TaskId = deliverable.TaskId;
                        deliverableInstance.TimeEstimate = deliverable.TimeEstimate;
                        deliverableInstance.UpdatedAt = deliverable.UpdatedAt;
                        deliverableInstance.Workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == deliverable.WorkspaceId);
                        deliverableInstance.UploadedRequirement = await _context.PMUploadedRequirements.Where(x => x.IsActive == true && x.DeliverableId == deliverableInstance.Id).ToListAsync();
                        deliverableInstance.Videos = await _context.Videos.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
                        deliverableInstance.userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == deliverable.CreatedById);

                        deliverableArray.Add(deliverableInstance);
                    }

                }
            

            return CommonResponse.Send(ResponseCodes.SUCCESS, deliverableArray, ResponseMessage.EntitySuccessfullyFound);

        }





        //public async Task<ApiCommonResponse> getCurrentDeliverableStatus(HttpContext httpContext,List<StatusFlow> statusFlows)
        //{

        //    var deliverableArray = new List<DeliverableWithStatusDTO>();
        //    foreach (var status in statusFlows)
        //    {

        //        var deliverablesStatuses = await _context.DeliverableStatuses.Where(x => x.IsDeleted == false && x.StatusId == status.Id).ToListAsync();


        //        if(deliverablesStatuses.Count > 0)
        //        {
        //            foreach(var deliverableStatus in deliverablesStatuses)
        //            {

        //                var deliverable = await _context.Deliverables.Where(x => x.IsActive == true && x.Id == deliverableStatus.DeliverableId).FirstOrDefaultAsync();

        //                var deliverableInstance = new DeliverableWithStatusDTO();
        //                deliverableInstance.Caption = deliverable.Caption;
        //                deliverableInstance.Alias = deliverable.Alias;
        //                deliverableInstance.Description = deliverable.Description;
        //                deliverableInstance.AssignTask = await _context.AssignTasks.FirstOrDefaultAsync(x => x.IsActive == true && x.DeliverableId == deliverable.Id);
        //                deliverableInstance.AssignTaskId = deliverable.AssignTaskId;
        //                deliverableInstance.Balances = await _context.Balances.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
        //                deliverableInstance.Budget = deliverable.Budget;
        //                deliverableInstance.CreatedAt = deliverable.CreatedAt;
        //                deliverableInstance.CreatedById = deliverable.CreatedById;
        //                deliverableInstance.DatePicked = deliverable.DatePicked;
        //                deliverableInstance.DeliverableAssignees = await _context.DeliverableAssignees.Where(x => x.IsActive == true && x.DeliverableId == deliverable.Id).ToListAsync();
        //                deliverableInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == deliverable.Id).ToListAsync();
        //                deliverableInstance.DependentType = deliverable.DependentType;
        //                deliverableInstance.Documents = await _context.Documents.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
        //                deliverableInstance.EndDate = deliverable.EndDate;
        //                deliverableInstance.Id = deliverable.Id;
        //                deliverableInstance.IsActive = deliverable.IsActive;
        //                deliverableInstance.Notes = deliverable.Notes;
        //                deliverableInstance.Pictures = await _context.Pictures.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
        //                deliverableInstance.Requirements = await _context.PMRequirements.Where(x => x.DeliverableId == deliverable.Id && x.IsActive == true).ToListAsync();
        //                deliverableInstance.StartDate = deliverable.StartDate;
        //                deliverableInstance.StatusFlow = status;
        //                deliverableInstance.Task = await _context.Tasks.Where(x => x.IsActive == true && x.Id == deliverable.TaskId).Include(x => x.Project).FirstOrDefaultAsync();
        //                deliverableInstance.TaskId = deliverable.TaskId;
        //                deliverableInstance.TimeEstimate = deliverable.TimeEstimate;
        //                deliverableInstance.UpdatedAt = deliverable.UpdatedAt;
        //                deliverableInstance.Videos = await _context.Videos.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
        //                deliverableInstance.userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == deliverable.CreatedById);

        //                deliverableArray.Add(deliverableInstance);
        //            }

        //        }


        //    }

        //    return CommonResponse.Send(ResponseCodes.SUCCESS, deliverableArray, ResponseMessage.EntitySuccessfullyFound);

        //}



        public async Task<ApiCommonResponse> getAssignedDeliverableStatus(HttpContext httpContext, List<DeliverableStatusDTO> deliverableStatusDTOs)
        {

            if (deliverableStatusDTOs.Count == 0)
            {
                return CommonResponse.Send
                (

                ResponseCodes.FAILURE,
                null,
                "No Assigned deliverables was provided.."
                );
            }

            else
            {
                var statusDToList = new List<StatusCategoryDTO>();
                var workspaceStatusList = new List<WorkspaceRoot>();
                foreach (var deliverable in deliverableStatusDTOs)
                {

                    var deliverableStatusInstance = new DeliverableStatus();
                    var statusDTO = new StatusCategoryDTO();
                    var workspaceStatus = new WorkspaceRoot();
                    var checkIfDeliverableBelongsToStatus = await _context.DeliverableStatuses.Where(x => x.IsDeleted == false && x.DeliverableId == deliverable.Id).FirstOrDefaultAsync();
                    if (checkIfDeliverableBelongsToStatus == null)
                    {
                        workspaceStatus.Caption = deliverable.Workspace.Caption;
                        workspaceStatus.Alias = deliverable.Workspace.Alias;
                        workspaceStatus.Description = deliverable.Workspace.Description;
                        workspaceStatus.Id = deliverable.Workspace.Id;
                        workspaceStatus.IsActive = deliverable.Workspace.IsActive;
                        workspaceStatus.IsPublic = deliverable.Workspace.IsPublic;
                        workspaceStatus.StatusFlowOption = deliverable.Workspace.StatusFlowOption;
                        var firstStatus = deliverable.statusFlows.First();
                        statusDTO.Caption = firstStatus.Caption;
                        statusDTO.CreatedAt = firstStatus.CreatedAt;
                        statusDTO.CreatedById = firstStatus.CreatedById;
                        statusDTO.Description = firstStatus.Description;
                        statusDTO.id = firstStatus.Id;
                        statusDTO.IsDeleted = firstStatus.IsDeleted;
                        statusDTO.LevelCount = firstStatus.LevelCount;
                        statusDTO.Panthone = firstStatus.Panthone;
                        //statusDTO.deliverableDTO = await getDeliverableDTO(httpContext,deliverable.Id);
                        statusDTO.deliverableDTO.Add(deliverable);
                        //workspaceStatus.StatusCategoryDTO.Add(statusDTO);
                        //statusDToList.Add(statusDTO);

                        deliverableStatusInstance.Version = 0;
                        deliverableStatusInstance.StatusId = statusDTO.id;
                        deliverableStatusInstance.IsDeleted = false;
                        deliverableStatusInstance.DeliverableId = deliverable.Id;
                        deliverableStatusInstance.CreatedAt = DateTime.Now;
                        deliverableStatusInstance.CreatedById = httpContext.GetLoggedInUserId();

                        await _context.DeliverableStatuses.AddAsync(deliverableStatusInstance);
                        await _context.SaveChangesAsync();

                    }

                    else
                    {

                        workspaceStatus.Caption = deliverable.Workspace.Caption;
                        workspaceStatus.Alias = deliverable.Workspace.Alias;
                        workspaceStatus.Description = deliverable.Workspace.Description;
                        workspaceStatus.Id = deliverable.Workspace.Id;
                        workspaceStatus.IsActive = deliverable.Workspace.IsActive;
                        workspaceStatus.IsPublic = deliverable.Workspace.IsPublic;
                        workspaceStatus.StatusFlowOption = deliverable.Workspace.StatusFlowOption;
                        var getStatus = await _context.StatusFlows.Where(x => x.IsDeleted == false && x.Id == checkIfDeliverableBelongsToStatus.StatusId).FirstOrDefaultAsync();
                        statusDTO.Caption = getStatus.Caption;
                        statusDTO.CreatedAt = getStatus.CreatedAt;
                        statusDTO.CreatedById = getStatus.CreatedById;
                        statusDTO.Description = getStatus.Description;
                        statusDTO.id = getStatus.Id;
                        statusDTO.IsDeleted = getStatus.IsDeleted;
                        statusDTO.LevelCount = getStatus.LevelCount;
                        statusDTO.Panthone = getStatus.Panthone;
                        statusDTO.deliverableDTO.Add(deliverable);
                        //workspaceStatus.StatusCategoryDTO.Add(statusDTO);
                        //statusDToList.Add(statusDTO);

                    }
                    workspaceStatusList.Add(workspaceStatus);
                }


                return CommonResponse.Send
                (

                ResponseCodes.SUCCESS,
                workspaceStatusList,
                ResponseMessage.EntitySuccessfullyFound
                );

            }

        }


        public async Task<ApiCommonResponse> getAssignedWorkspaceStatusDeliverable(HttpContext httpContext)
        {
            var getWorkspaceQuery = await _context.DeliverableStatuses.Where(x => x.IsDeleted == false).Include(x => x.Status).Include(x => x.Deliverable).ToListAsync();
            // var finalQuery = await getWorkspaceQuery.Where(x=>x.)
            return CommonResponse.Send
                (

                ResponseCodes.SUCCESS,
                getWorkspaceQuery,
                ResponseMessage.EntitySuccessfullyFound
                );
        }

        



      
       
      

        public async Task<List<StatusFlow>> getDeliverableStatusFlow(HttpContext httpContext, long? taskId)
        {
            var getProjectIdFromTask = await _context.Tasks.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == taskId);
            if(getProjectIdFromTask == null)
            {
                return null;
            }
            else
            {
                var getStatusFlows = new List<StatusFlow>();
                var getProject = await _context.Projects.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == getProjectIdFromTask.ProjectId);
                if(getProject != null)
                {
                    getStatusFlows = await _context.StatusFlows.Where(x => x.IsDeleted == false && x.WorkspaceId == getProject.WorkspaceId).ToListAsync();
                }

                return getStatusFlows;
            }
        }

        public async Task<Workspace> getDeliverableWorkspace(HttpContext httpContext, long? taskId)
        {
            var getProjectIdFromTask = await _context.Tasks.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == taskId);
            if (getProjectIdFromTask == null)
            {
                return null;
            }
            else
            {
                var getWorkspace = new Workspace();
                var getProject = await _context.Projects.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == getProjectIdFromTask.ProjectId);
                if (getProject != null)
                {
                    getWorkspace = await _context.Workspaces.Where(x => x.IsActive == true && x.Id == getProject.WorkspaceId).FirstOrDefaultAsync();
                }

                return getWorkspace;
            }
        }

        public async Task<ApiCommonResponse> addmoreStatus(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO)
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,currentStatus);

        }


        public async Task<ApiCommonResponse> moveStatusSequenec(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO)
        {


            var getCurrentWorkspaceStatus = await _context.StatusFlows.Where(x => x.IsDeleted == false && x.WorkspaceId == workspaceId).ToListAsync();

            if(getCurrentWorkspaceStatus.Count > 0)
            {
                foreach(var status in getCurrentWorkspaceStatus)
                {
                    status.IsDeleted = true;
                    _context.StatusFlows.Update(status);
                }

                _context.SaveChanges();
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
            return CommonResponse.Send(ResponseCodes.SUCCESS, currentStatus);

        }

        

        public async Task<ApiCommonResponse> createDefaultStatus(HttpContext httpContext, List<DefaultStatusDTO> defaultStatusFlows)
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,defaultStatusArray);

        }


        public async Task<ApiCommonResponse> updateStatusFlowOptionToDefault(HttpContext httpContext, long workspaceId)
        {

            var getWorkspace = await _context.Workspaces.Where(x => x.IsActive == true && x.Id == workspaceId && x.CreatedById == httpContext.GetLoggedInUserId()).FirstOrDefaultAsync();
            if(getWorkspace != null)
            {
                var getAllStatusFlowToDisable = await _context.StatusFlows.Where(x => x.IsDeleted == false && x.WorkspaceId == workspaceId).ToListAsync();
                if(getAllStatusFlowToDisable.Count > 0)
                {
                    foreach(var status in getAllStatusFlowToDisable)
                    {
                        status.IsDeleted = true;
                        _context.StatusFlows.Update(status);
                    }
                    _context.SaveChanges();
                }
            }

            var getDefaultStatus = await _context.DefaultStatusFlows.Where(x => x.IsDeleted == false).ToListAsync();
            if(getDefaultStatus.Count > 0)
            {
                var statusArray = new List<StatusFlow>();
                foreach (var defaultStatus in getDefaultStatus)
                {
                    var statusInstance = new StatusFlow();
                    statusInstance.Caption = defaultStatus.Caption;
                    statusInstance.CreatedAt = DateTime.Now;
                    statusInstance.CreatedById = httpContext.GetLoggedInUserId();
                    statusInstance.Description = defaultStatus.Description;
                    statusInstance.IsDeleted = false;
                    statusInstance.LevelCount = defaultStatus.LevelCount;
                    statusInstance.Panthone = defaultStatus.Panthone;
                    statusInstance.WorkspaceId = workspaceId;
                    statusArray.Add(statusInstance);
                }

                getWorkspace.StatusFlowOption = "Default";
                _context.Workspaces.Update(getWorkspace);
                await _context.StatusFlows.AddRangeAsync(statusArray);
                await _context.SaveChangesAsync();
            }

            var getCurrentWorkspaceStatus = await _context.StatusFlows.Where(x => x.IsDeleted == false && x.WorkspaceId == workspaceId).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS, getCurrentWorkspaceStatus, "Successfully converted to default status..");
        }

        public async Task<ApiCommonResponse> updateStatusFlowOptionToCustom(HttpContext httpContext,long workspaceId,List<StatusFlowDTO> statusFlowDTOs)
        {


            var getWorkspace = await _context.Workspaces.Where(x => x.IsActive == true && x.Id == workspaceId && x.CreatedById == httpContext.GetLoggedInUserId()).FirstOrDefaultAsync();
            if (getWorkspace != null)
            {
                var getAllStatusFlowToDisable = await _context.StatusFlows.Where(x => x.IsDeleted == false && x.WorkspaceId == workspaceId).ToListAsync();
                if (getAllStatusFlowToDisable.Count > 0)
                {
                    foreach (var status in getAllStatusFlowToDisable)
                    {
                        status.IsDeleted = true;
                        _context.StatusFlows.Update(status);
                    }
                    _context.SaveChanges();
                }
            }

            if (statusFlowDTOs.Count > 0)
            {
                var statusArray = new List<StatusFlow>();
                foreach (var customStatus in statusFlowDTOs)
                {
                    var statusInstance = new StatusFlow();
                    statusInstance.Caption = customStatus.Caption;
                    statusInstance.CreatedAt = DateTime.Now;
                    statusInstance.CreatedById = httpContext.GetLoggedInUserId();
                    statusInstance.Description = customStatus.Description;
                    statusInstance.IsDeleted = false;
                    statusInstance.LevelCount = customStatus.LevelCount;
                    statusInstance.Panthone = customStatus.Panthone;
                    statusInstance.WorkspaceId = workspaceId;
                    statusArray.Add(statusInstance);
                }

                getWorkspace.StatusFlowOption = "Custom";
                _context.Workspaces.Update(getWorkspace);
                await _context.StatusFlows.AddRangeAsync(statusArray);
                await _context.SaveChangesAsync();
            }

            var getCurrentWorkspaceStatus = await _context.StatusFlows.Where(x => x.IsDeleted == false && x.WorkspaceId == workspaceId).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS, getCurrentWorkspaceStatus, "Successfully converted to Custom status..");
        }


        public async Task<ApiCommonResponse> getAllDefaultStatus()

        {

            var allDefaultStatus = await _context.DefaultStatusFlows.Where(x => x.IsDeleted == false).ToListAsync();

            if (allDefaultStatus == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,allDefaultStatus);


        }


        public async Task<ApiCommonResponse> getAllProjects(HttpContext httpContext)

        {

            var getAllProjects = await _context.Projects.Where(x => x.IsActive == true &&  x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

            if (getAllProjects == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, getAllProjects);


        }


        public async Task<ApiCommonResponse> getAllProjectCreatorsWorkspace(HttpContext httpContext)

        {

            var getAllWorkspaces = await _context.Workspaces.Where(x => x.IsActive == true).ToListAsync();
            if(getAllWorkspaces.Count() == 0)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

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

                return CommonResponse.Send(ResponseCodes.SUCCESS, workspaceArray);
            }

        }


        public async Task<ApiCommonResponse> getAllDeliverables(HttpContext httpContext)

        {

            var getAllDeliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

            if (getAllDeliverables.Count() == 0)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            }
            else
            {

                var deliverableToDisplayArray = new List<Deliverable>();

                foreach (var item in getAllDeliverables)
                {
                    var deliverableToDisplayInstance = new Deliverable();
                    deliverableToDisplayInstance.Alias = item.Alias;
                    deliverableToDisplayInstance.Caption = item.Caption;
                    deliverableToDisplayInstance.Budget = item.Budget;
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

                return CommonResponse.Send(ResponseCodes.SUCCESS, deliverableToDisplayArray);

            }




        }


        public async Task<ApiCommonResponse> getAllDeliverablesByTaskId(HttpContext httpContext,long taskId)

        {

            var getAllDeliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == taskId && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

            if (getAllDeliverables == null || getAllDeliverables.Count() == 0)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            }
            else
            {

                var deliverableToDisplayArray = new List<DeliverableDTO>();

                foreach (var item in getAllDeliverables)
                {
                    var deliverableToDisplayInstance = new DeliverableDTO();
                    deliverableToDisplayInstance.Alias = item.Alias;
                    deliverableToDisplayInstance.Caption = item.Caption;
                    deliverableToDisplayInstance.Budget = item.Budget;
                    deliverableToDisplayInstance.Description = item.Description;
                    deliverableToDisplayInstance.CreatedById = httpContext.GetLoggedInUserId();
                    deliverableToDisplayInstance.DatePicked = item.DatePicked;
                    deliverableToDisplayInstance.DependentType = item.DependentType;
                    deliverableToDisplayInstance.EndDate = item.EndDate;
                    deliverableToDisplayInstance.Requirements = await _context.PMRequirements.Where(x => x.IsActive == true && x.DeliverableId == item.Id).ToListAsync();
                    deliverableToDisplayInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == item.Id).ToListAsync();
                    deliverableToDisplayInstance.Id = item.Id;
                    deliverableToDisplayInstance.TimeEstimate = item.TimeEstimate;
                    deliverableToDisplayInstance.StartDate = item.StartDate;
                    deliverableToDisplayInstance.PMIllustrations = await _context.PMIllustrations.Where(x => x.IsActive == true && x.TaskOrDeliverableId == item.Id).ToListAsync();
                    var getAssignTaskById = await _context.AssignTasks.FirstOrDefaultAsync(x => x.IsActive == true && x.DeliverableId == item.Id);
                    var assigneeToBeSaved = new AssignDeliverableDTO();
                    if (getAssignTaskById != null){
                        assigneeToBeSaved.Caption = getAssignTaskById.Caption;
                        assigneeToBeSaved.Alias = getAssignTaskById.Alias;
                        assigneeToBeSaved.CreatedAt = getAssignTaskById.CreatedAt;
                        assigneeToBeSaved.CreatedById = getAssignTaskById.CreatedById;
                        assigneeToBeSaved.DeliverableAssigneeId = getAssignTaskById.DeliverableAssigneeId;
                        assigneeToBeSaved.DeliverableUser = await getUser(assigneeToBeSaved.DeliverableAssigneeId, httpContext);
                        assigneeToBeSaved.DeliverableId = getAssignTaskById.DeliverableId;
                        assigneeToBeSaved.Description = getAssignTaskById.Description;
                        assigneeToBeSaved.DueDate = getAssignTaskById.DueDate;
                        assigneeToBeSaved.Id = getAssignTaskById.Id; 
                        assigneeToBeSaved.IsActive = getAssignTaskById.IsActive;
                        assigneeToBeSaved.Priority = getAssignTaskById.Priority;
                        assigneeToBeSaved.UpdatedAt = getAssignTaskById.UpdatedAt;
                    }

                    deliverableToDisplayInstance.AssignDeliverableDTO = assigneeToBeSaved;
                    deliverableToDisplayInstance.IsActive = item.IsActive;

                    deliverableToDisplayArray.Add(deliverableToDisplayInstance);

                }

                return CommonResponse.Send(ResponseCodes.SUCCESS, deliverableToDisplayArray);

            }


        }



        public async Task<ApiCommonResponse> getDeliverablesById(HttpContext httpContext, long id)

        {

            var deliverable = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == id);

            if (deliverable == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            }
            else
            {

                var deliverableInstance = new DeliverableWithStatusDTO();
                deliverableInstance.Caption = deliverable.Caption;
                deliverableInstance.Alias = deliverable.Alias;
                deliverableInstance.Description = deliverable.Description;
                deliverableInstance.AssignTask = await _context.AssignTasks.FirstOrDefaultAsync(x => x.IsActive == true && x.DeliverableId == deliverable.Id);
                deliverableInstance.AssignTaskId = deliverableInstance.AssignTask.DeliverableAssigneeId;
                deliverableInstance.Balances = await _context.Balances.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
                deliverableInstance.Budget = deliverable.Budget;
                deliverableInstance.IsPicked = deliverable.IsPicked;
                deliverableInstance.IsApproved = deliverable.IsApproved;
                deliverableInstance.IsDeclined = deliverable.IsDeclined;
                deliverableInstance.CreatedAt = deliverable.CreatedAt;
                deliverableInstance.CreatedBy = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == deliverable.CreatedById);
                deliverableInstance.CreatedById = deliverable.CreatedById;
                deliverableInstance.DatePicked = deliverable.DatePicked;
                deliverableInstance.DeliverableAssignees = await _context.DeliverableAssignees.Where(x => x.IsActive == true && x.DeliverableId == deliverable.Id).ToListAsync();
                deliverableInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == deliverable.Id).ToListAsync();
                deliverableInstance.DependentType = deliverable.DependentType;
                deliverableInstance.Documents = await _context.Documents.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
                deliverableInstance.EndDate = deliverable.EndDate;
                deliverableInstance.Id = deliverable.Id;
                deliverableInstance.IsPushedForApproval = deliverable.IsPushedForApproval;
                deliverableInstance.DatePushedForApproval = deliverable.DatePushedForApproval;
                deliverableInstance.DeclineReason = deliverable.DeclineReason;
                deliverableInstance.IsActive = deliverable.IsActive;
                deliverableInstance.Notes = await _context.PMNotes.Where(x => x.DeliverableId == deliverable.Id && x.IsActive == true).ToListAsync();
                deliverableInstance.PMIllustrations = await _context.PMIllustrations.Where(x => x.IsActive == true && x.TaskOrDeliverableId == deliverable.Id).ToListAsync();
                deliverableInstance.Pictures = await _context.Pictures.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
                deliverableInstance.Requirements = await _context.PMRequirements.Where(x => x.DeliverableId == deliverable.Id && x.IsActive == true).ToListAsync();
                deliverableInstance.StartDate = deliverable.StartDate;
                deliverableInstance.StatusFlow = await _context.StatusFlows.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == deliverable.StatusId);
                deliverableInstance.Task = await _context.Tasks.Where(x => x.IsActive == true && x.Id == deliverable.TaskId).Include(x => x.Project).FirstOrDefaultAsync();
                deliverableInstance.TaskId = deliverable.TaskId;
                deliverableInstance.TimeEstimate = deliverable.TimeEstimate;
                deliverableInstance.UpdatedAt = deliverable.UpdatedAt;
                deliverableInstance.UploadedRequirement = await _context.PMUploadedRequirements.Where(x => x.IsActive == true && x.DeliverableId == deliverableInstance.Id).ToListAsync();
                deliverableInstance.Videos = await _context.Videos.Where(x => x.DeliverableId == deliverable.Id).ToListAsync();
                deliverableInstance.AssignedTo = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == deliverableInstance.AssignTaskId);


                return CommonResponse.Send(ResponseCodes.SUCCESS, deliverableInstance);

            }


        }



        public async Task<DeliverableStatusDTO> getDeliverableDTO(HttpContext httpContext, long? id)

        {

            var getAllDeliverables = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == id);

            if (getAllDeliverables == null)
            {
                return null;
            }
            else
            {

                var deliverableToDisplayInstance = new DeliverableStatusDTO();
                deliverableToDisplayInstance.Alias = getAllDeliverables.Alias;
                deliverableToDisplayInstance.Budget = getAllDeliverables.Budget;
                deliverableToDisplayInstance.Caption = getAllDeliverables.Caption;
                deliverableToDisplayInstance.Description = getAllDeliverables.Description;
                deliverableToDisplayInstance.CreatedById = httpContext.GetLoggedInUserId();
                deliverableToDisplayInstance.DatePicked = getAllDeliverables.DatePicked;
                deliverableToDisplayInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == getAllDeliverables.Id && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                deliverableToDisplayInstance.DependentType = getAllDeliverables.DependentType;
                deliverableToDisplayInstance.EndDate = getAllDeliverables.EndDate;
                deliverableToDisplayInstance.Id = getAllDeliverables.Id;
                deliverableToDisplayInstance.TimeEstimate = getAllDeliverables.TimeEstimate;
                deliverableToDisplayInstance.Task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == getAllDeliverables.TaskId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true);
                deliverableToDisplayInstance.StartDate = getAllDeliverables.StartDate;
                deliverableToDisplayInstance.Requirements = await _context.PMRequirements.Where(x => x.DeliverableId == getAllDeliverables.Id && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == false).ToListAsync();
                deliverableToDisplayInstance.AssignDeliverable = await _context.AssignTasks.Where(x => x.IsActive == true && x.DeliverableId == getAllDeliverables.Id).FirstOrDefaultAsync();
            
                deliverableToDisplayInstance.IsActive = getAllDeliverables.IsActive;


                return deliverableToDisplayInstance;
            }


        }



        public async Task<ApiCommonResponse> getWorkByProjectCreatorId(HttpContext httpContext)

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
                    return CommonResponse.Send(ResponseCodes.SUCCESS, projectCreatorList);
                }

                return CommonResponse.Send(ResponseCodes.SUCCESS, projectCreatorList);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, projectCreatorList);
        }


        public async Task<ApiCommonResponse> getWatchersByProjectId(HttpContext httpContext, long projectId)

        {

            var getWatchersByProjectId = await _context.Watchers.Where(x => x.IsActive == true  && x.ProjectId == projectId).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS, getWatchersByProjectId);
        }


        public async Task<ApiCommonResponse> getProjectByProjectName(HttpContext httpContext,string projectName)

        {

            var getProjectByCaption = await _context.Projects.FirstOrDefaultAsync(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId() && x.Caption == projectName);

            if (getProjectByCaption == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, getProjectByCaption);


        }


        public async Task<ApiCommonResponse> createNewDeliverableFromTask(HttpContext httpContext, long TaskId, DeliverableDTO deliverableDTO)

        {

            var getAvailableTask = await _context.Tasks.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId() && x.Id == TaskId).ToListAsync();

            if (getAvailableTask == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

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

                if (deliverableDTO.Requirements.Count() > 0)
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

                if (deliverableDTO.Dependencies.Count() > 0)
                {
                    var dependyArray = new List<Dependency>();
                    foreach (var item in deliverableDTO.Dependencies)
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

                var getAllDeliverables = await _context.Deliverables.Where(x => x.TaskId == TaskId && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
                //var deliverableToDisplayArray = new List<DeliverableDTO>();

                //foreach (var item in getAllDeliverables)
                //{
                //    var deliverableToDisplayInstance = new DeliverableDTO();
                //    deliverableToDisplayInstance.Alias = item.Alias;
                //    deliverableToDisplayInstance.Caption = item.Caption;
                //    deliverableToDisplayInstance.Budget = item.Budget;
                //    deliverableToDisplayInstance.Description = item.Description;
                //    deliverableToDisplayInstance.CreatedById = httpContext.GetLoggedInUserId();
                //    deliverableToDisplayInstance.DatePicked = item.DatePicked;
                //    deliverableToDisplayInstance.DependentType = item.DependentType;
                //    deliverableToDisplayInstance.EndDate = item.EndDate;
                //    deliverableToDisplayInstance.Requirements = await _context.PMRequirements.Where(x => x.IsActive == true && x.DeliverableId == item.Id).ToListAsync();
                //    deliverableToDisplayInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == item.Id).ToListAsync();
                //    deliverableToDisplayInstance.Id = item.Id;
                //    deliverableToDisplayInstance.TimeEstimate = item.TimeEstimate;
                //    deliverableToDisplayInstance.StartDate = item.StartDate;
                //    var getAssignTaskById = await _context.AssignTasks.FirstOrDefaultAsync(x => x.IsActive == true && x.DeliverableId == item.Id);
                //    var assigneeToBeSaved = new AssignDeliverableDTO();
                //    if (getAssignTaskById != null)
                //    {
                //        assigneeToBeSaved.Caption = getAssignTaskById.Caption;
                //        assigneeToBeSaved.Alias = getAssignTaskById.Alias;
                //        assigneeToBeSaved.CreatedAt = getAssignTaskById.CreatedAt;
                //        assigneeToBeSaved.CreatedById = getAssignTaskById.CreatedById;
                //        assigneeToBeSaved.DeliverableAssigneeId = getAssignTaskById.DeliverableAssigneeId;
                //        assigneeToBeSaved.DeliverableUser = await getUser(assigneeToBeSaved.DeliverableAssigneeId, httpContext);
                //        assigneeToBeSaved.DeliverableId = getAssignTaskById.DeliverableId;
                //        assigneeToBeSaved.Description = getAssignTaskById.Description;
                //        assigneeToBeSaved.DueDate = getAssignTaskById.DueDate;
                //        assigneeToBeSaved.Id = getAssignTaskById.Id;
                //        assigneeToBeSaved.IsActive = getAssignTaskById.IsActive;
                //        assigneeToBeSaved.Priority = getAssignTaskById.Priority;
                //        assigneeToBeSaved.UpdatedAt = getAssignTaskById.UpdatedAt;
                //    }

                //    deliverableToDisplayInstance.AssignDeliverableDTO = assigneeToBeSaved;
                //    deliverableToDisplayInstance.IsActive = item.IsActive;

                //    deliverableToDisplayArray.Add(deliverableToDisplayInstance);

                //}

                return CommonResponse.Send(ResponseCodes.SUCCESS, getAllDeliverables);

            }


        }

        public async Task<ApiCommonResponse> updateDeliverable(HttpContext httpContext, long taskId,long deliverableId,DeliverableDTO deliverableDTO)
        {
            var getDeliverable = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.TaskId == taskId && x.Id == deliverableId);
            if (getDeliverable == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            }

            else
            {

                if (deliverableDTO.Caption == null)
                    deliverableDTO.Caption = getDeliverable.Caption;
                if (deliverableDTO.Alias == null)
                    deliverableDTO.Alias = getDeliverable.Alias;
                if (deliverableDTO.Description == null)
                    deliverableDTO.Description = getDeliverable.Description;
                if (deliverableDTO.Budget == 0)
                    deliverableDTO.Budget = getDeliverable.Budget;
                if (deliverableDTO.StartDate == DateTime.MinValue)
                    deliverableDTO.StartDate = getDeliverable.StartDate;
                if (deliverableDTO.EndDate == DateTime.MinValue)
                    deliverableDTO.EndDate = getDeliverable.EndDate;

                getDeliverable.Caption = deliverableDTO.Caption;
                getDeliverable.Alias = deliverableDTO.Alias;
                getDeliverable.Description = deliverableDTO.Description;
                getDeliverable.Budget = deliverableDTO.Budget;
                getDeliverable.StartDate = deliverableDTO.StartDate;
                getDeliverable.EndDate = deliverableDTO.EndDate;

                _context.Deliverables.Update(getDeliverable);
                await _context.SaveChangesAsync();
                var updatedResult = await _context.Deliverables.Where(x => x.TaskId == taskId && x.IsActive == true  && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();


                return CommonResponse.Send(ResponseCodes.SUCCESS, updatedResult);


            }

        }


        public async Task<ApiCommonResponse> disableDeliverable(HttpContext httpContext, long taskId, long deliverableId)
        {
            var getDeliverable = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == taskId && x.Id == deliverableId).FirstOrDefaultAsync();
            if (getDeliverable == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            }

            else
            {

                getDeliverable.IsActive = false;

                _context.Deliverables.Update(getDeliverable);
                await _context.SaveChangesAsync();
                var updatedResult = await _context.Deliverables.Where(x => x.TaskId == taskId  && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();


                return CommonResponse.Send(ResponseCodes.SUCCESS, updatedResult);


            }

        }





        public async Task<ApiCommonResponse> createNewDeliverable(HttpContext httpContext, long TaskId,DeliverableDTO deliverableDTO)

        {

            var getAvailableTask = await _context.Tasks.Where(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId() && x.Id == TaskId).ToListAsync();
           
            if (getAvailableTask == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

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

                var updatedResult = await _context.Deliverables.Where(x => x.TaskId == TaskId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();


                return CommonResponse.Send(ResponseCodes.SUCCESS, updatedResult);

            }


        }



        public async Task<ApiCommonResponse> addMoreTaskAssignees(HttpContext context, long taskId,  List<TaskAssigneeDTO> taskAssigneeDTO)
        {
            var getTask = await _context.Tasks.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == taskId);
            if (getTask == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            }



            else if(getTask != null)
            {
                foreach (var existing in taskAssigneeDTO)
                {
                    var findAssignee = await _context.TaskAssignees.FirstOrDefaultAsync(x => x.TaskAssigneeId == existing.TaskAssigneeId && x.TaskId == existing.TaskId && x.IsActive == true);
                    if (findAssignee != null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Task assignee already exist");

                    }
                }
                var asigneeArray = new List<TaskAssignee>();
                foreach(var assign in taskAssigneeDTO) {
                    var assignee = new TaskAssignee();
                    assignee.TaskId = taskId;
                    assignee.Name = assign.Name;
                    assignee.IsActive = true;
                    assignee.TaskAssigneeId = assign.TaskAssigneeId;
                    assignee.CreatedById = context.GetLoggedInUserId();
                    assignee.CreatedAt = DateTime.Now;
                    assignee.UserImage = assign.ProfileImage;

                    asigneeArray.Add(assignee);
                }

                await _context.AddRangeAsync(asigneeArray);
                await _context.SaveChangesAsync();

            }

            var getUpdatedAssigneeById = await _context.TaskAssignees.Where(x => x.IsActive == true && x.TaskId == taskId).ToListAsync();
            var distinctAssigneeId = getUpdatedAssigneeById.GroupBy(x => x.TaskAssigneeId)
                        .Select(g => g.First())
                        .ToList();
            return CommonResponse.Send(ResponseCodes.SUCCESS, distinctAssigneeId);



        }

        public async Task<ApiCommonResponse> disableTaskAssignee(HttpContext context, long taskId, long assigneeId)
        {
            var getAssignee = await _context.TaskAssignees.FirstOrDefaultAsync(x=>x.TaskAssigneeId == assigneeId && x.TaskId == taskId && x.IsActive == true);
            if(getAssignee == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            }
            else
            {
                getAssignee.IsActive = false;
                _context.TaskAssignees.Update(getAssignee);
                await _context.SaveChangesAsync();

                var getUpdatedAssigneeById = await _context.TaskAssignees.Where(x => x.IsActive == true && x.TaskId == taskId).ToListAsync();
                var getDistinctUpdatedAssignee = getUpdatedAssigneeById.GroupBy(x => x.TaskAssigneeId)
                        .Select(g => g.First())
                        .ToList();

                return CommonResponse.Send(ResponseCodes.SUCCESS, getUpdatedAssigneeById);
            }
        }





        public async Task<ApiCommonResponse> createNewTask(HttpContext context,long projectId ,TaskDTO taskDTO)
        {
            var taskSummaryList = new List<TaskSummaryDTO>();
            var ProjectExist = await _context.Projects.FirstOrDefaultAsync(x => x.IsActive == true && x.CreatedById == context.GetLoggedInUserId() && x.Id == projectId);
            if(ProjectExist == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
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
                            CreatedAt = DateTime.Now,
                            UserImage = item.ProfileImage,
                        };

                        taskAssigneeArray.Add(taskAssigneeInstance);
                    }

                    await _context.TaskAssignees.AddRangeAsync(taskAssigneeArray);
                    await _context.SaveChangesAsync();
                    var getUpdatedList = await _context.Tasks.Where(x => x.ProjectId == projectId && x.CreatedById == context.GetLoggedInUserId()).ToListAsync();
                    return CommonResponse.Send(ResponseCodes.SUCCESS, getUpdatedList);

                    //foreach (var item in getUpdatedList)
                    //{
                    //    var taskSummary = new TaskSummaryDTO();
                    //    taskSummary.Alias = item.Alias;
                    //    taskSummary.Caption = item.Caption;
                    //    taskSummary.CreatedAt = item.CreatedAt;
                    //    taskSummary.CreatedById = item.CreatedById;
                    //    taskSummary.Description = item.Description;
                    //    taskSummary.IsAssigned = item.IsAssigned;
                    //    taskSummary.IsMilestone = item.IsMilestone;
                    //    taskSummary.IsReassigned = item.IsReassigned;
                    //    taskSummary.IsWorkbenched = item.IsWorkbenched;
                    //    taskSummary.ProjectId = item.ProjectId;
                    //    taskSummary.TaskEndDate = item.TaskEndDate;
                    //    taskSummary.TaskStartDate = item.TaskStartDate;
                    //    taskSummary.Project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == item.ProjectId && x.IsActive == true);
                    //    taskSummary.TaskAssignees = await _context.TaskAssignees.Where(X => X.TaskId == item.Id && X.IsActive == true).ToListAsync();
                    //    taskSummary.workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == taskSummary.Project.WorkspaceId && x.IsActive == true);
                    //    taskSummaryList.Add(taskSummary);
                    //}

                }
                return CommonResponse.Send(ResponseCodes.FAILURE, null,ResponseMessage.EntityNotFound);
            }
               
        }

        public async Task<ApiCommonResponse> AssignDeliverable(HttpContext context, long taskId,long deliverableId, long assigneDeliverableId,  AssignDeliverableDTO assignDeliverableDTO)
        {
           
               var getAssigndDeliverable = await _context.AssignTasks.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == assigneDeliverableId && x.DeliverableId == deliverableId);
           

                if (assigneDeliverableId != 0)
                {

                    if (assignDeliverableDTO.DeliverableAssigneeId == null)
                        assignDeliverableDTO.DeliverableAssigneeId = getAssigndDeliverable.DeliverableAssigneeId;
                    if (assignDeliverableDTO.Priority == null)
                        assignDeliverableDTO.Priority = getAssigndDeliverable.Priority;

                    var getStatuses = await _context.Deliverables.Where(x => x.Id == deliverableId && x.IsActive == true)
                                                 .Include(x => x.Task)
                                                 .ThenInclude(x => x.Project)
                                                 .ThenInclude(x => x.Workspace)
                                                 .ThenInclude(x => x.StatusFlows)
                                                 .FirstOrDefaultAsync();


                    var deliverableToBeUpdated = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == deliverableId);

                    var deliverable = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == deliverableId);
                    deliverable.IsAssigned = true;
                    deliverable.WorkspaceId = getStatuses.Task.Project.WorkspaceId;
                    deliverable.StatusId = getStatuses.Task.Project.Workspace.StatusFlows.First().Id;
                    _context.Deliverables.Update(deliverable);
             
                    getAssigndDeliverable.Priority = assignDeliverableDTO.Priority;
                    getAssigndDeliverable.DeliverableAssigneeId = assignDeliverableDTO.DeliverableAssigneeId;
                    _context.AssignTasks.Update(getAssigndDeliverable);
                    await _context.SaveChangesAsync();

            }

               else
                {
                    var deliverableToBeAssigned = new AssignTask();

                    if(assignDeliverableDTO.DeliverableAssigneeId != 0)
                    {
                    deliverableToBeAssigned.IsActive = true;
                    deliverableToBeAssigned.Caption = assignDeliverableDTO.Caption;
                    deliverableToBeAssigned.Alias = assignDeliverableDTO.Alias;
                    deliverableToBeAssigned.Description = assignDeliverableDTO.Description;
                    deliverableToBeAssigned.Priority = assignDeliverableDTO.Priority;
                    deliverableToBeAssigned.DeliverableId = deliverableId;
                    deliverableToBeAssigned.DeliverableAssigneeId = assignDeliverableDTO.DeliverableAssigneeId;
                    deliverableToBeAssigned.DueDate = assignDeliverableDTO.DueDate;
                    deliverableToBeAssigned.CreatedById = context.GetLoggedInUserId();
                    deliverableToBeAssigned.CreatedAt = DateTime.Now;
                    



                    var getStatuses = await _context.Deliverables.Where(x => x.Id == deliverableId && x.IsActive == true)
                                                     .Include(x => x.Task)
                                                     .ThenInclude(x => x.Project)
                                                     .ThenInclude(x => x.Workspace)
                                                     .ThenInclude(x => x.StatusFlows)
                                                     .FirstOrDefaultAsync();


                    var deliverableToBeUpdated = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == deliverableId);

                    deliverableToBeUpdated.WorkspaceId = getStatuses.Task.Project.WorkspaceId;
                    deliverableToBeUpdated.StatusId = getStatuses.Task.Project.Workspace.StatusFlows.First().Id;
                    deliverableToBeUpdated.IsAssigned = true;
                    _context.Deliverables.Update(deliverableToBeUpdated);
                    await _context.SaveChangesAsync();

                }

                else
                {
                    deliverableToBeAssigned.IsActive = true;
                    deliverableToBeAssigned.Caption = assignDeliverableDTO.Caption;
                    deliverableToBeAssigned.Alias = assignDeliverableDTO.Alias;
                    deliverableToBeAssigned.Description = assignDeliverableDTO.Description;
                    deliverableToBeAssigned.Priority = assignDeliverableDTO.Priority;
                    deliverableToBeAssigned.DeliverableId = assignDeliverableDTO.DeliverableId;
                    deliverableToBeAssigned.DeliverableAssigneeId = assignDeliverableDTO.DeliverableAssigneeId;
                    deliverableToBeAssigned.DueDate = assignDeliverableDTO.DueDate;
                    deliverableToBeAssigned.CreatedById = context.GetLoggedInUserId();
                    deliverableToBeAssigned.CreatedAt = DateTime.Now;

                }

                    
                    await _context.AssignTasks.AddAsync(deliverableToBeAssigned);
                    await _context.SaveChangesAsync();
            }

                    var getAllDeliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == taskId  && x.CreatedById == context.GetLoggedInUserId()).ToListAsync();



            return CommonResponse.Send(ResponseCodes.SUCCESS,  getAllDeliverables, ResponseMessage.EntitySuccessfullySaved);
            
        }






        public async Task<ApiCommonResponse> createDeliverableIllustrattions(HttpContext context, long deliverableId,long taskId, IllustrationsDTO illustrationsDTO)
        {
            
            var getCurrentDeliverable = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true  && x.Id == deliverableId);


            if (getCurrentDeliverable == null)
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            else
            {
                
                    var illustration = new PMIllustration();
                    illustration.Alias = illustrationsDTO.Alias;
                    illustration.Caption = illustrationsDTO.Caption;
                    illustration.CreatedAt = DateTime.Now;
                    illustration.CreatedById = context.GetLoggedInUserId();
                    illustration.Description = illustrationsDTO.Description;
                    illustration.IllustrationImage = illustrationsDTO.IllustrationImage;
                    illustration.IsActive = true;
                    illustration.TaskOrDeliverableId = deliverableId;
                    //illustrationArray.Add(illustration);
                
                await _context.PMIllustrations.AddAsync(illustration);
                await _context.SaveChangesAsync();
            }


            var getAllDeliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == taskId && x.CreatedById == context.GetLoggedInUserId()).ToListAsync();
            //var deliverableToDisplayArray = new List<DeliverableDTO>();

            //foreach (var item in getAllDeliverables)
            //{
            //    var deliverableToDisplayInstance = new DeliverableDTO();
            //    deliverableToDisplayInstance.Alias = item.Alias;
            //    deliverableToDisplayInstance.Caption = item.Caption;
            //    deliverableToDisplayInstance.Budget = item.Budget;
            //    deliverableToDisplayInstance.Description = item.Description;
            //    deliverableToDisplayInstance.CreatedById = context.GetLoggedInUserId();
            //    deliverableToDisplayInstance.DatePicked = item.DatePicked;
            //    deliverableToDisplayInstance.DependentType = item.DependentType;
            //    deliverableToDisplayInstance.EndDate = item.EndDate;
            //    deliverableToDisplayInstance.Requirements = await _context.PMRequirements.Where(x => x.IsActive == true && x.DeliverableId == item.Id).ToListAsync();
            //    deliverableToDisplayInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == item.Id).ToListAsync();
            //    deliverableToDisplayInstance.Id = item.Id;
            //    deliverableToDisplayInstance.TimeEstimate = item.TimeEstimate;
            //    deliverableToDisplayInstance.StartDate = item.StartDate;
            //    var getAssignTaskById = await _context.AssignTasks.FirstOrDefaultAsync(x => x.IsActive == true && x.DeliverableId == item.Id);
            //    deliverableToDisplayInstance.PMIllustrations = await _context.PMIllustrations.Where(x => x.IsActive == true && x.TaskOrDeliverableId == deliverableId).ToListAsync();
            //    var assigneeToBeSaved = new AssignDeliverableDTO();
            //    if (getAssignTaskById != null)
            //    {
            //        assigneeToBeSaved.Caption = getAssignTaskById.Caption;
            //        assigneeToBeSaved.Alias = getAssignTaskById.Alias;
            //        assigneeToBeSaved.CreatedAt = getAssignTaskById.CreatedAt;
            //        assigneeToBeSaved.CreatedById = getAssignTaskById.CreatedById;
            //        assigneeToBeSaved.DeliverableAssigneeId = getAssignTaskById.DeliverableAssigneeId;
            //        assigneeToBeSaved.DeliverableUser = await getUser(assigneeToBeSaved.DeliverableAssigneeId, context);
            //        assigneeToBeSaved.DeliverableId = getAssignTaskById.DeliverableId;
            //        assigneeToBeSaved.Description = getAssignTaskById.Description;
            //        assigneeToBeSaved.DueDate = getAssignTaskById.DueDate;
            //        assigneeToBeSaved.Id = getAssignTaskById.Id;
            //        assigneeToBeSaved.IsActive = getAssignTaskById.IsActive;
            //        assigneeToBeSaved.Priority = getAssignTaskById.Priority;
            //        assigneeToBeSaved.UpdatedAt = getAssignTaskById.UpdatedAt;
            //    }

            //    deliverableToDisplayInstance.AssignDeliverableDTO = assigneeToBeSaved;
            //    deliverableToDisplayInstance.IsActive = item.IsActive;

            //    deliverableToDisplayArray.Add(deliverableToDisplayInstance);

            //}

            return CommonResponse.Send(ResponseCodes.SUCCESS, getAllDeliverables);


        }





        public async Task<ApiCommonResponse> DeleteIllustration(HttpContext context, long taskId,long deliverableId ,long illustrationId)
        {

            var getCurrentDeliverable = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == deliverableId);


            if (getCurrentDeliverable == null)
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);



            else
            {
                var illustrationInstance = await _context.PMIllustrations.FirstOrDefaultAsync(x => x.Id == illustrationId && x.IsActive == true && x.TaskOrDeliverableId == deliverableId);
                illustrationInstance.IsActive = false;
                _context.PMIllustrations.Update(illustrationInstance);
                await _context.SaveChangesAsync();
            }


            var getAllDeliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == taskId && x.CreatedById == context.GetLoggedInUserId()).ToListAsync();
            //var deliverableToDisplayArray = new List<DeliverableDTO>();

            //foreach (var item in getAllDeliverables)
            //{
            //    var deliverableToDisplayInstance = new DeliverableDTO();
            //    deliverableToDisplayInstance.Alias = item.Alias;
            //    deliverableToDisplayInstance.Caption = item.Caption;
            //    deliverableToDisplayInstance.Budget = item.Budget;
            //    deliverableToDisplayInstance.Description = item.Description;
            //    deliverableToDisplayInstance.CreatedById = context.GetLoggedInUserId();
            //    deliverableToDisplayInstance.DatePicked = item.DatePicked;
            //    deliverableToDisplayInstance.DependentType = item.DependentType;
            //    deliverableToDisplayInstance.Requirements = await _context.PMRequirements.Where(x => x.IsActive == true && x.DeliverableId == item.Id).ToListAsync();
            //    deliverableToDisplayInstance.Dependencies = await _context.Dependencies.Where(x => x.DependencyDeliverableId == item.Id).ToListAsync();
            //    deliverableToDisplayInstance.EndDate = item.EndDate;
            //    deliverableToDisplayInstance.Id = item.Id;
            //    deliverableToDisplayInstance.TimeEstimate = item.TimeEstimate;
            //    deliverableToDisplayInstance.StartDate = item.StartDate;
            //    var getAssignTaskById = await _context.AssignTasks.FirstOrDefaultAsync(x => x.IsActive == true && x.DeliverableId == item.Id);
            //    deliverableToDisplayInstance.PMIllustrations = await _context.PMIllustrations.Where(x => x.IsActive == true && x.TaskOrDeliverableId == deliverableId).ToListAsync();
            //    var assigneeToBeSaved = new AssignDeliverableDTO();
            //    if (getAssignTaskById != null)
            //    {
            //        assigneeToBeSaved.Caption = getAssignTaskById.Caption;
            //        assigneeToBeSaved.Alias = getAssignTaskById.Alias;
            //        assigneeToBeSaved.CreatedAt = getAssignTaskById.CreatedAt;
            //        assigneeToBeSaved.CreatedById = getAssignTaskById.CreatedById;
            //        assigneeToBeSaved.DeliverableAssigneeId = getAssignTaskById.DeliverableAssigneeId;
            //        assigneeToBeSaved.DeliverableUser = await getUser(assigneeToBeSaved.DeliverableAssigneeId, context);
            //        assigneeToBeSaved.DeliverableId = getAssignTaskById.DeliverableId;
            //        assigneeToBeSaved.Description = getAssignTaskById.Description;
            //        assigneeToBeSaved.DueDate = getAssignTaskById.DueDate;
            //        assigneeToBeSaved.Id = getAssignTaskById.Id;
            //        assigneeToBeSaved.IsActive = getAssignTaskById.IsActive;
            //        assigneeToBeSaved.Priority = getAssignTaskById.Priority;
            //        assigneeToBeSaved.UpdatedAt = getAssignTaskById.UpdatedAt;
            //    }

            //    deliverableToDisplayInstance.AssignDeliverableDTO = assigneeToBeSaved;
            //    deliverableToDisplayInstance.IsActive = item.IsActive;

            //    deliverableToDisplayArray.Add(deliverableToDisplayInstance);

            //}

            return CommonResponse.Send(ResponseCodes.SUCCESS, getAllDeliverables);


        }





        public async Task<ApiCommonResponse> getAllTask(HttpContext httpContext)
        {
            var taskSummaryList = new List<TaskSummaryDTO>();
            var getAllTask = await _context.Tasks.Where(x => x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
           if(getAllTask == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

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

                return CommonResponse.Send(ResponseCodes.SUCCESS, taskSummaryList);
            }

        }





        public async Task<ApiCommonResponse> getProjectByWorkspaceId(HttpContext httpContext, long workspaceId)
        {
            var getProjectsByWorkspaceId = await _context.Projects.Where(x => x.WorkspaceId == workspaceId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true).ToListAsync();
            var ProjectArr = new List<ProjectSummaryDTO>();
            if (getProjectsByWorkspaceId.Count() == 0)
            {

                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
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

                return CommonResponse.Send(ResponseCodes.SUCCESS, ProjectArr);
            }
        }




        public async Task<ApiCommonResponse> pickUptask(long taskId,HttpContext httpContext)
        {
            var taskToBePicked = await _context.Tasks.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == taskId);
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

            return CommonResponse.Send(ResponseCodes.SUCCESS, taskArray);

        }

        public async Task<ApiCommonResponse> getManagersProjects(string email,int emailId)
       
        {

            var getProjects = await _projectAllocationRepository.getAllManagerProjects(email,emailId);

            if (getProjects == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,getProjects);


        }

        public async Task<ApiCommonResponse> getRequirementsByDeliverableId(HttpContext httpContext, long deliverableId)
        {
            var gottenRequirements = await _context.PMRequirements.Where(x => x.DeliverableId == deliverableId && x.CreatedById == httpContext.GetLoggedInUserId() & x.IsActive == true).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS, gottenRequirements);
        }

        public async Task<ApiCommonResponse> createProject(HttpContext httpContext, ProjectDTO projectDTO)
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
            return CommonResponse.Send(ResponseCodes.SUCCESS, getAllProjects);

        }


        public async Task<ApiCommonResponse> getProjectById(HttpContext httpContext, long projectId)
        {
            var projectQuery = await _context.Projects.Where(x => x.Id == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId())
                                                               .FirstOrDefaultAsync();
            var projects = new List<Project>();
            if (projectQuery != null)
            {

                var projectinstance = new Project
                {
                    Alias = projectQuery.Alias,
                    Caption = projectQuery.Caption,
                    Description = projectQuery.Description,
                    CreatedAt = projectQuery.CreatedAt,
                    IsActive = projectQuery.IsActive,
                    Id = projectQuery.Id,
                    WorkspaceId = projectQuery.WorkspaceId,
                    Watchers = await _context.Watchers.Where(x => x.IsActive == true && x.ProjectId == projectId).ToListAsync(),
                    Tasks = await _context.Tasks.Where(x => x.IsActive == true && x.ProjectId == projectId).ToListAsync(),
                    Workspace = await _context.Workspaces.Where(x => x.IsActive == true && x.Id == projectQuery.WorkspaceId).FirstOrDefaultAsync(),
                    ProjectImage = projectQuery.ProjectImage,
                    CreatedById = projectQuery.CreatedById,
                    CreatedBy = await _context.UserProfiles.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == projectQuery.CreatedById),
                    UpdatedAt = projectQuery.UpdatedAt,
                };

                return CommonResponse.Send(ResponseCodes.SUCCESS, projectinstance, "Project successfully retrieved");
            }
            else
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Project retrieved successfully but currently empty");
            }

        }

        public async Task<ApiCommonResponse> addmoreWatchers(HttpContext httpContext, long projectId, List<WatchersDTO> watchersDTOs)
        {
            var getProject = await _context.Projects.FirstOrDefaultAsync(x => x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());
            var watcherToBeSaved = new List<Watcher>();
            if(getProject != null)
            {

                foreach(var watchers in watchersDTOs)
                {
                    var checkIfWatcherExist = await _context.Watchers.FirstOrDefaultAsync(x => x.IsActive == true && x.ProjectId == projectId && x.ProjectWatcherId == watchers.ProjectWatcherId);
                    if(checkIfWatcherExist != null)
                    {
                        continue;
                    }
                    else
                    {
                        var watcherInstance = new Watcher();
                        watcherInstance.CreatedAt = watchers.CreatedAt;
                        watcherInstance.CreatedBy = await _context.UserProfiles.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == watchers.ProjectWatcherId);
                        watcherInstance.CreatedById = watchers.CreatedById;
                        watcherInstance.IsActive = true;
                        watcherInstance.ProjectId = projectId;

                        watcherInstance.ProjectWatcherId = watchers.ProjectWatcherId;
                        watcherInstance.UpdatedAt = watchers.UpdatedAt;
                        watcherToBeSaved.Add(watcherInstance);
                    }

                }

                if (watcherToBeSaved.Count == 0)
                {

                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Watchers could not be added because one or more already exist.");
                }
                else
                {
                    await _context.Watchers.AddRangeAsync(watcherToBeSaved);
                    await _context.SaveChangesAsync();
                }

                var watcherResult = await _context.Watchers.Where(x => x.IsActive == true && x.ProjectId == projectId).ToListAsync();

                return CommonResponse.Send(ResponseCodes.SUCCESS, watcherResult, "Watchers was successfully added.");

            }

            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Project Id does not exist");
        }

        //public async Task<ApiCommonResponse> addmoreWatchers(HttpContext httpContext, long projectId, List<WatchersDTO> watchersDTOs)
        //{
        //    var gottenProject = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId());

        //    var watchersArray = new List<Watcher>();
        //    var getCurrentWatchers = await _context.Watchers.Where(x => x.ProjectId == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
        //    if (getCurrentWatchers.Count() == 0)
        //    {

        //        foreach (var item in watchersDTOs)
        //        {
        //            var watchersInstance = new Watcher();
        //            watchersInstance.CreatedById = httpContext.GetLoggedInUserId();
        //            watchersInstance.ProjectWatcherId = item.ProjectWatcherId;
        //            watchersInstance.ProjectId = item.ProjectId;
        //            watchersInstance.IsActive = true;
        //            watchersInstance.CreatedAt = DateTime.Now;

        //            watchersArray.Add(watchersInstance);

        //        }

        //    }
        //    else
        //    {

        //        foreach (var item in watchersDTOs)
        //        {

        //            var getExistingWatcher = await _context.Watchers.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId() && x.ProjectWatcherId == item.ProjectWatcherId);

        //            if (getExistingWatcher == null)
        //            {

        //                var watchersInstance = new Watcher();
        //                watchersInstance.CreatedById = httpContext.GetLoggedInUserId();
        //                watchersInstance.ProjectWatcherId = item.ProjectWatcherId;
        //                watchersInstance.ProjectId = item.ProjectId;
        //                watchersInstance.IsActive = true;
        //                watchersInstance.CreatedAt = DateTime.Now;

        //                watchersArray.Add(watchersInstance);
        //            }

        //        }

        //    }

        //    await _context.Watchers.AddRangeAsync(watchersArray);
        //    await _context.SaveChangesAsync();
        //    var getUpdatedWatchers = await _context.Watchers.Where(x => x.ProjectId == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

        //    return  CommonResponse.Send(ResponseCodes.SUCCESS, getUpdatedWatchers,"Watcher(s) were successfully added.");
        //}




        public async Task<ApiCommonResponse> removeWatcher(HttpContext httpContext, long projectId, long projectWatcherId)
        {
            var ifWatcherExist = await _context.Watchers.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.ProjectWatcherId == projectWatcherId && x.IsActive == true);
            if (ifWatcherExist == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            else
            {
                ifWatcherExist.IsActive = false;
                _context.Watchers.Update(ifWatcherExist);
                await _context.SaveChangesAsync();
                var getUpdatedWatchers = await _context.Watchers.Where(x => x.ProjectId == projectId && x.IsActive == true).ToListAsync();
                return CommonResponse.Send(ResponseCodes.SUCCESS, getUpdatedWatchers);

            }
        }


        public async Task<ApiCommonResponse> removeProject(HttpContext httpContext, long projectId, long workspaceId)
        {
            var ifProjectExist = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId && x.WorkspaceId == workspaceId && x.IsActive == true);
            if (ifProjectExist == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            else
            {
                var checkIfTaskExist = await _context.Tasks.Where(x => x.IsActive == true && x.ProjectId == projectId).ToListAsync();

                if(checkIfTaskExist.Count == 0)
                {
                    ifProjectExist.IsActive = false;
                    _context.Projects.Update(ifProjectExist);
                    await _context.SaveChangesAsync();
                    var getUpdatedProject = await _context.Projects.Where(x => x.WorkspaceId == workspaceId && x.IsActive == true).ToListAsync();
                    return CommonResponse.Send(ResponseCodes.SUCCESS, getUpdatedProject,"Project successfully disabled");
                }
                else
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null,"This Project cannot be disabled because it has task attached to it.");
                }


            }
        }


        public async Task<ApiCommonResponse> removeTask(HttpContext httpContext, long taskId, long projectId)
        {
            var ifTaskExist = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId && x.ProjectId == projectId && x.IsActive == true);
            if (ifTaskExist == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            else
            {
                var checkIfDeliverableExist = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == taskId).ToListAsync();

                if (checkIfDeliverableExist.Count == 0)
                {
                    ifTaskExist.IsActive = false;
                    _context.Tasks.Update(ifTaskExist);
                    await _context.SaveChangesAsync();
                    var getUpdatedTask = await _context.Tasks.Where(x => x.ProjectId == projectId && x.IsActive == true).ToListAsync();
                    return CommonResponse.Send(ResponseCodes.SUCCESS, getUpdatedTask, "Project successfully disabled");
                }
                else
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "This Task cannot be disabled because it has deliverables attached to it.");
                }


            }
        }


        public async Task<ApiCommonResponse> removeDeliverable(HttpContext httpContext, long deliverableId, long taskId)
        {
            var ifDeliverableExist = await _context.Deliverables.FirstOrDefaultAsync(x => x.Id == deliverableId && x.TaskId == taskId && x.IsActive == true);
            if (ifDeliverableExist == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            else
            {
                var checkIfAssignExist = await _context.AssignTasks.Where(x => x.IsActive == true && x.DeliverableId == deliverableId).ToListAsync();

                if (checkIfAssignExist.Count == 0)
                {
                    ifDeliverableExist.IsActive = false;
                    _context.Deliverables.Update(ifDeliverableExist);
                    await _context.SaveChangesAsync();
                    var getUpdatedDeliverable = await _context.Deliverables.Where(x => x.TaskId == taskId && x.IsActive == true).ToListAsync();
                    return CommonResponse.Send(ResponseCodes.SUCCESS, getUpdatedDeliverable, "Deliverable successfully disabled");
                }
                else
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "This Deliverable cannot be disabled because it has been assigned.");
                }


            }
        }




        public async Task<ApiCommonResponse> getTaskById(HttpContext httpContext, long taskId)
        {
            var checkIfTaskExistByCaption = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId && x.CreatedById == httpContext.GetLoggedInUserId());
            if (checkIfTaskExistByCaption == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

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
                return CommonResponse.Send(ResponseCodes.SUCCESS, taskSummary);

            }

        }

        public async Task<ApiCommonResponse> getTaskByProjectId(HttpContext httpContext, long projectId)
        {
            var checkIfTaskExistById = await _context.Tasks.Where(x => x.ProjectId == projectId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
            var taskSummaryList = new List<TaskSummaryDTO>();

            foreach (var item in checkIfTaskExistById)
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
                var getTaskAssignees = await _context.TaskAssignees.Where(X => X.TaskId == item.Id && X.IsActive == true && X.IsActive == true).ToListAsync();
                taskSummary.TaskAssignees = getTaskAssignees.GroupBy(x => x.TaskAssigneeId)
                    .Select(g => g.First())
                    .ToList();
                taskSummary.AssigneeLength = taskSummary.TaskAssignees.Count();
                taskSummary.workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == taskSummary.Project.WorkspaceId && x.IsActive == true);

                taskSummaryList.Add(taskSummary);

            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, taskSummaryList);

        }

        public async Task<ApiCommonResponse> updateTask(HttpContext httpContext, long TaskId, TaskDTO taskDTO)
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

                    return CommonResponse.Send(ResponseCodes.SUCCESS, taskSummary);


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

                    return CommonResponse.Send(ResponseCodes.SUCCESS, taskSummary);

                }

            }
            else
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            }
        }

            public async Task<ApiCommonResponse> getAssignedTask(HttpContext httpContext)
            {

            var projectArray = new List<Project>();
           
            var getAllAssigned = await _context.Tasks.Where(x => x.IsActive == true && x.TaskAssignees.Any(t => t.TaskAssigneeId == httpContext.GetLoggedInUserId()))
                                 .Include(x => x.Project)
                                 .Include(x=>x.TaskAssignees.Where(x=>x.IsActive == true))
                                 .ToListAsync();

            


            if (getAllAssigned.Count() > 0)
               {

                    foreach (var assigned in getAllAssigned)
                    {
                        if(assigned.Project.IsActive == true)
                        {
                            projectArray.Add(assigned.Project);
                        }
                        
                    }

                var projectResult = projectArray.GroupBy(p => p.Id)
                     .Select(result => result.First())
                     .ToArray();

                return CommonResponse.Send(ResponseCodes.SUCCESS, projectResult, ResponseMessage.EntitySuccessfullyFound);
             }

                
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, ResponseMessage.EntityNotFound);
             } 

        

               public async Task<List<TaskAssigneeDTO>> getAssignees(HttpContext httpContext, long taskId)
                {
                    var taskAssigneeList = new List<TaskAssigneeDTO>();

                    if (taskId != 0)
                    {


                var getTaskAssignees = await _context.TaskAssignees.Where(x => x.IsActive == true && x.TaskId == taskId && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

                foreach (var taskAssignees in getTaskAssignees)
                {
                    var taskAssignee = new TaskAssigneeDTO();
                    taskAssignee.CreatedAt = taskAssignees.CreatedAt;
                    taskAssignee.CreatedById = taskAssignees.CreatedById;
                    taskAssignee.IsActive = taskAssignees.IsActive;
                    taskAssignee.Name = taskAssignees.Name;
                    taskAssignee.TaskAssigneeId = taskAssignees.TaskAssigneeId;
                    taskAssignee.ProfileImage = _context.UserProfiles.Where(x => x.Id == taskAssignee.TaskAssigneeId).FirstOrDefault().ImageUrl;
                    taskAssignee.TaskId = taskAssignees.TaskId;
                    taskAssignee.UpdatedAt = taskAssignees.UpdatedAt;

                    taskAssigneeList.Add(taskAssignee);
                }

            }

            return taskAssigneeList;

        }

        public async Task<DeliverableUser> getUser(long? userId, HttpContext httpContext)
        {

            var getUser = await _context.UserProfiles.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == userId);
            var user = new DeliverableUser();
            if (getUser == null)
            {
                return null;
            }
            else
            {

                user.userId = getUser.Id;
                user.email = getUser.Email;
                user.fullname = getUser.FirstName + " " + getUser.LastName;
                user.imageUrl = getUser.ImageUrl;

                return user;
            }

        }



        public async Task<ApiCommonResponse> dropTask(long taskId, long taskOwnershipId, HttpContext httpContext)
        {
            var taskToBePicked = await _context.Tasks.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == taskId);
            var taskOwnerShip = await _context.TaskOwnerships.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == taskOwnershipId && x.TaskOwnerId == httpContext.GetLoggedInUserId());
            if (taskToBePicked == null || taskOwnerShip == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "No Assignee details  was found");

            }
            else
            {

                var canDropTask = await _context.Deliverables.Where(x=>x.IsActive == true && x.TaskId == taskId && x.IsAssigned == true)
                             .ToListAsync();

                if (canDropTask.Count > 0)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "This task contains deliverables that has been assigned,therefore cannot  be dropped.");
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

                return CommonResponse.Send(ResponseCodes.SUCCESS, taskArray);

            }


            }


        }

        public async Task<ApiCommonResponse> updateProject(HttpContext httpContext, long projectId, ProjectDTO projectDTO)
        {
            var projectFound = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId && x.CreatedById == httpContext.GetLoggedInUserId() && x.IsActive == true);

            if (projectFound != null)
            {
                var getProjectByCaption = await _context.Projects.FirstOrDefaultAsync(x => x.IsActive == true && x.Caption == projectDTO.Caption);

                if (projectDTO.Caption == projectFound.Caption || getProjectByCaption != null)
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
                                   return CommonResponse.Send(ResponseCodes.SUCCESS, updatedProject);


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
                    return CommonResponse.Send(ResponseCodes.SUCCESS, updatedProject);

                }

            }
            else
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

            }
        }
        public async Task<ApiCommonResponse> getTaskByCaption(HttpContext httpContext, string caption)
        {
            var checkIfTaskExistByCaption = await _context.Tasks.Where(x => x.Caption == caption.Trim() && x.CreatedById == httpContext.GetLoggedInUserId()).FirstOrDefaultAsync();

            if (checkIfTaskExistByCaption == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);

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
                return CommonResponse.Send(ResponseCodes.SUCCESS, taskSummary);

            }

        }

        public async Task<ApiCommonResponse> getAllPickedTask(HttpContext httpContext)
        {
            var getUpdatedTaskownerShip = await _context.TaskOwnerships.Where(x => x.IsDeleted == false && x.TaskOwnerId == httpContext.GetLoggedInUserId()).ToListAsync();

            if (getUpdatedTaskownerShip == null || getUpdatedTaskownerShip.Count() == 0)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,"No assigned found");


            }
            else
            {

                var taskArray = new List<Task>();
                foreach (var item in getUpdatedTaskownerShip)
                {

                    var taskGotten = await _context.Tasks.Where(x => x.IsActive == true && x.TaskOwnershipId == item.Id).ToListAsync();

                    taskArray.AddRange(taskGotten);

                }



                return CommonResponse.Send(ResponseCodes.SUCCESS, taskArray);


            }

        }

        public async Task<ApiCommonResponse> createTaskIllustration(IllustrationsDTO illustrationsDTO, long taskId, HttpContext httpContext)
        {
            
            
                var illustrationInstance = new PMIllustration();
                illustrationInstance.Caption = illustrationsDTO.Caption;
                illustrationInstance.Alias = illustrationsDTO.Alias;
                illustrationInstance.Description = illustrationsDTO.Description;
                illustrationInstance.CreatedAt = DateTime.Now;
                illustrationInstance.CreatedById = httpContext.GetLoggedInUserId();
                illustrationInstance.IllustrationImage = illustrationsDTO.IllustrationImage;
                illustrationInstance.IsActive = true;
                illustrationInstance.TaskId = taskId;

                //illustrationList.Add(illustrationInstance);
            

            await _context.PMIllustrations.AddAsync(illustrationInstance);
            await _context.SaveChangesAsync();
            var getTaskIllustration = await _context.PMIllustrations.Where(x => x.IsActive == true && x.TaskId == taskId).ToListAsync();

            return CommonResponse.Send(ResponseCodes.SUCCESS, getTaskIllustration);


        }

        public async Task<ApiCommonResponse> removeIllustrationById(long taskId, long illustrationId)
        {
            var getTaskIllustration = await _context.PMIllustrations.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == illustrationId);
            if (getTaskIllustration == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            else
            {
                getTaskIllustration.IsActive = false;
                await _context.SaveChangesAsync();

                return CommonResponse.Send(ResponseCodes.SUCCESS, getTaskIllustration);

            }


        }

        public async Task<ApiCommonResponse> getAllPrivacyAccessByWorkspaceId(HttpContext httpContext, long workspaceId)
        {
            var privacyAccesses = await _context.PrivacyAccesses.Where(x => x.WorkspaceId == workspaceId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS, privacyAccesses);



        }
        public async Task<ApiCommonResponse> getTaskIllustrationById(long taskId)
        {
            var getTaskIllustration = await _context.PMIllustrations.Where(x => x.IsActive == true && x.TaskId == taskId).ToListAsync();
            return CommonResponse.Send(ResponseCodes.SUCCESS, getTaskIllustration);

        }




    }
}
