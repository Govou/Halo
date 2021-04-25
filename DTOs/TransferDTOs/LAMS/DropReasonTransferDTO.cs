using System.Collections.Generic;
using HalobizMigrations.Models;


namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class DropReasonTransferDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
    }
}