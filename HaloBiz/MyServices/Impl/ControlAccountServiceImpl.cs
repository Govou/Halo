using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HalobizMigrations.Data;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class ControlAccountServiceImpl : IControlAccountService
    {
        private readonly ILogger<ControlAccountServiceImpl> _logger;
        private readonly IControlAccountRepository _controlAccountRepo;
        private readonly IMapper _mapper;

        public ControlAccountServiceImpl(ILogger<ControlAccountServiceImpl> logger,  IControlAccountRepository controlAccountRepo, IMapper mapper)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._controlAccountRepo = controlAccountRepo;
        }

        public async Task<ApiCommonResponse> AddControlAccount(HttpContext context, ControlAccountReceivingDTO controlAccountReceivingDTO)
        {

                var controlAcc = _mapper.Map<ControlAccount>(controlAccountReceivingDTO);
                controlAcc.CreatedById = context.GetLoggedInUserId();

                var savedControlAccount = await _controlAccountRepo.SaveControlAccount(controlAcc);
                if (savedControlAccount == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
                var controlAccountTransferDTOs = _mapper.Map<ControlAccountTransferDTO>(controlAcc);
                return CommonResponse.Send(ResponseCodes.SUCCESS,controlAccountTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteControlAccount(long id)
        {
            var controlAccountTodelete = await _controlAccountRepo.FindControlAccountById(id);
            if (controlAccountTodelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _controlAccountRepo.DeleteControlAccount(controlAccountTodelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetControlAccountByAlias(string alias)
        {
            var controlAccount = await _controlAccountRepo.FindControlAccountByAlias(alias);
            if (controlAccount == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var controlAccountTransferDTOs = _mapper.Map<ControlAccountTransferDTO>(controlAccount);
            return CommonResponse.Send(ResponseCodes.SUCCESS,controlAccountTransferDTOs);
        }
        public async Task<ApiCommonResponse> GetControlAccountByCaption(string caption)
        {
            var controlAccount = await _controlAccountRepo.FindControlAccountByName(caption);
            if (controlAccount == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var controlAccountTransferDTOs = _mapper.Map<ControlAccountTransferDTO>(controlAccount);
            return CommonResponse.Send(ResponseCodes.SUCCESS,controlAccountTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetControlAccountById(long id)
        {
            var controlAccount = await _controlAccountRepo.FindControlAccountById(id);
            if (controlAccount == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var controlAccountTransferDTOs = _mapper.Map<ControlAccountTransferDTO>(controlAccount);
            return CommonResponse.Send(ResponseCodes.SUCCESS,controlAccountTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetAllControlAccounts()
        {
            var controlAccountes = await _controlAccountRepo.FindAllControlAccount();
            if (controlAccountes == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var controlAccountTransferDTOs = _mapper.Map<IEnumerable<ControlAccountTransferDTO>>(controlAccountes);
            return CommonResponse.Send(ResponseCodes.SUCCESS,controlAccountTransferDTOs);
        }
        public async Task<ApiCommonResponse> GetAllIncomeControlAccounts()
        {
            var controlAccounts = await _controlAccountRepo.FindAllIncomeControlAccount();
            if (controlAccounts == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var controlAccountTransferDTOs = _mapper.Map<IEnumerable<ControlAccountTransferDTO>>(controlAccounts);
            return CommonResponse.Send(ResponseCodes.SUCCESS,controlAccountTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateControlAccount(long id, ControlAccountReceivingDTO controlAccountReceivingDTO)
        {
            var controlAccountToUpdate = await _controlAccountRepo.FindControlAccountById(id);
            if (controlAccountToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            controlAccountToUpdate.Alias = controlAccountReceivingDTO.Alias;
            controlAccountToUpdate.Description = controlAccountReceivingDTO.Description;
            var updatedControlAccount = await _controlAccountRepo.UpdateControlAccount(controlAccountToUpdate);

            if (updatedControlAccount == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var controlAccountTransferDTOs = _mapper.Map<ControlAccountTransferDTO>(updatedControlAccount);
            return CommonResponse.Send(ResponseCodes.SUCCESS,controlAccountTransferDTOs);
        }
    }
}