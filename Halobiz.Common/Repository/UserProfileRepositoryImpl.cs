using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Halobiz.Common.Repository
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

        Task<UserProfile> UpdateUserProfile(UserProfile userProfile);

        Task<bool> RemoveUserProfile(UserProfile userProfile);
        Task<IEnumerable<UserProfile>> FetchAllUserProfilesWithEscalationLevelConfiguration();

    }

    public class UserProfileRepositoryImpl : IUserProfileRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<UserProfileRepositoryImpl> _logger;
        public UserProfileRepositoryImpl(HalobizContext context, ILogger<UserProfileRepositoryImpl> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<UserProfile> SaveUserProfile(UserProfile userProfile)
        {
            var UserProfileEntity = await _context.UserProfiles.AddAsync(userProfile);
            if(await SaveChanges())
            {
                return UserProfileEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateUserProfiles(IEnumerable<UserProfile> userProfiles)
        {
            _context.UserProfiles.UpdateRange(userProfiles);
            return await SaveChanges();
        }

        public async Task<UserProfile> FindUserById(long Id)
        {
            return await _context.UserProfiles
                .Include(x => x.Sbu)
                .FirstOrDefaultAsync( user => user.Id == Id && user.IsDeleted == false);
        }

        public async Task<UserProfile> FindUserByEmail(string email)
        {
            return await _context.UserProfiles
                .Include(x => x.Sbu)
                .FirstOrDefaultAsync( user => user.Email == email && user.IsDeleted == false);
        }

        public async Task<IEnumerable<UserProfile>> FindAllUserProfile()
        {
            return await _context.UserProfiles.AsNoTracking()
                .Include(x => x.Sbu)
                .Where(user => user.IsDeleted == false)
                .OrderBy(user => user.Email)
                .ToListAsync();
        }

        public async Task<List<UserProfile>> FindAllUserProfilesAttachedToRole(long roleId)
        {
            return await _context.UserProfiles
                .Where(user => user.RoleId == roleId && user.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Object>> FindAllUsersNotInAnProfile(long sbuId)
        {
            var sbu = await _context.StrategicBusinessUnits.FirstOrDefaultAsync(x => x.Id == sbuId);
            if(sbu == null) return  new List<Object>();
            var operatingEntityId = sbu.OperatingEntityId;

            var listOfSbuNotPartOfOperatingEntity = await  _context.StrategicBusinessUnits
                        .Where(x => x.OperatingEntityId != operatingEntityId).Select(x => x.Id).ToListAsync();

            return await _context.UserProfiles
                .Where(x => listOfSbuNotPartOfOperatingEntity.Contains((long)x.Sbuid) && x.IsDeleted == false)
                .Select(x => new {
                    email = x.Email,
                    sbuId = x.Sbuid,
                    firstName = x.FirstName,
                    lastname = x.LastName,
                    id = x.Id
                    })
                .OrderBy(user => user.email)
                .ToListAsync();
        }

        public async Task<UserProfile> UpdateUserProfile(UserProfile userProfile)
        {
            var UserProfileEntity =  _context.UserProfiles.Update(userProfile);
            if(await SaveChanges())
            {
                return UserProfileEntity.Entity;
            }
            return null;
        }

        public async Task<bool> RemoveUserProfile(UserProfile userProfile)
        {
            userProfile.IsDeleted = true;
            _context.UserProfiles.Update(userProfile);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            try{
                return await _context.SaveChangesAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
       
        public async Task<IEnumerable<UserProfile>> FetchAllUserProfilesWithEscalationLevelConfiguration()
        {
            return await _context.ProfileEscalationLevels
                .Include(x => x.UserProfile)
                .Where(x => x.IsDeleted == false)
                .Select(x => x.UserProfile)
                .ToListAsync();
        }
    }
    
}