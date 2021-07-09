using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IControlAccountService
    {
        Task<ApiResponse> AddControlAccount(HttpContext context, ControlAccountReceivingDTO ControlAccountReceivingDTO);
        Task<ApiResponse> DeleteControlAccount(long id);
        Task<ApiResponse> GetControlAccountByAlias(string alias);
        Task<ApiResponse> GetControlAccountByCaption(string caption);
        Task<ApiResponse> GetControlAccountById(long id);
        Task<ApiResponse> GetAllControlAccounts();
        Task<ApiResponse> GetAllIncomeControlAccounts();
        Task<ApiResponse> UpdateControlAccount(long id, ControlAccountReceivingDTO controlAccountReceivingDTO);
    }
}