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
    public class NoteServiceImpl : INoteService
    {
        private readonly ILogger<NoteServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly INoteRepository _serviceTypeRepo;
        private readonly IMapper _mapper;

        public NoteServiceImpl(IModificationHistoryRepository historyRepo, INoteRepository NoteRepo, ILogger<NoteServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._serviceTypeRepo = NoteRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddNote(HttpContext context, NoteReceivingDTO serviceTypeReceivingDTO)
        {

            var serviceType = _mapper.Map<Note>(serviceTypeReceivingDTO);
            serviceType.CreatedById = context.GetLoggedInUserId();
            var savedserviceType = await _serviceTypeRepo.SaveNote(serviceType);
            if (savedserviceType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var serviceTypeTransferDTO = _mapper.Map<NoteTransferDTO>(serviceType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceTypeTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteNote(long id)
        {
            var serviceTypeToDelete = await _serviceTypeRepo.FindNoteById(id);
            if (serviceTypeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _serviceTypeRepo.DeleteNote(serviceTypeToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllNote()
        {
            var serviceTypes = await _serviceTypeRepo.FindAllNotes();
            if (serviceTypes == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTypeTransferDTO = _mapper.Map<IEnumerable<NoteTransferDTO>>(serviceTypes);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceTypeTransferDTO);
        }

        public async Task<ApiCommonResponse> GetNoteById(long id)
        {
            var serviceType = await _serviceTypeRepo.FindNoteById(id);
            if (serviceType == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTypeTransferDTOs = _mapper.Map<NoteTransferDTO>(serviceType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceTypeTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetNoteByName(string name)
        {
            var serviceType = await _serviceTypeRepo.FindNoteByName(name);
            if (serviceType == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTypeTransferDTOs = _mapper.Map<NoteTransferDTO>(serviceType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceTypeTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateNote(HttpContext context, long id, NoteReceivingDTO serviceTypeReceivingDTO)
        {
            var serviceTypeToUpdate = await _serviceTypeRepo.FindNoteById(id);
            if (serviceTypeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {serviceTypeToUpdate.ToString()} \n";

            serviceTypeToUpdate.Caption = serviceTypeReceivingDTO.Caption;
            serviceTypeToUpdate.Description = serviceTypeReceivingDTO.Description;
            var updatedserviceType = await _serviceTypeRepo.UpdateNote(serviceTypeToUpdate);

            summary += $"Details after change, \n {updatedserviceType.ToString()} \n";

            if (updatedserviceType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "serviceType",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedserviceType.Id
            };
            await _historyRepo.SaveHistory(history);

            var serviceTypeTransferDTOs = _mapper.Map<NoteTransferDTO>(updatedserviceType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceTypeTransferDTOs);
        }
    }
}
