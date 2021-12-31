using AutoMapper;
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

            return  new ApiOkResponse(getManagers);


        }

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

        public async Task<ApiResponse> removeFromCategory(long id,int category,long projectId)
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

        public async Task<ApiResponse> removeFromProjectCreator(long workspaceId,long creatorId)
        {
            var creatorToDelete = await _context.ProjectCreators.FirstOrDefaultAsync(x => x.ProjectCreatorProfileId == creatorId && x.IsActive != false && x.WorkspaceId == workspaceId);
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

        public async Task<ApiCommonResponse> updateToPublic(long workspaceId)
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

        public async Task<ApiResponse> updateWorkspace(HttpContext httpContext, long id,UpdateWorkspaceDTO workspaceDTO)
        {
            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
            if (workspace != null)
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
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,remainderUser);
        }


        public async Task<ApiResponse> addMorePrivateUser(HttpContext httpContext, long workspaceId, List<AddMoreUserDto> privateUserid)
        {
            var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == workspaceId);
            if (workspace == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,remainderUser);

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
            return CommonResponse.Send(ResponseCodes.SUCCESS,currentStatus);

        }


        public async Task<ApiCommonResponse> moveStatusSequenec(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO)
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,statusArray);

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
            return CommonResponse.Send(ResponseCodes.SUCCESS,statusArray);
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

                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 200,
                    responseMessage = "Successfull retrieved all deliverables",
                    data = deliverableToDisplayArray,
                };
            }

            


        }


        public async Task<ApiGenericResponse<List<DeliverableDTO>>> getAllDeliverablesByTaskId(HttpContext httpContext,long taskId)

        {

            var getAllDeliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == taskId && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();

            if (getAllDeliverables == null || getAllDeliverables.Count() == 0)
            {
                return new ApiGenericResponse<List<DeliverableDTO>>
                {
                    responseCode = 404,
                    responseMessage = "No Deliverables was found",
                    data = null,
                };
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

                return new ApiGenericResponse<List<DeliverableDTO>>
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
                deliverableToDisplayInstance.Budget = getAllDeliverables.Budget;
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


        public async Task<ApiGenericResponse<List<Deliverable>>> createNewDeliverableFromTask(HttpContext httpContext, long TaskId, DeliverableDTO deliverableDTO)

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

                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 200,
                    responseMessage = "Deliverable successfully saved",
                    data = getAllDeliverables,
                };
            }


        }

        public async Task<ApiGenericResponse<List<Deliverable>>> updateDeliverable(HttpContext httpContext, long taskId,long deliverableId,DeliverableDTO deliverableDTO)
        {
            var getDeliverable = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.TaskId == taskId && x.Id == deliverableId);
            if (getDeliverable == null)
            {
                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 404,
                    responseMessage = "Deliverable with Id" + deliverableId + "could not be found",
                    data = null,
                };
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


                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 200,
                    responseMessage = "Deliverable successfully Updated",
                    data = updatedResult,
                };

            }

        }


        public async Task<ApiGenericResponse<List<Deliverable>>> disableDeliverable(HttpContext httpContext, long taskId, long deliverableId)
        {
            var getDeliverable = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.TaskId == taskId && x.Id == deliverableId);
            if (getDeliverable == null)
            {
                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 404,
                    responseMessage = "Deliverable with Id" + deliverableId + "could not be found",
                    data = null,
                };
            }

            else
            {

                getDeliverable.IsActive = false;

                _context.Deliverables.Update(getDeliverable);
                await _context.SaveChangesAsync();
                var updatedResult = await _context.Deliverables.Where(x => x.TaskId == taskId  && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();


                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 200,
                    responseMessage = "Deliverable successfully Updated",
                    data = updatedResult,
                };

            }

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

                var updatedResult = await _context.Deliverables.Where(x => x.TaskId == TaskId && x.IsActive == true && x.CreatedById == httpContext.GetLoggedInUserId()).ToListAsync();


                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 200,
                    responseMessage = "Deliverable successfully saved",
                    data = updatedResult,
                };
            }


        }



        public async Task<ApiGenericResponse<IEnumerable<TaskAssignee>>> addMoreTaskAssignees(HttpContext context, long taskId,  List<TaskAssigneeDTO> taskAssigneeDTO)
        {
            var getTask = await _context.Tasks.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == taskId);
            if (getTask == null)
            {
                return new ApiGenericResponse<IEnumerable<TaskAssignee>>
                {
                    responseCode = 404,
                    responseMessage = "Task with Id" + taskId + "could not be found",
                    data = null,
                };
            }

            
            
            else if(getTask != null)
            {
                foreach (var existing in taskAssigneeDTO)
                {
                    var findAssignee = await _context.TaskAssignees.FirstOrDefaultAsync(x => x.TaskAssigneeId == existing.TaskAssigneeId && x.TaskId == existing.TaskId && x.IsActive == true);
                    if (findAssignee != null)
                    {
                        return new ApiGenericResponse<IEnumerable<TaskAssignee>>
                        {
                            responseCode = 404,
                            responseMessage = "Task assignee already exists",
                            data = null,
                        };
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

                    asigneeArray.Add(assignee);
                }

                await _context.AddRangeAsync(asigneeArray);
                await _context.SaveChangesAsync();

            }

            var getUpdatedAssigneeById = await _context.TaskAssignees.Where(x => x.IsActive == true && x.TaskId == taskId).ToListAsync();
            var distinctAssigneeId = getUpdatedAssigneeById.GroupBy(x => x.TaskAssigneeId)
                        .Select(g => g.First())
                        .ToList();
            return new ApiGenericResponse<IEnumerable<TaskAssignee>>
            {
                responseCode = 200,
                responseMessage = "Successfully saved task assignees",
                data = distinctAssigneeId
            };


        }

        public async Task<ApiGenericResponse<List<TaskAssignee>>> disableTaskAssignee(HttpContext context, long taskId, long assigneeId)
        {
            var getAssignee = await _context.TaskAssignees.FirstOrDefaultAsync(x=>x.TaskAssigneeId == assigneeId && x.TaskId == taskId && x.IsActive == true);
            if(getAssignee == null)
            {
                return new ApiGenericResponse<List<TaskAssignee>>
                {
                    responseCode = 404,
                    responseMessage = "Assignee with Id" + assigneeId + "could not be found",
                    data = null,
                };
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

                return new ApiGenericResponse<List<TaskAssignee>>
                {
                    responseCode = 200,
                    responseMessage = "Successfully saved task assignees",
                    data = getUpdatedAssigneeById,
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





        public async Task<ApiGenericResponse<List<Deliverable>>> AssignDeliverable(HttpContext context, long taskId,long deliverableId, long assigneDeliverableId,  AssignDeliverableDTO assignDeliverableDTO)
        {
           
               var getAssigndDeliverable = await _context.AssignTasks.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == assigneDeliverableId && x.DeliverableId == deliverableId);
           

                if (assigneDeliverableId > 0)
                {

                    if (assignDeliverableDTO.DeliverableAssigneeId == null)
                        assignDeliverableDTO.DeliverableAssigneeId = getAssigndDeliverable.DeliverableAssigneeId;
                    if (assignDeliverableDTO.Priority == null)
                        assignDeliverableDTO.Priority = getAssigndDeliverable.Priority;

                    getAssigndDeliverable.Priority = assignDeliverableDTO.Priority;
                    getAssigndDeliverable.DeliverableAssigneeId = assignDeliverableDTO.DeliverableAssigneeId;
                    _context.AssignTasks.Update(getAssigndDeliverable);
                    await _context.SaveChangesAsync();

            }

               else
                {

                    var deliverableToBeAssigned = new AssignTask
                    {
                        IsActive = true,
                        Caption = assignDeliverableDTO.Caption,
                        Alias = assignDeliverableDTO.Alias,
                        Description = assignDeliverableDTO.Description,
                        Priority = assignDeliverableDTO.Priority,
                        DeliverableId = assignDeliverableDTO.DeliverableId,
                        DeliverableAssigneeId = assignDeliverableDTO.DeliverableAssigneeId,
                        DueDate = assignDeliverableDTO.DueDate,
                        CreatedById = context.GetLoggedInUserId(),
                        CreatedAt = DateTime.Now
                    };
                    await _context.AssignTasks.AddAsync(deliverableToBeAssigned);
                    await _context.SaveChangesAsync();
            }

                    
                    
                

                    var getAllDeliverables = await _context.Deliverables.Where(x => x.IsActive == true && x.TaskId == taskId  && x.CreatedById == context.GetLoggedInUserId()).ToListAsync();
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
                    //    deliverableToDisplayInstance.PMIllustrations = await _context.PMIllustrations.Where(x => x.IsActive == true && x.TaskOrDeliverableId == deliverableId).ToListAsync();
                    //    var getAssignTaskById = await _context.AssignTasks.FirstOrDefaultAsync(x => x.IsActive == true && x.DeliverableId == item.Id);
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

                    //    }

                        return new ApiGenericResponse<List<Deliverable>>
                        {
                            responseCode = 200,
                            responseMessage = "Deliverable successfully saved.",
                            data = getAllDeliverables,
                        };

             }






        public async Task<ApiGenericResponse<List<Deliverable>>> createDeliverableIllustrattions(HttpContext context, long deliverableId,long taskId, List<IllustrationsDTO> illustrationsDTO)
        {
            
            var getCurrentDeliverable = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true  && x.Id == deliverableId);


            if (getCurrentDeliverable == null)
                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 404,
                    responseMessage = "Deliverable was not found.",
                    data = null,
                };


            else
            {
                var illustrationArray = new List<PMIllustration>();
                foreach(var item in illustrationsDTO)
                {
                    var illustration = new PMIllustration();
                    illustration.Alias = item.Alias;
                    illustration.Caption = item.Caption;
                    illustration.CreatedAt = DateTime.Now;
                    illustration.CreatedById = context.GetLoggedInUserId();
                    illustration.Description = item.Description;
                    illustration.IllustrationImage = item.IllustrationImage;
                    illustration.IsActive = true;
                    illustration.TaskOrDeliverableId = deliverableId;
                    illustrationArray.Add(illustration);
                }
                await _context.PMIllustrations.AddRangeAsync(illustrationArray);
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

            return new ApiGenericResponse<List<Deliverable>>
            {
                responseCode = 200,
                responseMessage = "Deliverable successfully saved.",
                data = getAllDeliverables,
            };

        }





        public async Task<ApiGenericResponse<List<Deliverable>>> DeleteIllustration(HttpContext context, long taskId,long deliverableId ,long illustrationId)
        {

            var getCurrentDeliverable = await _context.Deliverables.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == deliverableId);


            if (getCurrentDeliverable == null)
                return new ApiGenericResponse<List<Deliverable>>
                {
                    responseCode = 404,
                    responseMessage = "Deliverable was not found.",
                    data = null,
                };


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

            return new ApiGenericResponse<List<Deliverable>>
            {
                responseCode = 200,
                responseMessage = "Deliverable successfully saved.",
                data = getAllDeliverables,
            };

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

        public async Task<ApiResponse> getManagersProjects(string email,int emailId)
       
        {

            var getProjects = await _projectAllocationRepository.getAllManagerProjects(email,emailId);

            if (getProjects == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS,getProjects);


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
