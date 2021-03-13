using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class DeleteLogRepositoryImpl : IDeleteLogRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<DeleteLogRepositoryImpl> _logger;
        public DeleteLogRepositoryImpl(DataContext context, ILogger<DeleteLogRepositoryImpl> logger, IOfficeRepository officeRepository)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<DeleteLog> SaveDeleteLog(DeleteLog deleteLog)
        {
            var deleteLogEntity = await _context.DeleteLogs.AddAsync(deleteLog);
            if(await SaveChanges())
            {
                return deleteLogEntity.Entity;
            }
            return null;
        }

        public async Task<IEnumerable<DeleteLog>> FindAllDeleteLogs()
        {
            return await _context.DeleteLogs
                .ToListAsync();
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