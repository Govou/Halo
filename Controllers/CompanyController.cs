using HaloBiz.DTOs.ApiDTOs;
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
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            this._companyService = companyService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetAllCompanies()
        {
            var response = await _companyService.GetAllCompanies();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var company = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<CompanyTransferDTO>)company);
        }
        [HttpGet("name/{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var response = await _companyService.GetCompanyByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var company = ((ApiOkResponse)response).Result;
            return Ok((CompanyTransferDTO)company);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _companyService.GetCompanyById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var company = ((ApiOkResponse)response).Result;
            return Ok((CompanyTransferDTO)company);
        }

        //[HttpPost("")]
        //public async Task<ActionResult> AddNewCompany(CompanyReceivingDTO companyReceiving)
        //{
        //    var response = await _companyService.AddCompany(companyReceiving);
        //    if (response.StatusCode >= 400)
        //        return StatusCode(response.StatusCode, response);
        //    var company = ((ApiOkResponse)response).Result;
        //    return Ok((CompanyTransferDTO)company);
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, CompanyReceivingDTO companyReceiving)
        {
            var response = await _companyService.UpdateCompany(id, companyReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var company = ((ApiOkResponse)response).Result;
            return Ok((CompanyTransferDTO)company);
        }
    }
}
