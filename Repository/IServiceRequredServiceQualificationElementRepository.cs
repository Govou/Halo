using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.ManyToManyRelationship;

namespace HaloBiz.Repository
{
    public interface IServiceRequredServiceQualificationElementRepository
    {
        Task<bool> SaveRangeServiceRequredServiceQualificationElement(IEnumerable<ServiceRequredServiceQualificationElement> serviceRequredServiceQualificationElement);
        
        Task<ServiceRequredServiceQualificationElement> SaveServiceRequredServiceQualificationElement(ServiceRequredServiceQualificationElement serviceRequredServiceQualificationElement);
       

        Task<ServiceRequredServiceQualificationElement> FindServiceRequredServiceQualificationElementById(long serviceId, long serviceElementId);
        

        Task<bool> DeleteServiceRequredServiceQualificationElement(ServiceRequredServiceQualificationElement serviceRequredServiceQualificationElement);
        

        Task<bool> DeleteRangeServiceRequredServiceQualificationElement(IEnumerable<ServiceRequredServiceQualificationElement> serviceRequredServiceQualificationElements);
        
    }
}