using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class EscalationMatrixRepositoryImpl : IEscalationMatrixRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<EscalationMatrixRepositoryImpl> _logger;
        public EscalationMatrixRepositoryImpl(HalobizContext context, ILogger<EscalationMatrixRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteEscalationMatrix(EscalationMatrix escalationMatrix)
        {
            escalationMatrix.IsDeleted = true;
            _context.EscalationMatrices.Update(escalationMatrix);
            return await SaveChanges();
        }

        public async Task<IEnumerable<EscalationMatrix>> FindAllEscalationMatrixs()
        {
            return await _context.EscalationMatrices
                .Include(escalationMatrix => escalationMatrix.ComplaintAttendants)
               .Where(escalationMatrix => escalationMatrix.IsDeleted == false)
               .OrderBy(escalationMatrix => escalationMatrix.CreatedAt)
               .ToListAsync();
        }

        public async Task<EscalationMatrix> FindEscalationMatrixById(long Id)
        {
            return await _context.EscalationMatrices
                .Include(escalationMatrix => escalationMatrix.ComplaintAttendants)
                .Where(escalationMatrix => escalationMatrix.IsDeleted == false)
                .FirstOrDefaultAsync(escalationMatrix => escalationMatrix.Id == Id && escalationMatrix.IsDeleted == false);

        }

        /*public async Task<EscalationMatrix> FindEscalationMatrixByName(string name)
        {
            return await _context.EscalationMatrices
                 .Where(escalationMatrix => escalationMatrix.IsDeleted == false)
                 .FirstOrDefaultAsync(escalationMatrix => escalationMatrix.Caption == name && escalationMatrix.IsDeleted == false);

        }*/

        public async Task<EscalationMatrix> SaveEscalationMatrix(EscalationMatrix escalationMatrix)
        {
            var escalationMatrixEntity = await _context.EscalationMatrices.AddAsync(escalationMatrix);
            if (await SaveChanges())
            {
                return escalationMatrixEntity.Entity;
            }
            return null;
        }

        public async Task<EscalationMatrix> UpdateEscalationMatrix(EscalationMatrix escalationMatrix)
        {
            var escalationMatrixEntity = _context.EscalationMatrices.Update(escalationMatrix);
            if (await SaveChanges())
            {
                return escalationMatrixEntity.Entity;
            }
            return null;
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
