using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class ModificationHistoryRepositoryImpl : IModificationHistoryRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ModificationHistoryRepositoryImpl> _logger;
        public ModificationHistoryRepositoryImpl(HalobizContext context, ILogger<ModificationHistoryRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }

        public async Task<ModificationHistory> SaveHistory(ModificationHistory history)
        {
            var savedEntity = await _context.ModificationHistories.AddAsync(history);
            if(await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ModificationHistory> FindHistoryByEntityChanged(string entityName)
        {
            return await _context.ModificationHistories
                .Include(history => history.ChangedBy)
                .FirstOrDefaultAsync( history => history.ModelChanged.ToLower() == entityName.ToLower());
        }

        public async Task<ModificationHistory> FindHistoryByUserName(string firstName)
        {
            return await _context.ModificationHistories
                .Include(history => history.ChangedBy)
                .FirstOrDefaultAsync( history => history.ChangedBy.FirstName.ToLower() == firstName.ToLower());
        }

        public async Task<IEnumerable<ModificationHistory>> FindAllModifications()
        {
            return await _context.ModificationHistories
                .Include(history => history.ChangedBy)
                .ToListAsync();
        }


        public async Task<bool> RemoveModificationRecord(ModificationHistory record)
        {
            _context.ModificationHistories.Remove(record);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            try{
                return await _context.SaveChangesAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}