using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;

using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models;
using Microsoft.AspNetCore.Http;
using HaloBiz.Helpers;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class QuoteServiceRepositoryImpl : IQuoteServiceRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<QuoteServiceRepositoryImpl> _logger;
        public QuoteServiceRepositoryImpl(HalobizContext context, ILogger<QuoteServiceRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<QuoteService> SaveQuoteService(QuoteService quoteService)
        {
            var quoteServiceEntity = await _context.QuoteServices.AddAsync(quoteService);
            if(await SaveChanges())
            {
                return quoteServiceEntity.Entity;
            }
            return null;
        }

        public async Task<bool> SaveQuoteServiceRange(IEnumerable<QuoteService> quoteServices)
        {
            await _context.QuoteServices.AddRangeAsync(quoteServices);
            return await SaveChanges();
        }

        public async Task<QuoteService> FindQuoteServiceById(long Id)
        {
            var quoteService = await _context.QuoteServices
                .Where(quoteService => quoteService.Id == Id && quoteService.IsDeleted == false)
                .Include(x => x.QuoteServiceDocuments)
                .Include(x => x.SbutoQuoteServiceProportions).ThenInclude(x => x.UserInvolved)
                .Include(x => x.ContractServices)
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync();
            if(quoteService == null)
            {
                return null;
            }
            quoteService.Service = await _context.Services.FirstOrDefaultAsync(x => x.Id == quoteService.ServiceId && !x.IsDeleted.Value);
            return quoteService;
        }
        public async Task<QuoteService> FindQuoteServiceByTag(string tag)
        {
            return await _context.QuoteServices
                .Where(quoteService => quoteService.UniqueTag == tag)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<QuoteService>> FindAllQuoteServiceByQuoteId(long id)
        {
            return await _context.QuoteServices
                .Where(quoteService => quoteService.QuoteId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuoteService>> FindAllQuoteService()
        {
            return await _context.QuoteServices
                .Include(x => x.QuoteServiceDocuments)
                .Include(x => x.SbutoQuoteServiceProportions)
                .Include(x => x.ContractServices)
                .Where(quoteService => quoteService.IsDeleted == false)
                .OrderBy(quoteService => quoteService.CreatedAt)
                .ToListAsync();
        }

        public async Task<QuoteService> UpdateQuoteService(QuoteService quoteService)
        {
            var quoteServiceEntity =  _context.QuoteServices.Update(quoteService);
            if(await SaveChanges())
            {
                return quoteServiceEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateQuoteServicesByQuoteId(long quoteId, IEnumerable<QuoteService> quoteServices, HttpContext context)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var userId = context.GetLoggedInUserId();
                //check first that this quote exist
                var exist = _context.Quotes.Any(x => x.Id == quoteId);
                if (!exist)
                    return false;

                //find all the services that are new
                var newServices = quoteServices.Where(x => x.Id == 0).ToList();

                foreach (var item in newServices)
                {
                    item.QuoteId = quoteId;
                    item.CreatedById = userId;
                    item.CreatedAt = DateTime.Now;
                    item.UpdatedAt = DateTime.Now;
                }

                var oldServices = await _context.QuoteServices
                                .Where(quoteService => quoteService.QuoteId == quoteId)
                                .AsNoTracking()
                                .ToListAsync();

                //check which services
                List<QuoteService> toDelete = new List<QuoteService>();
                List<QuoteService> toUpdate = new List<QuoteService>();

                foreach (var service in oldServices)
                {
                    var forEdit = quoteServices.Where(x => x.Id == service.Id).FirstOrDefault();
                    if (forEdit != null)
                    {
                        forEdit.QuoteId = quoteId;
                        forEdit.CreatedById = userId;
                        toUpdate.Add(forEdit);
                    }
                    else
                        toDelete.Add(service);
                }


                //update the services to update
                _context.QuoteServices.UpdateRange(toUpdate);
                _context.QuoteServices.AddRange(newServices);
                _context.QuoteServices.RemoveRange(toDelete);

                await _context.SaveChangesAsync();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // TODO: Handle failure
                _logger.LogError("Error updating services", ex);
                transaction.Rollback();
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteQuoteService(QuoteService quoteService)
        {
            quoteService.IsDeleted = true;
            _context.QuoteServices.Update(quoteService);
            return await SaveChanges();
        }

        public async Task<bool> DeleteQuoteServiceRange(IEnumerable<QuoteService> quoteServices)
        {
            foreach (var qs in quoteServices)
            {
                qs.IsDeleted = true;
            }
            _context.QuoteServices.UpdateRange(quoteServices);
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