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

    public class ClientContactQualificationRepositoryImpl : IClientContactQualificationRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ClientContactQualificationRepositoryImpl> _logger;
        public ClientContactQualificationRepositoryImpl(DataContext context, ILogger<ClientContactQualificationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<ClientContactQualification> SaveClientContactQualification(ClientContactQualification clientContactQualification)
        {
            var clientContactQualificationEntity = await _context.ClientContactQualifications.AddAsync(clientContactQualification);
            if(await SaveChanges())
            {
                return clientContactQualificationEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<ClientContactQualification>> GetClientContactQualifications()
        {
            return await _context.ClientContactQualifications
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<ClientContactQualification> UpdateClientContactQualification(ClientContactQualification clientContactQualification)
        {
             var clientContactQualificationEntity =  _context.ClientContactQualifications.Update(clientContactQualification);
            if(await SaveChanges())
            {
                return clientContactQualificationEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteClientContactQualification(ClientContactQualification clientContactQualification)
        {
            clientContactQualification.IsDeleted = true;
            _context.ClientContactQualifications.Update(clientContactQualification);
            return await SaveChanges();
        }
        public async Task<ClientContactQualification> FindClientContactQualificationById(long Id)
        {
           return await _context.ClientContactQualifications
            .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
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