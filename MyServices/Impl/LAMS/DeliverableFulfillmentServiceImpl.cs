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
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class DeliverableFulfillmentServiceImpl : IDeliverableFulfillmentService
    {
        private readonly ILogger<DeliverableFulfillmentServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IDeliverableFulfillmentRepository _deliverableFulfillmentRepo;
        private readonly ITaskFulfillmentRepository _taskFulfillmentRepo;
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DeliverableFulfillmentServiceImpl(IModificationHistoryRepository historyRepo, 
            IDeliverableFulfillmentRepository deliverableFulfillmentRepo, 
            ITaskFulfillmentRepository taskFulfillmentRepo,
            IUserProfileRepository userProfileRepo,
            DataContext dataContext,
            ILogger<DeliverableFulfillmentServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._deliverableFulfillmentRepo = deliverableFulfillmentRepo;
            this._taskFulfillmentRepo = taskFulfillmentRepo;
            this._userProfileRepo = userProfileRepo;
            this._context = dataContext;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddDeliverableFulfillment(HttpContext context, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO)
        {
            var deliverableFulfillment = _mapper.Map<DeliverableFulfillment>(deliverableFulfillmentReceivingDTO);
            deliverableFulfillment.CreatedById = context.GetLoggedInUserId();
            var savedDeliverableFulfillment = await _deliverableFulfillmentRepo.SaveDeliverableFulfillment(deliverableFulfillment);
            if (savedDeliverableFulfillment == null)
            {
                return new ApiResponse(500);
            }
            var deliverableFulfillmentTransferDTO = _mapper.Map<DeliverableFulfillmentTransferDTO>(savedDeliverableFulfillment);
            return new ApiOkResponse(deliverableFulfillmentTransferDTO);
        }

        public async Task<ApiResponse> GetAllDeliverableFulfillment()
        {
            var deliverableFulfillments = await _deliverableFulfillmentRepo.FindAllDeliverableFulfillment();
            if (deliverableFulfillments == null)
            {
                return new ApiResponse(404);
            }
            var deliverableFulfillmentTransferDTO = _mapper.Map<IEnumerable<DeliverableFulfillmentTransferDTO>>(deliverableFulfillments);
            return new ApiOkResponse(deliverableFulfillmentTransferDTO);
        }

        public async Task<ApiResponse> GetDeliverableFulfillmentById(long id)
        {
            var deliverableFulfillment = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
            if (deliverableFulfillment == null)
            {
                return new ApiResponse(404);
            }
            var deliverableFulfillmentTransferDTOs = _mapper.Map<DeliverableFulfillmentTransferDTO>(deliverableFulfillment);
            return new ApiOkResponse(deliverableFulfillmentTransferDTOs);
        }

        public async Task<ApiResponse> GetDeliverableFulfillmentByName(string name)
        {
            var deliverableFulfillment = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentByName(name);
            if (deliverableFulfillment == null)
            {
                return new ApiResponse(404);
            }
            var deliverableFulfillmentTransferDTOs = _mapper.Map<DeliverableFulfillmentTransferDTO>(deliverableFulfillment);
            return new ApiOkResponse(deliverableFulfillmentTransferDTOs);
        }

        public async Task<ApiResponse> UpdateDeliverableFulfillment(HttpContext context, long id, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO)
        {
            var deliverableFulfillmentToUpdate = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
            if (deliverableFulfillmentToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {deliverableFulfillmentToUpdate.ToString()} \n" ;

            deliverableFulfillmentToUpdate.Caption = deliverableFulfillmentReceivingDTO.Caption;
            deliverableFulfillmentToUpdate.Description = deliverableFulfillmentReceivingDTO.Description;
            deliverableFulfillmentToUpdate.TaskFullfillmentId = deliverableFulfillmentReceivingDTO.TaskFullfillmentId;
            deliverableFulfillmentToUpdate.ResponsibleId = deliverableFulfillmentReceivingDTO.ResponsibleId;
            deliverableFulfillmentToUpdate.StartDate = deliverableFulfillmentReceivingDTO.StartDate;
            deliverableFulfillmentToUpdate.EndDate = deliverableFulfillmentReceivingDTO.EndDate;
            deliverableFulfillmentToUpdate.DeliveryDate = deliverableFulfillmentReceivingDTO.DeliveryDate;
            deliverableFulfillmentToUpdate.TaskCompletionDate = deliverableFulfillmentReceivingDTO.TaskCompletionDate;
            deliverableFulfillmentToUpdate.TaskCompletionTime = deliverableFulfillmentReceivingDTO.TaskCompletionTime;
            deliverableFulfillmentToUpdate.Budget = deliverableFulfillmentReceivingDTO.Budget;
            deliverableFulfillmentToUpdate.DeliverableCompletionReferenceNo = deliverableFulfillmentReceivingDTO.DeliverableCompletionReferenceNo;
            deliverableFulfillmentToUpdate.DeliverableCompletionReferenceUrl = deliverableFulfillmentReceivingDTO.DeliverableCompletionReferenceUrl;
            deliverableFulfillmentToUpdate.DateAndTimeOfProvidedEvidence = deliverableFulfillmentReceivingDTO.DateAndTimeOfProvidedEvidence;
            deliverableFulfillmentToUpdate.DeliverableCompletionDate = deliverableFulfillmentReceivingDTO.DeliverableCompletionDate;
            deliverableFulfillmentToUpdate.DeliverableCompletionTime = deliverableFulfillmentReceivingDTO.DeliverableCompletionTime;

            var updatedDeliverableFulfillment = await _deliverableFulfillmentRepo.UpdateDeliverableFulfillment(deliverableFulfillmentToUpdate);

            summary += $"Details after change, \n {updatedDeliverableFulfillment.ToString()} \n";

            if (updatedDeliverableFulfillment == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "DeliverableFulfillment",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedDeliverableFulfillment.Id
            };

            await _historyRepo.SaveHistory(history);

            var deliverableFulfillmentTransferDTOs = _mapper.Map<DeliverableFulfillmentTransferDTO>(updatedDeliverableFulfillment);
            return new ApiOkResponse(deliverableFulfillmentTransferDTOs);

        }

        public async Task<ApiResponse> DeleteDeliverableFulfillment(long id)
        {
            var deliverableFulfillmentToDelete = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
            if (deliverableFulfillmentToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _deliverableFulfillmentRepo.DeleteDeliverableFulfillment(deliverableFulfillmentToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> SetWhoIsResponsible(HttpContext context, long id, long userProfileId)
        {
            var deliverableFulfillmentToUpdate = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
            if (deliverableFulfillmentToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var userProfile = await _userProfileRepo.FindUserById(userProfileId);
            if (userProfile == null)
            {
                return new ApiResponse(500);
            }

            var summary = $"Initial details before change, \n {deliverableFulfillmentToUpdate.ToString()} \n";

            deliverableFulfillmentToUpdate.ResponsibleId = userProfileId;

            var updatedDeliverableFulfillment = await _deliverableFulfillmentRepo.UpdateDeliverableFulfillment(deliverableFulfillmentToUpdate);

            summary += $"Details after change, \n {updatedDeliverableFulfillment.ToString()} \n";

            if (updatedDeliverableFulfillment == null)
            {
                return new ApiResponse(500);
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
            return new ApiOkResponse(deliverableFulfillmentTransferDTOs);

        }

        public async Task<ApiResponse> ReAssignDeliverableFulfillment(HttpContext context, long id, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var deliverableFulfillmentToUpdate = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
                    if (deliverableFulfillmentToUpdate == null)
                    {
                        return new ApiResponse(404);
                    }

                    var userProfile = await _userProfileRepo.FindUserById(deliverableFulfillmentReceivingDTO.ResponsibleId.Value);
                    if (userProfile == null)
                    {
                        return new ApiResponse(500);
                    }

                    var deliverableFulfillment = _mapper.Map<DeliverableFulfillment>(deliverableFulfillmentReceivingDTO);
                    deliverableFulfillment.TaskFullfillmentId = deliverableFulfillmentToUpdate.TaskFullfillmentId;
                    deliverableFulfillment.CreatedById = context.GetLoggedInUserId();
                    var savedDeliverableFulfillment = await _deliverableFulfillmentRepo.SaveDeliverableFulfillment(deliverableFulfillment);
                    if (savedDeliverableFulfillment == null)
                    {
                        return new ApiResponse(500);
                    }

                    var summary = $"Initial details before change, \n {deliverableFulfillmentToUpdate.ToString()} \n";

                    deliverableFulfillmentToUpdate.WasReassigned = true;
                    deliverableFulfillmentToUpdate.DateTimeReassigned = DateTime.Now;
                    deliverableFulfillmentToUpdate.DeliverableStatus = true;

                    var updatedDeliverableFulfillment = await _deliverableFulfillmentRepo.UpdateDeliverableFulfillment(deliverableFulfillmentToUpdate);

                    summary += $"Details after change, \n {updatedDeliverableFulfillment.ToString()} \n";

                    if (updatedDeliverableFulfillment == null)
                    {
                        return new ApiResponse(500);
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

                    var deliverableFulfillmentTransferDTOs = _mapper.Map<DeliverableFulfillmentTransferDTO>(savedDeliverableFulfillment);
                    return new ApiOkResponse(deliverableFulfillmentTransferDTOs);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    return new ApiResponse(500);
                }
            }        
        }

        public async Task<ApiResponse> SetIsPicked(HttpContext context, long id)
        {
            var deliverableFulfillmentToUpdate = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
            if (deliverableFulfillmentToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {deliverableFulfillmentToUpdate.ToString()} \n";

            deliverableFulfillmentToUpdate.IsPicked = true;
            deliverableFulfillmentToUpdate.DateAndTimePicked = DateTime.Now;

            var updatedDeliverableFulfillment = await _deliverableFulfillmentRepo.UpdateDeliverableFulfillment(deliverableFulfillmentToUpdate);

            summary += $"Details after change, \n {updatedDeliverableFulfillment.ToString()} \n";

            if (updatedDeliverableFulfillment == null)
            {
                return new ApiResponse(500);
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
            return new ApiOkResponse(deliverableFulfillmentTransferDTOs);

        }

        public async Task<ApiResponse> SetRequestedForValidation(HttpContext context, long id)
        {
            var deliverableFulfillmentToUpdate = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
            if (deliverableFulfillmentToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {deliverableFulfillmentToUpdate.ToString()} \n";

            deliverableFulfillmentToUpdate.IsRequestedForValidation = true;
            deliverableFulfillmentToUpdate.DateTimeRequestedForValidation = DateTime.Now;
            var updatedDeliverableFulfillment = await _deliverableFulfillmentRepo.UpdateDeliverableFulfillment(deliverableFulfillmentToUpdate);

            summary += $"Details after change, \n {updatedDeliverableFulfillment.ToString()} \n";

            if (updatedDeliverableFulfillment == null)
            {
                return new ApiResponse(500);
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
            return new ApiOkResponse(deliverableFulfillmentTransferDTOs);

        }

        public async Task<ApiResponse> SetDeliveredStatus(HttpContext context, long id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var deliverableFulfillmentToUpdate = await _deliverableFulfillmentRepo.FindDeliverableFulfillmentById(id);
                    if (deliverableFulfillmentToUpdate == null)
                    {
                        return new ApiResponse(404);
                    }

                    var summary = $"Initial details before change, \n {deliverableFulfillmentToUpdate.ToString()} \n";

                    deliverableFulfillmentToUpdate.DeliverableStatus = true;
                    deliverableFulfillmentToUpdate.DeliverableCompletionTime = DateTime.Now;

                    var updatedDeliverableFulfillment = await _deliverableFulfillmentRepo.UpdateDeliverableFulfillment(deliverableFulfillmentToUpdate);

                    summary += $"Details after change, \n {updatedDeliverableFulfillment.ToString()} \n";

                    if (updatedDeliverableFulfillment == null)
                    {
                        return new ApiResponse(500);
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
                            return new ApiResponse(500);
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
                    return new ApiOkResponse(deliverableFulfillmentTransferDTOs);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    return new ApiResponse(500);
                }
            }
        }
    }
}