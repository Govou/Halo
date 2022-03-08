using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Adapters;
using HalobizMigrations.Data;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.MailDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HalobizMigrations.Models;

using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using halobiz_backend.DTOs.TransferDTOs.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Halobiz.Common.Repository;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class DeliverableFulfillmentServiceImpl : IDeliverableFulfillmentService
    {
        private readonly ILogger<DeliverableFulfillmentServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IMailAdapter _mailAdapter;
        private readonly IDeliverableFulfillmentRepository _deliverableFulfillmentRepo;
        private readonly ITaskFulfillmentRepository _taskFulfillmentRepo;
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;

        public DeliverableFulfillmentServiceImpl(IModificationHistoryRepository historyRepo, 
            IDeliverableFulfillmentRepository deliverableFulfillmentRepo, 
            ITaskFulfillmentRepository taskFulfillmentRepo,
            IMailAdapter mailAdapter,
            IUserProfileRepository userProfileRepo,
            HalobizContext dataContext,
            ILogger<DeliverableFulfillmentServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            _mailAdapter = mailAdapter;
            this._deliverableFulfillmentRepo = deliverableFulfillmentRepo;
            this._taskFulfillmentRepo = taskFulfillmentRepo;
            this._userProfileRepo = userProfileRepo;
            this._context = dataContext;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddDeliverableFulfillment(HttpContext context, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO)
        {
            var deliverableFulfillment = _mapper.Map<DeliverableFulfillment>(deliverableFulfillmentReceivingDTO);
            deliverableFulfillment.CreatedById = context.GetLoggedInUserId();
            var savedDeliverableFulfillment = await _deliverableFulfillmentRepo.SaveDeliverableFulfillment(deliverableFulfillment);
            if (savedDeliverableFulfillment == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var deliverableFulfillmentTransferDTO = _mapper.Map<DeliverableFulfillmentTransferDTO>(savedDeliverableFulfillment);
            return CommonResponse.Send(ResponseCodes.SUCCESS,deliverableFulfillmentTransferDTO);
        }

        public async Task<ApiCommonResponse> DeliverableToAssignedUserRatio(long taskMasterId)
        {
            var listOfDeliverableToAssignedPeronRation = new List<DeliverableToAssignedUserRatioTransferDTO>(); 
            try{

            var deliverables = await _deliverableFulfillmentRepo.FindAllAssignedDeliverableFulfillmentForTaskMaster(taskMasterId);
            if(deliverables.Count() == 0){
                return CommonResponse.Send(ResponseCodes.SUCCESS,listOfDeliverableToAssignedPeronRation);
            }

            var userTaskDictionary = new Dictionary<UserProfile, int>();
            foreach (var deliverable in deliverables)
            {
                if(userTaskDictionary.ContainsKey(deliverable.Responsible))
                {
                    userTaskDictionary[deliverable.Responsible]  += 1;
                }
                else{
                    userTaskDictionary.Add(deliverable.Responsible, 1);
                }
            }

            userTaskDictionary.Keys.ToList().ForEach(x => {
                listOfDeliverableToAssignedPeronRation.Add(new DeliverableToAssignedUserRatioTransferDTO(){
                    User  = x,
                    Proportion = userTaskDictionary[x]
                });
            });

            return CommonResponse.Send(ResponseCodes.SUCCESS,listOfDeliverableToAssignedPeronRation);

            }catch(Exception e )
            {
                _logger.LogError(e.Message);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            
        }

        public async Task<ApiCommonResponse> GetAllDeliverableFulfillment()
        {
            var deliverableFulfillments = await _deliverableFulfillmentRepo.FindAllDeliverableFulfillment();
            if (deliverableFulfillments == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var deliverableFulfillmentTransferDTO = _mapper.Map<IEnumerable<DeliverableFulfillmentTransferDTO>>(deliverableFulfillments);
            return CommonResponse.Send(ResponseCodes.SUCCESS,deliverableFulfillmentTransferDTO);
        }

        public async Task<ApiCommonResponse> GetDeliverableFulfillmentById(long id)
        {
            var deliverableFulfillment = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
            if (deliverableFulfillment == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var deliverableFulfillmentTransferDTOs = _mapper.Map<DeliverableFulfillmentTransferDTO>(deliverableFulfillment);
            return CommonResponse.Send(ResponseCodes.SUCCESS,deliverableFulfillmentTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetDeliverableFulfillmentByName(string name)
        {
            var deliverableFulfillment = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentByName(name);
            if (deliverableFulfillment == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var deliverableFulfillmentTransferDTOs = _mapper.Map<DeliverableFulfillmentTransferDTO>(deliverableFulfillment);
            return CommonResponse.Send(ResponseCodes.SUCCESS,deliverableFulfillmentTransferDTOs);
        }
        public async Task<ApiCommonResponse> GetUserDeliverableFulfillmentStat(long userId)
        {
            try{
                var stat = await _deliverableFulfillmentRepo.GetUserDeliverableStat(userId);
                return CommonResponse.Send(ResponseCodes.SUCCESS,stat);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> GetUserDeliverableFulfillmentDashboard(long userId)
        {
            try
            {
                var stat = await _deliverableFulfillmentRepo.GetUserDeliverableDashboard(userId);
                return CommonResponse.Send(ResponseCodes.SUCCESS,stat);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> UpdateDeliverableFulfillment(HttpContext context, long deliverableId, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO)
        {
            var isUpdateToAssignResponsible = false;
            var isUpdateThatAssignsDeliverable = false;
            
            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try{
                    var deliverableFulfillmentToUpdate = await _context.DeliverableFulfillments.FirstOrDefaultAsync( x => x.Id == deliverableId && x.IsDeleted == false);
                    if (deliverableFulfillmentToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
                    }

                    isUpdateToAssignResponsible = deliverableFulfillmentToUpdate.ResponsibleId == 0 && deliverableFulfillmentReceivingDTO.ResponsibleId > 0;
                    isUpdateThatAssignsDeliverable = deliverableFulfillmentToUpdate.ResponsibleId == null && deliverableFulfillmentReceivingDTO.ResponsibleId > 0;
                    
                    var summary = $"Initial details before change, \n {deliverableFulfillmentToUpdate.ToString()} \n" ;

                    deliverableFulfillmentToUpdate.Caption = deliverableFulfillmentReceivingDTO.Caption;
                    deliverableFulfillmentToUpdate.Description = deliverableFulfillmentReceivingDTO.Description;
                    deliverableFulfillmentToUpdate.TaskFullfillmentId = deliverableFulfillmentReceivingDTO.TaskFullfillmentId;
                    deliverableFulfillmentToUpdate.ResponsibleId = deliverableFulfillmentReceivingDTO.ResponsibleId;
                    deliverableFulfillmentToUpdate.StartDate = deliverableFulfillmentReceivingDTO.StartDate;
                    deliverableFulfillmentToUpdate.EndDate = deliverableFulfillmentReceivingDTO.EndDate;
                    deliverableFulfillmentToUpdate.DeliveryDate = deliverableFulfillmentReceivingDTO.DeliveryDate;
                    deliverableFulfillmentToUpdate.Budget = deliverableFulfillmentReceivingDTO.Budget;
                    deliverableFulfillmentToUpdate.DeliverableCompletionReferenceNo = deliverableFulfillmentReceivingDTO.DeliverableCompletionReferenceNo;
                    deliverableFulfillmentToUpdate.DeliverableCompletionReferenceUrl = deliverableFulfillmentReceivingDTO.DeliverableCompletionReferenceUrl;
                    deliverableFulfillmentToUpdate.ServiceCode = deliverableFulfillmentReceivingDTO.ServiceCode;
                    deliverableFulfillmentToUpdate.EscallationTimeDurationForPicking = deliverableFulfillmentReceivingDTO.EscallationTimeDurationForPicking;
                    deliverableFulfillmentToUpdate.Priority = (int)deliverableFulfillmentReceivingDTO.Priority;

                    var updatedDeliverableFulfillment =  _context.DeliverableFulfillments.Update(deliverableFulfillmentToUpdate).Entity;
                    await _context.SaveChangesAsync();

                    summary += $"Details after change, \n {updatedDeliverableFulfillment.ToString()} \n";

                    ModificationHistory history = new ModificationHistory(){
                        ModelChanged = "DeliverableFulfillment",
                        ChangeSummary = summary,
                        ChangedById = context.GetLoggedInUserId(),
                        ModifiedModelId = updatedDeliverableFulfillment.Id
                    };

                    await _historyRepo.SaveHistory(history);

                    if(isUpdateToAssignResponsible)
                    {
                        await CheckAllDeliverablesAndSetTaskAssignmentStatus( deliverableFulfillmentToUpdate.TaskFullfillmentId);                     
                    }

                    if (isUpdateThatAssignsDeliverable)
                    {
                        await SendNewDeliverableAssignedMail(updatedDeliverableFulfillment);
                    }

                    await transaction.CommitAsync();
                    var deliverableFulfillmentTransferDTOs = _mapper.Map<DeliverableFulfillmentTransferDTO>(updatedDeliverableFulfillment);
                    return CommonResponse.Send(ResponseCodes.SUCCESS,deliverableFulfillmentTransferDTOs);
                }catch(Exception e){
                    await transaction.RollbackAsync();
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }

        }

        public async Task<ApiCommonResponse> DeleteDeliverableFulfillment(long id)
        {
            var deliverableFulfillmentToDelete = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
            if (deliverableFulfillmentToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _deliverableFulfillmentRepo.DeleteDeliverableFulfillment(deliverableFulfillmentToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }



        private async Task<bool> CheckAllDeliverablesAndSetTaskAssignmentStatus( long taskId)
        {

            var taskFulfillment = await _context.TaskFulfillments
                .Include(x => x.DeliverableFulfillments)
                .FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == taskId);
            
            foreach (var deliverable in taskFulfillment.DeliverableFulfillments)
            {
                if(deliverable.ResponsibleId == null || deliverable.ResponsibleId == 0)
                {
                    return false;
                }
            }

            taskFulfillment.IsAllDeliverableAssigned  = true;
            _context.TaskFulfillments.Update(taskFulfillment);
            await  _context.SaveChangesAsync();
            return true;
        } 

        public async Task<ApiCommonResponse> ReAssignDeliverableFulfillment(HttpContext context, long id, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var deliverableFulfillmentToUpdate = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
                    if (deliverableFulfillmentToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
                    }

                    var userProfile = await _userProfileRepo.FindUserById(deliverableFulfillmentReceivingDTO.ResponsibleId.Value);
                    if (userProfile == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }

                    var deliverableFulfillment = _mapper.Map<DeliverableFulfillment>(deliverableFulfillmentReceivingDTO);
                    deliverableFulfillment.TaskFullfillmentId = deliverableFulfillmentToUpdate.TaskFullfillmentId;
                    deliverableFulfillment.CreatedById = context.GetLoggedInUserId();
                    var savedDeliverableFulfillment = await _deliverableFulfillmentRepo.SaveDeliverableFulfillment(deliverableFulfillment);
                    if (savedDeliverableFulfillment == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }

                    var summary = $"Initial details before change, \n {deliverableFulfillmentToUpdate.ToString()} \n";

                    deliverableFulfillmentToUpdate.WasReassigned = true;
                    deliverableFulfillmentToUpdate.DateTimeReassigned = DateTime.Now;
                    deliverableFulfillmentToUpdate.DeliverableStatus = true;

                    var updatedDeliverableFulfillment = await _deliverableFulfillmentRepo.UpdateDeliverableFulfillment(deliverableFulfillmentToUpdate);

                    summary += $"Details after change, \n {updatedDeliverableFulfillment.ToString()} \n";

                    if (updatedDeliverableFulfillment == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                    ModificationHistory history = new ModificationHistory()
                    {
                        ModelChanged = "DeliverableFulfillment",
                        ChangeSummary = summary,
                        ChangedById = context.GetLoggedInUserId(),
                        ModifiedModelId = updatedDeliverableFulfillment.Id
                    };

                    await _historyRepo.SaveHistory(history);

                    await CheckAllDeliverablesAndSetTaskAssignmentStatus(updatedDeliverableFulfillment.TaskFullfillmentId);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var deliverableFulfillmentTransferDTOs = _mapper.Map<DeliverableFulfillmentTransferDTO>(savedDeliverableFulfillment);
                    return CommonResponse.Send(ResponseCodes.SUCCESS,deliverableFulfillmentTransferDTOs);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }        
        }

        public async Task<ApiCommonResponse> SetIsPicked(HttpContext context, long id)
        {
            var deliverableFulfillmentToUpdate = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
            if (deliverableFulfillmentToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {deliverableFulfillmentToUpdate.ToString()} \n";

            deliverableFulfillmentToUpdate.IsPicked = true;
            deliverableFulfillmentToUpdate.DateAndTimePicked = DateTime.Now;

            var updatedDeliverableFulfillment = await _deliverableFulfillmentRepo.UpdateDeliverableFulfillment(deliverableFulfillmentToUpdate);

            summary += $"Details after change, \n {updatedDeliverableFulfillment.ToString()} \n";

            if (updatedDeliverableFulfillment == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "DeliverableFulfillment",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedDeliverableFulfillment.Id
            };

            await _historyRepo.SaveHistory(history);

            var deliverableFulfillmentTransferDTOs = _mapper.Map<DeliverableFulfillmentTransferDTO>(updatedDeliverableFulfillment);
            return CommonResponse.Send(ResponseCodes.SUCCESS,deliverableFulfillmentTransferDTOs);

        }

        public async Task<ApiCommonResponse> SetRequestedForValidation(HttpContext context, long id, DeliverableFulfillmentApprovalReceivingDTO dto)
        {
            var deliverableFulfillmentToUpdate = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
            if (deliverableFulfillmentToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {deliverableFulfillmentToUpdate.ToString()} \n";

            deliverableFulfillmentToUpdate.IsRequestedForValidation = true;
            deliverableFulfillmentToUpdate.DateTimeRequestedForValidation = DateTime.Now;
            deliverableFulfillmentToUpdate.DeliverableCompletionReferenceNo = dto.DeliverableCompletionReferenceNo;
            deliverableFulfillmentToUpdate.DeliverableCompletionReferenceUrl = dto.DeliverableCompletionReferenceUrl;
            var updatedDeliverableFulfillment = await _deliverableFulfillmentRepo.UpdateDeliverableFulfillment(deliverableFulfillmentToUpdate);

            summary += $"Details after change, \n {updatedDeliverableFulfillment.ToString()} \n";

            if (updatedDeliverableFulfillment == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "DeliverableFulfillment",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedDeliverableFulfillment.Id
            };

            await _historyRepo.SaveHistory(history);

            var deliverableFulfillmentTransferDTOs = _mapper.Map<DeliverableFulfillmentTransferDTO>(updatedDeliverableFulfillment);
            return CommonResponse.Send(ResponseCodes.SUCCESS,deliverableFulfillmentTransferDTOs);

        }

        public async Task<ApiCommonResponse> SetDeliveredStatus(HttpContext context, long id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var deliverableFulfillmentToUpdate = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
                    if (deliverableFulfillmentToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
                    }

                    var summary = $"Initial details before change, \n {deliverableFulfillmentToUpdate.ToString()} \n";

                    deliverableFulfillmentToUpdate.DeliverableStatus = true;
                    deliverableFulfillmentToUpdate.DeliverableCompletionTime = DateTime.Now;

                    var updatedDeliverableFulfillment = await _deliverableFulfillmentRepo.UpdateDeliverableFulfillment(deliverableFulfillmentToUpdate);

                    summary += $"Details after change, \n {updatedDeliverableFulfillment.ToString()} \n";

                    if (updatedDeliverableFulfillment == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }

                    bool allTaskDeliverablesDelivered = _context.DeliverableFulfillments.Where(x => x.TaskFullfillmentId == updatedDeliverableFulfillment.TaskFullfillmentId).All(x => x.DeliverableStatus == true);
                    if (allTaskDeliverablesDelivered)
                    {
                        var taskFulfillment = updatedDeliverableFulfillment.TaskFullfillment;
                        taskFulfillment.TaskCompletionStatus = true;
                        taskFulfillment.TaskCompletionDateTime = DateTime.Now;

                        var updatedTaskFulfillment = await _taskFulfillmentRepo.UpdateTaskFulfillment(taskFulfillment);

                        if (updatedTaskFulfillment == null)
                        {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                        }
                    }

                    ModificationHistory history = new ModificationHistory()
                    {
                        ModelChanged = "DeliverableFulfillment",
                        ChangeSummary = summary,
                        ChangedById = context.GetLoggedInUserId(),
                        ModifiedModelId = updatedDeliverableFulfillment.Id
                    };

                    await _historyRepo.SaveHistory(history);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var deliverableFulfillmentTransferDTOs = _mapper.Map<DeliverableFulfillmentTransferDTO>(updatedDeliverableFulfillment);
                    return CommonResponse.Send(ResponseCodes.SUCCESS,deliverableFulfillmentTransferDTOs);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }
        }

        private async Task SendNewDeliverableAssignedMail(DeliverableFulfillment deliverableFulfillment)
        {
            deliverableFulfillment.Responsible = await _context.UserProfiles.FindAsync(deliverableFulfillment.ResponsibleId);
            deliverableFulfillment.TaskFullfillment = await _context.TaskFulfillments.AsNoTracking()
                .Where(x => x.Id == deliverableFulfillment.TaskFullfillmentId)
                .Include(x => x.CustomerDivision)
                .Include(x => x.Responsible)
                .Include(x => x.ContractService).ThenInclude(x => x.Service)
                .FirstOrDefaultAsync();

            var serializedDeliverableInfo = JsonConvert.SerializeObject(deliverableFulfillment, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            Action action = async () =>
            {
                await _mailAdapter.SendNewDeliverableAssigned(serializedDeliverableInfo);
            };
            action.RunAsTask();
        }
    }
}