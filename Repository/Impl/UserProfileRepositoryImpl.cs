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

        public async Task<UserProfile> FindUserById(long Id)
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync( user => user.Id == Id && user.IsDeleted == false);
        }

        public async Task<UserProfile> FindUserByEmail(string email)
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync( user => user.Email == email && user.IsDeleted == false);
        }

        public async Task<IEnumerable<UserProfile>> FindAllUserProfile()
        {
            return await _context.UserProfiles.Where(user => user.IsDeleted == false).ToListAsync();
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
    }
    
}