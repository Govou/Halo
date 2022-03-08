using System;
using System.Collections.Generic;
using HalobizMigrations.Models;

namespace HaloBiz.DTOs.ContactDTO
{
    public class LeadsOpportunityData
    {
        public List<Lead> Initiate { get; set; }

        public List<Lead> DealCapture { get; set; }

        public List<Lead> Negotiations { get; set; }

        public List<Lead> Conversion { get; set; }

        public List<Lead> Closed { get; set; }

        public List<Lead> Dropped { get; set; }

        public long ConversionRatio { get; set; }

    }
}
