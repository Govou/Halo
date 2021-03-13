using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IRelationshipRepository
    {
        Task<Relationship> SaveRelationship(Relationship relationship);
        Task<Relationship> FindRelationshipById(long Id);
        Task<Relationship> FindRelationshipByName(string name);
        Task<IEnumerable<Relationship>> FindAllRelationship();
        Task<Relationship> UpdateRelationship(Relationship relationship);
        Task<bool> DeleteRelationship(Relationship relationship);
    }
}