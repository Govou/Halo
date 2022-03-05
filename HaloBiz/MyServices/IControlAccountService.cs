using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IControlAccountService
    {
        Task<ApiCommonResponse> AddControlAccount(HttpContext context, ControlAccountReceivingDTO ControlAccountReceivingDTO);
        Task<ApiCommonResponse> DeleteControlAccount(long id);
        Task<ApiCommonResponse> GetControlAccountByAlias(string alias);
        Task<ApiCommonResponse> GetControlAccountByCaption(string caption);
        Task<ApiCommonResponse> GetControlAccountById(long id);
        Task<ApiCommonResponse> GetAllControlAccounts();
        Task<ApiCommonResponse> GetAllIncomeControlAccounts();
        Task<ApiCommonResponse> UpdateControlAccount(long id, ControlAccountReceivingDTO controlAccountReceivingDTO);
    }
}