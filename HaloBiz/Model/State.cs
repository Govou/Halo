using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model
{
    public class State
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public int StateCode { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Capital { get; set; }
        [Required]
        public string  Slogan { get; set; }
        public IEnumerable<LGA> LGAs { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}