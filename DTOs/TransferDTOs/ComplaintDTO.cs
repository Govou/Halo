using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ComplaintDTO
    {
    }
    public class ComplaintHandlingDTO
    {
        public List<ComplaintTransferDTO> assignedComplaints { get; set; }
        public List<ComplaintTransferDTO> workbenchComplaints { get; set; }
        public int TotalComplaintsAssigned { get; set; }
        public int TotalComplaintsInWorkbench { get; set; }
        public int TotalComplaintsBeingHandled { get; set; }
        public int TotalComplaintsClosed { get; set; }
    }

    public class ComplaintHandlingStatsDTO
    {
        public int TotalComplaintsAssigned { get; set; }
        public int TotalComplaintsInWorkbench { get; set; }
        public int TotalComplaintsBeingHandled { get; set; }
        public int TotalComplaintsClosed { get; set; }
    }
}
