using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IBankService
    {
        Task<ApiResponse> AddBank(HttpContext context, BankReceivingDTO bankReceivingDTO);
        Task<ApiResponse> GetAllBank();
        Task<ApiResponse> GetBankById(long id);
        Task<ApiResponse> GetBankByName(string name);
        Task<ApiResponse> UpdateBank(HttpContext context, long id, BankReceivingDTO bankReceivingDTO);
        Task<ApiResponse> DeleteBank(long id);
    }
}