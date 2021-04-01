using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.LAMS;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadConversionService
    {
        Task<bool> ConvertLeadToClient(long leadId, long loggedInUserId);
        Task<bool> GenerateInvoices(ContractService contractService, long customerDivisionId, string serviceCode, long loggedInUserId);
        Task<bool> GenerateAmortizations(ContractService contractService, CustomerDivision customerDivision);
        Task<bool> CreateTaskAndDeliverables(ContractService contractServcie, long customerDivisionId, string endorsementType, long? loggedInUserId);
        Task<bool> CreateAccounts(
                                    ContractService contractService,
                                    CustomerDivision customerDivision,
                                    long branchId,
                                    long officeId,
                                    Services service,
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