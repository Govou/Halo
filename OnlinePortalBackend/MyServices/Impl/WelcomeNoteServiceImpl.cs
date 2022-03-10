using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using HalobizMigrations.Models.OnlinePortal;
using OnlinePortalBackend.Repository;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class WelcomeNoteServiceImpl : IWelcomeNoteService
    {
        private readonly IWelcomeNoteRepository _welcomeNoteRepo;
        private readonly IMapper _mapper;
        private readonly IModificationHistoryRepository _historyRepo ;
        public WelcomeNoteServiceImpl(IWelcomeNoteRepository welcomeNoteRepo, 
            IMapper mapper, 
            IModificationHistoryRepository historyRepo)
        {
            _mapper = mapper;
            _welcomeNoteRepo = welcomeNoteRepo;
            _historyRepo = historyRepo;
        }

        public async Task<ApiResponse> AddWelcomeNote(WelcomeNoteReceivingDTO welcomeNoteReceivingDTO)
        {
            var welcomeNote = _mapper.Map<WelcomeNote>(welcomeNoteReceivingDTO);
            var savedWelcomeNote = await _welcomeNoteRepo.SaveWelcomeNote(welcomeNote);
            if(savedWelcomeNote == null)
            {
                return new ApiResponse(500);
            }
            var welcomeNoteTransferDto = _mapper.Map<WelcomeNoteTransferDTO>(welcomeNote);
            return new ApiOkResponse(welcomeNoteTransferDto);
        }

        public async Task<ApiResponse> FindWelcomeNoteById(long id)
        {
            var welcomeNote = await _welcomeNoteRepo.FindWelcomeNoteById(id);
            if(welcomeNote == null)
            {
                return new ApiResponse(404);
            }
            var welcomeNoteTransferDto = _mapper.Map<WelcomeNoteTransferDTO>(welcomeNote);
            return new ApiOkResponse(welcomeNoteTransferDto);
        }        

        public async Task<ApiResponse> FindAllWelcomeNotes()
        {
            var welcomeNotes = await _welcomeNoteRepo.FindAllWelcomeNotes();
            if(welcomeNotes == null )
            {
                return new ApiResponse(404);
            }
            var welcomeNotesTransferDto = _mapper.Map<IEnumerable<WelcomeNoteTransferDTO>>(welcomeNotes);
            return new ApiOkResponse(welcomeNotesTransferDto);
        }

        public async Task<ApiResponse> UpdateWelcomeNote(HttpContext context, long welcomeNoteId, WelcomeNoteReceivingDTO welcomeNoteReceivingDTO)
        {
            var welcomeNoteToUpdate = await _welcomeNoteRepo.FindWelcomeNoteById(welcomeNoteId);
            if(welcomeNoteToUpdate == null)
            {
                return new ApiResponse(404);
            }
            var summary = $"Initial details before change, \n {welcomeNoteToUpdate.ToString()} \n" ;
            welcomeNoteToUpdate.BotInformation = welcomeNoteReceivingDTO.BotInformation;
            welcomeNoteToUpdate.WelcomeText = welcomeNoteReceivingDTO.WelcomeText;

            summary += $"Details after change, \n {welcomeNoteToUpdate} \n";

            var updatedWelcomeNote = await _welcomeNoteRepo.UpdateWelcomeNote(welcomeNoteToUpdate);

            if(updatedWelcomeNote == null)
            {
                return new ApiResponse(500);
            }      

            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "WelcomeNote",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedWelcomeNote.Id
            };

            await _historyRepo.SaveHistory(history);

            var welcomeNoteTransferDto = _mapper.Map<WelcomeNoteTransferDTO>(updatedWelcomeNote);
            return new ApiOkResponse(welcomeNoteTransferDto);
        }

        public async Task<ApiResponse> DeleteWelcomeNote(long welcomeNoteId)
        {
            var welcomeNoteToDelete = await _welcomeNoteRepo.FindWelcomeNoteById(welcomeNoteId);
            if(welcomeNoteToDelete == null)
            {
                return new ApiResponse(404);
            }

            if(!await _welcomeNoteRepo.RemoveWelcomeNote(welcomeNoteToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}