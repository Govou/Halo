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
        public async Task<ApiCommonResponse> GetNote()
        {
            return await _NoteService.GetAllNote();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _NoteService.GetNoteByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _NoteService.GetNoteById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewNote(NoteReceivingDTO NoteReceiving)
        {
            return await _NoteService.AddNote(HttpContext, NoteReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, NoteReceivingDTO NoteReceiving)
        {
            return await _NoteService.UpdateNote(HttpContext, id, NoteReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _NoteService.DeleteNote(id);
        }
    }
}
