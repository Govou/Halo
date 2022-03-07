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
    public class LeadDivisionRepositoryImpl : ILeadDivisionRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<LeadDivisionRepositoryImpl> _logger;
        public LeadDivisionRepositoryImpl(HalobizContext context, ILogger<LeadDivisionRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<LeadDivision> SaveLeadDivision(LeadDivision leadDivision)
        {
            var leadDivisionEntity = await _context.LeadDivisions.AddAsync(leadDivision);
            if(await SaveChanges())
            {
                return leadDivisionEntity.Entity;
            }
            return null;
        }

        public async Task<LeadDivision> FindLeadDivisionById(long Id)
        {
            var leadDivision = await _context.LeadDivisions
                .Include(x => x.Quote)
                .FirstOrDefaultAsync( leadDivision => leadDivision.Id == Id && leadDivision.IsDeleted == false);
            
            if(leadDivision == null)
            {
                return null;
            }

            if(leadDivision.BranchId > 0)
            {
                leadDivision.Branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == leadDivision.BranchId && !x.IsDeleted.Value);
            }
            if(leadDivision.OfficeId > 0)
            {
                leadDivision.Office =await  _context.Offices.FirstOrDefaultAsync(x => x.Id == leadDivision.OfficeId && !x.IsDeleted.Value);
            }
            if(leadDivision.PrimaryContactId > 0)
            {
                leadDivision.PrimaryContact = await  _context.LeadDivisionContacts
                                    .FirstOrDefaultAsync(x => x.Id == leadDivision.PrimaryContactId && !x.IsDeleted);
            }
            leadDivision.LeadDivisionKeyPeople = await _context.LeadDivisionKeyPeople
                                    .Where(x => x.LeadDivisionId == leadDivision.Id && !x.IsDeleted).ToListAsync();
            
            if(leadDivision.LeadOriginId > 0)
            {
                leadDivision.LeadOrigin = await _context.LeadOrigins
                                    .FirstOrDefaultAsync(x => x.Id == leadDivision.LeadOriginId && !x.IsDeleted);
            }

            if(leadDivision.LeadTypeId > 0)
            {
                leadDivision.LeadType = await _context.LeadTypes
                                    .FirstOrDefaultAsync(x => x.Id == leadDivision.LeadTypeId && !x.IsDeleted);
            }
            if(leadDivision.LeadTypeId > 0)
            {
                leadDivision.LeadType = await _context.LeadTypes
                                    .FirstOrDefaultAsync(x => x.Id == leadDivision.LeadTypeId && !x.IsDeleted);
            }
            return leadDivision;
        }

        public async Task<LeadDivision> FindLeadDivisionByName(string name)
        {
            var leadDivision = await _context.LeadDivisions
                .Include(x => x.Quote)
                .FirstOrDefaultAsync( leadDivision => leadDivision.DivisionName == name && leadDivision.IsDeleted == false);
            
            if(leadDivision == null)
            {
                return null;
            }

            if(leadDivision.BranchId > 0)
            {
                leadDivision.Branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == leadDivision.BranchId && !x.IsDeleted.Value);
            }
            if(leadDivision.OfficeId > 0)
            {
                leadDivision.Office =await  _context.Offices.FirstOrDefaultAsync(x => x.Id == leadDivision.OfficeId && !x.IsDeleted.Value);
            }
            if(leadDivision.PrimaryContactId > 0)
            {
                leadDivision.PrimaryContact = await  _context.LeadDivisionContacts
                                    .FirstOrDefaultAsync(x => x.Id == leadDivision.PrimaryContactId && !x.IsDeleted);
            }
            leadDivision.LeadDivisionKeyPeople = await _context.LeadDivisionKeyPeople
                                    .Where(x => x.LeadDivisionId == leadDivision.Id && !x.IsDeleted).ToListAsync();
            
            if(leadDivision.LeadOriginId > 0)
            {
                leadDivision.LeadOrigin = await _context.LeadOrigins
                                    .FirstOrDefaultAsync(x => x.Id == leadDivision.LeadOriginId && !x.IsDeleted);
            }

            if(leadDivision.LeadTypeId > 0)
            {
                leadDivision.LeadType = await _context.LeadTypes
                                    .FirstOrDefaultAsync(x => x.Id == leadDivision.LeadTypeId && !x.IsDeleted);
            }
            if(leadDivision.LeadTypeId > 0)
            {
                leadDivision.LeadType = await _context.LeadTypes
                                    .FirstOrDefaultAsync(x => x.Id == leadDivision.LeadTypeId && !x.IsDeleted);
            }

            leadDivision.OtherLeadCaptureInfo = await _context.OtherLeadCaptureInfos
                                .FirstOrDefaultAsync(x => x.LeadDivisionId == leadDivision.Id && !x.IsDeleted);
            return leadDivision;

        }

        public async Task<LeadDivision> FindLeadDivisionByRCNumber(string rcNumber)
        {
            return await _context.LeadDivisions
                .Include(x => x.Branch)
                .Include(x => x.Office)
                .Include(x => x.PrimaryContact)
                .Include(x => x.SecondaryContact)
                .Include(x => x.LeadDivisionKeyPeople)
                .Include(x => x.LeadOrigin)
                .Include(x => x.LeadType)
                .Include(x => x.Quote)
                .FirstOrDefaultAsync(leadDivision => leadDivision.Rcnumber == rcNumber && leadDivision.IsDeleted == false);
        }

        public async Task<IEnumerable<LeadDivision>> FindAllLeadDivision()
        {
            return await _context.LeadDivisions
                .Include(x => x.Branch)
                .Include(x => x.Office)
                .Include(x => x.PrimaryContact)
                .Include(x => x.SecondaryContact)
                .Include(x => x.LeadDivisionKeyPeople)
                .Include(x => x.LeadOrigin)
                .Include(x => x.LeadType)
                .Include(x => x.Quote)
                .Where(leadDivision => leadDivision.IsDeleted == false)
                .OrderBy(leadDivision => leadDivision.CreatedAt)
                .ToListAsync();
        }

        public async Task<LeadDivision> UpdateLeadDivision(LeadDivision leadDivision)
        {
            var leadDivisionEntity =  _context.LeadDivisions.Update(leadDivision);
            if(await SaveChanges())
            {
                return leadDivisionEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteLeadDivision(LeadDivision leadDivision)
        {
            leadDivision.IsDeleted = true;
            _context.LeadDivisions.Update(leadDivision);
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