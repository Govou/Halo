using System;
using HaloBiz.DTOs.TransferDTOs;

namespace HaloBiz.DTOs.ContactDTO
{
    public class ContactDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public long DesignationId { get; set; }
        public DesignationTransferDTO Designation { get; set; }
        public long ClientContactQualificationId { get; set; }
        public ClientContactQualificationTransferDTO ClientContactQualification { get; set; }
        public string DateOfBirth { get; set; }
    }
}
