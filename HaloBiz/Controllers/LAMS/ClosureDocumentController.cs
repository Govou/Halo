using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ClosureDocumentController : ControllerBase
    {
        private readonly IClosureDocumentService _closureDocumentService;

        public ClosureDocumentController(IClosureDocumentService closureDocumentService)
        {
            this._closureDocumentService = closureDocumentService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetClosureDocument()
        {
            return await _closureDocumentService.GetAllClosureDocument();
           
        }
        [HttpGet("caption/{caption}")]
        public async Task<ApiCommonResponse> GetByCaption(string caption)
        {
            return await _closureDocumentService.GetClosureDocumentByCaption(caption);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _closureDocumentService.GetClosureDocumentById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewClosureDocument(ClosureDocumentReceivingDTO closureDocumentReceiving)
        {
            return await _closureDocumentService.AddClosureDocument(HttpContext, closureDocumentReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ClosureDocumentReceivingDTO closureDocumentReceivingDTO)
        {
            return await _closureDocumentService.UpdateClosureDocument(HttpContext, id, closureDocumentReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _closureDocumentService.DeleteClosureDocument(id);
        }
    }
}