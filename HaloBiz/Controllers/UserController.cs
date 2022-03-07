using System.Collections.Generic;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs.RoleManagement;
using HaloBiz.DTOs.ReceivingDTO;
using HaloBiz.DTOs.ReceivingDTOs.RoleManagement;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserController(IUserProfileService userProfileService)
        {
            this._userProfileService = userProfileService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetUsers()
        {
            return await _userProfileService.FindAllUsers(); 
        }

        [HttpGet("NotInSbu/{sbuId}")]
        public async Task<ApiCommonResponse> GetUsersNotInSbu(long sbuId)
        {
            return await _userProfileService.FindAllUsersNotInAnSBU(sbuId); 
        }

        [HttpGet("email/{email}")]
        public async Task<ApiCommonResponse> GetUserByEmail(string email)
        {
            return await _userProfileService.FindUserByEmail(email); 
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetUserById(long id)
        {
            return await _userProfileService.FindUserById(id); 
        }

        

        [HttpPost("")]
        public async Task<ApiCommonResponse> PostUserProfile(UserProfileReceivingDTO userProfileReceiving)
        {
           return await _userProfileService.AddUserProfile(userProfileReceiving); 
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> PutTModel(long id, UserProfileReceivingDTO userProfileReceiving)
        {
            return await _userProfileService.UpdateUserProfile(id, userProfileReceiving); 
        }

        [HttpPut("UpdateUserRole/{id}")]
        public async Task<ApiCommonResponse> UpdateUserRole(long id, List<RoleReceivingDTO> roles)
        {
            return await _userProfileService.UpdateUserRole(HttpContext, id, roles); 
        }
        
        [HttpPut("{id}/StrategicBusinessUnit/{SBUId}")]
        public async Task<ApiCommonResponse> AssignUserToSBU(long id, long SBUId)
        {
            return await _userProfileService.AssignUserToSBU(id, SBUId); 
        }

        [HttpPut("{id}/DetachStrategicBusinessUnit")]
        public async Task<ApiCommonResponse> DetachUserFromSBU(long id)
        {
            return await _userProfileService.DetachUserFromSBU(id); 
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteUserProfileById(int id)
        {
            return await _userProfileService.DeleteUserProfile(id);
        }
    }
}