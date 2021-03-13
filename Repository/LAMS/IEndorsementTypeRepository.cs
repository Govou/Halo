using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface IEndorsementTypeRepository
    {
        Task<EndorsementType> SaveEndorsementType(EndorsementType endorsementType);
        Task<EndorsementType> FindEndorsementTypeById(long Id);
        Task<EndorsementType> FindEndorsementTypeByName(string name);
        Task<IEnumerable<EndorsementType>> FindAllEndorsementType();
        Task<EndorsementType> UpdateEndorsementType(EndorsementType endorsementType);
        Task<bool> DeleteEndorsementType(EndorsementType endorsementType);
    }
}