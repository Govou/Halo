using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;
//using Controllers.Models;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.LeadAdministration,27)]

    public class NegotiationDocumentController : ControllerBase
    {
        private readonly INegotiationDocumentService _negotiationDocumentService;

        public NegotiationDocumentController(INegotiationDocumentService negotiationDocumentService)
        {
            this._negotiationDocumentService = negotiationDocumentService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetNegotiationDocument()
        {
            return await _negotiationDocumentService.GetAllNegotiationDocument();
        }
        [HttpGet("caption/{caption}")]
        public async Task<ApiCommonResponse> GetByCaption(string caption)
        {
            return await _negotiationDocumentService.GetNegotiationDocumentByCaption(caption);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _negotiationDocumentService.GetNegotiationDocumentById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewNegotiationDocument(NegotiationDocumentReceivingDTO negotiationDocumentReceiving)
        {
            return await _negotiationDocumentService.AddNegotiationDocument(HttpContext, negotiationDocumentReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, NegotiationDocumentReceivingDTO negotiationDocumentReceivingDTO)
        {
            return await _negotiationDocumentService.UpdateNegotiationDocument(HttpContext, id, negotiationDocumentReceivingDTO);            
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _negotiationDocumentService.DeleteNegotiationDocument(id);
        }
    }
}