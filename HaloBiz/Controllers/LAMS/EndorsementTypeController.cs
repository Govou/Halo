using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EndorsementTypeController : ControllerBase
    {
        private readonly IEndorsementTypeService _endorsementTypeService;

        public EndorsementTypeController(IEndorsementTypeService endorsementTypeService)
        {
            _endorsementTypeService = endorsementTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetEndorsementType()
        {
            return await _endorsementTypeService.GetAllEndorsementType();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _endorsementTypeService.GetEndorsementTypeByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _endorsementTypeService.GetEndorsementTypeById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewEndorsementType(EndorsementTypeReceivingDTO endorsementTypeReceiving)
        {
            return await _endorsementTypeService.AddEndorsementType(HttpContext, endorsementTypeReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, EndorsementTypeReceivingDTO endorsementTypeReceivingDTO)
        {
            return await _endorsementTypeService.UpdateEndorsementType(HttpContext, id, endorsementTypeReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _endorsementTypeService.DeleteEndorsementType(id);
        }
    }
}