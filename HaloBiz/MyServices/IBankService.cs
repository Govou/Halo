using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IBankService
    {
        Task<ApiCommonResponse> AddBank(HttpContext context, BankReceivingDTO bankReceivingDTO);
        Task<ApiCommonResponse> GetAllBank();
        Task<ApiCommonResponse> GetBankById(long id);
        Task<ApiCommonResponse> GetBankByName(string name);
        Task<ApiCommonResponse> UpdateBank(HttpContext context, long id, BankReceivingDTO bankReceivingDTO);
        Task<ApiCommonResponse> DeleteBank(long id);
    }
}