using System;
using System.Collections.Generic;
using HalobizMigrations.Models.Halobiz;

namespace HaloBiz.DTOs.ContactDTO
{
    public class LeadsClassificationData
    {
        public List<Suspect> UnqualifiedLeads { get; set; }
        public List<Suspect> LeadsInQualification { get; set; }
        public List<Suspect> QualifiedLeads { get; set; }
    }
}
