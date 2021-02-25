using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Helpers;
using HaloBiz.Model.LAMS;
using HaloBiz.Repository.LAMS;
using halobiz_backend.DTOs.TransferDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class TaskFulfillmentRepositoryImpl : ITaskFulfillmentRepository
    {
        private readonly DataContext _context;
        private readonly IServicesRepository _servicesRepo;
        private readonly ILogger<TaskFulfillmentRepositoryImpl> _logger;
        public TaskFulfillmentRepositoryImpl(DataContext context,
                            ILogger<TaskFulfillmentRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<TaskFulfillment> SaveTaskFulfillment(TaskFulfillment taskFulfillment)
        {
            var taskFulfillmentEntity = await _context.TaskFulfillments.AddAsync(taskFulfillment);
            if(await SaveChanges())
            {
                return taskFulfillmentEntity.Entity;
            }
            return null;
        }

        public async Task<TaskFulfillment> FindTaskFulfillmentById(long Id)
        {
            return await _context.TaskFulfillments
                .Where(taskFulfillment => taskFulfillment.Id == Id && taskFulfillment.IsDeleted == false)
                .Include(x => x.ContractService).ThenInclude(x => x.SBUToContractServiceProportions)
                .Include(x => x.Support)
                .Include(x => x.Responsible)
                .Include(x => x.Accountable)
                .Include(x => x.DeliverableFUlfillments).ThenInclude(x => x.Responsible)
                .FirstOrDefaultAsync();

        }

        public async Task<TaskFulfillment> FindTaskFulfillmentByName(string name)
        {
            return await _context.TaskFulfillments
                .Include(x => x.Support)
                .Include(x => x.Responsible)
                .Include(x => x.Accountable)
                .Include(x => x.DeliverableFUlfillments)
                .FirstOrDefaultAsync( taskFulfillment => taskFulfillment.Caption == name && taskFulfillment.IsDeleted == false);
        }
        public async Task<IEnumerable<TaskDeliverablesSummary>> GetTaskDeliverablesSummary(long responsibleId)
        {
            var userData = await _context.TaskFulfillments.Join(
                _context.DeliverableFulfillments, 
                    task => task.Id, deliverable => deliverable.TaskFullfillmentId,
                    (task, deliverable) => new {
                        DeliverableStatus = deliverable.DeliverableStatus,
                        TaskCaption = task.Caption,
                        TaskId  = task.Id,
                        TaskResponsibleId =  task.ResponsibleId,
                        DeliverableId = deliverable.Id,
                        IsPicked = deliverable.IsPicked,
                        DeliverableCaption = deliverable.Caption,
                        DeliveryDate = deliverable.EndDate?? DateTime.Now,
                        StartDate = deliverable.StartDate?? DateTime.Now,
                        Priority = deliverable.Priority?? 0 ,
                        DeliverableResponsibleId = deliverable.ResponsibleId
                    }
            ).Join(
                _context.UserProfiles,
                    task => task.TaskResponsibleId, taskOwner => taskOwner.Id,
                    (task, taskOwner) => new TaskDeliverablesSummary(){
                        TaskCaption = task.TaskCaption,
                        TaskId  = task.TaskId,
                        TaskResponsibleId =   task.TaskResponsibleId,
                        DeliverableId = task.DeliverableId,
                        DeliverableStatus = task.DeliverableStatus,
                        IsPicked = task.IsPicked,
                        DeliverableCaption = task.DeliverableCaption,
                        DeliveryDate = task.DeliveryDate,
                        Priority = task.Priority,
                        TaskResponsibleImageUrl = taskOwner.ImageUrl,
                        DeliverableResponsibleId = task.DeliverableResponsibleId,
                        TaskResponsibleName = $"{taskOwner.FirstName} {taskOwner.LastName}",
                        StartDate =  task.StartDate
                    }
            ).Where(x => x.DeliverableResponsibleId == responsibleId && x.DeliverableStatus == false).ToListAsync();

            return userData;
        }

        public async Task<IEnumerable<TaskFulfillment>> FindAllTaskFulfillment()
        {
            return await _context.TaskFulfillments
                .Where(taskFulfillment => taskFulfillment.IsDeleted == false)
                .OrderBy(taskFulfillment => taskFulfillment.CreatedAt)
                .ToListAsync();
        }

         public async Task<IEnumerable<TaskFulfillment>> FindAllTaskFulfillmentForTaskOwner(long taskOwnerId)
        {
            return await _context.TaskFulfillments
                .Where(taskFulfillment => taskFulfillment.IsDeleted == false 
                    && (taskFulfillment.AccountableId == taskOwnerId || taskFulfillment.SupportId == taskOwnerId))
                .Include(x => x.DeliverableFUlfillments)
                .OrderBy(taskFulfillment => taskFulfillment.CreatedAt)
                .ToListAsync();
        }

        public async Task<TaskFulfillment> UpdateTaskFulfillment(TaskFulfillment taskFulfillment)
        {
            var taskFulfillmentEntity =  _context.TaskFulfillments.Update(taskFulfillment);
            if(await SaveChanges())
            {
                return taskFulfillmentEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteTaskFulfillment(TaskFulfillment taskFulfillment)
        {
            taskFulfillment.IsDeleted = true;
            _context.TaskFulfillments.Update(taskFulfillment);
            return await SaveChanges();
        }
        private async Task<bool> SaveChanges()
        {
           try{
               return  await _context.SaveChangesAsync() > 0;
           }catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }

 
}