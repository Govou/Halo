using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.LeadAdministration,33)]

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
            return await _sQSSP.SaveSBUToQuoteProp(HttpContext, listOfsQSSP);
        }

        [HttpGet("{serviceQuoteId}")]
        public async Task<ApiCommonResponse> GetById(long serviceQuoteId)
        {
            return await _sQSSP.GetAllSBUQuoteProportionForQuoteService(serviceQuoteId);
        }

    }
}