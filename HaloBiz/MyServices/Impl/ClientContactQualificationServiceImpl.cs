using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
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
        public async  Task<ApiCommonResponse> AddClientContactQualification(HttpContext context, ClientContactQualificationReceivingDTO clientContactQualificationReceivingDTO)
        {
            var clientContactQualification = _mapper.Map<ClientContactQualification>(clientContactQualificationReceivingDTO);
            clientContactQualification.CreatedById = context.GetLoggedInUserId();
            var savedClientContactQualification = await _clientContactQualificationRepo.SaveClientContactQualification(clientContactQualification);
            if (savedClientContactQualification == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var clientContactQualificationTransferDTO = _mapper.Map<ClientContactQualificationTransferDTO>(clientContactQualification);
            return CommonResponse.Send(ResponseCodes.SUCCESS,clientContactQualificationTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteClientContactQualification(long id)
        {
            var clientContactQualificationToDelete = await _clientContactQualificationRepo.FindClientContactQualificationById(id);
            if(clientContactQualificationToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            if (!await _clientContactQualificationRepo.DeleteClientContactQualification(clientContactQualificationToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllClientContactQualification()
        {
            var clientContactQualification = await _clientContactQualificationRepo.GetClientContactQualifications();
            if (clientContactQualification == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var clientContactQualificationTransferDTO = _mapper.Map<IEnumerable<ClientContactQualificationTransferDTO>>(clientContactQualification);
            return CommonResponse.Send(ResponseCodes.SUCCESS,clientContactQualificationTransferDTO);
        }

        public  async Task<ApiCommonResponse> UpdateClientContactQualification(HttpContext context, long id, ClientContactQualificationReceivingDTO clientContactQualificationReceivingDTO)
        {
            var clientContactQualificationToUpdate = await _clientContactQualificationRepo.FindClientContactQualificationById(id);
            if (clientContactQualificationToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {clientContactQualificationToUpdate.ToString()} \n" ;

            clientContactQualificationToUpdate.Caption = clientContactQualificationReceivingDTO.Caption;
            clientContactQualificationToUpdate.Description = clientContactQualificationReceivingDTO.Description;
            var updatedClientContactQualification = await _clientContactQualificationRepo.UpdateClientContactQualification(clientContactQualificationToUpdate);

            summary += $"Details after change, \n {updatedClientContactQualification.ToString()} \n";

            if (updatedClientContactQualification == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ClientContactQualification",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedClientContactQualification.Id
            };

            await _historyRepo.SaveHistory(history);

            var clientContactQualificationTransferDTOs = _mapper.Map<ClientContactQualificationTransferDTO>(updatedClientContactQualification);
            return CommonResponse.Send(ResponseCodes.SUCCESS,clientContactQualificationTransferDTOs);
        }
    }
}