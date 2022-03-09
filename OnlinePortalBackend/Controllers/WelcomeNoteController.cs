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
    public class WelcomeNoteController : ControllerBase
    {
        private readonly IWelcomeNoteService _welcomeNoteService;

        public WelcomeNoteController(IWelcomeNoteService welcomeNoteService)
        {
            this._welcomeNoteService = welcomeNoteService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetWelcomeNotes()
        {
            var response = await _welcomeNoteService.FindAllWelcomeNotes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var welcomeNote = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<WelcomeNoteTransferDTO>)welcomeNote);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _welcomeNoteService.FindWelcomeNoteById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var welcomeNote = ((ApiOkResponse)response).Result;
            return Ok((WelcomeNoteTransferDTO)welcomeNote);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewWelcomeNote(WelcomeNoteReceivingDTO welcomeNoteReceiving)
        {
            var response = await _welcomeNoteService.AddWelcomeNote(welcomeNoteReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var welcomeNote = ((ApiOkResponse)response).Result;
            return Ok((WelcomeNoteTransferDTO)welcomeNote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, WelcomeNoteReceivingDTO welcomeNoteReceiving)
        {
            var response = await _welcomeNoteService.UpdateWelcomeNote(HttpContext, id, welcomeNoteReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var welcomeNote = ((ApiOkResponse)response).Result;
            return Ok((WelcomeNoteTransferDTO)welcomeNote);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _welcomeNoteService.DeleteWelcomeNote(id);
            return StatusCode(response.StatusCode);
        }
    }
}