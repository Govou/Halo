﻿using HaloBiz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class BranchReceivingDTO
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        [Required, MaxLength(500, ErrorMessage="Requires a maximum of 500 characters")]
        public string Address { get; set; }
        [Required(ErrorMessage="Provide a valid HeadId")]
        public int HeadId { get; set; }
    }
}
