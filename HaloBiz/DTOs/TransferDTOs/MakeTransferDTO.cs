using HalobizMigrations.Models;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class MakeTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
    }
}