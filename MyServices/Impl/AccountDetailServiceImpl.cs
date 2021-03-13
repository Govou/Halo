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
    public class AccountDetailServiceImpl : IAccountDetailService
    { 
        private readonly ILogger<AccountDetailServiceImpl> _logger;
        private readonly IAccountDetailsRepository _AccountDetailsRepo;
        private readonly IMapper _mapper;

    public AccountDetailServiceImpl(IAccountDetailsRepository accountDetailsRepo, ILogger<AccountDetailServiceImpl> logger, IMapper mapper)
    {
        this._mapper = mapper;
        this._AccountDetailsRepo = accountDetailsRepo;
        this._logger = logger;
    }

    public async Task<ApiResponse> AddAccountDetail(HttpContext context, AccountDetailReceivingDTO accountDetailReceivingDTO)
    {
        var acctClass = _mapper.Map<AccountDetail>(accountDetailReceivingDTO);
            acctClass.CreatedById = context.GetLoggedInUserId();
            var savedAccountDetail = await _AccountDetailsRepo.SaveAccountDetail(acctClass);
        if (savedAccountDetail == null)
        {
            return new ApiResponse(500);
        }
        var AccountDetailTransferDTOs = _mapper.Map<AccountDetailTransferDTO>(acctClass);
        return new ApiOkResponse(AccountDetailTransferDTOs);
    }

    public async Task<ApiResponse> DeleteAccountDetail(long id)
    {
        var AccountDetailToDelete = await _AccountDetailsRepo.FindAccountDetailById(id);
        if (AccountDetailToDelete == null)
        {
            return new ApiResponse(404);
        }

        if (!await _AccountDetailsRepo.DeleteAccountDetail(AccountDetailToDelete))
        {
            return new ApiResponse(500);
        }

        return new ApiOkResponse(true);
    }


    public async Task<ApiResponse> GetAccountDetailById(long id)
    {
        var AccountDetail = await _AccountDetailsRepo.FindAccountDetailById(id);
        if (AccountDetail == null)
        {
            return new ApiResponse(404);
        }
        var AccountDetailTransferDTOs = _mapper.Map<AccountDetailTransferDTO>(AccountDetail);
        return new ApiOkResponse(AccountDetailTransferDTOs);
    }

    public async Task<ApiResponse> GetAllAccountDetails()
    {
        var AccountDetail = await _AccountDetailsRepo.FindAllAccountDetails();
        if (AccountDetail == null)
        {
            return new ApiResponse(404);
        }
        var AccountDetailTransferDTOs = _mapper.Map<IEnumerable<AccountDetailTransferDTO>>(AccountDetail);
        return new ApiOkResponse(AccountDetailTransferDTOs);
    }


        public async Task<ApiResponse> UpdateAccountDetail(long id, AccountDetailReceivingDTO accountDetailReceivingDTO)
    {
        var AccountDetailToUpdate = await _AccountDetailsRepo.FindAccountDetailById(id);
        if (AccountDetailToUpdate == null)
        {
            return new ApiResponse(404);
        }
            AccountDetailToUpdate.Description = accountDetailReceivingDTO.Description; 
            AccountDetailToUpdate.OfficeId = accountDetailReceivingDTO.OfficeId;
            AccountDetailToUpdate.BranchId = accountDetailReceivingDTO.BranchId; 
            AccountDetailToUpdate.AccountMasterId = accountDetailReceivingDTO.AccountMasterId;
            AccountDetailToUpdate.AccountId = accountDetailReceivingDTO.AccountId;
        var updatedAccountDetail = await _AccountDetailsRepo.UpdateAccountDetail(AccountDetailToUpdate);

        if (updatedAccountDetail == null)
        {
            return new ApiResponse(500);
        }
        var AccountDetailTransferDTOs = _mapper.Map<AccountDetailTransferDTO>(updatedAccountDetail);
        return new ApiOkResponse(AccountDetailTransferDTOs);
    }
}
}
