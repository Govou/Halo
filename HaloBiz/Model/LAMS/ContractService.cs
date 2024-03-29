using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Helpers;

namespace HaloBiz.Model.LAMS
{
    public class ContractService : BaseContractServiceModel
    {
        public long? QuoteServiceId { get; set; } = null;
        public IEnumerable<TaskFulfillment> TaskFulfillments { get; set; }
        public virtual QuoteService QuoteService { get; set; }
        public IEnumerable<ClosureDocument> ClosureDocuments { get; set; }
        public VersionType Version { get; set; } = VersionType.Latest;

        public double AdHocInvoicedAmount { get; set; } = 0;
        public IEnumerable<SBUToContractServiceProportion> SBUToContractServiceProportions { get; set; }

    }
}