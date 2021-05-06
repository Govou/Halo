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
        public List<ComplaintDTO> assignedComplaints { get; set; }
        public List<ComplaintDTO> workbenchComplaints { get; set; }
    }

    public class ComplaintHandlingStatsDTO
    {
        public int TotalComplaintsAssigned { get; set; }
        public int TotalComplaintsInWorkbench { get; set; }
        public int TotalComplaintsBeingHandled { get; set; }
        public int ToalComplaintsClosed { get; set; }
    }
}
