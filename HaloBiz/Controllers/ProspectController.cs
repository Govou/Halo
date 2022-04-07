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
    [ModuleName(HalobizModules.LeadAdministration)]

    public class ProspectController : ControllerBase
    {
        private readonly IProspectService _ProspectService;

        public ProspectController(IProspectService prospectService)
        {
            this._ProspectService = prospectService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetProspect()
        {
            return await _ProspectService.GetAllProspect();
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _ProspectService.GetProspectById(id);
        }

        [HttpGet("GetByEmail/{email}")]
        public async Task<ApiCommonResponse> GetByEmail(string email)
        {
            return await _ProspectService.GetProspectByEmail(email);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewProspect(ProspectReceivingDTO ProspectReceiving)
        {
            return await _ProspectService.AddProspect(HttpContext, ProspectReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ProspectReceivingDTO ProspectReceiving)
        {
            return await _ProspectService.UpdateProspect(HttpContext, id, ProspectReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _ProspectService.DeleteProspect(id);
        }
    }
}
