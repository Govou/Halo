using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Model.LAMS 
{
    public class NegotiationDocument : Documents
    {
        public long QuoteServiceId { get; set; }
        public QuoteService QuoteService { get; set; }
    }
}
