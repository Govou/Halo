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
    public class ClientBeneficiaryServiceImpl : IClientBeneficiaryService
    {
        private readonly ILogger<ClientBeneficiaryServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IClientBeneficiaryRepository _clientBeneficiaryRepo;
        private readonly IMapper _mapper;

        public ClientBeneficiaryServiceImpl(IModificationHistoryRepository historyRepo, IClientBeneficiaryRepository clientBeneficiaryRepo, ILogger<ClientBeneficiaryServiceImpl> logger, IMapper mapper)
        {
            _mapper = mapper;
            _historyRepo = historyRepo;
            _clientBeneficiaryRepo = clientBeneficiaryRepo;
            _logger = logger;
        }

        public async Task<ApiResponse> AddClientBeneficiary(HttpContext context, ClientBeneficiaryReceivingDTO clientBeneficiaryReceivingDTO)
        {
            var clientBeneficiary = _mapper.Map<ClientBeneficiary>(clientBeneficiaryReceivingDTO);
            clientBeneficiary.CreatedById = context.GetLoggedInUserId();
            var savedClientBeneficiary = await _clientBeneficiaryRepo.SaveClientBeneficiary(clientBeneficiary);
            if (savedClientBeneficiary == null)
            {
                return new ApiResponse(500);
            }
            var clientBeneficiaryTransferDTO = _mapper.Map<ClientBeneficiaryTransferDTO>(savedClientBeneficiary);
            return new ApiOkResponse(clientBeneficiaryTransferDTO);
        }

        public async Task<ApiResponse> GetAllClientBeneficiary()
        {
            var clientBeneficiarys = await _clientBeneficiaryRepo.FindAllClientBeneficiary();
            if (clientBeneficiarys == null)
            {
                return new ApiResponse(404);
            }
            var clientBeneficiaryTransferDTO = _mapper.Map<IEnumerable<ClientBeneficiaryTransferDTO>>(clientBeneficiarys);
            return new ApiOkResponse(clientBeneficiaryTransferDTO);
        }

        public async Task<ApiResponse> GetClientBeneficiaryById(long id)
        {
            var clientBeneficiary = await _clientBeneficiaryRepo.FindClientBeneficiaryById(id);
            if (clientBeneficiary == null)
            {
                return new ApiResponse(404);
            }
            var clientBeneficiaryTransferDTOs = _mapper.Map<ClientBeneficiaryTransferDTO>(clientBeneficiary);
            return new ApiOkResponse(clientBeneficiaryTransferDTOs);
        }

        public async Task<ApiResponse> GetClientBeneficiaryByCode(string code)
        {
            var clientBeneficiary = await _clientBeneficiaryRepo.FindClientBeneficiaryByCode(code);
            if (clientBeneficiary == null)
            {
                return new ApiResponse(404);
            }
            var clientBeneficiaryTransferDTOs = _mapper.Map<ClientBeneficiaryTransferDTO>(clientBeneficiary);
            return new ApiOkResponse(clientBeneficiaryTransferDTOs);
        }

        public async Task<ApiResponse> UpdateClientBeneficiary(HttpContext context, long id, ClientBeneficiaryReceivingDTO clientBeneficiaryReceivingDTO)
        {
            var clientBeneficiaryToUpdate = await _clientBeneficiaryRepo.FindClientBeneficiaryById(id);
            if (clientBeneficiaryToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {clientBeneficiaryToUpdate.ToString()} \n" ;

            clientBeneficiaryToUpdate.BeneficiaryFamilyCode = clientBeneficiaryReceivingDTO.BeneficiaryFamilyCode;
            clientBeneficiaryToUpdate.BeneficiaryCode = clientBeneficiaryReceivingDTO.BeneficiaryCode;
            clientBeneficiaryToUpdate.IsPrincipal = clientBeneficiaryReceivingDTO.IsPrincipal;
            clientBeneficiaryToUpdate.Title = clientBeneficiaryReceivingDTO.Title;
            clientBeneficiaryToUpdate.FirstName = clientBeneficiaryReceivingDTO.FirstName;
            clientBeneficiaryToUpdate.LastName = clientBeneficiaryReceivingDTO.LastName;
            clientBeneficiaryToUpdate.MiddleName = clientBeneficiaryReceivingDTO.MiddleName;
            clientBeneficiaryToUpdate.Mobile = clientBeneficiaryReceivingDTO.Mobile;
            clientBeneficiaryToUpdate.Email = clientBeneficiaryReceivingDTO.Email;
            clientBeneficiaryToUpdate.Address = clientBeneficiaryReceivingDTO.Address;
            clientBeneficiaryToUpdate.MeansOfIdentifcationId = clientBeneficiaryReceivingDTO.MeansOfIdentifcationId;
            clientBeneficiaryToUpdate.IdentificationNumber = clientBeneficiaryReceivingDTO.IdentificationNumber;
            clientBeneficiaryToUpdate.RelationshipId = clientBeneficiaryReceivingDTO.RelationshipId;
            clientBeneficiaryToUpdate.ClientId = clientBeneficiaryReceivingDTO.ClientId;
            clientBeneficiaryToUpdate.ImageUrl = clientBeneficiaryReceivingDTO.ImageUrl;

            var updatedClientBeneficiary = await _clientBeneficiaryRepo.UpdateClientBeneficiary(clientBeneficiaryToUpdate);

            summary += $"Details after change, \n {updatedClientBeneficiary.ToString()} \n";

            if (updatedClientBeneficiary == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ClientBeneficiary",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedClientBeneficiary.Id
            };

            await _historyRepo.SaveHistory(history);

            var clientBeneficiaryTransferDTOs = _mapper.Map<ClientBeneficiaryTransferDTO>(updatedClientBeneficiary);
            return new ApiOkResponse(clientBeneficiaryTransferDTOs);

        }

        public async Task<ApiResponse> DeleteClientBeneficiary(long id)
        {
            var clientBeneficiaryToDelete = await _clientBeneficiaryRepo.FindClientBeneficiaryById(id);
            if (clientBeneficiaryToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _clientBeneficiaryRepo.DeleteClientBeneficiary(clientBeneficiaryToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}