using System.Collections.Generic;
using HaloBiz.Model.ManyToManyRelationship;

namespace HaloBiz.Model
{
    public class RequredServiceQualificationElement : SetupBaseModel
    {
        public IList<ServiceRequredServiceQualificationElement> Services { get; set; }
    }
}