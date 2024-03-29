﻿using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using halobiz_backend.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models.Halobiz;
using HaloBiz.DTOs;
using Microsoft.Extensions.Configuration;

namespace HaloBiz.MyServices.Impl
{
    public class AccountServiceImpl : IAccountService
    {
        private readonly ILogger<AccountServiceImpl> _logger;
        private readonly IAccountRepository _AccountRepo;
        private readonly IMapper _mapper;
        private readonly HalobizContext _context;
        private readonly IConfiguration _configuration;

        public AccountServiceImpl(HalobizContext context, IConfiguration conf, IAccountRepository AccountRepo, ILogger<AccountServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._AccountRepo = AccountRepo;
            this._logger = logger;
            _context = context;
            _configuration = conf;
        }

        public async Task<ApiCommonResponse> AddAccount(HttpContext context, AccountReceivingDTO AccountReceivingDTO)
        {
            var acctClass = _mapper.Map<Account>(AccountReceivingDTO);
            acctClass.CreatedById = context.GetLoggedInUserId();
            var savedAccount = await _AccountRepo.SaveAccount(acctClass);
            if (savedAccount == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var AccountTransferDTOs = _mapper.Map<AccountTransferDTO>(acctClass);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountTransferDTOs);
        }

        public async Task<ApiCommonResponse> DeleteAccount(long id)
        {
            var AccountTodelete = await _AccountRepo.FindAccountById(id);
            if (AccountTodelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _AccountRepo.DeleteAccount(AccountTodelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAccountByAlias(string alias)
        {
            var Account = await _AccountRepo.FindAccountByAlias(alias);
            if (Account == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var AccountTransferDTOs = _mapper.Map<AccountTransferDTO>(Account);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetAccountById(long id)
        {
            var Account = await _AccountRepo.FindAccountById(id);
            if (Account == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var AccountTransferDTOs = _mapper.Map<AccountTransferDTO>(Account);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetAllAccounts()
        {
            var Accountes = await _AccountRepo.FindAllAccounts();
            if (Accountes == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var AccountTransferDTOs = _mapper.Map<IEnumerable<AccountTransferDTO>>(Accountes);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountTransferDTOs);
        }
        
        public async Task<ApiCommonResponse> GetCashBookAccounts()
        {
            var Accountes = await _AccountRepo.GetCashAccounts();
            if (Accountes == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var AccountTransferDTOs = _mapper.Map<IEnumerable<AccountTransferDTO>>(Accountes);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetServiceAccounts()
        {
            var serviceAccounts = await _context.ServiceAccounts
                .Include(x=>x.Account)
                    .ThenInclude(x=>x.ControlAccount)
                .Include(x=>x.Service)
                .ToListAsync();

            return serviceAccounts.Any()
                ? CommonResponse.Send(ResponseCodes.SUCCESS, serviceAccounts)
                : CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
        }

        public async Task<ApiCommonResponse> GetAccountForService()
        {
            var controlAcc = _configuration.GetSection("AccountsInformation:CostOfSalesControlAccount").Value;
            if(string.IsNullOrEmpty(controlAcc))
                CommonResponse.Send(ResponseCodes.FAILURE,null, "Cost of Sales control account number not registered in appsetting.json");
           
            var accountNumber = long.Parse(controlAcc);
            var controlAccount = await _context.ControlAccounts.Where(x=>x.AccountNumber == accountNumber).FirstOrDefaultAsync();

            if (controlAccount==null)
                CommonResponse.Send(ResponseCodes.FAILURE, null, "Cost of Sales control account number in appsettings does not exist on the system");
            
            var accounts = await _context.Accounts
                .Where(x=>x.ControlAccountId==controlAccount.Id)
                .Include(x => x.ControlAccount)
                .ToListAsync();

            return accounts.Any()
                ? CommonResponse.Send(ResponseCodes.SUCCESS, accounts)
                : CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
        }

        public async Task<ApiCommonResponse> SaveServiceAccounts(ServiceAccountsDTO dto, HttpContext context)
        {
            if(dto.ServiceId==null || dto.AccountId==null)
                return CommonResponse.Send(ResponseCodes.FAILURE,null, "Some required inputs are missing"); ;

            try
            {
                var userId = context.GetLoggedInUserId();
                var req = new ServiceAccount
                {
                    AccountId = dto.AccountId,
                    ServiceId = dto.ServiceId,
                    CreatedById = userId,
                    CreatedAt = DateTime.Now,
                    Description = dto.Description
                    
                };

               await _context.ServiceAccounts.AddAsync(req);
              var affected = await _context.SaveChangesAsync();
                return affected > 0 
                    ? CommonResponse.Send(ResponseCodes.SUCCESS) 
                    : CommonResponse.Send(ResponseCodes.FAILURE, null, "Not saved");
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, e.Message);
            }
        }

        public async Task<ApiCommonResponse> SearchForAccountDetails(AccountSearchDTO accountSearchDTO)
        {
            var account = await _AccountRepo.FindAccountById(accountSearchDTO.AccountId);
            if(account == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            double balance;
            if( accountSearchDTO.VoucherTypeIds.Count() > 0)
            {
                balance = account.AccountDetails
                    .Where(x => x.TransactionDate < accountSearchDTO.TransactionStart
                                && accountSearchDTO.VoucherTypeIds.Contains(x.VoucherId))
                    .Sum(x => x.Debit - x.Credit);

                account.AccountDetails = account.AccountDetails.Where(
                x => x.TransactionDate >= accountSearchDTO.TransactionStart 
                    && x.TransactionDate <= accountSearchDTO.TransactionEnd 
                        && accountSearchDTO.VoucherTypeIds.Contains(x.VoucherId)).ToList();
            }else{
                balance = account.AccountDetails
                    .Where(x => x.TransactionDate < accountSearchDTO.TransactionStart)
                    .Sum(x => x.Debit - x.Credit);

                account.AccountDetails = account.AccountDetails.Where(
                x => x.TransactionDate >= accountSearchDTO.TransactionStart 
                    && x.TransactionDate <= accountSearchDTO.TransactionEnd 
                      ).ToList();
            }

            var openingBalanceAccountDetail = new AccountDetail
            {
                 TransactionDate = accountSearchDTO.TransactionStart,
                 TransactionId = "NIL",
                 Description = "Opening Balance",
                 Debit = balance,
                 Credit = 0
            };

            account.AccountDetails = account.AccountDetails.Prepend(openingBalanceAccountDetail).ToList();

            var AccountTransferDTOs = _mapper.Map<AccountTransferDTO>(account);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountTransferDTOs);
        }
        public async Task<ApiCommonResponse> GetAllTradeIncomeTaxAccounts()
        {
            var Accountes = await _AccountRepo.FindAllTradeIncomeAccounts();
            if (Accountes == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var AccountTransferDTOs = _mapper.Map<IEnumerable<AccountTransferDTO>>(Accountes);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateAccount(long id, AccountReceivingDTO AccountReceivingDTO)
        {
            var AccountToUpdate = await _AccountRepo.FindAccountById(id);
            if (AccountToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            AccountToUpdate.Alias = AccountReceivingDTO.Alias;
            AccountToUpdate.Name = AccountReceivingDTO.Name;
            AccountToUpdate.Description = AccountReceivingDTO.Description;
            AccountToUpdate.IsDebitBalance = AccountReceivingDTO.IsDebitBalance;
            AccountToUpdate.IsActive = AccountReceivingDTO.IsActive;
            var updatedAccount = await _AccountRepo.UpdateAccount(AccountToUpdate);

            if (updatedAccount == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var AccountTransferDTOs = _mapper.Map<AccountTransferDTO>(updatedAccount);
            return CommonResponse.Send(ResponseCodes.SUCCESS,AccountTransferDTOs);
        }
    }
}
