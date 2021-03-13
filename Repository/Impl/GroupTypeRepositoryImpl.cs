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
    public class GroupTypeRepositoryImpl : IGroupTypeRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<GroupTypeRepositoryImpl> _logger;
        public GroupTypeRepositoryImpl(DataContext context, ILogger<GroupTypeRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<GroupType> SaveGroupType(GroupType groupType)
        {
            var groupTypeEntity = await _context.GroupType.AddAsync(groupType);
            if(await SaveChanges())
            {
                return groupTypeEntity.Entity;
            }
            return null;
        }

        public async Task<GroupType> FindGroupTypeById(long Id)
        {
            return await _context.GroupType
                .FirstOrDefaultAsync( groupType => groupType.Id == Id && groupType.IsDeleted == false);
        }

        public async Task<GroupType> FindGroupTypeByName(string name)
        {
            return await _context.GroupType
                .FirstOrDefaultAsync( groupType => groupType.Caption == name && groupType.IsDeleted == false);
        }

        public async Task<IEnumerable<GroupType>> FindAllGroupType()
        {
            return await _context.GroupType
                .Where(groupType => groupType.IsDeleted == false)
                .OrderBy(groupType => groupType.CreatedAt)
                .ToListAsync();
        }

        public async Task<GroupType> UpdateGroupType(GroupType groupType)
        {
            var groupTypeEntity =  _context.GroupType.Update(groupType);
            if(await SaveChanges())
            {
                return groupTypeEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteGroupType(GroupType groupType)
        {
            groupType.IsDeleted = true;
            _context.GroupType.Update(groupType);
            return await SaveChanges();
        }
        private async Task<bool> SaveChanges()
        {
           try
           {
               return  await _context.SaveChangesAsync() > 0;
           }
           catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}