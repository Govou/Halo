using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;


namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadConversionService
    {
        Task<bool> onMigrationAccountsForContracts(ContractService contractService,
                                                                      CustomerDivision customerDivision,
                                                                      long contractId, long userId);
        Task<(bool, string)> ConvertLeadToClient(long leadId, long loggedInUserId);
        Task<(bool, string)> GenerateInvoices(ContractService contractService, long customerDivisionId, string serviceCode, long loggedInUserId);
        Task<(bool, string)> GenerateAmortizations(ContractService contractService, CustomerDivision customerDivision, double billableAmount, ContractServiceForEndorsement endorsement =  null);

        Task<(bool, string)> CreateTaskAndDeliverables(ContractService contractServcie, long customerDivisionId, string endorsementType, long? loggedInUserId);
        Task<(bool, string)> CreateAccounts(
                                    ContractService contractService,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId,
                                    Service service,
                                    FinanceVoucherType accountVoucherType,
                                    QuoteService quoteService,
                                    long loggedInUserId,
                                    bool isReversal,
                                    Invoice invoice
                                    );
        List<Invoice> GenerateListOfInvoiceCycle(
                                            ContractService contractService, 
                                            long customerDivisionId,
                                            string serviceCode,
                                            long loggedInUserId
                                            );

        public Task<string> GetDtrackCustomerNumber(CustomerDivision customer);
    }
}