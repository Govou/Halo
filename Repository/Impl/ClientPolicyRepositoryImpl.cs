using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class ClientPolicyRepositoryImpl : IClientPolicyRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ClientPolicyRepositoryImpl> _logger;
        public ClientPolicyRepositoryImpl(HalobizContext context, ILogger<ClientPolicyRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteClientPolicy(ClientPolicy clientPolicy)
        {
            clientPolicy.IsDeleted = true;
            _context.ClientPolicies.Update(clientPolicy);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ClientPolicy>> FindAllClientPolicies()
        {
            return await _context.ClientPolicies
               .Where(clientPolicy => clientPolicy.IsDeleted == false)
               .OrderBy(clientPolicy => clientPolicy.CreatedAt)
               .ToListAsync();
        }

        public async Task<ClientPolicy> FindClientPolicyById(long Id)
        {
            return await _context.ClientPolicies
                .Where(clientPolicy => clientPolicy.IsDeleted == false)
                .FirstOrDefaultAsync(clientPolicy => clientPolicy.Id == Id && clientPolicy.IsDeleted == false);

        }

        public async Task<ClientPolicy> FindClientPolicyByContractId(long contractId)
        {
            return await _context.ClientPolicies
                .SingleOrDefaultAsync(clientPolicy => 
                        clientPolicy.ContractId == contractId 
                        && clientPolicy.ContractServiceId == null
                        && !clientPolicy.IsDeleted);
        }

        public async Task<ClientPolicy> FindClientPolicyByContractServiceId(long contractServiceId)
        {
            return await _context.ClientPolicies
                .SingleOrDefaultAsync(clientPolicy =>
                        clientPolicy.ContractServiceId == contractServiceId
                        && clientPolicy.ContractId == null
                        && !clientPolicy.IsDeleted);
        }

        /*public async Task<ClientPolicy> FindClientPolicyByName(string name)
        {
            return await _context.ClientPolicies
                 .Where(clientPolicy => clientPolicy.IsDeleted == false)
                 .FirstOrDefaultAsync(clientPolicy => clientPolicy.Caption == name && clientPolicy.IsDeleted == false);

        }*/

        public async Task<ClientPolicy> SaveClientPolicy(ClientPolicy clientPolicy)
        {
            var clientPolicyEntity = await _context.ClientPolicies.AddAsync(clientPolicy);
            if (await SaveChanges())
            {
                return clientPolicyEntity.Entity;
            }
            return null;
        }

        public async Task<ClientPolicy> UpdateClientPolicy(ClientPolicy clientPolicy)
        {
            var clientPolicyEntity = _context.ClientPolicies.Update(clientPolicy);
            if (await SaveChanges())
            {
                return clientPolicyEntity.Entity;
            }
            return null;
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
