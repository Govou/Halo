using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.ManyToManyRelationship;

namespace HaloBiz.Repository
{
    public interface IServiceRequiredServiceDocumentRepository
    {
        Task<bool> SaveRangeServiceRequiredServiceDocument(IEnumerable<ServiceRequiredServiceDocument> serviceRequiredServiceDocument);
        Task<ServiceRequiredServiceDocument> SaveServiceRequiredServiceDocument(ServiceRequiredServiceDocument serviceRequiredServiceDocument);
        Task<ServiceRequiredServiceDocument> FindServiceRequiredServiceDocumentById(long serviceId, long serviceDocumentId);
        Task<bool> DeleteServiceRequiredServiceDocument(ServiceRequiredServiceDocument serviceRequiredServiceDocument);
        Task<bool> DeleteRangeServiceRequiredServiceDocument(IEnumerable<ServiceRequiredServiceDocument> serviceRequiredServiceDocuments);
    }
}