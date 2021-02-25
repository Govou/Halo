using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Model.LAMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface ICustomerDivisionRepository
    {
        Task<CustomerDivision> SaveCustomerDivision(CustomerDivision entity);

        Task<CustomerDivision> FindCustomerDivisionById(long Id);
        Task<CustomerDivision> GetCustomerDivisionBreakDownById(long id);
        Task<CustomerDivision> FindCustomerDivisionByName(string name);
        Task<IEnumerable<CustomerDivision>> FindAllCustomerDivision();
         Task<List<ContractToPaidAmountTransferDTO>> GetPaymentsPerContractByCustomerDivisionId(long customerId);
        Task<CustomerDivision> UpdateCustomerDivision(CustomerDivision entity);
        Task<bool> DeleteCustomerDivision(CustomerDivision entity);
        Task<IEnumerable<object>> FindCustomerDivisionsByGroupType(long groupTypeId);
    }
}
