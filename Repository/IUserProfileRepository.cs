using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IUserProfileRepository
    {

        Task<UserProfile> SaveUserProfile(UserProfile userProfile);
        Task<bool> UpdateUserProfiles(IEnumerable<UserProfile> userProfile);

        Task<UserProfile> FindUserById(long Id);
        Task<IEnumerable<Object>> FindAllUsersNotInAnProfile(long sbuId);

        Task<UserProfile> FindUserByEmail(string email);

        Task<IEnumerable<UserProfile>> FindAllUserProfile();
        Task<List<UserProfile>> FindAllUserProfilesAttachedToRole(long roleId);
        Task<IEnumerable<UserProfile>> FindAllSuperAdmins();

        Task<UserProfile> UpdateUserProfile(UserProfile userProfile);

        Task<bool> RemoveUserProfile(UserProfile userProfile);

    }
}