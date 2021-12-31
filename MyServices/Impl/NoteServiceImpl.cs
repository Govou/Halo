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

        public async Task<ApiResponse> AddNote(HttpContext context, NoteReceivingDTO serviceTypeReceivingDTO)
        {

            var serviceType = _mapper.Map<Note>(serviceTypeReceivingDTO);
            serviceType.CreatedById = context.GetLoggedInUserId();
            var savedserviceType = await _serviceTypeRepo.SaveNote(serviceType);
            if (savedserviceType == null)
            {
                return new ApiResponse(500);
            }
            var serviceTypeTransferDTO = _mapper.Map<NoteTransferDTO>(serviceType);
            return new ApiOkResponse(serviceTypeTransferDTO);
        }

        public async Task<ApiResponse> DeleteNote(long id)
        {
            var serviceTypeToDelete = await _serviceTypeRepo.FindNoteById(id);
            if (serviceTypeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceTypeRepo.DeleteNote(serviceTypeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllNote()
        {
            var serviceTypes = await _serviceTypeRepo.FindAllNotes();
            if (serviceTypes == null)
            {
                return new ApiResponse(404);
            }
            var serviceTypeTransferDTO = _mapper.Map<IEnumerable<NoteTransferDTO>>(serviceTypes);
            return new ApiOkResponse(serviceTypeTransferDTO);
        }

        public async Task<ApiResponse> GetNoteById(long id)
        {
            var serviceType = await _serviceTypeRepo.FindNoteById(id);
            if (serviceType == null)
            {
                return new ApiResponse(404);
            }
            var serviceTypeTransferDTOs = _mapper.Map<NoteTransferDTO>(serviceType);
            return new ApiOkResponse(serviceTypeTransferDTOs);
        }

        public async Task<ApiResponse> GetNoteByName(string name)
        {
            var serviceType = await _serviceTypeRepo.FindNoteByName(name);
            if (serviceType == null)
            {
                return new ApiResponse(404);
            }
            var serviceTypeTransferDTOs = _mapper.Map<NoteTransferDTO>(serviceType);
            return new ApiOkResponse(serviceTypeTransferDTOs);
        }

        public async Task<ApiResponse> UpdateNote(HttpContext context, long id, NoteReceivingDTO serviceTypeReceivingDTO)
        {
            var serviceTypeToUpdate = await _serviceTypeRepo.FindNoteById(id);
            if (serviceTypeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {serviceTypeToUpdate.ToString()} \n";

            serviceTypeToUpdate.Caption = serviceTypeReceivingDTO.Caption;
            serviceTypeToUpdate.Description = serviceTypeReceivingDTO.Description;
            var updatedserviceType = await _serviceTypeRepo.UpdateNote(serviceTypeToUpdate);

            summary += $"Details after change, \n {updatedserviceType.ToString()} \n";

            if (updatedserviceType == null)
            {
                return new ApiResponse(500);
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
            return new ApiOkResponse(serviceTypeTransferDTOs);
        }
    }
}
