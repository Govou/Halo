using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface INegotiationDocumentRepository
    {
        Task<NegotiationDocument> SaveNegotiationDocument(NegotiationDocument negotiationDocument);
        Task<NegotiationDocument> FindNegotiationDocumentById(long Id);
        Task<NegotiationDocument> FindNegotiationDocumentByCaption(string caption);
        Task<IEnumerable<NegotiationDocument>> FindAllNegotiationDocument();
        Task<NegotiationDocument> UpdateNegotiationDocument(NegotiationDocument negotiationDocument);
        Task<bool> DeleteNegotiationDocument(NegotiationDocument negotiationDocument);
    }
}