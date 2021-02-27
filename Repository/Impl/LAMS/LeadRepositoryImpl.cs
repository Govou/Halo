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
    public class LeadRepositoryImpl : ILeadRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<LeadRepositoryImpl> _logger;

        public LeadRepositoryImpl(DataContext context, ILogger<LeadRepositoryImpl> logger)
        {
            this._context = context;
            this._logger = logger;
        }
        public void DeleteLead(Lead lead)
        {
            lead.IsDeleted = true;
            _context.Leads.Update(lead);
            
        }

        public async Task<IEnumerable<Lead>> FindAllLead()
        {
            return await _context.Leads.Where(lead => lead.IsDeleted == false).ToListAsync();

        }

        public async Task<IEnumerable<Lead>> FindAllUnApprovedLeads()
        {
            return await _context.Leads.Where(lead => lead.LeadConversionStatus == false && lead.IsDeleted == false).ToListAsync();

        }

        public async Task<Lead> FindLeadById(long Id)
        {
            var lead =  await _context.Leads.Include(lead => lead.GroupType)
                .Include(lead => lead.DropReason)
                .Include(lead => lead.LeadType)
                .Include(lead => lead.LeadOrigin)
                .FirstOrDefaultAsync(lead => lead.Id == Id);
            if(lead == null)
            {
                return null;
            }
            lead.DropReason = await _context.DropReasons.FirstOrDefaultAsync(dropReason => lead.DropReasonId == dropReason.Id);
            if(lead.PrimaryContactId != null)
                lead.PrimaryContact = await _context.LeadContacts.FirstOrDefaultAsync(primaryContact => lead.PrimaryContactId == primaryContact.Id);
            if(lead.SecondaryContactId != null)
                lead.SecondaryContact = await _context.LeadContacts.FirstOrDefaultAsync(contact => lead.SecondaryContactId == contact.Id);
            lead.LeadDivisions = await _context.LeadDivisions
                                    .Include(division => division.PrimaryContact)
                                    .Include(division => division.SecondaryContact)
                                    .Include(division => division.LeadDivisionKeyPersons.Where(x => x.IsDeleted == false))
                                    .Include(division => division.Quote)
                                        .ThenInclude(quote => quote.QuoteServices.Where(x => x.IsDeleted == false))
                                        .ThenInclude(x => x.QuoteServiceDocuments.Where(x => x.IsDeleted == false))
                                    .Where(division => division.LeadId == lead.Id).ToListAsync();
            //lead.LeadDivisions.ToList().ForEach(x => x.Lead = null);
            lead.LeadKeyPersons = await _context.LeadKeyPeople
                                    .Where(x => x.LeadId == lead.Id && x.IsDeleted == false).ToListAsync();
            if(lead.LeadKeyPersons != null)
            {
                lead.LeadKeyPersons.ToList().ForEach(x => x.Lead = null);
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
            if(lead.PrimaryContactId != null)
                lead.PrimaryContact = await _context.LeadContacts.FirstOrDefaultAsync(primaryContact => lead.PrimaryContactId == primaryContact.Id);
            if(lead.SecondaryContactId != null)
                lead.SecondaryContact = await _context.LeadContacts.FirstOrDefaultAsync(contact => lead.SecondaryContactId == contact.Id);
            lead.LeadDivisions = await _context.LeadDivisions
                                    .Include(division => division.PrimaryContact)
                                    .Include(division => division.SecondaryContact)
                                    .Include(division => division.LeadDivisionKeyPersons)
                                    .Include(division => division.Quote)
                                        .ThenInclude(quote => quote.QuoteServices.Where(x => x.IsDeleted == false))
                                            .ThenInclude(x => x.QuoteServiceDocuments.Where(x => x.IsDeleted == false))
                                    .Include(division => division.Quote)
                                        .ThenInclude(quote => quote.QuoteServices.Where(x => x.IsDeleted == false))
                                            .ThenInclude(x => x.SBUToQuoteServiceProportions.Where(x => x.IsDeleted == false))
                                    .Where(division => division.LeadId == lead.Id).ToListAsync();
            lead.LeadKeyPersons = await _context.LeadKeyPeople
                                    .Where(x => x.LeadId == lead.Id && x.IsDeleted == false).ToListAsync();
            if(lead.LeadKeyPersons != null)
            {
                lead.LeadKeyPersons.ToList().ForEach(x => x.Lead = null);
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