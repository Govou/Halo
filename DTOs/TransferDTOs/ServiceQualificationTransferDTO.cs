using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ServiceQualificationTransferDTO
    {
        public long Id { get; set; }
        public long ServiceId { get; set; }
        public ServiceTransferDTO Service { get; set; }
        public DateTime? DateToStart { get; set; }
        public long SuspectQualificationId { get; set; }
        public SuspectQualificationTransferDTO SuspectQualification { get; set; }
        public long? QuantityEstimate { get; set; }
        public decimal? Budget { get; set; }
        public long? EstimatedDurationInMonths { get; set; }
    }
}
