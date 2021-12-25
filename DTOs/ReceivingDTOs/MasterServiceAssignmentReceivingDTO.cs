﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class MasterServiceAssignmentReceivingDTO
    {
      
        public bool IsReturnJourney { get; set; }
        //public long? PrimaryTripAssignmentId { get; set; }
        //public long TripTypeId { get; set; }
        public long SMORouteId { get; set; }
        public long? SMORegionId { get; set; }
        //public DateTime ReadinessTime { get; set; }
        public string DropoffLocation { get; set; }
        public string PickoffLocation { get; set; }
        //public DateTime DropoffDate { get; set; }
        public DateTime PickupDate { get; set; }
        public long ContractServiceId { get; set; }
        public long? CustomerDivisionId { get; set; }
        public long? SourceTypeId { get; set; }
        public DateTime PickoffTime { get; set; }
    }
}
