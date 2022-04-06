using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
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
    [ModuleName(HalobizModules.Setups)]

    public class DivisionController : ControllerBase
    {
        private readonly IDivisonService _divisonService;

        public DivisionController(IDivisonService divisonService)
        {
            this._divisonService = divisonService;
        }
        [HttpGet("")]
        public async Task<ApiCommonResponse> GetDivisions()
        {
            return await _divisonService.GetAllDivisions();
        }

        [HttpGet("GetDivisionsOpEntityAndSbu")]
        public async Task<ApiCommonResponse> GetDivGetDivisionsOpEntityAndSbuisions()
        {
            return await _divisonService.GetAllDivisionsAndSbu();
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _divisonService.GetDivisionByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _divisonService.GetDivisionnById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewBranch(DivisionReceivingDTO divisionReceiving)
        {
            return await _divisonService.AddDivision(divisionReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, DivisionReceivingDTO divisionReceiving)
        {
            return await _divisonService.UpdateDivision(id, divisionReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _divisonService.DeleteDivision(id);
        }
    }
}
