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
    public class ComplaintRepositoryImpl : IComplaintRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ComplaintRepositoryImpl> _logger;
        public ComplaintRepositoryImpl(HalobizContext context, ILogger<ComplaintRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteComplaint(Complaint complaint)
        {
            complaint.IsDeleted = true;
            _context.Complaints.Update(complaint);
            return await SaveChanges();
        }

        public async Task<IEnumerable<Complaint>> FindAllComplaints()
        {
            return await _context.Complaints
               .Where(complaint => complaint.IsDeleted == false)
               .OrderBy(complaint => complaint.CreatedAt)
               .ToListAsync();
        }

        public async Task<Complaint> FindComplaintById(long Id)
        {
            return await _context.Complaints
                .Where(complaint => complaint.IsDeleted == false)
                .FirstOrDefaultAsync(complaint => complaint.Id == Id && complaint.IsDeleted == false);

        }

        /*public async Task<Complaint> FindComplaintByName(string name)
        {
            return await _context.Complaints
                 .Where(complaint => complaint.IsDeleted == false)
                 .FirstOrDefaultAsync(complaint => complaint.Caption == name && complaint.IsDeleted == false);

        }*/

        public async Task<Complaint> SaveComplaint(Complaint complaint)
        {
            var complaintEntity = await _context.Complaints.AddAsync(complaint);
            if (await SaveChanges())
            {
                return complaintEntity.Entity;
            }
            return null;
        }

        public async Task<Complaint> UpdateComplaint(Complaint complaint)
        {
            var complaintEntity = _context.Complaints.Update(complaint);
            if (await SaveChanges())
            {
                return complaintEntity.Entity;
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
