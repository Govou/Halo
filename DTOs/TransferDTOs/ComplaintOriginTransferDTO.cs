using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ComplaintOriginTransferDTO : BaseSetupTransferDTO
    {
        public string Code { get; set; }
    }
}