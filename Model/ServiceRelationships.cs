using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model
{   
    public class ServiceRelationships
    {
        [Key]
        public long Id { get; set; }
        public long ServiceAdminId { get; set; }
        public long ServiceDirectId { get; set; }        
    }
}
