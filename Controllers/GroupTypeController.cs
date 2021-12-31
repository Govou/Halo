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
        public async Task<ApiCommonResponse> GetGroupType()
        {
            return await _groupTypeService.GetAllGroupType();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _groupTypeService.GetGroupTypeByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _groupTypeService.GetGroupTypeById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewGroupType(GroupTypeReceivingDTO groupTypeReceiving)
        {
            return await _groupTypeService.AddGroupType(HttpContext, groupTypeReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, GroupTypeReceivingDTO groupTypeReceiving)
        {
            return await _groupTypeService.UpdateGroupType(HttpContext, id, groupTypeReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _groupTypeService.DeleteGroupType(id);
        }
    }
}