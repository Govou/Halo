using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class MeansOfIdentificationRepositoryImpl : IMeansOfIdentificationRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<MeansOfIdentificationRepositoryImpl> _logger;
        public MeansOfIdentificationRepositoryImpl(HalobizContext context, ILogger<MeansOfIdentificationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<MeansOfIdentification> SaveMeansOfIdentification(MeansOfIdentification meansOfIdentification)
        {
            var meansOfIdentificationEntity = await _context.MeansOfIdentifications.AddAsync(meansOfIdentification);
            if(await SaveChanges())
            {
                return meansOfIdentificationEntity.Entity;
            }
            return null;
        }

        public async Task<MeansOfIdentification> FindMeansOfIdentificationById(long Id)
        {
            return await _context.MeansOfIdentifications
                .FirstOrDefaultAsync( meansOfIdentification => meansOfIdentification.Id == Id && meansOfIdentification.IsDeleted == false);
        }

        public async Task<MeansOfIdentification> FindMeansOfIdentificationByName(string name)
        {
            return await _context.MeansOfIdentifications
                .FirstOrDefaultAsync( meansOfIdentification => meansOfIdentification.Caption == name && meansOfIdentification.IsDeleted == false);
        }

        public async Task<IEnumerable<MeansOfIdentification>> FindAllMeansOfIdentification()
        {
            return await _context.MeansOfIdentifications
                .Where(meansOfIdentification => meansOfIdentification.IsDeleted == false)
                .OrderBy(meansOfIdentification => meansOfIdentification.CreatedAt)
                .ToListAsync();
        }

        public async Task<MeansOfIdentification> UpdateMeansOfIdentification(MeansOfIdentification meansOfIdentification)
        {
            var meansOfIdentificationEntity =  _context.MeansOfIdentifications.Update(meansOfIdentification);
            if(await SaveChanges())
            {
                return meansOfIdentificationEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteMeansOfIdentification(MeansOfIdentification meansOfIdentification)
        {
            meansOfIdentification.IsDeleted = true;
            _context.MeansOfIdentifications.Update(meansOfIdentification);
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