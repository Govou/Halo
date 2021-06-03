using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class CronJobServiceImpl : ICronJobService
    {
        private readonly ILogger<CronJobServiceImpl> _logger;
        private readonly HalobizContext _context;

        private readonly string _dTrackBaseUrl;
        private readonly string _dTrackUsername;
        private readonly string _dTrackPassword;

        public CronJobServiceImpl(
            IConfiguration config,
            HalobizContext context,
            ILogger<CronJobServiceImpl> logger)
        {
            _context = context;
            _logger = logger;

            _dTrackBaseUrl = config["DTrackBaseUrl"] ?? config.GetSection("AppSettings:DTrackBaseUrl").Value;
            _dTrackUsername = config["DTrackUsername"] ?? config.GetSection("AppSettings:DTrackUsername").Value;
            _dTrackPassword = config["DTrackPassword"] ?? config.GetSection("AppSettings:DTrackPassword").Value;
        }

        public async Task<ApiResponse> MigrateNewCustomersToDTRACK(HttpContext context)
        {
            return new ApiOkResponse(true);
            try
            {
                _logger.LogInformation("Running Customer Creation Job");

                DisableFlurlCertificateValidation();

                var customerAccountsThatHaveNotBeenIntegrated = await _context.Accounts
                    .Where(x => x.IntegrationFlag != true && x.ControlAccount.Caption == "Receivable").ToListAsync();

                if(customerAccountsThatHaveNotBeenIntegrated.Count > 0)
                {
                    _logger.LogInformation("These are the account(s) yet to be integrated");
                    _logger.LogInformation(string.Join(", ", customerAccountsThatHaveNotBeenIntegrated.Select(x => x.Name)));
                }
                else
                {
                    _logger.LogInformation("These are no new customer accounts to integrate.");
                }            

                foreach (var account in customerAccountsThatHaveNotBeenIntegrated)
                {
                    #region Get CustomerDivision, State, Primary Contact, Customer and ControlAccount
                    var customerDivision = await _context.CustomerDivisions.SingleOrDefaultAsync(x => x.ReceivableAccountId == account.Id);
                    if (customerDivision == null)
                    {
                        _logger.LogInformation($"Customer account {account.Name} does not have customer division");
                        continue;
                    }

                    /*if(!customerDivision.StateId.HasValue)
                    {
                        _logger.LogInformation($"Customer account {account.Name} does not have state");
                        continue;
                    };*/

                    var state = await _context.States.FindAsync(customerDivision.StateId ?? 1);

                    if (state == null)
                    {
                        _logger.LogInformation($"Customer account {account.Name} does not have state");
                        continue;
                    }
                    else customerDivision.State = state;

                    if (!customerDivision.PrimaryContactId.HasValue)
                    {
                        _logger.LogInformation($"Customer account {account.Name} does not have primary contact");
                        continue;
                    };
                    var primaryContact = await _context.LeadDivisionContacts.FindAsync(customerDivision.PrimaryContactId.Value);
                    if (primaryContact == null)
                    {
                        _logger.LogInformation($"Customer account {account.Name} does not have primary contact");
                        continue;
                    };

                    var customer = await _context.Customers.FindAsync(customerDivision.CustomerId);
                    if (customer == null)
                    {
                        _logger.LogInformation($"Customer account {account.Name} does not have customer");
                        continue;
                    };

                    if (account.ControlAccountId == 0)
                    {
                        _logger.LogInformation($"Customer account {account.Name} does not have control account");
                        continue;
                    };
                    var controlAccount = await _context.ControlAccounts.FindAsync(account.ControlAccountId);
                    if (controlAccount == null)
                    {
                        _logger.LogInformation($"Customer account {account.Name} does not have control account");
                        continue;
                    }
                    #endregion

                    var tokenResponse = await _dTrackBaseUrl.AppendPathSegment("token")
                        .PostUrlEncodedAsync(new
                        {
                            grant_type = "password",
                            username = _dTrackUsername,
                            password = _dTrackPassword
                        })
                        .ReceiveJson();

                    string token = tokenResponse.access_token;
                    _logger.LogInformation($"The token => {token}");

                    var requestBody = new
                    {
                        CustomerNumber = $"{account.Alias}_{customerDivision.DivisionName.Substring(0, 1)}{customerDivision.Rcnumber.Substring(0, 2)}",
                        Name = customerDivision.DivisionName,
                        GLAccount = controlAccount.Alias, // "180101",
                        ShortName = $"{customerDivision.DivisionName.Substring(0, 3)}{customerDivision.DivisionName.Substring(customerDivision.DivisionName.Length - 4, 3)}{customerDivision.Rcnumber.Substring(customerDivision.Rcnumber.Length - 4, 3)}",
                        AddressLine1 = customerDivision.Address.Length > 30 ? customerDivision.Address.Substring(0, 30) : customerDivision.Address,
                        EmailAddress = customerDivision.Email,
                        TelephoneNumber = customerDivision.PhoneNumber,
                        Location = $"{Extensions.GetStateShortName(customerDivision.State.Capital)}",
                        BusinessSector = Extensions.GetIndustryShortName(customerDivision.Industry),
                        OtherInfo = $"{customerDivision.DivisionName}-{customer.Rcnumber}-{customer.CreatedAt}",
                        Contact = $"{primaryContact.FirstName} {primaryContact.LastName}"
                    };

                    _logger.LogInformation($"Request|{JsonConvert.SerializeObject(requestBody)}");

                    var response = await _dTrackBaseUrl.AllowAnyHttpStatus()
                        .AppendPathSegment("api/Customers/Create")
                        .WithHeader("Authorization", $"Bearer {token}")
                        .PostJsonAsync(requestBody);

                    var responseMessage = await response.GetStringAsync();
                    _logger.LogInformation($"Response | [{response.StatusCode}] | {responseMessage}");

                    if (response.StatusCode == 200 || response.StatusCode == 204)
                    {
                        account.IntegrationFlag = true;
                        _context.Accounts.Update(account);
                        _logger.LogInformation($"Successfully integrated [{account.Name}] with Id [{account.Id}].");
                    }
                    else
                    {
                        _logger.LogError($"The call to the customer creation endpoint failed for [{account.Name}].");
                    }
                }

                return new ApiOkResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occured while running customer creation job");
                _logger.LogError($"Exception details => {ex.Message}");
                _logger.LogError($"Exception details => {ex.StackTrace}");
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> PostNewAccountingRecordsToDTRACK(HttpContext context)
        {
            return new ApiOkResponse(true);
            try
            {
                _logger.LogInformation("Running Account Posting Job");

                DisableFlurlCertificateValidation();

                var integratedCustomerAcccounts = await _context.Accounts
                    .Where(x => x.IntegrationFlag == true && x.ControlAccount.Caption == "Receivable")
                    .ToListAsync();

                _logger.LogInformation($"Integrated Customer Accounts Count => {integratedCustomerAcccounts.Count()}");

                foreach (var account in integratedCustomerAcccounts)
                {
                    #region Get Customer Division and Account Masters
                    _logger.LogInformation($"Processing customer account => {account.Name}");

                    var customerDivisions = await _context.CustomerDivisions
                        .Where(x => x.ReceivableAccountId == account.Id)
                        .ToListAsync();

                    if (customerDivisions == null)
                    {
                        _logger.LogInformation($"No customer division tied to account => [{account.Id}]");
                        return new ApiResponse(500, $"No customer division tied to account => [{ account.Id }]");
                    }

                    if (customerDivisions.Count() > 1)
                    {
                        _logger.LogInformation($"There is a lot of customer divisions for {account.Name}. Skipping..");
                        continue;
                    }

                    var customerDivision = customerDivisions.FirstOrDefault();

                    var accountMasters = await _context.AccountMasters
                        .Where(x => x.CustomerDivisionId == customerDivision.Id && x.IntegrationFlag != true)
                        .ToListAsync();

                    _logger.LogInformation($"Total number of account masters => {accountMasters.Count()}");
                    #endregion

                    foreach (var accountMaster in accountMasters)
                    {
                        #region Get Voucher Type and Account Details
                        _logger.LogInformation($"Processing account master => [{accountMaster.Id}]");

                        var voucherType = await _context.FinanceVoucherTypes.FindAsync(accountMaster.VoucherId);
                        if (voucherType == null)
                        {
                            _logger.LogInformation($"Invalid voucher type for account master => [{accountMaster.Id}]");
                            return new ApiResponse(500, $"Invalid voucher type for account master => [{accountMaster.Id}]");
                        }

                        var accountDetails = await _context.AccountDetails
                            .Where(x => x.AccountMasterId == accountMaster.Id && !x.IntegrationFlag && x.AccountId.HasValue)
                            .ToListAsync();

                        if (accountDetails.Count() < 1)
                        {
                            _logger.LogInformation($"No account details tied to account master => [{accountMaster.Id}]");
                            return new ApiResponse(500, $"No account details tied to account master => [{accountMaster.Id}]");
                        }
                        #endregion

                        var journalLines = new List<object>();

                        foreach (var accountDetail in accountDetails)
                        {
                            #region Get Detail's Account and Control Account
                            var detailAccount = await _context.Accounts.FindAsync(accountDetail.AccountId.Value);
                            if (detailAccount == null) throw new Exception($"Account Detail {accountDetail.Id} is not tied to an account");

                            if (detailAccount.ControlAccountId == 0) throw new Exception($"Account {detailAccount.Name} is not tied to a control account.");

                            var controlAccount = await _context.ControlAccounts.FindAsync(detailAccount.ControlAccountId);
                            if (controlAccount == null) throw new Exception($"Account {detailAccount.Name} is not tied to a control account.");
                            #endregion

                            string costCenter = string.Empty;
                            var caption = controlAccount.Caption.ToLower();
                            if (caption.Contains("revenue") || caption.Contains("income")) costCenter = "05";

                            var alias = detailAccount.Alias;
                            if (caption.ToLower().Equals("receivable") && customerDivision.Rcnumber != null)
                            {
                                alias = $"{alias}_{customerDivision.DivisionName.Substring(0, 1)}{customerDivision.Rcnumber.Substring(0, 2)}";
                            }

                            var journalLine = new
                            {
                                GLAccount = controlAccount.Alias,
                                SubAccount = "01",
                                AccountSection = "01",
                                CostCenter = costCenter,
                                Amount = Math.Abs(accountDetail.Credit - accountDetail.Debit),
                                Factor = (accountDetail.Credit > 0 && accountDetail.Debit == 0) ? -1 : 1,
                                Details = accountDetail.Description,
                                SourceCode = alias
                            };

                            journalLines.Add(journalLine);
                        }

                        string token = await GetAPIToken();

                        var journalHeader = new
                        {
                            Year = accountMaster.CreatedAt.Year,
                            Month = accountMaster.CreatedAt.Month,
                            SubAccount = "01",
                            AccountSection = "01",
                            CurrencyCode = "NGN",
                            ExchangeRate = 1,
                            TranDate = accountMaster.CreatedAt.ToString("s"),
                            DocumentDate = accountMaster.CreatedAt.ToString("s"),
                            TranType = voucherType.Alias ?? "SALE",
                            Module = $"HaloBiz"
                        };

                        var requestBody = new
                        {
                            JournalHeader = journalHeader,
                            JournalLines = journalLines,
                            Notes = accountMaster.Description
                        };

                        _logger.LogInformation($"Request|{JsonConvert.SerializeObject(requestBody)}");

                        var response = await _dTrackBaseUrl.AllowAnyHttpStatus()
                            .AppendPathSegment("api/Journals/PostEntries")
                            .WithHeader("Authorization", $"Bearer {token}")
                            .PostJsonAsync(requestBody);

                        var responseMessage = await response.GetStringAsync();
                        _logger.LogInformation($"Response | [{response.StatusCode}] | {responseMessage}");

                        if (response.StatusCode == 200)
                        {
                            var journalCode = responseMessage;

                            accountMaster.IntegrationFlag = true;
                            accountMaster.DtrackJournalCode = journalCode;

                            _context.AccountMasters.Update(accountMaster);

                            foreach (var acctDetail in accountDetails)
                            {
                                acctDetail.IntegrationFlag = true;
                            }

                            _context.AccountDetails.UpdateRange(accountDetails);

                            _logger.LogInformation($"Successfully integrated account master [{accountMaster.Id}] with details count [{accountDetails.Count()}].");
                        }
                        else
                        {
                            _logger.LogError($"The call to the account posting endpoint failed.");
                        }
                    }
                }

                return new ApiOkResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occured while running account posting job");
                _logger.LogError($"Exception details => {ex.Message}");
                _logger.LogError($"Exception details => {ex.StackTrace}");
                return new ApiResponse(500, ex.Message);
            }
        }

        private async Task<string> GetAPIToken()
        {
            var tokenResponse = await _dTrackBaseUrl.AppendPathSegment("token")
                        .PostUrlEncodedAsync(new
                        {
                            grant_type = "password",
                            username = _dTrackUsername,
                            password = _dTrackPassword
                        }).ReceiveJson();

            string token = tokenResponse.access_token;
            _logger.LogInformation($"The token => {token}");
            return token;
        }

        void DisableFlurlCertificateValidation()
        {
            FlurlHttp.ConfigureClient(_dTrackBaseUrl, cli =>
                cli.Settings.HttpClientFactory = new UntrustedCertClientFactory());
        }
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
}
