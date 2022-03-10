using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.MyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SecurityQuestionController : ControllerBase
    {
        private readonly ISecurityQuestionService _seurityQuestionService;

        public SecurityQuestionController(ISecurityQuestionService seurityQuestionService)
        {
            this._seurityQuestionService = seurityQuestionService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetSecurityQuestiones()
        {
            var response = await _seurityQuestionService.GetAllSecurityQuestiones();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var seurityQuestion = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<SecurityQuestionTransferDTO>)seurityQuestion);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _seurityQuestionService.GetSecurityQuestionById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var seurityQuestion = ((ApiOkResponse)response).Result;
            return Ok((SecurityQuestionTransferDTO)seurityQuestion);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewSecurityQuestion(SecurityQuestionReceivingDTO seurityQuestionReceiving)
        {
            var response = await _seurityQuestionService.AddSecurityQuestion(seurityQuestionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var seurityQuestion = ((ApiOkResponse)response).Result;
            return Ok((SecurityQuestionTransferDTO)seurityQuestion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, SecurityQuestionReceivingDTO seurityQuestionReceiving)
        {
            var response = await _seurityQuestionService.UpdateSecurityQuestion(id, seurityQuestionReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var seurityQuestion = ((ApiOkResponse)response).Result;
            return Ok((SecurityQuestionTransferDTO)seurityQuestion);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _seurityQuestionService.DeleteSecurityQuestion(id);
            return StatusCode(response.StatusCode);
        }
    }
}