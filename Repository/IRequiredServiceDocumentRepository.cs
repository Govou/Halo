using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IRequiredServiceDocumentRepository
    {
        Task<RequiredServiceDocument> SaveRequiredServiceDocument(RequiredServiceDocument requiredServiceDocument);
        Task<RequiredServiceDocument> FindRequiredServiceDocumentById(long Id);
        Task<RequiredServiceDocument> FindRequiredServiceDocumentByName(string caption);
        Task<IEnumerable<RequiredServiceDocument>> FindAllRequiredServiceDocuments();
        Task<RequiredServiceDocument> UpdateRequiredServiceDocument(RequiredServiceDocument RequiredServiceDocument);
        Task<bool> DeleteRequiredServiceDocument(RequiredServiceDocument RequiredServiceDocument);
    }
}