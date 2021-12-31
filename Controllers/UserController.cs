using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
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
        public async Task<ActionResult> GetUsers()
        {
            var response = await _userProfileService.FindAllUsers();
            if(response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var user = ((ApiOkResponse) response).Result;
            return Ok((IEnumerable<UserProfileTransferDTO>) user);
        }

        [HttpGet("NotInSbu/{sbuId}")]
        public async Task<ActionResult> GetUsersNotInSbu(long sbuId)
        {
            var response = await _userProfileService.FindAllUsersNotInAnSBU(sbuId);
            if(response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var user = ((ApiOkResponse) response).Result;
            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult> GetUserByEmail(string email)
        {
            var response = await _userProfileService.FindUserByEmail(email);
            if(response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var user = ((ApiOkResponse) response).Result;
            return Ok((UserProfileTransferDTO) user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(long id)
        {
            var response = await _userProfileService.FindUserById(id);
            if(response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var user = ((ApiOkResponse) response).Result;
            return Ok((UserProfileTransferDTO) user);
        }

        

        [HttpPost("")]
        public async Task<ActionResult> PostUserProfile(UserProfileReceivingDTO userProfileReceiving)
        {
           var response = await _userProfileService.AddUserProfile(userProfileReceiving);
            if(response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var user = ((ApiOkResponse) response).Result;
            return Ok((UserProfileTransferDTO) user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTModel(long id, UserProfileReceivingDTO userProfileReceiving)
        {
            var response = await _userProfileService.UpdateUserProfile(id, userProfileReceiving);
            if(response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var user = ((ApiOkResponse) response).Result;
            return Ok((UserProfileTransferDTO) user);
        }

        [HttpPut("UpdateUserRole/{id}")]
        public async Task<IActionResult> UpdateUserRole(long id, List<RoleReceivingDTO> roles)
        {
            var response = await _userProfileService.UpdateUserRole(HttpContext, id, roles);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var user = ((ApiOkResponse)response).Result;
            return Ok(user);
        
        }
        
        [HttpPut("{id}/StrategicBusinessUnit/{SBUId}")]
        public async Task<IActionResult> AssignUserToSBU(long id, long SBUId)
        {
            var response = await _userProfileService.AssignUserToSBU(id, SBUId);
            if(response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var user = ((ApiOkResponse) response).Result;
            return Ok((UserProfileTransferDTO) user);
        }

        [HttpPut("{id}/DetachStrategicBusinessUnit")]
        public async Task<IActionResult> DetachUserFromSBU(long id)
        {
            var response = await _userProfileService.DetachUserFromSBU(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var user = ((ApiOkResponse)response).Result;
            return Ok((UserProfileTransferDTO)user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserProfileById(int id)
        {
            var response = await _userProfileService.DeleteUserProfile(id);
            return StatusCode(response.StatusCode);
        }
    }
}