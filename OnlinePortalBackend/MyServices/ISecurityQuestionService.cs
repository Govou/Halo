using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface ISecurityQuestionService
    {
        Task<ApiResponse> AddSecurityQuestion(SecurityQuestionReceivingDTO securityQuestionReceivingDTO);
        Task<ApiResponse> GetSecurityQuestionById(long id);
        Task<ApiResponse> GetAllSecurityQuestiones();
        Task<ApiResponse> UpdateSecurityQuestion(long id, SecurityQuestionReceivingDTO securityQuestionReceivingDTO);
        Task<ApiResponse> DeleteSecurityQuestion(long id);
    }
}
