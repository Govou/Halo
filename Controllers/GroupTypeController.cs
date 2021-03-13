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
    public class GroupTypeController : ControllerBase
    {
        private readonly IGroupTypeService _groupTypeService;

        public GroupTypeController(IGroupTypeService groupTypeService)
        {
            this._groupTypeService = groupTypeService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetGroupType()
        {
            var response = await _groupTypeService.GetAllGroupType();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var groupType = ((ApiOkResponse)response).Result;
            return Ok(groupType);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _groupTypeService.GetGroupTypeByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var groupType = ((ApiOkResponse)response).Result;
            return Ok(groupType);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _groupTypeService.GetGroupTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var groupType = ((ApiOkResponse)response).Result;
            return Ok(groupType);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewGroupType(GroupTypeReceivingDTO groupTypeReceiving)
        {
            var response = await _groupTypeService.AddGroupType(HttpContext, groupTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var groupType = ((ApiOkResponse)response).Result;
            return Ok(groupType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, GroupTypeReceivingDTO groupTypeReceiving)
        {
            var response = await _groupTypeService.UpdateGroupType(HttpContext, id, groupTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var groupType = ((ApiOkResponse)response).Result;
            return Ok(groupType);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _groupTypeService.DeleteGroupType(id);
            return StatusCode(response.StatusCode);
        }
    }
}