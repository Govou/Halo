using System;
using System.Collections.Generic;
using HalobizMigrations.Models.ProjectManagement;

namespace HaloBiz.DTOs.ProjectManagementDTO
{
    public class CalenderRequestDTO
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<MeetingStaffDTO> StaffsInvolved { get; set;}
        public List<ContactsInvolvedDTO> ContactsInvolved { get; set; }
        
    }
}