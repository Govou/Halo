using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;

namespace OnlinePortalBackend.MyServices
{
    public interface IWelcomeNoteService
    {
        Task<ApiResponse> AddWelcomeNote(WelcomeNoteReceivingDTO welcomeNoteReceivingDTO);
        Task<ApiResponse> FindWelcomeNoteById(long id);
        Task<ApiResponse> FindAllWelcomeNotes();
        Task<ApiResponse> UpdateWelcomeNote(HttpContext context, long userId, WelcomeNoteReceivingDTO welcomeNoteReceivingDTO);
        Task<ApiResponse> DeleteWelcomeNote(long userId);
    }
}