using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;

using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class LeadRepositoryImpl : ILeadRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<LeadRepositoryImpl> _logger;

        public LeadRepositoryImpl(HalobizContext context, ILogger<LeadRepositoryImpl> logger)
        {
            this._context = context;
            this._logger = logger;
        }
        public async Task<bool> DeleteLead(Lead lead)
        {
            lead.IsDeleted = true;
            _context.Leads.Update(lead);
            return await SaveChanges();
        }

        public async Task<IEnumerable<Lead>> FindAllLead()
        {
            return await _context.Leads.Where(lead => lead.IsDeleted == false).ToListAsync();

        }

        public async Task<IEnumerable<Lead>> FindUserLeads(long userId)
        {
            return await _context.Leads.Where(lead => !lead.IsDeleted && !lead.LeadConversionStatus && lead.CreatedById == userId).ToListAsync();
        }

        public async Task<IEnumerable<Lead>> FindAllUnApprovedLeads()
        {
            return await _context.Leads.Where(lead => lead.LeadCaptureStatus && lead.LeadQualificationStatus && lead.LeadOpportunityStatus && lead.LeadClosureStatus.Value
                                        && !lead.LeadConversionStatus && !lead.IsLeadDropped && !lead.IsDeleted).ToListAsync();
        }

        public async Task<Lead> FindLeadById(long Id)
        {
            var lead =  await _context.Leads
                .Where(lead => lead.Id == Id)
                .Include(lead => lead.GroupType)
                .Include(lead => lead.DropReason)
                .Include(lead => lead.LeadType)
                .Include(lead => lead.LeadOrigin)
                .FirstOrDefaultAsync();
            if(lead == null)
            {
                return null;
            }

            lead.DropReason = await _context.
                DropReasons.Where(dropReason => lead.DropReasonId == dropReason.Id).FirstOrDefaultAsync();
           lead.LeadDivisions = await _context.LeadDivisions.AsNoTracking()
                                    .Include(x => x.Branch)
                                    .Include(x => x.Office)
                                    .Include(division => division.PrimaryContact)
                                    .Include(division => division.SecondaryContact)
                                    .Include(division => division.LeadDivisionKeyPeople.Where(x => x.IsDeleted == false))
                                    .Include(division => division.Quote)
                                        .ThenInclude(quote => quote.QuoteServices.Where(x => x.IsDeleted == false))
                                        .ThenInclude(x => x.QuoteServiceDocuments.Where(x => x.IsDeleted == false))
                                    .Where(division => division.LeadId == lead.Id).ToListAsync();
            lead.LeadKeyPeople = await _context.LeadKeyPeople
                                    .Where(x => x.LeadId == lead.Id && x.IsDeleted == false).ToListAsync();
            if(lead.LeadKeyPeople != null)
            {
                lead.LeadKeyPeople.ToList().ForEach(x => x.Lead = null);
            }
            return lead;
        }

        

        public async Task<Lead> FindLeadByReferenceNo(string refNo)
        {
            var lead =  await _context.Leads.Include(lead => lead.GroupType)
                .Include(lead => lead.DropReason)
                .Include(lead => lead.LeadType)
                .Include(lead => lead.LeadOrigin)
                .FirstOrDefaultAsync(lead => lead.ReferenceNo == refNo);

            if(lead == null)
            {
                return null;
            }
            lead.DropReason = await _context.DropReasons.FirstOrDefaultAsync(dropReason => lead.DropReasonId == dropReason.Id);

            lead.LeadDivisions = await _context.LeadDivisions
                                    .Include(division => division.PrimaryContact)
                                    .Include(division => division.SecondaryContact)
                                    .Include(division => division.LeadDivisionKeyPeople)
                                    .Include(division => division.Quote)
                                        .ThenInclude(quote => quote.QuoteServices.Where(x => x.IsDeleted == false))
                                            .ThenInclude(x => x.QuoteServiceDocuments.Where(x => x.IsDeleted == false))
                                    .Include(division => division.Quote)
                                        .ThenInclude(quote => quote.QuoteServices.Where(x => x.IsDeleted == false))
                                            .ThenInclude(x => x.SbutoQuoteServiceProportions.Where(x => x.IsDeleted == false))
                                    .Where(division => division.LeadId == lead.Id).ToListAsync();

            lead.LeadKeyPeople = await _context.LeadKeyPeople
                                    .Include(x => x.Designation)
                                    .Include(x => x.ClientContactQualification)
                                    .Where(x => x.LeadId == lead.Id && x.IsDeleted == false).AsNoTracking().ToListAsync();
            if(lead.LeadKeyPeople != null)
            {
                lead.LeadKeyPeople.ToList().ForEach(x => x.Lead = null);
            }

            return lead;
        }

        public async Task<bool> SaveChanges()
        {
            try{
                return await _context.SaveChangesAsync() > 0;
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<Lead> SaveLead(Lead lead)
        {
            var savedLead = await _context.Leads.AddAsync(lead);
            if(await SaveChanges())
            {
                return savedLead.Entity;
            }
            return null;
        }

        public async Task<Lead> UpdateLead(Lead lead)
        {
            var updatedLead =  _context.Leads.Update(lead);
            if(await SaveChanges())
            {
                return updatedLead.Entity;
            }
            return null;
        }
    }
}