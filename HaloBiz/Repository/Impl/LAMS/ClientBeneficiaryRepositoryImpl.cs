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
    public class ClientBeneficiaryRepositoryImpl : IClientBeneficiaryRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ClientBeneficiaryRepositoryImpl> _logger;
        public ClientBeneficiaryRepositoryImpl(HalobizContext context, ILogger<ClientBeneficiaryRepositoryImpl> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ClientBeneficiary> SaveClientBeneficiary(ClientBeneficiary clientBeneficiary)
        {
            var clientBeneficiaryEntity = await _context.ClientBeneficiaries.AddAsync(clientBeneficiary);
            if(await SaveChanges())
            {
                return clientBeneficiaryEntity.Entity;
            }
            return null;
        }

        public async Task<ClientBeneficiary> FindClientBeneficiaryById(long Id)
        {
            return await _context.ClientBeneficiaries
                .FirstOrDefaultAsync(clientBeneficiary => clientBeneficiary.Id == Id && clientBeneficiary.IsDeleted == false);
        }

        public async Task<ClientBeneficiary> FindClientBeneficiaryByCode(string code)
        {
            return await _context.ClientBeneficiaries
                .FirstOrDefaultAsync( clientBeneficiary => clientBeneficiary.BeneficiaryCode == code && clientBeneficiary.IsDeleted == false);
        }

        public async Task<IEnumerable<ClientBeneficiary>> FindAllClientBeneficiary()
        {
            return await _context.ClientBeneficiaries
                .Where(clientBeneficiary => clientBeneficiary.IsDeleted == false)
                .OrderBy(clientBeneficiary => clientBeneficiary.CreatedAt)
                .Include(x => x.Relationship)
                .ToListAsync();
        }

        public async Task<ClientBeneficiary> UpdateClientBeneficiary(ClientBeneficiary clientBeneficiary)
        {
            var clientBeneficiaryEntity =  _context.ClientBeneficiaries.Update(clientBeneficiary);
            if(await SaveChanges())
            {
                return clientBeneficiaryEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteClientBeneficiary(ClientBeneficiary clientBeneficiary)
        {
            clientBeneficiary.IsDeleted = true;
            _context.ClientBeneficiaries.Update(clientBeneficiary);
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