using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model.LAMS
{
    public enum ContactType
    {
        Primary, Secondary
    }
    public class LeadContact : Contact
    {
        [Required]
        public ContactType Type { get; set; }
    }
}
