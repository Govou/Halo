using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class EndorsementTypeRepositoryImpl : IEndorsementTypeRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<EndorsementTypeRepositoryImpl> _logger;
        public EndorsementTypeRepositoryImpl(HalobizContext context, ILogger<EndorsementTypeRepositoryImpl> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<EndorsementType> SaveEndorsementType(EndorsementType endorsementType)
        {
            var endorsementTypeEntity = await _context.EndorsementTypes.AddAsync(endorsementType);
            if(await SaveChanges())
            {
                return endorsementTypeEntity.Entity;
            }
            return null;
        }

        public async Task<EndorsementType> FindEndorsementTypeById(long Id)
        {
            return await _context.EndorsementTypes
                .FirstOrDefaultAsync(endorsementType => endorsementType.Id == Id && endorsementType.IsDeleted == false);
        }

        public async Task<EndorsementType> FindEndorsementTypeByName(string name)
        {
            return await _context.EndorsementTypes
                .FirstOrDefaultAsync(endorsementType => endorsementType.Caption == name && endorsementType.IsDeleted == false);
        }

        public async Task<IEnumerable<EndorsementType>> FindAllEndorsementType()
        {
            return await _context.EndorsementTypes
                .Where(endorsementType => endorsementType.IsDeleted == false)
                .OrderBy(endorsementType => endorsementType.CreatedAt)
                .ToListAsync();
        }

        public async Task<EndorsementType> UpdateEndorsementType(EndorsementType endorsementType)
        {
            var endorsementTypeEntity =  _context.EndorsementTypes.Update(endorsementType);
            if(await SaveChanges())
            {
                return endorsementTypeEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteEndorsementType(EndorsementType endorsementType)
        {
            endorsementType.IsDeleted = true;
            _context.EndorsementTypes.Update(endorsementType);
            return await SaveChanges();
        }
        private async Task<bool> SaveChanges()
        {
           try{
               return  await _context.SaveChangesAsync() > 0;
           }catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}