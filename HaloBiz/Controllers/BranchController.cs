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
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            this._branchService = branchService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetBranches()
        {
            return await _branchService.GetAllBranches();
        }
        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _branchService.GetBranchByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _branchService.GetBranchById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewBranch(BranchReceivingDTO branchReceiving)
        {
            return await _branchService.AddBranch(branchReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, BranchReceivingDTO branchReceiving)
        {
            return await _branchService.UpdateBranch(id, branchReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _branchService.DeleteBranch(id);
        }
    }
}
