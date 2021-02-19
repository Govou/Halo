using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class AccountServiceImpl : IAccountService
    {
        private readonly ILogger<AccountServiceImpl> _logger;
        private readonly IAccountRepository _AccountRepo;
        private readonly IMapper _mapper;

        public AccountServiceImpl(IAccountRepository AccountRepo, ILogger<AccountServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._AccountRepo = AccountRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddAccount(HttpContext context, AccountReceivingDTO AccountReceivingDTO)
        {
            var acctClass = _mapper.Map<Account>(AccountReceivingDTO);
            acctClass.CreatedById = context.GetLoggedInUserId();
            var savedAccount = await _AccountRepo.SaveAccount(acctClass);
            if (savedAccount == null)
            {
                return new ApiResponse(500);
            }
            var AccountTransferDTOs = _mapper.Map<AccountTransferDTO>(acctClass);
            return new ApiOkResponse(AccountTransferDTOs);
        }

        public async Task<ApiResponse> DeleteAccount(long id)
        {
            var AccountTodelete = await _AccountRepo.FindAccountById(id);
            if (AccountTodelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _AccountRepo.DeleteAccount(AccountTodelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAccountByAlias(string alias)
        {
            var Account = await _AccountRepo.FindAccountByAlias(alias);
            if (Account == null)
            {
                return new ApiResponse(404);
            }
            var AccountTransferDTOs = _mapper.Map<AccountTransferDTO>(Account);
            return new ApiOkResponse(AccountTransferDTOs);
        }

        public async Task<ApiResponse> GetAccountById(long id)
        {
            var Account = await _AccountRepo.FindAccountById(id);
            if (Account == null)
            {
                return new ApiResponse(404);
            }
            var AccountTransferDTOs = _mapper.Map<AccountTransferDTO>(Account);
            return new ApiOkResponse(AccountTransferDTOs);
        }

        public async Task<ApiResponse> GetAllAccounts()
        {
            var Accountes = await _AccountRepo.FindAllAccounts();
            if (Accountes == null)
            {
                return new ApiResponse(404);
            }
            var AccountTransferDTOs = _mapper.Map<IEnumerable<AccountTransferDTO>>(Accountes);
            return new ApiOkResponse(AccountTransferDTOs);
        }
        
        public async Task<ApiResponse> GetAllTradeIncomeTaxAccounts()
        {
            var Accountes = await _AccountRepo.FindAllTradeIncomeAccounts();
            if (Accountes == null)
            {
                return new ApiResponse(404);
            }
            var AccountTransferDTOs = _mapper.Map<IEnumerable<AccountTransferDTO>>(Accountes);
            return new ApiOkResponse(AccountTransferDTOs);
        }

        public async Task<ApiResponse> UpdateAccount(long id, AccountReceivingDTO AccountReceivingDTO)
        {
            var AccountToUpdate = await _AccountRepo.FindAccountById(id);
            if (AccountToUpdate == null)
            {
                return new ApiResponse(404);
            }
            AccountToUpdate.Alias = AccountReceivingDTO.Alias;
            AccountToUpdate.Name = AccountReceivingDTO.Name;
            AccountToUpdate.Description = AccountReceivingDTO.Description;
            AccountToUpdate.IsDebitBalance = AccountReceivingDTO.IsDebitBalance;
            AccountToUpdate.IsActive = AccountReceivingDTO.IsActive;
            var updatedAccount = await _AccountRepo.UpdateAccount(AccountToUpdate);

            if (updatedAccount == null)
            {
                return new ApiResponse(500);
            }
            var AccountTransferDTOs = _mapper.Map<AccountTransferDTO>(updatedAccount);
            return new ApiOkResponse(AccountTransferDTOs);
        }
    }
}
