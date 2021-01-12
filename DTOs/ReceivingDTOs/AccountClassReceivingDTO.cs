using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class AccountClassReceivingDTO
    {
    [Required]
    public string Caption { get; set; }
    [StringLength(1000)]
    public string Description { get; set; }
    public long AccountClassAlias { get; set; }
    public long CreatedById { get; set; }
}
}
