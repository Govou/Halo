using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface INoteService
    {
        Task<ApiResponse> AddNote(HttpContext context, NoteReceivingDTO serviceTypeReceivingDTO);
        Task<ApiResponse> GetAllNote();
        Task<ApiResponse> GetNoteById(long id);
        Task<ApiResponse> GetNoteByName(string name);
        Task<ApiResponse> UpdateNote(HttpContext context, long id, NoteReceivingDTO serviceTypeReceivingDTO);
        Task<ApiResponse> DeleteNote(long id);
    }
}
