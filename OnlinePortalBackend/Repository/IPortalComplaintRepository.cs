using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models.OnlinePortal;

namespace OnlinePortalBackend.Repository
{
    public interface IPortalComplaintRepository
    {
        Task<PortalComplaint> SavePortalComplaint(PortalComplaint controlRoomAlert);
        Task<bool> UpdatePortalComplaints(IEnumerable<PortalComplaint> controlRoomAlert);
        Task<PortalComplaint> FindPortalComplaintById(long Id);
        Task<IEnumerable<PortalComplaint>> FindAllPortalComplaints();
        Task<PortalComplaint> UpdatePortalComplaint(PortalComplaint controlRoomAlert);
        Task<bool> RemovePortalComplaint(PortalComplaint controlRoomAlert);
    }
}