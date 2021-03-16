using System.Collections.Generic;
using System;

namespace HaloBiz.Model.LAMS
{
    public class ContractServiceForEndorsement : BaseContractServiceModel
    {
        public long EndorsementTypeId { get; set; }
        public EndorsementType EndorsementType { get; set; }
        public IEnumerable<TaskFulfillment> TaskFulfillments { get; set; }
        public bool IsRequestedForApproval { get; set; } = true;
        public bool IsApproved { get; set; }
        public bool IsDeclined { get; set; }
        public long CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public long? PreviousContractServiceId { get; set; }
        public DateTime? DateForNewContractToTakeEffect { get; set; }
        public bool IsConvertedToContractService { get; set; } = false;
    }
}