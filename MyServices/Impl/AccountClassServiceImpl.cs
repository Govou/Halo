using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Repository;
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

        public async Task<ApiResponse> AddAccountClass(AccountClassReceivingDTO accountClassReceivingDTO)
        {
            var acctClass = _mapper.Map<AccountClass>(accountClassReceivingDTO);
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

        public async Task<ApiResponse> UpdateAccountClass(long id, AccountClassReceivingDTO accountClassReceivingDTO)
        {
            var accountClassToUpdate = await _accountClassRepo.FindAccountClassById(id);
            if (accountClassToUpdate == null)
            {
                return new ApiResponse(404);
            }
            accountClassToUpdate.Caption = accountClassReceivingDTO.Caption;
            accountClassToUpdate.Description = accountClassReceivingDTO.Description;
            accountClassToUpdate.CreatedById = accountClassReceivingDTO.CreatedById;
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
