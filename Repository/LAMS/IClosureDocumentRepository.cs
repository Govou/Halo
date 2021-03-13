using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface IClosureDocumentRepository
    {
        Task<ClosureDocument> SaveClosureDocument(ClosureDocument closureDocument);
        Task<ClosureDocument> FindClosureDocumentById(long Id);
        Task<ClosureDocument> FindClosureDocumentByCaption(string caption);
        Task<IEnumerable<ClosureDocument>> FindAllClosureDocument();
        Task<ClosureDocument> UpdateClosureDocument(ClosureDocument closureDocument);
        Task<bool> DeleteClosureDocument(ClosureDocument closureDocument);
    }
}