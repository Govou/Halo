using System.Collections.Generic;
using HalobizMigrations.Models;

namespace HaloBiz.DTOs
{
    public class ContractFulfillMentStructure
    {
        public string FulfillmentClass { get; set; }
        
        public QuoteService[] QuoteService { get; set; }
    }
}