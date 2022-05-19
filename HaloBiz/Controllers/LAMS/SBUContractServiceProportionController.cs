using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.Impl.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.LeadAdministration,32)]

    public class SBUContractServiceProportionController : ControllerBase
    {
        private readonly ISBUToContractServiceProportionsService _sQSSP;

        public SBUContractServiceProportionController(ISBUToContractServiceProportionsService sQSSP)
        {
            _sQSSP = sQSSP;
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewQuoteServiceDocument( IEnumerable<SbutoContractServiceProportionReceivingDTO> listOfsQSSP)
        {
            return await _sQSSP.SaveSBUToQuoteProp(HttpContext, listOfsQSSP);
        }

        [HttpGet("{contractServiceId}")]
        public async Task<ApiCommonResponse> GetById(long contractServiceId)
        {
            return await _sQSSP.GetAllSBUProportionForContractService(contractServiceId);
        }

    }
}