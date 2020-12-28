using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IUserProfileRepository
    {

        Task<UserProfile> SaveUserProfile(UserProfile userProfile);

        Task<UserProfile> FindUserById(long Id);

        Task<UserProfile> FindUserByEmail(string email);

        Task<IEnumerable<UserProfile>> FindAllUserProfile();

        Task<UserProfile> UpdateUserProfile(UserProfile userProfile);

        Task<bool> RemoveUserProfile(UserProfile userProfile);

    }
}