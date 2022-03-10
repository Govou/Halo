using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models.OnlinePortal;
using HalobizMigrations.Data;

namespace OnlinePortalBackend.Repository.Impl
{
    public class PortalComplaintRepositoryImpl : IPortalComplaintRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<PortalComplaintRepositoryImpl> _logger;
        public PortalComplaintRepositoryImpl(HalobizContext context, ILogger<PortalComplaintRepositoryImpl> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<PortalComplaint> SavePortalComplaint(PortalComplaint controlRoomAlert)
        {
            var controlRoomAlertEntity = await _context.PortalComplaints.AddAsync(controlRoomAlert);
            if (await SaveChanges())
            {
                return controlRoomAlertEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdatePortalComplaints(IEnumerable<PortalComplaint> controlRoomAlerts)
        {
            _context.PortalComplaints.UpdateRange(controlRoomAlerts);
            return await SaveChanges();
        }

        public async Task<PortalComplaint> FindPortalComplaintById(long Id)
        {
            return await _context.PortalComplaints
                .FirstOrDefaultAsync(user => user.Id == Id && user.IsDeleted == false);
        }

        public async Task<IEnumerable<PortalComplaint>> FindAllPortalComplaints()
        {
            return await _context.PortalComplaints
                .Where(user => user.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<PortalComplaint> UpdatePortalComplaint(PortalComplaint controlRoomAlert)
        {
            var PortalComplaintEntity = _context.PortalComplaints.Update(controlRoomAlert);
            if (await SaveChanges())
            {
                return PortalComplaintEntity.Entity;
            }
            return null;
        }

        public async Task<bool> RemovePortalComplaint(PortalComplaint controlRoomAlert)
        {
            controlRoomAlert.IsDeleted = true;
            _context.PortalComplaints.Update(controlRoomAlert);
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