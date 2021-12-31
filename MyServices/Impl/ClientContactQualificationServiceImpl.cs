using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class ClientContactQualificationServiceImpl : IClientContactQualificationService
    {
        private readonly ILogger<ClientContactQualificationServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IClientContactQualificationRepository _clientContactQualificationRepo;
        private readonly IMapper _mapper;

        public ClientContactQualificationServiceImpl(IModificationHistoryRepository historyRepo, IClientContactQualificationRepository clientContactQualificationRepo, ILogger<ClientContactQualificationServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._clientContactQualificationRepo = clientContactQualificationRepo;
            this._logger = logger;
        }
        public async  Task<ApiResponse> AddClientContactQualification(HttpContext context, ClientContactQualificationReceivingDTO clientContactQualificationReceivingDTO)
        {
            var clientContactQualification = _mapper.Map<ClientContactQualification>(clientContactQualificationReceivingDTO);
            clientContactQualification.CreatedById = context.GetLoggedInUserId();
            var savedClientContactQualification = await _clientContactQualificationRepo.SaveClientContactQualification(clientContactQualification);
            if (savedClientContactQualification == null)
            {
                return new ApiResponse(500);
            }
            var clientContactQualificationTransferDTO = _mapper.Map<ClientContactQualificationTransferDTO>(clientContactQualification);
            return new ApiOkResponse(clientContactQualificationTransferDTO);
        }

        public async Task<ApiResponse> DeleteClientContactQualification(long id)
        {
            var clientContactQualificationToDelete = await _clientContactQualificationRepo.FindClientContactQualificationById(id);
            if(clientContactQualificationToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _clientContactQualificationRepo.DeleteClientContactQualification(clientContactQualificationToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllClientContactQualification()
        {
            var clientContactQualification = await _clientContactQualificationRepo.GetClientContactQualifications();
            if (clientContactQualification == null)
            {
                return new ApiResponse(404);
            }
            var clientContactQualificationTransferDTO = _mapper.Map<IEnumerable<ClientContactQualificationTransferDTO>>(clientContactQualification);
            return new ApiOkResponse(clientContactQualificationTransferDTO);
        }

        public  async Task<ApiResponse> UpdateClientContactQualification(HttpContext context, long id, ClientContactQualificationReceivingDTO clientContactQualificationReceivingDTO)
        {
            var clientContactQualificationToUpdate = await _clientContactQualificationRepo.FindClientContactQualificationById(id);
            if (clientContactQualificationToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {clientContactQualificationToUpdate.ToString()} \n" ;

            clientContactQualificationToUpdate.Caption = clientContactQualificationReceivingDTO.Caption;
            clientContactQualificationToUpdate.Description = clientContactQualificationReceivingDTO.Description;
            var updatedClientContactQualification = await _clientContactQualificationRepo.UpdateClientContactQualification(clientContactQualificationToUpdate);

            summary += $"Details after change, \n {updatedClientContactQualification.ToString()} \n";

            if (updatedClientContactQualification == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ClientContactQualification",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedClientContactQualification.Id
            };

            await _historyRepo.SaveHistory(history);

            var clientContactQualificationTransferDTOs = _mapper.Map<ClientContactQualificationTransferDTO>(updatedClientContactQualification);
            return new ApiOkResponse(clientContactQualificationTransferDTOs);
        }
    }
}