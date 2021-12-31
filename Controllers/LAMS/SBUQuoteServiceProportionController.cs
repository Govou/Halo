using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SBUQuoteServiceProportionController : ControllerBase
    {
        private readonly ISbutoQuoteServiceProportionsService _sQSSP;

        public SBUQuoteServiceProportionController(ISbutoQuoteServiceProportionsService sQSSP)
        {
            _sQSSP = sQSSP;
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewQuoteServiceDocument( IEnumerable<SbutoQuoteServiceProportionReceivingDTO> listOfsQSSP)
        {
            var response = await _sQSSP.SaveSBUToQuoteProp(HttpContext, listOfsQSSP);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var sbuToQuoteServiceProp = ((ApiOkResponse)response).Result;
            return Ok(sbuToQuoteServiceProp);
        }

        [HttpGet("{serviceQuoteId}")]
        public async Task<ApiCommonResponse> GetById(long serviceQuoteId)
        {
            var response = await _sQSSP.GetAllSBUQuoteProportionForQuoteService(serviceQuoteId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var sbuToQuoteServiceProp = ((ApiOkResponse)response).Result;
            return Ok(sbuToQuoteServiceProp);
        }

    }
}