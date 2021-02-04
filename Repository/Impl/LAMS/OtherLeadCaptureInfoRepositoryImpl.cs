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
    public class OtherLeadCaptureInfoRepositoryImpl : IOtherLeadCaptureInfoRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<OtherLeadCaptureInfoRepositoryImpl> _logger;
        public OtherLeadCaptureInfoRepositoryImpl(DataContext context, ILogger<OtherLeadCaptureInfoRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<OtherLeadCaptureInfo> SaveOtherLeadCaptureInfo(OtherLeadCaptureInfo otherLeadCaptureInfo)
        {
            var otherLeadCaptureInfoEntity = await _context.OtherLeadCaptureInfos.AddAsync(otherLeadCaptureInfo);
            if(await SaveChanges())
            {
                return otherLeadCaptureInfoEntity.Entity;
            }
            return null;
        }

        public async Task<OtherLeadCaptureInfo> FindOtherLeadCaptureInfoById(long Id)
        {
            return await _context.OtherLeadCaptureInfos
                .FirstOrDefaultAsync( otherLeadCaptureInfo => otherLeadCaptureInfo.Id == Id && otherLeadCaptureInfo.IsDeleted == false);
        }

        public async Task<IEnumerable<OtherLeadCaptureInfo>> FindAllOtherLeadCaptureInfo()
        {
            return await _context.OtherLeadCaptureInfos
                .Where(otherLeadCaptureInfo => otherLeadCaptureInfo.IsDeleted == false)
                .OrderBy(otherLeadCaptureInfo => otherLeadCaptureInfo.CreatedAt)
                .ToListAsync();
        }

        public async Task<OtherLeadCaptureInfo> UpdateOtherLeadCaptureInfo(OtherLeadCaptureInfo otherLeadCaptureInfo)
        {
            var otherLeadCaptureInfoEntity =  _context.OtherLeadCaptureInfos.Update(otherLeadCaptureInfo);
            if(await SaveChanges())
            {
                return otherLeadCaptureInfoEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteOtherLeadCaptureInfo(OtherLeadCaptureInfo otherLeadCaptureInfo)
        {
            otherLeadCaptureInfo.IsDeleted = true;
            _context.OtherLeadCaptureInfos.Update(otherLeadCaptureInfo);
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