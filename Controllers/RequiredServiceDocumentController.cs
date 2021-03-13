using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RequiredServiceDocumentController : ControllerBase
    {
        private readonly IRequiredServiceDocumentService _requiredServiceDocumentService;

        public RequiredServiceDocumentController(IRequiredServiceDocumentService requiredServiceDocumentService)
        {
            this._requiredServiceDocumentService = requiredServiceDocumentService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetRequiredServiceDocument()
        {
            var response = await _requiredServiceDocumentService.GetAllRequiredServiceDocument();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var requiredServiceDocument = ((ApiOkResponse)response).Result;
            return Ok(requiredServiceDocument);
        }

        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _requiredServiceDocumentService.GetRequiredServiceDocumentByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var requiredServiceDocument = ((ApiOkResponse)response).Result;
            return Ok(requiredServiceDocument);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _requiredServiceDocumentService.GetRequiredServiceDocumentById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var requiredServiceDocument = ((ApiOkResponse)response).Result;
            return Ok(requiredServiceDocument);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewRequiredServiceDocument(RequiredServiceDocumentReceivingDTO requiredServiceDocumentReceiving)
        {
            var response = await _requiredServiceDocumentService.AddRequiredServiceDocument(HttpContext, requiredServiceDocumentReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var requiredServiceDocument = ((ApiOkResponse)response).Result;
            return Ok(requiredServiceDocument);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, RequiredServiceDocumentReceivingDTO requiredServiceDocumentReceiving)
        {
            var response = await _requiredServiceDocumentService.UpdateRequiredServiceDocument(HttpContext, id, requiredServiceDocumentReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var requiredServiceDocument = ((ApiOkResponse)response).Result;
            return Ok(requiredServiceDocument);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _requiredServiceDocumentService.DeleteRequiredServiceDocument(id);
            return StatusCode(response.StatusCode);
        }
    }
}