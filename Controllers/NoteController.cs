using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _NoteService;

        public NoteController(INoteService noteService)
        {
            this._NoteService = noteService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetNote()
        {
            var response = await _NoteService.GetAllNote();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Note = ((ApiOkResponse)response).Result;
            return Ok(Note);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _NoteService.GetNoteByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Note = ((ApiOkResponse)response).Result;
            return Ok(Note);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _NoteService.GetNoteById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Note = ((ApiOkResponse)response).Result;
            return Ok(Note);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewNote(NoteReceivingDTO NoteReceiving)
        {
            var response = await _NoteService.AddNote(HttpContext, NoteReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Note = ((ApiOkResponse)response).Result;
            return Ok(Note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, NoteReceivingDTO NoteReceiving)
        {
            var response = await _NoteService.UpdateNote(HttpContext, id, NoteReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Note = ((ApiOkResponse)response).Result;
            return Ok(Note);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _NoteService.DeleteNote(id);
            return StatusCode(response.StatusCode);
        }
    }
}
