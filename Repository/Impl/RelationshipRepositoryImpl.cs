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
    public class RelationshipRepositoryImpl : IRelationshipRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<RelationshipRepositoryImpl> _logger;
        public RelationshipRepositoryImpl(DataContext context, ILogger<RelationshipRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<Relationship> SaveRelationship(Relationship relationship)
        {
            var relationshipEntity = await _context.Relationships.AddAsync(relationship);
            if(await SaveChanges())
            {
                return relationshipEntity.Entity;
            }
            return null;
        }

        public async Task<Relationship> FindRelationshipById(long Id)
        {
            return await _context.Relationships
                .FirstOrDefaultAsync( relationship => relationship.Id == Id && relationship.IsDeleted == false);
        }

        public async Task<Relationship> FindRelationshipByName(string name)
        {
            return await _context.Relationships
                .FirstOrDefaultAsync( relationship => relationship.Caption == name && relationship.IsDeleted == false);
        }

        public async Task<IEnumerable<Relationship>> FindAllRelationship()
        {
            return await _context.Relationships
                .Where(relationship => relationship.IsDeleted == false)
                .OrderBy(relationship => relationship.CreatedAt)
                .ToListAsync();
        }

        public async Task<Relationship> UpdateRelationship(Relationship relationship)
        {
            var relationshipEntity =  _context.Relationships.Update(relationship);
            if(await SaveChanges())
            {
                return relationshipEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteRelationship(Relationship relationship)
        {
            relationship.IsDeleted = true;
            _context.Relationships.Update(relationship);
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