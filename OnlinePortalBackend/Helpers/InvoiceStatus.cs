using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Helpers
{
    public enum InvoiceStatus
    {
        NotReceipted, PartlyReceipted, CompletelyReceipted
    }

    public enum InvoiceType
    {
        New, Supplementary, Renewal, AdHoc
    }
}
