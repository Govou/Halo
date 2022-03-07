using System.Collections.Generic;
using HaloBiz.Model.ManyToManyRelationship;

namespace HaloBiz.Model
{
    public class RequiredServiceDocument : SetupBaseModel
    {
        public string Type { get; set; }
        public IList<ServiceRequiredServiceDocument> Services { get; set; }
    }
}