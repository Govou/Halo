using System.Collections.Generic;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Model
{
    public class GroupType : SetupBaseModel
    {
        public IEnumerable<Lead> Leads { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
    }
}