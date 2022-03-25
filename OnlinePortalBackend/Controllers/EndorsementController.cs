using Microsoft.AspNetCore.Mvc;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndorsementController : ControllerBase
    {
        private readonly IEndorsementService _cartContractService;
        //  private readonly ICartContractDetailService _cartContractDetailService;

        public EndorsementController(IEndorsementService cartContractService)
        {
            _cartContractService = cartContractService;
        }

        [HttpPost]
        public async Task<ApiCommonResponse> CreateCartContract(CartContractDTO request)
        {
            return await _cartContractService.CreateCartContract(HttpContext, request);
        }


        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {

            return await _cartContractService.GetCartContractServiceById(HttpContext, id);
        }
    }
}
