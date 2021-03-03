using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface IClientBeneficiaryRepository
    {
        Task<ClientBeneficiary> SaveClientBeneficiary(ClientBeneficiary clientBeneficiary);
        Task<ClientBeneficiary> FindClientBeneficiaryById(long Id);
        Task<ClientBeneficiary> FindClientBeneficiaryByCode(string code);
        Task<IEnumerable<ClientBeneficiary>> FindAllClientBeneficiary();
        Task<ClientBeneficiary> UpdateClientBeneficiary(ClientBeneficiary clientBeneficiary);
        Task<bool> DeleteClientBeneficiary(ClientBeneficiary clientBeneficiary);
    }
}