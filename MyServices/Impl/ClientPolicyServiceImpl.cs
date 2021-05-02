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
using HalobizMigrations.Models.Halobiz;

namespace HaloBiz.MyServices.Impl
{
    public class ClientPolicyServiceImpl : IClientPolicyService
    {
        private readonly ILogger<ClientPolicyServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IClientPolicyRepository _clientPolicyRepo;
        private readonly IMapper _mapper;

        public ClientPolicyServiceImpl(IModificationHistoryRepository historyRepo, IClientPolicyRepository ClientPolicyRepo, ILogger<ClientPolicyServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._clientPolicyRepo = ClientPolicyRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddClientPolicy(HttpContext context, ClientPolicyReceivingDTO clientPolicyReceivingDTO)
        {

            var clientPolicy = _mapper.Map<ClientPolicy>(clientPolicyReceivingDTO);
            clientPolicy.CreatedById = context.GetLoggedInUserId();
            var savedclientPolicy = await _clientPolicyRepo.SaveClientPolicy(clientPolicy);
            if (savedclientPolicy == null)
            {
                return new ApiResponse(500);
            }
            var clientPolicyTransferDTO = _mapper.Map<ClientPolicyTransferDTO>(clientPolicy);
            return new ApiOkResponse(clientPolicyTransferDTO);
        }

        public async Task<ApiResponse> DeleteClientPolicy(long id)
        {
            var clientPolicyToDelete = await _clientPolicyRepo.FindClientPolicyById(id);
            if (clientPolicyToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _clientPolicyRepo.DeleteClientPolicy(clientPolicyToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllClientPolicies()
        {
            var clientPolicys = await _clientPolicyRepo.FindAllClientPolicies();
            if (clientPolicys == null)
            {
                return new ApiResponse(404);
            }
            var clientPolicyTransferDTO = _mapper.Map<IEnumerable<ClientPolicyTransferDTO>>(clientPolicys);
            return new ApiOkResponse(clientPolicyTransferDTO);
        }

        public async Task<ApiResponse> GetClientPolicyById(long id)
        {
            var clientPolicy = await _clientPolicyRepo.FindClientPolicyById(id);
            if (clientPolicy == null)
            {
                return new ApiResponse(404);
            }
            var clientPolicyTransferDTOs = _mapper.Map<ClientPolicyTransferDTO>(clientPolicy);
            return new ApiOkResponse(clientPolicyTransferDTOs);
        }

        /*public async Task<ApiResponse> GetClientPolicyByName(string name)
        {
            var clientPolicy = await _clientPolicyRepo.FindClientPolicyByName(name);
            if (clientPolicy == null)
            {
                return new ApiResponse(404);
            }
            var clientPolicyTransferDTOs = _mapper.Map<ClientPolicyTransferDTO>(clientPolicy);
            return new ApiOkResponse(clientPolicyTransferDTOs);
        }*/

        public async Task<ApiResponse> UpdateClientPolicy(HttpContext context, long id, ClientPolicyReceivingDTO clientPolicyReceivingDTO)
        {
            var clientPolicyToUpdate = await _clientPolicyRepo.FindClientPolicyById(id);
            if (clientPolicyToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {clientPolicyToUpdate.ToString()} \n";

            clientPolicyToUpdate.ContractId = clientPolicyReceivingDTO.ContractId;
            clientPolicyToUpdate.ContractServiceId = clientPolicyReceivingDTO.ContractServiceId;
            clientPolicyToUpdate.CustomerDivisionId = clientPolicyReceivingDTO.CustomerDivisionId;

            var updatedclientPolicy = await _clientPolicyRepo.UpdateClientPolicy(clientPolicyToUpdate);

            summary += $"Details after change, \n {updatedclientPolicy.ToString()} \n";

            if (updatedclientPolicy == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "clientPolicy",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedclientPolicy.Id
            };
            await _historyRepo.SaveHistory(history);

            var clientPolicyTransferDTOs = _mapper.Map<ClientPolicyTransferDTO>(updatedclientPolicy);
            return new ApiOkResponse(clientPolicyTransferDTOs);
        }
    }
}
