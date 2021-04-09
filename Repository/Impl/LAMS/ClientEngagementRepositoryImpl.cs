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
    public class ClientEngagementRepositoryImpl : IClientEngagementRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ClientEngagementRepositoryImpl> _logger;
        public ClientEngagementRepositoryImpl(DataContext context, ILogger<ClientEngagementRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<ClientEngagement> SaveClientEngagement(ClientEngagement clientEngagement)
        {
            var clientEngagementEntity = await _context.ClientEngagements.AddAsync(clientEngagement);
            if(await SaveChanges())
            {
                return clientEngagementEntity.Entity;
            }
            return null;
        }

        public async Task<ClientEngagement> FindClientEngagementById(long Id)
        {
            return await _context.ClientEngagements
                .FirstOrDefaultAsync( clientEngagement => clientEngagement.Id == Id && clientEngagement.IsDeleted == false);
        }

        public async Task<ClientEngagement> FindClientEngagementByName(string name)
        {
            return await _context.ClientEngagements
                .FirstOrDefaultAsync( clientEngagement => clientEngagement.Caption == name && clientEngagement.IsDeleted == false);
        }

        public async Task<IEnumerable<ClientEngagement>> FindAllClientEngagement()
        {
            return await _context.ClientEngagements
                .Where(clientEngagement => clientEngagement.IsDeleted == false)
                .OrderBy(clientEngagement => clientEngagement.CreatedAt)
                .ToListAsync();
        }

        public async Task<ClientEngagement> UpdateClientEngagement(ClientEngagement clientEngagement)
        {
            var clientEngagementEntity =  _context.ClientEngagements.Update(clientEngagement);
            if(await SaveChanges())
            {
                return clientEngagementEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteClientEngagement(ClientEngagement clientEngagement)
        {
            clientEngagement.IsDeleted = true;
            _context.ClientEngagements.Update(clientEngagement);
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