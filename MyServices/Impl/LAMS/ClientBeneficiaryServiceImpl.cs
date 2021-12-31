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

        public async Task<ApiCommonResponse> AddClientBeneficiary(HttpContext context, ClientBeneficiaryReceivingDTO clientBeneficiaryReceivingDTO)
        {
            var clientBeneficiary = _mapper.Map<ClientBeneficiary>(clientBeneficiaryReceivingDTO);
            clientBeneficiary.CreatedById = context.GetLoggedInUserId();
            var savedClientBeneficiary = await _clientBeneficiaryRepo.SaveClientBeneficiary(clientBeneficiary);
            if (savedClientBeneficiary == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var clientBeneficiaryTransferDTO = _mapper.Map<ClientBeneficiaryTransferDTO>(savedClientBeneficiary);
            return new ApiOkResponse(clientBeneficiaryTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllClientBeneficiary()
        {
            var clientBeneficiarys = await _clientBeneficiaryRepo.FindAllClientBeneficiary();
            if (clientBeneficiarys == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var clientBeneficiaryTransferDTO = _mapper.Map<IEnumerable<ClientBeneficiaryTransferDTO>>(clientBeneficiarys);
            return new ApiOkResponse(clientBeneficiaryTransferDTO);
        }

        public async Task<ApiCommonResponse> GetClientBeneficiaryById(long id)
        {
            var clientBeneficiary = await _clientBeneficiaryRepo.FindClientBeneficiaryById(id);
            if (clientBeneficiary == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var clientBeneficiaryTransferDTOs = _mapper.Map<ClientBeneficiaryTransferDTO>(clientBeneficiary);
            return new ApiOkResponse(clientBeneficiaryTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetClientBeneficiaryByCode(string code)
        {
            var clientBeneficiary = await _clientBeneficiaryRepo.FindClientBeneficiaryByCode(code);
            if (clientBeneficiary == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var clientBeneficiaryTransferDTOs = _mapper.Map<ClientBeneficiaryTransferDTO>(clientBeneficiary);
            return new ApiOkResponse(clientBeneficiaryTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateClientBeneficiary(HttpContext context, long id, ClientBeneficiaryReceivingDTO clientBeneficiaryReceivingDTO)
        {
            var clientBeneficiaryToUpdate = await _clientBeneficiaryRepo.FindClientBeneficiaryById(id);
            if (clientBeneficiaryToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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

        public async Task<ApiCommonResponse> DeleteClientBeneficiary(long id)
        {
            var clientBeneficiaryToDelete = await _clientBeneficiaryRepo.FindClientBeneficiaryById(id);
            if (clientBeneficiaryToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _clientBeneficiaryRepo.DeleteClientBeneficiary(clientBeneficiaryToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}