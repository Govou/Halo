using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IClientPolicyRepository
    {
        Task<ClientPolicy> SaveClientPolicy(ClientPolicy clientPolicy);
        Task<ClientPolicy> FindClientPolicyById(long Id);
        Task<ClientPolicy> FindClientPolicyByContractServiceId(long Id);
        //Task<ClientPolicy> FindClientPolicyByName(string name);
        Task<IEnumerable<ClientPolicy>> FindAllClientPolicies();
        Task<ClientPolicy> UpdateClientPolicy(ClientPolicy clientPolicy);
        Task<bool> DeleteClientPolicy(ClientPolicy clientPolicy);
    }
}
