using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HalobizMigrations.Models;

using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class ClientEngagementServiceImpl : IClientEngagementService
    {
        private readonly ILogger<ClientEngagementServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IClientEngagementRepository _clientEngagementRepo;
        private readonly IMapper _mapper;

        public ClientEngagementServiceImpl(IModificationHistoryRepository historyRepo, IClientEngagementRepository clientEngagementRepo, ILogger<ClientEngagementServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._clientEngagementRepo = clientEngagementRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddClientEngagement(HttpContext context, ClientEngagementReceivingDTO clientEngagementReceivingDTO)
        {
            var clientEngagement = _mapper.Map<ClientEngagement>(clientEngagementReceivingDTO);
            clientEngagement.CreatedById = context.GetLoggedInUserId();
            var savedClientEngagement = await _clientEngagementRepo.SaveClientEngagement(clientEngagement);
            if (savedClientEngagement == null)
            {
                return new ApiResponse(500);
            }
            var clientEngagementTransferDTO = _mapper.Map<ClientEngagementTransferDTO>(savedClientEngagement);
            return new ApiOkResponse(clientEngagementTransferDTO);
        }

        public async Task<ApiResponse> GetAllClientEngagement()
        {
            var clientEngagements = await _clientEngagementRepo.FindAllClientEngagement();
            if (clientEngagements == null)
            {
                return new ApiResponse(404);
            }
            var clientEngagementTransferDTO = _mapper.Map<IEnumerable<ClientEngagementTransferDTO>>(clientEngagements);
            return new ApiOkResponse(clientEngagementTransferDTO);
        }

        public async Task<ApiResponse> GetClientEngagementById(long id)
        {
            var clientEngagement = await _clientEngagementRepo.FindClientEngagementById(id);
            if (clientEngagement == null)
            {
                return new ApiResponse(404);
            }
            var clientEngagementTransferDTOs = _mapper.Map<ClientEngagementTransferDTO>(clientEngagement);
            return new ApiOkResponse(clientEngagementTransferDTOs);
        }

        public async Task<ApiResponse> GetClientEngagementByName(string name)
        {
            var clientEngagement = await _clientEngagementRepo.FindClientEngagementByName(name);
            if (clientEngagement == null)
            {
                return new ApiResponse(404);
            }
            var clientEngagementTransferDTOs = _mapper.Map<ClientEngagementTransferDTO>(clientEngagement);
            return new ApiOkResponse(clientEngagementTransferDTOs);
        }

        public async Task<ApiResponse> UpdateClientEngagement(HttpContext context, long id, ClientEngagementReceivingDTO clientEngagementReceivingDTO)
        {
            var clientEngagementToUpdate = await _clientEngagementRepo.FindClientEngagementById(id);
            if (clientEngagementToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {clientEngagementToUpdate.ToString()} \n" ;

            clientEngagementToUpdate.Caption = clientEngagementReceivingDTO.Caption;
            clientEngagementToUpdate.Date = clientEngagementReceivingDTO.Date;
            clientEngagementToUpdate.CustomerDivisionId = clientEngagementReceivingDTO.CustomerDivisionId;
            clientEngagementToUpdate.EngagementDiscussion = clientEngagementReceivingDTO.EngagementDiscussion;
            clientEngagementToUpdate.EngagementOutcome = clientEngagementReceivingDTO.EngagementOutcome;
            clientEngagementToUpdate.EngagementTypeId = clientEngagementReceivingDTO.EngagementTypeId;
            clientEngagementToUpdate.ContractServicesDiscussed = clientEngagementReceivingDTO.ContractServiceDiscussed;
            clientEngagementToUpdate.LeadKeyContactId = clientEngagementReceivingDTO.LeadKeyContactId;
            clientEngagementToUpdate.LeadKeyPersonId = clientEngagementReceivingDTO.LeadKeyPersonId;
            var updatedClientEngagement = await _clientEngagementRepo.UpdateClientEngagement(clientEngagementToUpdate);

            summary += $"Details after change, \n {updatedClientEngagement.ToString()} \n";

            if (updatedClientEngagement == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ClientEngagement",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedClientEngagement.Id
            };

            await _historyRepo.SaveHistory(history);

            var clientEngagementTransferDTOs = _mapper.Map<ClientEngagementTransferDTO>(updatedClientEngagement);
            return new ApiOkResponse(clientEngagementTransferDTOs);

        }

        public async Task<ApiResponse> DeleteClientEngagement(long id)
        {
            var clientEngagementToDelete = await _clientEngagementRepo.FindClientEngagementById(id);
            if (clientEngagementToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _clientEngagementRepo.DeleteClientEngagement(clientEngagementToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}