using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.MyServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndorsementController : ControllerBase
    {
        private readonly IEndorsementService _endorsementService;
        //  private readonly ICartContractDetailService _cartContractDetailService;

        public EndorsementController(IEndorsementService endorsementService)
        {
            _endorsementService = endorsementService;
        }

        [HttpGet("FetchEndorsements")]
        public async Task<ApiCommonResponse> FetchEndorsements(int limit = 10)
        {
            return await _endorsementService.FetchEndorsements(HttpContext, limit);
        }


        [HttpGet("TrackEndorsement/{id}")]
        public async Task<ApiCommonResponse> TrackEndorsement(long id)
        {
            return await _endorsementService.TrackEndorsement(HttpContext, id);
        }

        [HttpPost("TopUp")]
        public async Task<ApiCommonResponse> EndorsementTopUp(List<EndorsementDTO> endorsement)
        {
            return await _endorsementService.EndorsementTopUp(HttpContext, endorsement);
        }

        [HttpPost("Reduction")]
        public async Task<ApiCommonResponse> EndorsementReduction(List<EndorsementDTO> endorsement)
        {
            return await _endorsementService.EndorsementReduction(HttpContext, endorsement);
        }

      

        [HttpGet("GetContractServiceById")]
        public async Task<ApiCommonResponse> GetContractService(int id)
        {
            return await _endorsementService.GetContractService(id);
        }

        [HttpGet("GetContractServices")]
        public async Task<ApiCommonResponse> GetContractServices(int userId)
        {
            return await _endorsementService.GetContractServices(userId);
        }

        [HttpGet("GetPossibleDatesOfEffectForEndorsement")]
        public async Task<ApiCommonResponse> GetPossibleDatesOfEffectForEndorsement(int contractServiceId)
        {
            return await _endorsementService.GetPossibleDatesOfEffectForEndorsement(contractServiceId);
        }
    }
}
