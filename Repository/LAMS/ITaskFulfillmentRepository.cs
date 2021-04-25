using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using halobiz_backend.DTOs.TransferDTOs;
using HalobizMigrations.Models;

namespace HaloBiz.Repository.LAMS
{
    public interface ITaskFulfillmentRepository
    {
        Task<TaskFulfillment> SaveTaskFulfillment(TaskFulfillment taskFulfillment);
        Task<TaskFulfillment> FindTaskFulfillmentById(long Id);
        Task<TaskFulfillment> FindTaskFulfillmentByName(string name);
        Task<IEnumerable<TaskFulfillment>> FindAllTaskFulfillment();
        Task<IEnumerable<TaskFulfillment>> FindAllTaskFulfillmentForTaskOwner(long taskOwnerId);
        Task<TaskFulfillment> UpdateTaskFulfillment(TaskFulfillment taskFulfillment);
        Task<bool> DeleteTaskFulfillment(TaskFulfillment taskFulfillment);
        Task<IEnumerable<TaskDeliverablesSummary>> GetTaskDeliverablesSummary(long responsibleId);
        Task<IEnumerable<TaskFulfillment>> GetTaskFulfillmentsByCustomerDivisionId(long customerDivsionId);
        IQueryable<TaskFulfillment> GetTaskFulfillmentQueryable();
    }
}