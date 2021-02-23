using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model.LAMS;
using HaloBiz.Repository.LAMS;
using halobiz_backend.DTOs.TransferDTOs.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class DeliverableFulfillmentRepositoryImpl : IDeliverableFulfillmentRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<DeliverableFulfillmentRepositoryImpl> _logger;
        public DeliverableFulfillmentRepositoryImpl(DataContext context, ILogger<DeliverableFulfillmentRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<DeliverableFulfillment> SaveDeliverableFulfillment(DeliverableFulfillment deliverableFulfillment)
        {
            var deliverableFulfillmentEntity = await _context.DeliverableFulfillments.AddAsync(deliverableFulfillment);
            if(await SaveChanges())
            {
                return deliverableFulfillmentEntity.Entity;
            }
            return null;
        }

        public async Task<DeliverableFulfillment> FindDeliverableFulfillmentById(long Id)
        {
            return await _context.DeliverableFulfillments.Include(x => x.TaskFullfillment)
                .FirstOrDefaultAsync( deliverableFulfillment => deliverableFulfillment.Id == Id && deliverableFulfillment.IsDeleted == false);
        }

        public async Task<DeliverableFulfillment> FindDeliverableFulfillmentByName(string name)
        {
            return await _context.DeliverableFulfillments
                .FirstOrDefaultAsync( deliverableFulfillment => deliverableFulfillment.Caption == name && deliverableFulfillment.IsDeleted == false);
        }

        public async Task<IEnumerable<DeliverableFulfillment>> FindAllDeliverableFulfillment()
        {
            return await _context.DeliverableFulfillments
                .Where(deliverableFulfillment => deliverableFulfillment.IsDeleted == false)
                .OrderBy(deliverableFulfillment => deliverableFulfillment.CreatedAt)
                .ToListAsync();
        }
        public async Task<IEnumerable<DeliverableFulfillment>> FindAllDeliverableFulfillmentForTaskMaster(long taskMasterId)
        {
            var year = DateTime.Now.Year;
            var taskMasterTasks = await _context.TaskFulfillments
                        .Include(x => x.DeliverableFUlfillments).ThenInclude(x => x.Responsible)
                        .Where(x => x.ResponsibleId == taskMasterId && x.CreatedAt.Year == year && x.IsDeleted == false)
                        .ToListAsync();
            var listOFDeliverable =  taskMasterTasks.Select(x => x.DeliverableFUlfillments);
            var deliverables = new List<DeliverableFulfillment>();
            foreach (var deliverable in listOFDeliverable)
            {
                deliverables.AddRange(deliverable);
            }
            return deliverables.Where(x => x.ResponsibleId != null).ToList();
        }
        public async Task<object> GetUserDeliverableStat(long userId)
        {
            var userDeliverableInWorkBench = await _context.DeliverableFulfillments
                .Where(x => x.ResponsibleId == userId && x.DeliverableStatus == false
                     && x.IsDeleted == false).ToListAsync();

            var userDeliverableAtRisk = userDeliverableInWorkBench
                        .Where(x => CheckIfDeliverableAtRisk(x.StartDate, x.EndDate)).Count();

            var userDeliverableOverdue = userDeliverableInWorkBench
                    .Where(x => x.EndDate >= DateTime.Now).Count();

            var numberOfUserDeliverableAtRisk = userDeliverableInWorkBench.Where(
                    x => CheckIfDeliverableAtRisk(x.StartDate, x.EndDate)).Count();

            var userDeliverableOnTrack = userDeliverableInWorkBench.Count()
                            - (numberOfUserDeliverableAtRisk + userDeliverableOverdue);

            var completedDeliverable =  await _context.DeliverableFulfillments
                    .Where(x => x.ResponsibleId == userId && x.DeliverableStatus == true
                     && x.IsDeleted == false).CountAsync();
            
            var numberOfEarlyDeliverableCompletion = await _context.DeliverableFulfillments
            .Where(x => x.ResponsibleId == userId && x.DeliverableStatus == true 
                     && x.IsDeleted == false && x.EndDate >= x.DeliverableCompletionDate).CountAsync();

            double earlyDeliveryRate = numberOfEarlyDeliverableCompletion == 0 ? 0 
                        : (numberOfEarlyDeliverableCompletion / completedDeliverable) * 100 ;


            
            var unPickedDeliverable = userDeliverableInWorkBench.Where(x => x.IsPicked == false).Count();

            double pickRate = unPickedDeliverable == 0 ? 0.0 : (unPickedDeliverable / userDeliverableInWorkBench.Count()) / 100.00;
            
            return new{ 
                userDeliverableOnTrack,
                userDeliverableInWorkBench = userDeliverableInWorkBench.Count(), 
                userDeliverableAtRisk = numberOfUserDeliverableAtRisk, 
                userDeliverableOverdue = userDeliverableOverdue,
                pickRate,
                earlyDeliveryRate
                };
        }

        public bool CheckIfDeliverableAtRisk(DateTime? start, DateTime? end)
        {
            if(end < DateTime.Now || start == null || end == null) 
                return false;
            var diffInDate =((DateTime) end).Subtract((DateTime)start).TotalMilliseconds;
            var diffInStartDateAndPresentDate = DateTime.Now.Subtract((DateTime)start).TotalMilliseconds;
            return (diffInStartDateAndPresentDate / diffInDate) * 100 >= 90 ? true : false;
        }
        public async Task<IEnumerable<DeliverableFulfillment>> FindAllAssignedDeliverableFulfillmentForTaskMaster(long taskMasterId)
        {
            var deliverrables = await FindAllDeliverableFulfillmentForTaskMaster( taskMasterId);
            deliverrables.ToList().RemoveAll(x => x.DeliverableStatus  ||  x.ResponsibleId == 0 || x.ResponsibleId == null);
            return deliverrables;
        }

        public async Task<DeliverableFulfillment> UpdateDeliverableFulfillment(DeliverableFulfillment deliverableFulfillment)
        {
            var deliverableFulfillmentEntity =  _context.DeliverableFulfillments.Update(deliverableFulfillment);
            if(await SaveChanges())
            {
                return deliverableFulfillmentEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteDeliverableFulfillment(DeliverableFulfillment deliverableFulfillment)
        {
            deliverableFulfillment.IsDeleted = true;
            _context.DeliverableFulfillments.Update(deliverableFulfillment);
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