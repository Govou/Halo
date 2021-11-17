using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;


namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadConversionService
    {
        Task<(bool, string)> ConvertLeadToClient(long leadId, long loggedInUserId);
        Task<(bool, string)> GenerateInvoices(ContractService contractService, long customerDivisionId, string serviceCode, long loggedInUserId);
        Task<(bool, string)> GenerateAmortizations(ContractService contractService, CustomerDivision customerDivision);
        Task<(bool, string)> RemoveAmortizations(ContractService contractService, CustomerDivision customerDivision);

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
                                    bool isReversal
                                    );
        List<Invoice> GenerateListOfInvoiceCycle(
                                            ContractService contractService, 
                                            long customerDivisionId,
                                            string serviceCode,
                                            long loggedInUserId
                                            );
    }
}