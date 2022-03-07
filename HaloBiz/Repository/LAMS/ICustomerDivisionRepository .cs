using HaloBiz.DTOs.TransferDTOs.LAMS;
using HalobizMigrations.Models;
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
        Task<CustomerDivision> FindCustomerDivisionByDTrackCustomerNumber(string dTrackCustomerNumber);
        Task<List<TaskFulfillment>> FindTaskAndFulfillmentsByCustomerDivisionId(long customerDivisionId);
        Task<List<CustomerDivision>> GetClientsWithSecuredMobilityContractServices();
        Task<CustomerDivision> GetCustomerDivisionBreakDownById(long id);
        Task<CustomerDivision> FindCustomerDivisionByName(string name);
        Task<IEnumerable<object>> FindAllCustomerDivision();
         Task<List<ContractToPaidAmountTransferDTO>> GetPaymentsPerContractByCustomerDivisionId(long customerId);
        Task<CustomerDivision> UpdateCustomerDivision(CustomerDivision entity);
        Task<bool> DeleteCustomerDivision(CustomerDivision entity);
        Task<IEnumerable<object>> FindCustomerDivisionsByGroupType(long groupTypeId);
        Task<IEnumerable<object>> GetClientsUnAssignedToRMSbu();
        Task<IEnumerable<object>> GetClientsAttachedToRMSbu(long sbuId);
        Task<IEnumerable<object>> GetRMSbuClientsByGroupType(long sbuId, long clientTypeId);
    }
}
