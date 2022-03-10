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
    public class UserFriendlyQuestionServiceImpl : IUserFriendlyQuestionService
    {
        private readonly IUserFriendlyQuestionRepository _userFriendlyQuestionRepo;
        private readonly IMapper _mapper;
        private readonly IModificationHistoryRepository _historyRepo ;
        public UserFriendlyQuestionServiceImpl(IUserFriendlyQuestionRepository userFriendlyQuestionRepo, 
            IMapper mapper, 
            IModificationHistoryRepository historyRepo)
        {
            _mapper = mapper;
            _userFriendlyQuestionRepo = userFriendlyQuestionRepo;
            _historyRepo = historyRepo;
        }

        public async Task<ApiResponse> AddUserFriendlyQuestion(HttpContext context, UserFriendlyQuestionReceivingDTO userFriendlyQuestionReceivingDTO)
        {
            var userFriendlyQuestion = _mapper.Map<UserFriendlyQuestion>(userFriendlyQuestionReceivingDTO);
            userFriendlyQuestion.CreatedById = context.GetLoggedInUserId();
            var savedUserFriendlyQuestion = await _userFriendlyQuestionRepo.SaveUserFriendlyQuestion(userFriendlyQuestion);
            if(savedUserFriendlyQuestion == null)
            {
                return new ApiResponse(500);
            }
            var userFriendlyQuestionTransferDto = _mapper.Map<UserFriendlyQuestionTransferDTO>(userFriendlyQuestion);
            return new ApiOkResponse(userFriendlyQuestionTransferDto);
        }

        public async Task<ApiResponse> FindUserFriendlyQuestionById(long id)
        {
            var userFriendlyQuestion = await _userFriendlyQuestionRepo.FindUserFriendlyQuestionById(id);
            if(userFriendlyQuestion == null)
            {
                return new ApiResponse(404);
            }
            var userFriendlyQuestionTransferDto = _mapper.Map<UserFriendlyQuestionTransferDTO>(userFriendlyQuestion);
            return new ApiOkResponse(userFriendlyQuestionTransferDto);
        }        

        public async Task<ApiResponse> FindAllUserFriendlyQuestions()
        {
            var userFriendlyQuestions = await _userFriendlyQuestionRepo.FindAllUserFriendlyQuestions();
            if(userFriendlyQuestions == null )
            {
                return new ApiResponse(404);
            }
            var userFriendlyQuestionsTransferDto = _mapper.Map<IEnumerable<UserFriendlyQuestionTransferDTO>>(userFriendlyQuestions);
            return new ApiOkResponse(userFriendlyQuestionsTransferDto);
        }

        public async Task<ApiResponse> UpdateUserFriendlyQuestion(HttpContext context, long userFriendlyQuestionId, UserFriendlyQuestionReceivingDTO userFriendlyQuestionReceivingDTO)
        {
            var userFriendlyQuestionToUpdate = await _userFriendlyQuestionRepo.FindUserFriendlyQuestionById(userFriendlyQuestionId);
            if(userFriendlyQuestionToUpdate == null)
            {
                return new ApiResponse(404);
            }
            var summary = $"Initial details before change, \n {userFriendlyQuestionToUpdate.ToString()} \n" ;
            userFriendlyQuestionToUpdate.ServiceGroupId = userFriendlyQuestionReceivingDTO.ServiceGroupId;
            userFriendlyQuestionToUpdate.Question = userFriendlyQuestionReceivingDTO.Question;

            summary += $"Details after change, \n {userFriendlyQuestionToUpdate} \n";

            var updatedUserFriendlyQuestion = await _userFriendlyQuestionRepo.UpdateUserFriendlyQuestion(userFriendlyQuestionToUpdate);

            if(updatedUserFriendlyQuestion == null)
            {
                return new ApiResponse(500);
            }      

            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "UserFriendlyQuestion",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedUserFriendlyQuestion.Id
            };

            await _historyRepo.SaveHistory(history);

            var userFriendlyQuestionTransferDto = _mapper.Map<UserFriendlyQuestionTransferDTO>(updatedUserFriendlyQuestion);
            return new ApiOkResponse(userFriendlyQuestionTransferDto);
        }

        public async Task<ApiResponse> DeleteUserFriendlyQuestion(long userFriendlyQuestionId)
        {
            var userFriendlyQuestionToDelete = await _userFriendlyQuestionRepo.FindUserFriendlyQuestionById(userFriendlyQuestionId);
            if(userFriendlyQuestionToDelete == null)
            {
                return new ApiResponse(404);
            }

            if(!await _userFriendlyQuestionRepo.RemoveUserFriendlyQuestion(userFriendlyQuestionToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}