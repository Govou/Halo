using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.MyServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserFriendlyQuestionController : ControllerBase
    {
        private readonly IUserFriendlyQuestionService _userFriendlyQuestionService;

        public UserFriendlyQuestionController(IUserFriendlyQuestionService userFriendlyQuestionService)
        {
            this._userFriendlyQuestionService = userFriendlyQuestionService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetUserFriendlyQuestions()
        {
            var response = await _userFriendlyQuestionService.FindAllUserFriendlyQuestions();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var userFriendlyQuestion = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<UserFriendlyQuestionTransferDTO>)userFriendlyQuestion);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _userFriendlyQuestionService.FindUserFriendlyQuestionById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var userFriendlyQuestion = ((ApiOkResponse)response).Result;
            return Ok((UserFriendlyQuestionTransferDTO)userFriendlyQuestion);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewUserFriendlyQuestion(UserFriendlyQuestionReceivingDTO userFriendlyQuestionReceiving)
        {
            var response = await _userFriendlyQuestionService.AddUserFriendlyQuestion(HttpContext, userFriendlyQuestionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var userFriendlyQuestion = ((ApiOkResponse)response).Result;
            return Ok((UserFriendlyQuestionTransferDTO)userFriendlyQuestion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, UserFriendlyQuestionReceivingDTO userFriendlyQuestionReceiving)
        {
            var response = await _userFriendlyQuestionService.UpdateUserFriendlyQuestion(HttpContext, id, userFriendlyQuestionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var userFriendlyQuestion = ((ApiOkResponse)response).Result;
            return Ok((UserFriendlyQuestionTransferDTO)userFriendlyQuestion);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _userFriendlyQuestionService.DeleteUserFriendlyQuestion(id);
            return StatusCode(response.StatusCode);
        }
    }
}