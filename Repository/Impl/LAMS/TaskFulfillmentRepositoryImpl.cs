using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model.LAMS;
using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class TaskFulfillmentRepositoryImpl : ITaskFulfillmentRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<TaskFulfillmentRepositoryImpl> _logger;
        public TaskFulfillmentRepositoryImpl(DataContext context, ILogger<TaskFulfillmentRepositoryImpl> logger)
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
                .FirstOrDefaultAsync();
        }

        public async Task<TaskFulfillment> FindTaskFulfillmentByName(string name)
        {
            return await _context.TaskFulfillments
                .FirstOrDefaultAsync( taskFulfillment => taskFulfillment.Caption == name && taskFulfillment.IsDeleted == false);
        }

        public async Task<IEnumerable<TaskFulfillment>> FindAllTaskFulfillment()
        {
            return await _context.TaskFulfillments
                .Where(taskFulfillment => taskFulfillment.IsDeleted == false)
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