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

namespace HaloBiz.MyServices.Impl
{
    public class ProjectAllocationServiceImpl: IProjectAllocationServiceImpl
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ProjectAllocationServiceImpl> _logger;
        private readonly IProjectAllocationRepositoryImpl _projectAllocationRepository;
        private readonly IMapper _mapper;

        public ProjectAllocationServiceImpl(HalobizContext context,IProjectAllocationRepositoryImpl projectAllocationRepository, ILogger<ProjectAllocationServiceImpl> logger, IMapper mapper)
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
                                                                            
                if(existingManagerInTheSameGroup.Count == 0)
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



                var savedWorkSpaces = await _projectAllocationRepository.createWorkspace(context,workspaceDTO);
                
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

            var getManagers =  await _projectAllocationRepository.getAllProjectManager(serviceCategoryId);

            if (getManagers == null)
            {
                return new ApiOkResponse(getManagers);
            }

            return  new ApiOkResponse(getManagers);

            
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
                workspaces.PrivacyAccesses = await _context.PrivacyAccesses.Where(x => x.IsActive != false && x.WorkspaceId == item.Id).ToListAsync();
                workspaces.ProjectCreators = await _context.ProjectCreators.Where(x => x.IsActive != false && x.WorkspaceId == item.Id).ToListAsync();
                workspaces.StatusFlowDTO = await _context.StatusFlows.Where(x => x.IsDeleted == false && x.WorkspaceId == item.Id).ToListAsync();
                workspaces.ProjectCreatorsLength = (item.ProjectCreators == null ? 0 : item.ProjectCreators.Count());
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

        public async Task<ApiResponse> removeFromCategory(long id,int category,long projectId)
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

        public async Task<ApiResponse> removeFromProjectCreator(long workspaceId,long creatorId)
        {
            var creatorToDelete = await _context.ProjectCreators.FirstOrDefaultAsync(x => x.ProjectCreatorProfileId == creatorId && x.IsActive != false  && x.WorkspaceId == workspaceId);
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

        public async Task<ApiResponse> updateWorkspace(HttpContext httpContext, long id,UpdateWorkspaceDTO workspaceDTO)
        {
            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == id);
            if (workspace == null)
            {
                return new ApiResponse(404);
            }

            workspace.Alias = workspaceDTO.Alias;
            workspace.Caption = workspaceDTO.Caption;
            workspace.Description = workspaceDTO.Description;
            _context.Workspaces.Update(workspace);
            await _context.SaveChangesAsync();
            return new ApiOkResponse(workspace);
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





        public async Task<ApiResponse> getManagersProjects(string email,int emailId)
       
        {

            var getProjects = await _projectAllocationRepository.getAllManagerProjects(email,emailId);

            if (getProjects == null)
            {
                return new ApiResponse(400);
            }

            return new ApiOkResponse(getProjects);


        }
    }
}