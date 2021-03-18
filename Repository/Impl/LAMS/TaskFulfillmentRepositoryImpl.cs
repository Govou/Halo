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
        public  IQueryable<TaskFulfillment> GetTaskFulfillmentQueryable()
        {
            return _context.TaskFulfillments.Where(x => !x.IsDeleted).AsQueryable();
        }

        public async Task<TaskFulfillment> FindTaskFulfillmentById(long Id)
        {
            return await _context.TaskFulfillments
                .Where(taskFulfillment => taskFulfillment.Id == Id && taskFulfillment.IsDeleted == false)
                .Include(x => x.ContractService).ThenInclude(x => x.SBUToContractServiceProportions)
                .Include(x => x.Support)
                .Include(x => x.Responsible)
                .Include(x => x.Accountable)
                .Include(x => x.DeliverableFUlfillments.Where(x => x.IsDeleted == false && x.WasReassigned == false)).ThenInclude(x => x.Responsible)
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

            var query = from customerDivision in _context.CustomerDivisions
                join task in _context.TaskFulfillments on
                    customerDivision.Id equals task.CustomerDivisionId
                join deliverables in _context.DeliverableFulfillments on 
                    task.Id equals deliverables.TaskFullfillmentId
                join userProfile in _context.UserProfiles on 
                    deliverables.ResponsibleId equals userProfile.Id
                join contractService in _context.ContractServices on
                    task.ContractServiceId equals contractService.Id
                join service in _context.Services on
                    contractService.ServiceId equals service.Id
                where deliverables.ResponsibleId == responsibleId && 
                    !deliverables.DeliverableStatus && !deliverables.IsDeleted
                    && !deliverables.WasReassigned
                select new TaskDeliverablesSummary(){
                        TaskCaption = task.Caption,
                        TaskId  = task.Id,
                        TaskResponsibleId =   task.ResponsibleId,
                        DeliverableId = deliverables.Id,
                        DeliverableStatus = deliverables.DeliverableStatus,
                        IsPicked = deliverables.IsPicked,
                        DeliverableCaption = deliverables.Caption,
                        DeliveryDate = deliverables.EndDate?? DateTime.Now,
                        Priority = deliverables.Priority?? 0,
                        DeliverableWasReassigned = deliverables.WasReassigned,
                        TaskResponsibleImageUrl = userProfile.ImageUrl,
                        DeliverableResponsibleId = deliverables.ResponsibleId,
                        TaskResponsibleName = $"{userProfile.FirstName} {userProfile.LastName}",
                        StartDate =  deliverables.StartDate?? DateTime.Now ,
                        IsRequestedForValidation = deliverables.IsRequestedForValidation,
                        ServiceName = service.Name,
                        CustomerDivision = customerDivision.DivisionName

                };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TaskFulfillment>> FindAllTaskFulfillment()
        {
            return await _context.TaskFulfillments
                .Where(taskFulfillment => taskFulfillment.IsDeleted == false)
                .OrderBy(taskFulfillment => taskFulfillment.CreatedAt)
                .ToListAsync();
        }
        public async Task<IEnumerable<TaskFulfillment>> GetTaskFulfillmentsByCustomerDivisionId(long customerDivsionId)
        {
            return await _context.TaskFulfillments   
                .Include(x => x.DeliverableFUlfillments.Where(x => x.IsDeleted == false && x.WasReassigned == false))
                .Where(x => x.CustomerDivisionId == customerDivsionId && x.IsDeleted == false).ToListAsync();
        }

         public async Task<IEnumerable<TaskFulfillment>> FindAllTaskFulfillmentForTaskOwner(long taskOwnerId)
        {
            return await _context.TaskFulfillments
                .Where(taskFulfillment => taskFulfillment.IsDeleted == false 
                    && (taskFulfillment.AccountableId == taskOwnerId || taskFulfillment.SupportId == taskOwnerId))
                .Include(x => x.DeliverableFUlfillments.Where(x => x.WasReassigned == false && x.IsDeleted == false))
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