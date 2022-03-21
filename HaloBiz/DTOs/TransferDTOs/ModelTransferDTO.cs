using HalobizMigrations.Models;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ModelTransferDTO
    {
        public long Id { get; set; }
        public long MakeId { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
    }
}