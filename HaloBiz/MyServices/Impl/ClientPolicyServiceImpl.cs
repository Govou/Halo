using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
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

        public async Task<ApiCommonResponse> AddClientPolicy(HttpContext context, List<ClientPolicyReceivingDTO> clientPolicyReceivingDTOs)
        {
            var CreatedById = context.GetLoggedInUserId();

            foreach (var clientPolicyReceivingDTO in clientPolicyReceivingDTOs)
            {
                //check that this does not exist
                var thePolicy = await _clientPolicyRepo.FindClientPolicyByContractServiceId(clientPolicyReceivingDTO.ContractServiceId);
                if(thePolicy == null)
                {
                    var clientPolicy = _mapper.Map<ClientPolicy>(clientPolicyReceivingDTO);
                    clientPolicy.CreatedById = CreatedById;
                    var savedclientPolicy = await _clientPolicyRepo.SaveClientPolicy(clientPolicy);
                    if (savedclientPolicy == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                }               
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteClientPolicy(long id)
        {
            var clientPolicyToDelete = await _clientPolicyRepo.FindClientPolicyById(id);
            if (clientPolicyToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _clientPolicyRepo.DeleteClientPolicy(clientPolicyToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllClientPolicies()
        {
            var clientPolicys = await _clientPolicyRepo.FindAllClientPolicies();
            if (clientPolicys == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var clientPolicyTransferDTO = _mapper.Map<IEnumerable<ClientPolicyTransferDTO>>(clientPolicys);
            return CommonResponse.Send(ResponseCodes.SUCCESS,clientPolicyTransferDTO);
        }

        public async Task<ApiCommonResponse> GetClientPolicyById(long id)
        {
            var clientPolicy = await _clientPolicyRepo.FindClientPolicyById(id);
            if (clientPolicy == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var clientPolicyTransferDTOs = _mapper.Map<ClientPolicyTransferDTO>(clientPolicy);
            return CommonResponse.Send(ResponseCodes.SUCCESS,clientPolicyTransferDTOs);
        }
     

        public async Task<ApiCommonResponse> FindClientPolicyByContractServiceId(long id)
        {
            var clientPolicy = await _clientPolicyRepo.FindClientPolicyByContractServiceId(id);
            if (clientPolicy == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var clientPolicyTransferDTOs = _mapper.Map<ClientPolicyTransferDTO>(clientPolicy);
            return CommonResponse.Send(ResponseCodes.SUCCESS,clientPolicyTransferDTOs);
        }

        /*public async Task<ApiCommonResponse> GetClientPolicyByName(string name)
        {
            var clientPolicy = await _clientPolicyRepo.FindClientPolicyByName(name);
            if (clientPolicy == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var clientPolicyTransferDTOs = _mapper.Map<ClientPolicyTransferDTO>(clientPolicy);
            return CommonResponse.Send(ResponseCodes.SUCCESS,clientPolicyTransferDTOs);
        }*/

        public async Task<ApiCommonResponse> UpdateClientPolicy(HttpContext context, List< ClientPolicyReceivingDTO> clientPolicyReceivingDTOs)
        {
            foreach (var clientPolicyReceivingDTO in clientPolicyReceivingDTOs)
            {
                var clientPolicyToUpdate = await _clientPolicyRepo.FindClientPolicyByContractServiceId(clientPolicyReceivingDTO.ContractServiceId);
                if (clientPolicyToUpdate == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
                }

                var summary = $"Initial details before change, \n {clientPolicyToUpdate.ToString()} \n";

                clientPolicyToUpdate.ContractServiceId = clientPolicyReceivingDTO.ContractServiceId;
                clientPolicyToUpdate.AutoRenew = clientPolicyReceivingDTO.AutoRenew;
                clientPolicyToUpdate.RateReviewInterval = clientPolicyReceivingDTO.RateReviewInterval;
                clientPolicyToUpdate.RenewalTenor = clientPolicyReceivingDTO.RenewalTenor;
                clientPolicyToUpdate.NextRateReviewDate = clientPolicyReceivingDTO.NextRateReviewDate;

                var updatedclientPolicy = await _clientPolicyRepo.UpdateClientPolicy(clientPolicyToUpdate);

                summary += $"Details after change, \n {updatedclientPolicy} \n";

                if (updatedclientPolicy == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }

                ModificationHistory history = new ModificationHistory()
                {
                    ModelChanged = "clientPolicy",
                    ChangeSummary = summary,
                    ChangedById = context.GetLoggedInUserId(),
                    ModifiedModelId = updatedclientPolicy.Id
                };

                await _historyRepo.SaveHistory(history);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}
