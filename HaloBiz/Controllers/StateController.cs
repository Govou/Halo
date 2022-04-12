using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups)]

    public class StateController : ControllerBase
    {
        private readonly IStatesService stateService;
        public StateController(IStatesService stateService)
        {
            this.stateService = stateService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetStates()
        {
            return await stateService.GetAllStates();
            
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetStateByName(string name)
        {
            return await stateService.GetStateByName(name);
            
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetStateById(int id)
        {
            return await stateService.GetStateById(id);
            
        }

        [HttpGet("GetAllLgas")]
        public async Task<ApiCommonResponse> GetAllLGAs()
        {
            return await stateService.GetAllLgas();
            
        }
        
    }
}