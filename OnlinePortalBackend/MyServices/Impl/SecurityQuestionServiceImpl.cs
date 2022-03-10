using AutoMapper;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models.OnlinePortal;
using OnlinePortalBackend.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class SecurityQuestionServiceImpl : ISecurityQuestionService
    {
        private readonly ILogger<SecurityQuestionServiceImpl> _logger;
        private readonly ISecurityQuestionRepository _securityQuestionRepo;
        private readonly IMapper _mapper;

        public SecurityQuestionServiceImpl(ISecurityQuestionRepository securityQuestionRepo, ILogger<SecurityQuestionServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._securityQuestionRepo = securityQuestionRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddSecurityQuestion(SecurityQuestionReceivingDTO securityQuestionReceivingDTO)
        {
            var securityQuestion = _mapper.Map<SecurityQuestion>(securityQuestionReceivingDTO);
            var savedSecurityQuestion = await _securityQuestionRepo.SaveSecurityQuestion(securityQuestion);
            if (savedSecurityQuestion == null)
            {
                return new ApiResponse(500);
            }
            var securityQuestionTransferDTOs = _mapper.Map<SecurityQuestionTransferDTO>(securityQuestion);
            return new ApiOkResponse(securityQuestionTransferDTOs);
        }

        public async Task<ApiResponse> GetAllSecurityQuestiones()
        {
            var securityQuestiones = await _securityQuestionRepo.FindAllSecurityQuestiones();
            if (securityQuestiones == null)
            {
                return new ApiResponse(404);
            }
            var securityQuestionTransferDTOs = _mapper.Map<IEnumerable<SecurityQuestionTransferDTO>>(securityQuestiones);
            return new ApiOkResponse(securityQuestionTransferDTOs);
        }

        public async Task<ApiResponse> GetSecurityQuestionById(long id)
        {
            var securityQuestion = await _securityQuestionRepo.FindSecurityQuestionById(id);
            if (securityQuestion == null)
            {
                return new ApiResponse(404);
            }
            var securityQuestionTransferDTOs = _mapper.Map<SecurityQuestionTransferDTO>(securityQuestion);
            return new ApiOkResponse(securityQuestionTransferDTOs);
        }

        public async Task<ApiResponse> UpdateSecurityQuestion(long id, SecurityQuestionReceivingDTO securityQuestionReceivingDTO)
        {
            var securityQuestionToUpdate = await _securityQuestionRepo.FindSecurityQuestionById(id);
            if (securityQuestionToUpdate == null)
            {
                return new ApiResponse(404);
            }
            securityQuestionToUpdate.Question = securityQuestionReceivingDTO.Question;
            var updatedSecurityQuestion = await _securityQuestionRepo.UpdateSecurityQuestion(securityQuestionToUpdate);

            if (updatedSecurityQuestion == null)
            {
                return new ApiResponse(500);
            }
            var securityQuestionTransferDTOs = _mapper.Map<SecurityQuestionTransferDTO>(updatedSecurityQuestion);
            return new ApiOkResponse(securityQuestionTransferDTOs);
        }

        public async Task<ApiResponse> DeleteSecurityQuestion(long id)
        {
            var securityQuestionToDelete = await _securityQuestionRepo.FindSecurityQuestionById(id);
            if (securityQuestionToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _securityQuestionRepo.DeleteSecurityQuestion(securityQuestionToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

    }
}
