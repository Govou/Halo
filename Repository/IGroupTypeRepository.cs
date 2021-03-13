using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IGroupTypeRepository
    {
        Task<GroupType> SaveGroupType(GroupType groupType);
        Task<GroupType> FindGroupTypeById(long Id);
        Task<GroupType> FindGroupTypeByName(string name);
        Task<IEnumerable<GroupType>> FindAllGroupType();
        Task<GroupType> UpdateGroupType(GroupType groupType);
        Task<bool> DeleteGroupType(GroupType groupType);

    }
}