using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups)]

    public class RequiredServiceDocumentController : ControllerBase
    {
        private readonly IRequiredServiceDocumentService _requiredServiceDocumentService;

        public RequiredServiceDocumentController(IRequiredServiceDocumentService requiredServiceDocumentService)
        {
            this._requiredServiceDocumentService = requiredServiceDocumentService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetRequiredServiceDocument()
        {
            return await _requiredServiceDocumentService.GetAllRequiredServiceDocument();
        }

        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _requiredServiceDocumentService.GetRequiredServiceDocumentByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _requiredServiceDocumentService.GetRequiredServiceDocumentById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewRequiredServiceDocument(RequiredServiceDocumentReceivingDTO requiredServiceDocumentReceiving)
        {
            return await _requiredServiceDocumentService.AddRequiredServiceDocument(HttpContext, requiredServiceDocumentReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, RequiredServiceDocumentReceivingDTO requiredServiceDocumentReceiving)
        {
            return await _requiredServiceDocumentService.UpdateRequiredServiceDocument(HttpContext, id, requiredServiceDocumentReceiving);
            
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _requiredServiceDocumentService.DeleteRequiredServiceDocument(id);
        }
    }
}