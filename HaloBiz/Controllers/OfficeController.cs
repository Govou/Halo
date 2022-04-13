using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Profile)]

    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _officeService;

        public OfficeController(IOfficeService officeService)
        {
            this._officeService = officeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetOffices()
        {
            return await _officeService.GetAllOffices();
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _officeService.GetOfficeByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _officeService.GetOfficeById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewOffice(OfficeReceivingDTO officeReceivingDTO)
        {
            return await _officeService.AddOffice(officeReceivingDTO);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, OfficeReceivingDTO officeReceivingDTO)
        {
            return await _officeService.UpdateOffice(id, officeReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _officeService.DeleteOffice(id);
        }
    }
}