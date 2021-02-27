using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class UserProfileRepositoryImpl : IUserProfileRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<UserProfileRepositoryImpl> _logger;
        public UserProfileRepositoryImpl(DataContext context, ILogger<UserProfileRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
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
                .Include(x => x.SBU)
                .Include(x => x.Role).ThenInclude(x => x.RoleClaims)
                .FirstOrDefaultAsync( user => user.Id == Id && user.IsDeleted == false);
        }

        public async Task<UserProfile> FindUserByEmail(string email)
        {
            return await _context.UserProfiles
                .Include(x => x.SBU)
                .Include(x => x.Role).ThenInclude(x => x.RoleClaims)
                .FirstOrDefaultAsync( user => user.Email == email && user.IsDeleted == false);
        }

        public async Task<IEnumerable<UserProfile>> FindAllUserProfile()
        {
            return await _context.UserProfiles
                .Include(x => x.SBU)
                .Include(x => x.Role).ThenInclude(x => x.RoleClaims)
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
                .Where(x => listOfSbuNotPartOfOperatingEntity.Contains((long)x.SBUId) && x.IsDeleted == false)
                .Select(x => new {
                    email = x.Email,
                    sbuId = x.SBUId,
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

        public async Task<IEnumerable<UserProfile>> FindAllSuperAdmins()
        {
            return await _context.UserProfiles
              .Where(user => user.RoleId == 1 && user.IsDeleted == false).AsNoTracking()
              .ToListAsync();
        }
    }
    
}