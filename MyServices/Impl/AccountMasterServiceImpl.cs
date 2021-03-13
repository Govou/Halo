using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Repository;
using halobiz_backend.DTOs.QueryParamsDTOs;
using halobiz_backend.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class AccountMasterServiceImpl : IAccountMasterService
    {
        private readonly ILogger<AccountMasterServiceImpl> _logger;
        private readonly IAccountMasterRepository _AccountMasterRepo;
        private readonly IMapper _mapper;

        public AccountMasterServiceImpl(IAccountMasterRepository accountMasterRepo, ILogger<AccountMasterServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._AccountMasterRepo = accountMasterRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddAccountMaster(HttpContext context, AccountMasterReceivingDTO accountMasterReceivingDTO)
        {
            var acctClass = _mapper.Map<AccountMaster>(accountMasterReceivingDTO);
            acctClass.CreatedById = context.GetLoggedInUserId();
            var savedAccountMaster = await _AccountMasterRepo.SaveAccountMaster(acctClass);
            if (savedAccountMaster == null)
            {
                return new ApiResponse(500);
            }
            var AccountMasterTransferDTOs = _mapper.Map<AccountMasterTransferDTO>(acctClass);
            return new ApiOkResponse(AccountMasterTransferDTOs);
        }

        public async Task<ApiResponse> DeleteAccountMaster(long id)
        {
            var AccountMasterToDelete = await _AccountMasterRepo.FindAccountMasterById(id);
            if (AccountMasterToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _AccountMasterRepo.DeleteAccountMaster(AccountMasterToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAccountMasterById(long id)
        {
            var AccountMaster = await _AccountMasterRepo.FindAccountMasterById(id);
            if (AccountMaster == null)
            {
                return new ApiResponse(404);
            }
            var AccountMasterTransferDTOs = _mapper.Map<AccountMasterTransferDTO>(AccountMaster);
            return new ApiOkResponse(AccountMasterTransferDTOs);
        }

        public async Task<ApiResponse> QueryAccountMasters(AccountMasterTransactionDateQueryParams query)
        {
            if(query.VoucherTypeIds != null && query.VoucherTypeIds.Count > 0  
                    && query.StartDate != null && query.EndDate != null){
                return await GetAllAccountMastersByVoucherId(query);
            }
            else if(query.StartDate != null && query.EndDate != null && query.TransactionId == null)
            {
                return await GetAllAccountMastersByTransactionDate(query);
            }
            else if(query.ClientId != null && query.ClientId > 0 & query.Years != null)
            {
                return await GetAllAccountMastersByCustomerIdAndContractYear(query);
            }
            else if(query.TransactionId != null)
            {
                return await GetAllAccountMastersByTransactionId(query.TransactionId );
            }else{
                return await GetAllAccountMasters();
            }
        }
        public async Task<ApiResponse> GetAllAccountMasters()
        {
            var AccountMaster = await _AccountMasterRepo.FindAllAccountMasters();
            if (AccountMaster == null)
            {
                return new ApiResponse(404);
            }
            var AccountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(AccountMaster);
            return new ApiOkResponse(AccountMasterTransferDTOs);
        }
        public async Task<ApiResponse> GetAllAccountMastersByTransactionDate(AccountMasterTransactionDateQueryParams query)
        {
            try{
                var queryable =  _AccountMasterRepo.GetAccountMastersQueryable();
                var accountMasters = await queryable.Where(x => x.CreatedAt >= query.StartDate
                            && x.CreatedAt <= query.EndDate && x.IsDeleted == false).ToListAsync();
                var AccountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
                return new ApiOkResponse(AccountMasterTransferDTOs);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
        }
        public async Task<ApiResponse> GetAllAccountMastersByVoucherId(AccountMasterTransactionDateQueryParams query)
        {
            try{
                var queryable =  _AccountMasterRepo.GetAccountMastersQueryable();
                var accountMasters = await queryable.Where(x => x.CreatedAt >= query.StartDate
                            && x.CreatedAt <= query.EndDate && !x.IsDeleted && query.VoucherTypeIds.Contains(x.VoucherId)).ToListAsync();
                var AccountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
                return new ApiOkResponse(AccountMasterTransferDTOs);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
        }
        public async Task<ApiResponse> GetAllAccountMastersByTransactionId(string transactionId)
        {
            var accountMasters = await _AccountMasterRepo.FindAccountMastersByTransactionId(transactionId);
            if (accountMasters == null)
            {
                return new ApiResponse(404);
            }
            var accountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
            return new ApiOkResponse(accountMasterTransferDTOs);
        }

        public async Task<ApiResponse> GetAllAccountMastersByCustomerIdAndContractYear(AccountMasterTransactionDateQueryParams query)
        {
            try{
                var accountMasters = await _AccountMasterRepo.FindAllAccountMastersByCustomerId(query);
                var accountMasterTransferDTOs = _mapper.Map<IEnumerable<AccountMasterTransferDTO>>(accountMasters);
                return new ApiOkResponse(accountMasterTransferDTOs);
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return new ApiResponse(500);
            }
            
        }

        public async Task<ApiResponse> UpdateAccountMaster(long id, AccountMasterReceivingDTO accountMasterReceivingDTO)
        {
            var AccountMasterToUpdate = await _AccountMasterRepo.FindAccountMasterById(id);
            if (AccountMasterToUpdate == null)
            {
                return new ApiResponse(404);
            }
            AccountMasterToUpdate.Description = accountMasterReceivingDTO.Description;
            AccountMasterToUpdate.OfficeId = accountMasterReceivingDTO.OfficeId;
            AccountMasterToUpdate.BranchId = accountMasterReceivingDTO.BranchId;
            AccountMasterToUpdate.CustomerDivisionId = accountMasterReceivingDTO.CustomerDivisionId;
            var updatedAccountMaster = await _AccountMasterRepo.UpdateAccountMaster(AccountMasterToUpdate);

            if (updatedAccountMaster == null)
            {
                return new ApiResponse(500);
            }
            var AccountMasterTransferDTOs = _mapper.Map<AccountMasterTransferDTO>(updatedAccountMaster);
            return new ApiOkResponse(AccountMasterTransferDTOs);
        }
    }
    }
