using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model.LAMS;
using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class DropReasonRepositoryImpl : IDropReasonRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<DropReasonRepositoryImpl> _logger;
        public DropReasonRepositoryImpl(DataContext context, ILogger<DropReasonRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<DropReason> SaveDropReason(DropReason DropReason)
        {
            var DropReasonEntity = await _context.DropReasons.AddAsync(DropReason);
            if(await SaveChanges())
            {
                return DropReasonEntity.Entity;
            }
            return null;
        }

        public async Task<DropReason> FindDropReasonById(long Id)
        {
            return await _context.DropReasons              
                .FirstOrDefaultAsync( DropReason => DropReason.Id == Id && DropReason.IsDeleted == false);
        }

        public async Task<DropReason> FindDropReasonByTitle(string title)
        {
            return await _context.DropReasons               
                .FirstOrDefaultAsync(DropReason => DropReason.Title == title && DropReason.IsDeleted == false);
        }

        public async Task<IEnumerable<DropReason>> FindAllDropReason()
        {
            return await _context.DropReasons   
                .Where(DropReason => DropReason.IsDeleted == false)
                .OrderBy(DropReason => DropReason.CreatedAt)
                .ToListAsync();
        }

        public async Task<DropReason> UpdateDropReason(DropReason DropReason)
        {
            var DropReasonEntity =  _context.DropReasons.Update(DropReason);
            if(await SaveChanges())
            {
                return DropReasonEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteDropReason(DropReason DropReason)
        {
            DropReason.IsDeleted = true;
            _context.DropReasons.Update(DropReason);
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