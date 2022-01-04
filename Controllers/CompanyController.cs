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
        public async Task<ApiCommonResponse> GetAllCompanies()
        {
            return await _companyService.GetAllCompanies();
        }
        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _companyService.GetCompanyByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _companyService.GetCompanyById(id);
        }

        //[HttpPost("")]
        //public async Task<ApiCommonResponse> AddNewCompany(CompanyReceivingDTO companyReceiving)
        //{
        //    return await _companyService.AddCompany(companyReceiving);
        //    
        //        
        //    var company = ((ApiOkResponse)response).Result;
        //    return Ok((CompanyTransferDTO)company);
        //}

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, CompanyReceivingDTO companyReceiving)
        {
            return await _companyService.UpdateCompany(id, companyReceiving);
        }
    }
}
