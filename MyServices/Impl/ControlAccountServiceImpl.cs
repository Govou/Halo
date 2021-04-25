using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HalobizMigrations.Data;
using HaloBiz.DTOs.ApiDTOs;
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

        public async Task<ApiResponse> AddControlAccount(HttpContext context, ControlAccountReceivingDTO controlAccountReceivingDTO)
        {

                var controlAcc = _mapper.Map<ControlAccount>(controlAccountReceivingDTO);
                controlAcc.CreatedById = context.GetLoggedInUserId();

                var savedControlAccount = await _controlAccountRepo.SaveControlAccount(controlAcc);
                if (savedControlAccount == null)
                {
                    return new ApiResponse(500);
                }
                var controlAccountTransferDTOs = _mapper.Map<ControlAccountTransferDTO>(controlAcc);
                return new ApiOkResponse(controlAccountTransferDTOs);

        }

        public async Task<ApiResponse> DeleteControlAccount(long id)
        {
            var controlAccountTodelete = await _controlAccountRepo.FindControlAccountById(id);
            if (controlAccountTodelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _controlAccountRepo.DeleteControlAccount(controlAccountTodelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetControlAccountByAlias(string alias)
        {
            var controlAccount = await _controlAccountRepo.FindControlAccountByAlias(alias);
            if (controlAccount == null)
            {
                return new ApiResponse(404);
            }
            var controlAccountTransferDTOs = _mapper.Map<ControlAccountTransferDTO>(controlAccount);
            return new ApiOkResponse(controlAccountTransferDTOs);
        }
        public async Task<ApiResponse> GetControlAccountByCaption(string caption)
        {
            var controlAccount = await _controlAccountRepo.FindControlAccountByName(caption);
            if (controlAccount == null)
            {
                return new ApiResponse(404);
            }
            var controlAccountTransferDTOs = _mapper.Map<ControlAccountTransferDTO>(controlAccount);
            return new ApiOkResponse(controlAccountTransferDTOs);
        }

        public async Task<ApiResponse> GetControlAccountById(long id)
        {
            var controlAccount = await _controlAccountRepo.FindControlAccountById(id);
            if (controlAccount == null)
            {
                return new ApiResponse(404);
            }
            var controlAccountTransferDTOs = _mapper.Map<ControlAccountTransferDTO>(controlAccount);
            return new ApiOkResponse(controlAccountTransferDTOs);
        }

        public async Task<ApiResponse> GetAllControlAccounts()
        {
            var controlAccountes = await _controlAccountRepo.FindAllControlAccount();
            if (controlAccountes == null)
            {
                return new ApiResponse(404);
            }
            var controlAccountTransferDTOs = _mapper.Map<IEnumerable<ControlAccountTransferDTO>>(controlAccountes);
            return new ApiOkResponse(controlAccountTransferDTOs);
        }

        public async Task<ApiResponse> UpdateControlAccount(long id, ControlAccountReceivingDTO controlAccountReceivingDTO)
        {
            var controlAccountToUpdate = await _controlAccountRepo.FindControlAccountById(id);
            if (controlAccountToUpdate == null)
            {
                return new ApiResponse(404);
            }
            controlAccountToUpdate.Alias = controlAccountReceivingDTO.Alias;
            controlAccountToUpdate.Description = controlAccountReceivingDTO.Description;
            var updatedControlAccount = await _controlAccountRepo.UpdateControlAccount(controlAccountToUpdate);

            if (updatedControlAccount == null)
            {
                return new ApiResponse(500);
            }
            var controlAccountTransferDTOs = _mapper.Map<ControlAccountTransferDTO>(updatedControlAccount);
            return new ApiOkResponse(controlAccountTransferDTOs);
        }
    }
}