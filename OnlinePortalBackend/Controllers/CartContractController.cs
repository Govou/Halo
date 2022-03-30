using Halobiz.Common.DTOs.ApiDTOs;
//using HalobizMigrations.Models.OnlinePortal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.MyServices;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartContractController : ControllerBase
    {
        private readonly ICartContractService _cartContractService;

        public CartContractController(ICartContractService cartContractService)
        {
            _cartContractService = cartContractService;
        }

        //[HttpPost]
        //public async Task<ApiCommonResponse> CreateCartContract(CartContractDTO request)
        //{
        //    return await _cartContractService.CreateCartContract(HttpContext, request);
        //}


        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            
            return await _cartContractService.GetCartContractServiceById(HttpContext, id);
        }
    }
}
