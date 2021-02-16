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

        public TaskFulfillmentServiceImpl(IModificationHistoryRepository historyRepo, 
            ITaskFulfillmentRepository taskFulfillmentRepo, 
            IUserProfileRepository userProfileRepo,
            IOperatingEntityRepository operatingEntityRepo,
            DataContext dataContext,
            ILogger<TaskFulfillmentServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
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

        public async Task<ApiResponse> GetTaskFulfillmentById(long id)
        {
            var taskFulfillment = await _taskFulfillmentRepo.FindTaskFulfillmentById(id);
            if (taskFulfillment == null)
            {
                return new ApiResponse(404);
            }
            var taskFulfillmentTransferDTOs = _mapper.Map<TaskFulfillmentTransferDTO>(taskFulfillment);
            return new ApiOkResponse(taskFulfillmentTransferDTOs);
        }

        public async Task<ApiResponse> GetTaskFulfillmentByName(string name)
        {
            var taskFulfillment = await _taskFulfillmentRepo.FindTaskFulfillmentByName(name);
            if (taskFulfillment == null)
            {
                return new ApiResponse(404);
            }
            var taskFulfillmentTransferDTOs = _mapper.Map<TaskFulfillmentTransferDTO>(taskFulfillment);
            return new ApiOkResponse(taskFulfillmentTransferDTOs);
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
            taskFulfillmentTransferDTOs.DeliverableFulfillments = deliverableFulfillments;      

            return new ApiOkResponse(taskFulfillmentTransferDTOs);
        }
    }
}