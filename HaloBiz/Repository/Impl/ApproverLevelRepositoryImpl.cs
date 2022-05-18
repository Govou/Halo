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

    public class ApproverLevelRepositoryImpl : IApproverLevelRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ApproverLevelRepositoryImpl> _logger;
        public ApproverLevelRepositoryImpl(HalobizContext context, ILogger<ApproverLevelRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<ApproverLevel> SaveApproverLevel(ApproverLevel approverLevel)
        {
            var approverLevelEntity = await _context.ApproverLevels.AddAsync(approverLevel);
            if(await SaveChanges())
            {
                return approverLevelEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<ApproverLevel>> GetApproverLevels()
        {
            return await _context.ApproverLevels
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<ApproverLevel> UpdateApproverLevel(ApproverLevel approverLevel)
        {
             var approverLevelEntity =  _context.ApproverLevels.Update(approverLevel);
            if(await SaveChanges())
            {
                return approverLevelEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteApproverLevel(ApproverLevel approverLevel)
        {
            approverLevel.IsDeleted = true;
            _context.ApproverLevels.Update(approverLevel);
            return await SaveChanges();
        }
        public async Task<ApproverLevel> FindApproverLevelById(long Id)
        {
           return await _context.ApproverLevels
            .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
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

        public async Task<ApprovingLevelOffice> GetLastApprovingLevelOffice()
        {
            return await _context.ApprovingLevelOffices.LastOrDefaultAsync(x => x.IsDeleted == false);
        }

        public async Task<bool> SaveApprovingLevelOffice(ApprovingLevelOffice approvingLevelOffice)
        {
            try
            {
                await _context.ApprovingLevelOffices.AddAsync(approvingLevelOffice);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception error)
            {
                _logger.LogError(error.Message);
                return false;
            }
        }

        public async Task<IEnumerable<ApprovingLevelOffice>> GetApprovingLevelOffices()
        {
            try
            {
                return await _context.ApprovingLevelOffices
                    .Include(x => x.ApprovingOfficers)
                    .Where(x => x.IsDeleted == false)
                    .ToListAsync();
            }
            catch(Exception error)
            {
                _logger.LogError(error.Message);
                return new List<ApprovingLevelOffice>();
            }
        }

        public async Task<bool> DeleteApprovingLevelOffice(long approvingLevelOfficeId)
        {
            try
            {
                var office = await _context.ApprovingLevelOffices.FirstOrDefaultAsync(x => x.Id == approvingLevelOfficeId && x.IsDeleted == false);
                office.UpdatedAt = DateTime.Now;
                office.IsDeleted = true;
                _context.ApprovingLevelOffices.Update(office);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return false;
            }
        }

        public async Task<bool> UpdateApprovingLevelOffice(ApprovingLevelOffice approverLevel)
        {
            try
            {
                _context.ApprovingLevelOffices.Update(approverLevel);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return false;
            }
        }

        public async Task<bool> RemoveApprovingLevelOfficers(List<ApprovingLevelOfficer> approvingLevelOfficers)
        {
            try
            {
                _context.ApprovingLevelOfficers.RemoveRange(approvingLevelOfficers);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return false;
            }
        }

        public async Task<bool> SaveApprovingLevelOfficers(List<ApprovingLevelOfficer> approvingLevelOfficers)
        {
            try
            {
                await _context.ApprovingLevelOfficers.AddRangeAsync(approvingLevelOfficers);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return false;
            }
        }

        public async Task<ApprovingLevelOffice> FindApprovingLevelOfficeByID(long Id)
        {
            return await _context.ApprovingLevelOffices.FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<List<ApprovingLevelOfficer>> FindApprovingLevelOfficersByOfficeID(long Id)
        {
            return await _context.ApprovingLevelOfficers.Where(x => x.ApprovingLevelOfficeId == Id).ToListAsync();
        }
    }
}