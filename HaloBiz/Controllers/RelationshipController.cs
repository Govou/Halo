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
//using Controllers.Models;
    

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups)]

    public class RelationshipController : ControllerBase
    {
        private readonly IRelationshipService _relationshipService;

        public RelationshipController(IRelationshipService relationshipService)
        {
            this._relationshipService = relationshipService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetRelationship()
        {
            return await _relationshipService.GetAllRelationship();
        }

        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _relationshipService.GetRelationshipByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _relationshipService.GetRelationshipById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewRelationship(RelationshipReceivingDTO relationshipReceiving)
        {
            return await _relationshipService.AddRelationship(HttpContext, relationshipReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, RelationshipReceivingDTO relationshipReceiving)
        {
            return await _relationshipService.UpdateRelationship(HttpContext, id, relationshipReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _relationshipService.DeleteRelationship(id);
        }
    }
}