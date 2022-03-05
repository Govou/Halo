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
        Task<ApiCommonResponse> AddNote(HttpContext context, NoteReceivingDTO serviceTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllNote();
        Task<ApiCommonResponse> GetNoteById(long id);
        Task<ApiCommonResponse> GetNoteByName(string name);
        Task<ApiCommonResponse> UpdateNote(HttpContext context, long id, NoteReceivingDTO serviceTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteNote(long id);
    }
}
