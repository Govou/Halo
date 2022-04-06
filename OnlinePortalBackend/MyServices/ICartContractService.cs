using Halobiz.Common.DTOs.ApiDTOs;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Threading.Tasks;
                                                                                                       
namespace OnlinePortalBackend.MyServices
{
    public interface ICartContractService
    {
        Task<ApiCommonResponse> CreateCartContract(HttpContext context, CartContractDTO cartContract);
        Task<ApiCommonResponse> GetCartContractServiceById(HttpContext context, long id);
       // Task<ApiCommonResponse> GetAllContractsServcieForAContract(long contractId);
        //Task<ApiCommonResponse> GetAllCartContractsServceByid(long customerDivisionId);
    }
}
