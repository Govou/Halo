using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using halobiz_backend.DTOs.TransferDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class TaskFulfillmentServiceImpl : ITaskFulfillmentService
    {
        private readonly ILogger<TaskFulfillmentServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ITaskFulfillmentRepository _taskFulfillmentRepo;
        private readonly IOperatingEntityRepository _operatingEntityRepo;
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IServicesRepository _servicesRepo;

        public TaskFulfillmentServiceImpl(IModificationHistoryRepository historyRepo, 
            ITaskFulfillmentRepository taskFulfillmentRepo, 
            IUserProfileRepository userProfileRepo,
            IOperatingEntityRepository operatingEntityRepo,
            DataContext dataContext,
            ILogger<TaskFulfillmentServiceImpl> logger, IMapper mapper,
             IServicesRepository servicesRepo
            )
        {
            this._mapper = mapper;
            this._servicesRepo = servicesRepo;
            this._historyRepo = historyRepo;
            this._taskFulfillmentRepo = taskFulfillmentRepo;
            this._operatingEntityRepo = operatingEntityRepo;
            this._userProfileRepo = userProfileRepo;
            this._context = dataContext;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddTaskFulfillment(HttpContext context, TaskFulfillmentReceivingDTO taskFulfillmentReceivingDTO)
        {
            var taskFulfillment = _mapper.Map<TaskFulfillment>(taskFulfillmentReceivingDTO);
            taskFulfillment.CreatedById = context.GetLoggedInUserId();
            var savedTaskFulfillment = await _taskFulfillmentRepo.SaveTaskFulfillment(taskFulfillment);
            if (savedTaskFulfillment == null)
            {
                return new ApiResponse(500);
            }
            var taskFulfillmentTransferDTO = _mapper.Map<TaskFulfillmentTransferDTO>(savedTaskFulfillment);
            return new ApiOkResponse(taskFulfillmentTransferDTO);
        }

        public async Task<ApiResponse> GetAllTaskFulfillment()
        {
            var taskFulfillments = await _taskFulfillmentRepo.FindAllTaskFulfillment();
            if (taskFulfillments == null)
            {
                return new ApiResponse(404);
            }
            var taskFulfillmentTransferDTO = _mapper.Map<IEnumerable<TaskFulfillmentTransferDTO>>(taskFulfillments);
            return new ApiOkResponse(taskFulfillmentTransferDTO);
        }

        public async Task<ApiResponse> GetAllUnCompletedTaskFulfillmentForTaskOwner(long taskOwnerId)
        {
            var taskFulfillments = await _taskFulfillmentRepo.FindAllTaskFulfillmentForTaskOwner(taskOwnerId);
            if (taskFulfillments == null)
            {
                return new ApiResponse(404);
            }
            taskFulfillments.ToList().RemoveAll(x => x.TaskCompletionStatus);
            var taskFulfillmentTransferDTO = _mapper.Map<IEnumerable<TaskFulfillmentTransferDTO>>(taskFulfillments);
            return new ApiOkResponse(taskFulfillmentTransferDTO);
        }

        public async Task<ApiResponse> GetPMWidgetStatistics(long taskOwnerId)
        {
            var taskFulfillments = await _taskFulfillmentRepo.FindAllTaskFulfillmentForTaskOwner(taskOwnerId);
            var year = DateTime.Now.Year;
            var tasks = taskFulfillments.Where(x => x.CreatedAt.Year == year);
            var allTaskAssigned = 0;
            var completedTask = 0;
            var acceptedTask = 0;
            var qualifiedTasks = 0;

            foreach (var task in tasks)
            {
                if(task.IsAllDeliverableAssigned){
                    allTaskAssigned++;
                }

                if(task.TaskCompletionStatus){
                    completedTask++;
                }

                if(!task.TaskCompletionStatus){
                    acceptedTask +=  CheckIfAccepted(task.DeliverableFUlfillments);
                }

                if(!task.TaskCompletionStatus){
                    qualifiedTasks +=  CheckIfQualified(task.DeliverableFUlfillments);
                }
            }

            var data = new {allTaskAssigned, completedTask, acceptedTask, qualifiedTasks};

            return new ApiOkResponse(data); 
        }

        private int CheckIfAccepted(IEnumerable<DeliverableFulfillment> delivrables)
        {
            var isAllAccepted = true;
            foreach (var deliverable in delivrables)
            {
                if(deliverable.IsPicked == false)
                {
                    isAllAccepted = false;
                }
            }

            return isAllAccepted ? 1 : 0;
        }
        private int CheckIfQualified(IEnumerable<DeliverableFulfillment> delivrables)
        {
            var isAllAccepted = true;
            foreach (var deliverable in delivrables)
            {
                if(deliverable.IsRequestedForValidation == false)
                {
                    isAllAccepted = false;
                }
            }

            return isAllAccepted ? 1 : 0;
        }

        public async Task<ApiResponse> GetTaskFulfillmentById(long id)
        {
            var taskFulfillment = await _taskFulfillmentRepo.FindTaskFulfillmentById(id);
            if (taskFulfillment == null)
            {
                return new ApiResponse(404);
            }
            var serviceDetails = await _servicesRepo.GetServiceDetails(taskFulfillment.ContractService.ServiceId);
            var taskFulfillmentTransferDTOs = _mapper.Map<TaskFulfillmentTransferDetailsDTO>(taskFulfillment);
            taskFulfillmentTransferDTOs.ServiceDivisionDetails = serviceDetails;
            return new ApiOkResponse(taskFulfillmentTransferDTOs);
        }

        public async Task<ApiResponse> GetTaskFulfillmentByName(string name)
        {
            var taskFulfillment = await _taskFulfillmentRepo.FindTaskFulfillmentByName(name);
            if (taskFulfillment == null)
            {
                return new ApiResponse(404);
            }
            var taskFulfillmentTransferDTOs = _mapper.Map<TaskFulfillmentTransferDetailsDTO>(taskFulfillment);
            return new ApiOkResponse(taskFulfillmentTransferDTOs);
        }
        public async Task<ApiResponse> GetTaskDeliverableSummary(long responsibleId)
        {
            try
            {
                List<TaskWithListOfDeliverables> taskWithListOfDeliverables = new List<TaskWithListOfDeliverables>();
                var userDeliverableSummary =  await _taskFulfillmentRepo.GetTaskDeliverablesSummary(responsibleId);
                foreach (var deliverableSummary in userDeliverableSummary)
                {
                    var deliverable = GetTaskWithListOfDeliverables(taskWithListOfDeliverables, deliverableSummary.TaskId);
                    if(deliverable == null)
                    {
                        taskWithListOfDeliverables.Add(new TaskWithListOfDeliverables()
                            {
                                TaskCaption = deliverableSummary.TaskCaption,
                                TaskId = deliverableSummary.TaskId,
                                TaskResponsibleId = deliverableSummary.TaskResponsibleId,
                                TaskResponsibleImageUrl = deliverableSummary.TaskResponsibleImageUrl,
                                TaskResponsibleName = deliverableSummary.TaskResponsibleName,
                                DeliverableSummaries = new List<DeliverableSummary>(){
                                    new DeliverableSummary()
                                    {
                                            DeliverableId = deliverableSummary.DeliverableId,
                                            DeliverableCaption = deliverableSummary.DeliverableCaption,
                                            DeliveryDate = deliverableSummary.DeliveryDate,
                                            DeliverableStatus = deliverableSummary.DeliverableStatus,
                                            DeliverableResponsibleId = deliverableSummary.DeliverableResponsibleId,
                                            Priority = deliverableSummary.Priority,
                                            IsPicked = deliverableSummary.IsPicked,
                                            DeliveryState = GetDeliverableAtRiskStatus(deliverableSummary.StartDate, deliverableSummary.DeliveryDate)
                                    }
                                }
                        
                            });
                    }else{
                        deliverable.DeliverableSummaries.Add(
                            new DeliverableSummary()
                                    {
                                            DeliverableId = deliverableSummary.DeliverableId,
                                            DeliverableCaption = deliverableSummary.DeliverableCaption,
                                            DeliveryDate = deliverableSummary.DeliveryDate,
                                            DeliverableStatus = deliverableSummary.DeliverableStatus,
                                            DeliverableResponsibleId = deliverableSummary.DeliverableResponsibleId,
                                            Priority = deliverableSummary.Priority,
                                            IsPicked = deliverableSummary.IsPicked,
                                            DeliveryState = GetDeliverableAtRiskStatus(deliverableSummary.StartDate, deliverableSummary.DeliveryDate)
                                    }
                        );
                    }

                }
                return new ApiOkResponse(taskWithListOfDeliverables);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
           
        }

        private DeliveryState GetDeliverableAtRiskStatus(DateTime? start, DateTime? end)
        {
            if(end < DateTime.Now ) 
                return DeliveryState.Overdue;
            var diffInDate =((DateTime) end).Subtract((DateTime)start).TotalMilliseconds;
            var diffInStartDateAndPresentDate = DateTime.Now.Subtract((DateTime)start).TotalMilliseconds;
            return (diffInStartDateAndPresentDate / (double)diffInDate) * 100 >= 90 ? DeliveryState.AtRisk : DeliveryState.OnTrack;
        }

        public TaskWithListOfDeliverables GetTaskWithListOfDeliverables(IEnumerable<TaskWithListOfDeliverables> taskWithListOfDeliverables, long taskId)
        {
            if(taskWithListOfDeliverables.Count() == 0) 
                return null;

            return taskWithListOfDeliverables.FirstOrDefault(x => x.TaskId == taskId);

        }

        public async Task<ApiResponse> UpdateTaskFulfillment(HttpContext context, long id, TaskFulfillmentReceivingDTO taskFulfillmentReceivingDTO)
        {
            var taskFulfillmentToUpdate = await _taskFulfillmentRepo.FindTaskFulfillmentById(id);
            if (taskFulfillmentToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {taskFulfillmentToUpdate.ToString()} \n" ;

            taskFulfillmentToUpdate.Caption = taskFulfillmentReceivingDTO.Caption;
            taskFulfillmentToUpdate.Description = taskFulfillmentReceivingDTO.Description;
            taskFulfillmentToUpdate.CustomerDivisionId = taskFulfillmentReceivingDTO.CustomerDivisionId;
            taskFulfillmentToUpdate.ResponsibleId = taskFulfillmentReceivingDTO.ResponsibleId;
            taskFulfillmentToUpdate.ContractServiceId = taskFulfillmentReceivingDTO.ContractServiceId;
            taskFulfillmentToUpdate.AccountableId = taskFulfillmentReceivingDTO.AccountableId;
            taskFulfillmentToUpdate.ConsultedId = taskFulfillmentReceivingDTO.ConsultedId;
            taskFulfillmentToUpdate.InformedId = taskFulfillmentReceivingDTO.InformedId;
            taskFulfillmentToUpdate.SupportId = taskFulfillmentReceivingDTO.SupportId;
            taskFulfillmentToUpdate.Budget = taskFulfillmentReceivingDTO.Budget;
            taskFulfillmentToUpdate.IsAllDeliverableAssigned = taskFulfillmentReceivingDTO.IsAllDeliverableAssigned;
            taskFulfillmentToUpdate.StartDate = taskFulfillmentReceivingDTO.StartDate;
            taskFulfillmentToUpdate.EndDate = taskFulfillmentReceivingDTO.EndDate;
            taskFulfillmentToUpdate.ServiceCode = taskFulfillmentReceivingDTO.ServiceCode;

            var updatedTaskFulfillment = await _taskFulfillmentRepo.UpdateTaskFulfillment(taskFulfillmentToUpdate);

            summary += $"Details after change, \n {updatedTaskFulfillment.ToString()} \n";

            if (updatedTaskFulfillment == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "TaskFulfillment",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedTaskFulfillment.Id
            };

            await _historyRepo.SaveHistory(history);

            var taskFulfillmentTransferDTOs = _mapper.Map<TaskFulfillmentTransferDTO>(updatedTaskFulfillment);
            return new ApiOkResponse(taskFulfillmentTransferDTOs);

        }

        public async Task<ApiResponse> DeleteTaskFulfillment(long id)
        {
            var taskFulfillmentToDelete = await _taskFulfillmentRepo.FindTaskFulfillmentById(id);
            if (taskFulfillmentToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _taskFulfillmentRepo.DeleteTaskFulfillment(taskFulfillmentToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> SetIsPicked(HttpContext context, long id)
        {
            var taskFulfillmentToUpdate = await _taskFulfillmentRepo.FindTaskFulfillmentById(id);
            if (taskFulfillmentToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {taskFulfillmentToUpdate.ToString()} \n";

            taskFulfillmentToUpdate.IsPicked = true;
            taskFulfillmentToUpdate.DateTimePicked = DateTime.UtcNow;

            var updatedTaskFulfillment = await _taskFulfillmentRepo.UpdateTaskFulfillment(taskFulfillmentToUpdate);

            summary += $"Details after change, \n {updatedTaskFulfillment.ToString()} \n";

            if (updatedTaskFulfillment == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "TaskFulfillment",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedTaskFulfillment.Id
            };

            await _historyRepo.SaveHistory(history);

            var taskFulfillmentTransferDTOs = _mapper.Map<TaskFulfillmentTransferDTO>(updatedTaskFulfillment);
            return new ApiOkResponse(taskFulfillmentTransferDTOs);

        }

        public async Task<ApiResponse> GetTaskFulfillmentsByOperatingEntityHeadId(long id)
        {
            var validOperatingHeadId = await _context.OperatingEntities.AnyAsync(x => x.HeadId == id && x.IsDeleted == false);
            if (!validOperatingHeadId)
            {
                return new ApiResponse(404);
            }

            var taskFulfillments = _context.TaskFulfillments.Where(x => x.ResponsibleId == id && x.IsDeleted == false);
            if (taskFulfillments == null)
            {
                return new ApiResponse(404);
            }

            var taskFulfillmentTransferDTOs = _mapper.Map<IEnumerable<TaskFulfillmentTransferDTO>>(taskFulfillments);
            return new ApiOkResponse(taskFulfillmentTransferDTOs);
        }

        public async Task<ApiResponse> GetTaskFulfillmentDetails(long id)
        {
            var taskFulfillment = await _taskFulfillmentRepo.FindTaskFulfillmentById(id);
            if (taskFulfillment == null)
            {
                return new ApiResponse(404);
            }

            var customerDivision = await _context.CustomerDivisions.Where(x => x.Id == taskFulfillment.CustomerDivisionId && x.IsDeleted == false)
                .Include(x => x.Customer).Include(x => x.Contracts).SingleOrDefaultAsync();

            if (customerDivision == null)
            {
                return new ApiResponse(500);
            }

            var leadDivision = await _context.LeadDivisions.Where(x => x.RCNumber == customerDivision.RCNumber && x.DivisionName == customerDivision.DivisionName && x.IsDeleted == false)
                .Include(x => x.PrimaryContact).Include(x => x.SecondaryContact).Include(x => x.LeadDivisionKeyPersons).SingleOrDefaultAsync();

            if (leadDivision == null)
            {
                return new ApiResponse(500);
            }

            var deliverableFulfillments = await _context.DeliverableFulfillments.Where(x => x.TaskFullfillmentId == taskFulfillment.Id && x.IsDeleted == false).ToListAsync();

            taskFulfillment.CustomerDivision = customerDivision;

            var taskFulfillmentTransferDTOs = _mapper.Map<TaskFulfillmentTransferDetailsDTO>(taskFulfillment);

            taskFulfillmentTransferDTOs.PrimaryContact = leadDivision.PrimaryContact;
            taskFulfillmentTransferDTOs.SecondaryContact = leadDivision.SecondaryContact;
            taskFulfillmentTransferDTOs.LeadDivisionKeyPersons = leadDivision.LeadDivisionKeyPersons;
            taskFulfillmentTransferDTOs.DeliverableFulfillments = _mapper.Map<IEnumerable<DeliverableFulfillmentWithouthTaskFulfillmentTransferDTO>>(deliverableFulfillments);;      

            return new ApiOkResponse(taskFulfillmentTransferDTOs);
        }
    }
}