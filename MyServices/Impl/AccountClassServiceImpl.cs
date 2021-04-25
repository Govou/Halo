using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class AccountClassServiceImpl : IAccountClassService
    {
        private readonly ILogger<AccountClassServiceImpl> _logger;
        private readonly IAccountClassRepository _accountClassRepo;
        private readonly IMapper _mapper;

        public AccountClassServiceImpl(IAccountClassRepository accountClassRepo, ILogger<AccountClassServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._accountClassRepo = accountClassRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddAccountClass(HttpContext context, AccountClassReceivingDTO accountClassReceivingDTO)
        {
            var acctClass = _mapper.Map<AccountClass>(accountClassReceivingDTO);
            acctClass.CreatedById = context.GetLoggedInUserId();
            var savedAccountClass = await _accountClassRepo.SaveAccountClass(acctClass);
            if (savedAccountClass == null)
            {
                return new ApiResponse(500);
            }
            var accountClassTransferDTOs = _mapper.Map<AccountClassTransferDTO>(acctClass);
            return new ApiOkResponse(accountClassTransferDTOs);
        }

        public async Task<ApiResponse> DeleteAccountClass(long id)
        {
            var accountClassTodelete = await _accountClassRepo.FindAccountClassById(id);
            if (accountClassTodelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _accountClassRepo.DeleteAccountClass(accountClassTodelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAccountClassByCaption(string caption)
        {
            var accountClass = await _accountClassRepo.FindAccountClassByCaption(caption);
            if (accountClass == null)
            {
                return new ApiResponse(404);
            }
            var accountClassTransferDTOs = _mapper.Map<AccountClassTransferDTO>(accountClass);
            return new ApiOkResponse(accountClassTransferDTOs);
        }

        public async Task<ApiResponse> GetAccountClassById(long id)
        {
            var accountClass = await _accountClassRepo.FindAccountClassById(id);
            if (accountClass == null)
            {
                return new ApiResponse(404);
            }
            var accountClassTransferDTOs = _mapper.Map<AccountClassTransferDTO>(accountClass);
            return new ApiOkResponse(accountClassTransferDTOs);
        }

        public async Task<ApiResponse> GetAllAccountClasses()
        {
            var accountClasses = await _accountClassRepo.FindAllAccountClasses();
            if (accountClasses == null)
            {
                return new ApiResponse(404);
            }
            var accountClassTransferDTOs = _mapper.Map<IEnumerable<AccountClassTransferDTO>>(accountClasses);
            return new ApiOkResponse(accountClassTransferDTOs);
        }

        public async Task<ApiResponse> GetBreakdownOfAccountClass()
        {
            try{
                var accountsClasses = await _accountClassRepo.FindAllAccountClassesDownToAccountDetails();
                var  accountClassesWithValue = new List<AccountClassWithTotalTransferDTO>(accountsClasses.Count());
                foreach (var accountClass in accountsClasses)
                {
                    accountClassesWithValue.Add(CalculateAccountClassBalance(accountClass));
                }
                return new ApiOkResponse(accountClassesWithValue);

            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
        } 

        private AccountClassWithTotalTransferDTO CalculateAccountClassBalance(AccountClass accountClass)
        {
            var controlAccounts = new List<ControlAccountWithTotal>();
            var total = 0.0;
            ControlAccountWithTotal controlAccountWithTotal = null;

            foreach (var controlAccount in accountClass.ControlAccounts)
            {
                controlAccountWithTotal = CalculateControlAccountBalance(controlAccount);
                total += controlAccountWithTotal.Total;
                controlAccounts.Add(controlAccountWithTotal);
            }

            var accountClassWithTotal = _mapper.Map<AccountClassWithTotalTransferDTO>(accountClass); 
            accountClassWithTotal.ControlAccounts = controlAccounts;
            accountClassWithTotal.Total = total;
            return accountClassWithTotal;
        }
        private ControlAccountWithTotal CalculateControlAccountBalance(ControlAccount controlAccount)
        {
            var accounts = controlAccount.Accounts;
            double total = 0.0;
            foreach (var account in accounts)
            {
                total += CalculateAccountBalance(account);
            }

            var controlAccountWithTotal = _mapper.Map<ControlAccountWithTotal>(controlAccount);
            controlAccountWithTotal.Total = total;
            return controlAccountWithTotal;
        }

        private double CalculateAccountBalance(Account account)
        {
            double totalDebit = 0.0;
            double totalCredit = 0.0;

            foreach (var detail in account.AccountDetails)
            {
                totalCredit += detail.Credit;
                totalDebit += detail.Debit;
            }

            var total = totalDebit - totalCredit;

            return account.IsDebitBalance ? total : (total * -1);
        }

        public async Task<ApiResponse> UpdateAccountClass(long id, AccountClassReceivingDTO accountClassReceivingDTO)
        {
            var accountClassToUpdate = await _accountClassRepo.FindAccountClassById(id);
            if (accountClassToUpdate == null)
            {
                return new ApiResponse(404);
            }
            accountClassToUpdate.Caption = accountClassReceivingDTO.Caption;
            accountClassToUpdate.Description = accountClassReceivingDTO.Description;
            var updatedaccountClass = await _accountClassRepo.UpdateAccountClass(accountClassToUpdate);

            if (updatedaccountClass == null)
            {
                return new ApiResponse(500);
            }
            var accountClassTransferDTOs = _mapper.Map<AccountClassTransferDTO>(updatedaccountClass);
            return new ApiOkResponse(accountClassTransferDTOs);
        }
    }
}
