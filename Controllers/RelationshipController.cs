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
//using Controllers.Models;
    

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RelationshipController : ControllerBase
    {
        private readonly IRelationshipService _relationshipService;

        public RelationshipController(IRelationshipService relationshipService)
        {
            this._relationshipService = relationshipService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetRelationship()
        {
            var response = await _relationshipService.GetAllRelationship();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var relationship = ((ApiOkResponse)response).Result;
            return Ok(relationship);
        }

        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _relationshipService.GetRelationshipByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var relationship = ((ApiOkResponse)response).Result;
            return Ok(relationship);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _relationshipService.GetRelationshipById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var relationship = ((ApiOkResponse)response).Result;
            return Ok(relationship);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewRelationship(RelationshipReceivingDTO relationshipReceiving)
        {
            var response = await _relationshipService.AddRelationship(HttpContext, relationshipReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var relationship = ((ApiOkResponse)response).Result;
            return Ok(relationship);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, RelationshipReceivingDTO relationshipReceiving)
        {
            var response = await _relationshipService.UpdateRelationship(HttpContext, id, relationshipReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var relationship = ((ApiOkResponse)response).Result;
            return Ok(relationship);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _relationshipService.DeleteRelationship(id);
            return StatusCode(response.StatusCode);
        }
    }
}