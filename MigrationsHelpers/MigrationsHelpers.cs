using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using HaloBiz.DTOs.ApiDTOs;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HaloBiz.MigrationsHelpers
{
    public class HttpSender
    {
        private string _dTrackUsername = "DEVUSER";
        private string _dTrackPassword = "DhvV2G!fE3Mu$@F";
        private string _dTrackBaseUrl = "https://testplatform.halogensecurityapp.com/";

        private void DisableFlurlCertificateValidation()
        {
            FlurlHttp.ConfigureClient(_dTrackBaseUrl, cli =>
                cli.Settings.HttpClientFactory = new UntrustedCertClientFactory());
        }
        private async Task<string> GetAPIToken()
        {
            try
            {
                var tokenResponse = await _dTrackBaseUrl.AppendPathSegment("token")
                        .PostUrlEncodedAsync(new
                        {
                            grant_type = "password",
                            username = _dTrackUsername,
                            password = _dTrackPassword
                        }).ReceiveJson();

                string token = tokenResponse.access_token;
                return token;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<CustomerList> getCustomers() //
        {
            var token = await GetAPIToken();

            var response = await _dTrackBaseUrl.AllowAnyHttpStatus()
                           .AppendPathSegment("api/Customers")
                           .WithHeader("Authorization", $"Bearer {token}")
                           .GetAsync();

            var responseMessage = await response.GetStringAsync();
            return JsonConvert.DeserializeObject<CustomerList>(responseMessage);
        }

        public async Task<ServiceContracts> getServiceContract(int page) //
        {
            var token = await GetAPIToken();

            var response = await _dTrackBaseUrl.AllowAnyHttpStatus()
                           .AppendPathSegment("api/ServiceContracts")
                           .SetQueryParam("input.pageNumber", page)
                           .WithHeader("Authorization", $"Bearer {token}")
                           .GetAsync();

            var responseMessage = await response.GetStringAsync();
            return JsonConvert.DeserializeObject<ServiceContracts>(responseMessage);
        }

        public async Task<List<Account>> getCutOffPosition(AccountBalanceInput request)
        {
            var token = await GetAPIToken();

            var response = await _dTrackBaseUrl.AllowAnyHttpStatus()
                           .AppendPathSegment($"api/AccountBalances/GetBySubLedger")
                           .SetQueryParam("input.subAccount", request.SubAccount)
                           .SetQueryParam("input.glaccount", request.GLAccount)
                           .SetQueryParam("input.financialYear", request.FinancialYear)
                           .SetQueryParam("input.asAtDate", request.AsAtDate)
                           .WithHeader("Authorization", $"Bearer {token}")
                           .GetAsync();

            var responseMessage = await response.GetStringAsync();
            var result = JsonConvert.DeserializeObject<Ledger>(responseMessage);
            return result.Items;
        }
        public async Task<ServiceContractDetais> getServiceContractDetail(string contractNumber)
        {
            var token = await GetAPIToken();

            var response = await _dTrackBaseUrl.AllowAnyHttpStatus()
                           .AppendPathSegment($"api/Servicecontracts/GetContract")
                           .SetQueryParam("keys.contractNumber", contractNumber)
                           .WithHeader("Authorization", $"Bearer {token}")
                           .GetAsync();

            var responseMessage = await response.GetStringAsync();
            return JsonConvert.DeserializeObject<ServiceContractDetais>(responseMessage);
        }

        public async Task<Customero> getCustomerWithCustomerNumber(string customerNumber)
        {
            var token = await GetAPIToken();

            var response = await _dTrackBaseUrl.AllowAnyHttpStatus()
                           .AppendPathSegment($"api/customers/GetCustomer")
                           .SetQueryParam("keys.customerNumber", customerNumber)
                           .WithHeader("Authorization", $"Bearer {token}")
                           .GetAsync();

            var responseMessage = await response.GetStringAsync();
            return JsonConvert.DeserializeObject<Customero>(responseMessage);
        }
    }

    public class AccountBalanceInput
    {
        /// <summary>
        /// Business unit is mandatory. Usually '01','02','03', etc depending on how many business units you have.
        /// </summary>
        public string SubAccount { get; set; } = "01";
        /// <summary>
        /// Account Type is optional. Possible values are: Income, Expense, Asset, Liability, Capital
        /// </summary>
        public string AccountType { get; set; }
        /// <summary>
        /// SubType is optional
        /// </summary>
        public string SubType { get; set; }
        /// <summary>
        /// GLAccount (Account Number) is optional. Can be set to a specific account you want to get the Balance on.
        /// </summary>
        public string GLAccount { get; set; }

        /// <summary>
        /// Financial Year is mandatory
        /// </summary>
        public int FinancialYear { get; set; }
        /// <summary>
        /// StartPeriod and EndPeriod if both are set, will be used to get the range of period you want to get the summary for..
        /// </summary>
        public int StartPeriod { get; set; }
        public int EndPeriod { get; set; }
        /// <summary>
        /// AsAtDate is optional and nullable. If it has a value, it is used to get the upper limit of Transaction date to use in the summary.
        /// </summary>
        public DateTime? AsAtDate { get; set; }
    }

    public class UntrustedCertClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (a, b, c, d) => true
            };
        }
    }
    public class Customero
    {
        public string CustomerNumber { get; set; }
        public string Name { get; set; }
        public string GLAccount { get; set; }
        public string ShortName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string FaxNumber { get; set; }
        public string Contact { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string ExecContact { get; set; }
        public string ExecEmailAddress { get; set; }
        public string ExecPhoneNumber { get; set; }
        public string FinanceContact { get; set; }
        public string FinanceEmailAddress { get; set; }
        public string FinancePhoneNumber { get; set; }
        public string GroupCode { get; set; }
        public string Location { get; set; }
        public string BusinessSector { get; set; }
        public int LastJobSerial { get; set; }
        public int CreditDays { get; set; }
        public string OtherInfo { get; set; }
        public bool InActive { get; set; }
        public bool Preset { get; set; }
        public bool CashCustomer { get; set; }
        public bool NoBrand { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Archived { get; set; }
        public string SubAccount { get; set; }
        public long CustomerId { get; set; }
        public long CustomerDivisionId { get; set; }
        public CustomerDivision customerDivision { get; set; }
    }

    public class CustomerList
    {
        public List<Customero> Items { get; set; }
        public int TotalCount { get; set; }
    }
    public class ServiceContractItem
    {
        public string ContractNumber { get; set; }
        public int ItemNumber { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public int Taxable { get; set; }
        public string ServiceType { get; set; }
        public object SiteID { get; set; }
        public string CostCenter { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BillingCycle { get; set; }
        public string InvoiceItemDetail { get; set; }
        public int WorkingDays { get; set; }
        public double OvertimeRate { get; set; }
        public string ShiftPattern { get; set; }
        public int BilledYear { get; set; }
        public int BilledMonth { get; set; }
        public int SerialNo { get; set; }      
        public ServiceRelationshipEnum Enum { get; set; } = ServiceRelationshipEnum.Standalone;
        public string AdminDirectTie { get; set; }
        public ServiceTypes ApiContractService { get; set; }
        public long ContractId { get; set; }
    }

    public class ServiceContractDetais : Contracto
    {
        public List<ServiceContractItem> ServiceContractItems { get; set; }
    }
    public class Contracto
    {
        public string ContractNumber { get; set; }
        public string CustomerNumber { get; set; }
        public string Description { get; set; }
        public string OtherInfo { get; set; }
        public DateTime StartDate { get; set; }
        public string BillTo { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public int InvoiceDueDays { get; set; }
        public int Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string Remark { get; set; }
        public string SourceContract { get; set; }
        public string BaseContract { get; set; }
        public int FlowID { get; set; }
        public string SubAccount { get; set; }
        public string AccountSection { get; set; }
    }

    public class ServiceContracts
    {
        public List<Contracto> Items { get; set; }
        public int TotalCount { get; set; }
    }
    public class ServiceTypes
    {
        public string ServiceType { get; set; }
        public string ServiceTypeName { get; set; }
        public long ServiceId { get; set; }
        public bool IsVatable { get; set; }
        public string AdminDirectTie { get; set; }
        public ServiceRelationshipEnum Enum { get; set; } = ServiceRelationshipEnum.Standalone;
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Account
    {
        public string GLAccount { get; set; }
        public string AccountName { get; set; }
        public string SubLedgerType { get; set; }
        public string SubLedgerCode { get; set; }
        public string SubLedgerName { get; set; }
        public double Amount { get; set; }
    }

    public class Ledger
    {
        public List<Account> Items { get; set; }
        public int TotalCount { get; set; }
    }


}
