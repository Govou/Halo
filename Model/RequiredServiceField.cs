using System.Collections.Generic;
using HaloBiz.Model.ManyToManyRelationship;

namespace HaloBiz.Model
{
    public class RequiredServiceField : SetupBaseModel
    {
        public IList<ServiceRequiredServiceField> Services { get; set; }
    }
}