using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IProcessesRequiringApprovalRepository
    {
        Task<ProcessesRequiringApproval> SaveProcessesRequiringApproval(ProcessesRequiringApproval processesRequiringApproval);
        Task<IEnumerable<ProcessesRequiringApproval>> GetProcessesRequiringApprovals();
        Task<ProcessesRequiringApproval> UpdateProcessesRequiringApproval(ProcessesRequiringApproval processesRequiringApproval);
        Task<bool> DeleteProcessesRequiringApproval(ProcessesRequiringApproval processesRequiringApproval);
        Task<ProcessesRequiringApproval> FindProcessesRequiringApprovalById(long Id);
        Task<ProcessesRequiringApproval> FindProcessesRequiringApprovalByCaption(string caption);
    }
}