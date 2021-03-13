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
    public class ModeOfTransportRepositoryImpl : IModeOfTransportRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ModeOfTransportRepositoryImpl> _logger;
        public ModeOfTransportRepositoryImpl(DataContext context, ILogger<ModeOfTransportRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<ModeOfTransport> SaveModeOfTransport(ModeOfTransport modeOfTransport)
        {
            var modeOfTransportEntity = await _context.ModeOfTransports.AddAsync(modeOfTransport);
            if(await SaveChanges())
            {
                return modeOfTransportEntity.Entity;
            }
            return null;
        }

        public async Task<ModeOfTransport> FindModeOfTransportById(long Id)
        {
            return await _context.ModeOfTransports
                .FirstOrDefaultAsync( modeOfTransport => modeOfTransport.Id == Id && modeOfTransport.IsDeleted == false);
        }

        public async Task<ModeOfTransport> FindModeOfTransportByName(string name)
        {
            return await _context.ModeOfTransports
                .FirstOrDefaultAsync( modeOfTransport => modeOfTransport.Caption == name && modeOfTransport.IsDeleted == false);
        }

        public async Task<IEnumerable<ModeOfTransport>> FindAllModeOfTransport()
        {
            return await _context.ModeOfTransports
                .Where(modeOfTransport => modeOfTransport.IsDeleted == false)
                .OrderBy(modeOfTransport => modeOfTransport.CreatedAt)
                .ToListAsync();
        }

        public async Task<ModeOfTransport> UpdateModeOfTransport(ModeOfTransport modeOfTransport)
        {
            var modeOfTransportEntity =  _context.ModeOfTransports.Update(modeOfTransport);
            if(await SaveChanges())
            {
                return modeOfTransportEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteModeOfTransport(ModeOfTransport modeOfTransport)
        {
            modeOfTransport.IsDeleted = true;
            _context.ModeOfTransports.Update(modeOfTransport);
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