using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{

    public class ProcessesRequiringApprovalRepositoryImpl : IProcessesRequiringApprovalRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ProcessesRequiringApprovalRepositoryImpl> _logger;
        public ProcessesRequiringApprovalRepositoryImpl(HalobizContext context, ILogger<ProcessesRequiringApprovalRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<ProcessesRequiringApproval> SaveProcessesRequiringApproval(ProcessesRequiringApproval processesRequiringApproval)
        {
            var processesRequiringApprovalEntity = await _context.ProcessesRequiringApprovals.AddAsync(processesRequiringApproval);
            if(await SaveChanges())
            {
                return processesRequiringApprovalEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<ProcessesRequiringApproval>> GetProcessesRequiringApprovals()
        {
            return await _context.ProcessesRequiringApprovals
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<ProcessesRequiringApproval> UpdateProcessesRequiringApproval(ProcessesRequiringApproval processesRequiringApproval)
        {
             var processesRequiringApprovalEntity =  _context.ProcessesRequiringApprovals.Update(processesRequiringApproval);
            if(await SaveChanges())
            {
                return processesRequiringApprovalEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteProcessesRequiringApproval(ProcessesRequiringApproval processesRequiringApproval)
        {
            processesRequiringApproval.IsDeleted = true;
            _context.ProcessesRequiringApprovals.Update(processesRequiringApproval);
            return await SaveChanges();
        }
        public async Task<ProcessesRequiringApproval> FindProcessesRequiringApprovalById(long Id)
        {
           return await _context.ProcessesRequiringApprovals
            .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<ProcessesRequiringApproval> FindProcessesRequiringApprovalByCaption(string caption)
        {
            return await _context.ProcessesRequiringApprovals
             .FirstOrDefaultAsync(x => x.Caption == caption && x.IsDeleted == false);
        }

        private async Task<bool> SaveChanges()
        {
           try
           {
               return  await _context.SaveChangesAsync() > 0;
           }
           catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}