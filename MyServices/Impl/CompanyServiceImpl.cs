using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Model;
using HaloBiz.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class CompanyServiceImpl : ICompanyService
    {
        private readonly ILogger<CompanyServiceImpl> _logger;
        private readonly ICompanyRepository _companyRepo;
        private readonly IMapper _mapper;

        public CompanyServiceImpl(ICompanyRepository companyRepo, ILogger<CompanyServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._companyRepo = companyRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddCompany(CompanyReceivingDTO companyReceivingDTO)
        {
            var company = _mapper.Map<Company>(companyReceivingDTO);
            var savedCompany = await _companyRepo.SaveCompany(company);
            if (savedCompany == null)
            {
                return new ApiResponse(500);
            }
            var companyTransferDTOs = _mapper.Map<CompanyTransferDTO>(company);
            return new ApiOkResponse(companyTransferDTOs);
        }

        public async Task<ApiResponse> GetAllCompanies()
        {
            var companyes = await _companyRepo.FindAllCompanies();
            if (companyes == null)
            {
                return new ApiResponse(404);
            }
            var companyTransferDTOs = _mapper.Map<IEnumerable<CompanyTransferDTO>>(companyes);
            return new ApiOkResponse(companyTransferDTOs);
        }

        public async Task<ApiResponse> GetCompanyById(long id)
        {
            var company = await _companyRepo.FindCompanyById(id);
            if (company == null)
            {
                return new ApiResponse(404);
            }
            var companyTransferDTOs = _mapper.Map<CompanyTransferDTO>(company);
            return new ApiOkResponse(companyTransferDTOs);
        }

        public async Task<ApiResponse> GetCompanyByName(string name)
        {
            var company = await _companyRepo.FindCompanyByName(name);
            if (company == null)
            {
                return new ApiResponse(404);
            }
            var companyTransferDTOs = _mapper.Map<CompanyTransferDTO>(company);
            return new ApiOkResponse(companyTransferDTOs);
        }

        public async Task<ApiResponse> UpdateCompany(long id, CompanyReceivingDTO companyReceivingDTO)
        {
            var companyToUpdate = await _companyRepo.FindCompanyById(id);
            if (companyToUpdate == null)
            {
                return new ApiResponse(404);
            }
            companyToUpdate.Name = companyReceivingDTO.Name;
            companyToUpdate.Description = companyReceivingDTO.Description;
            companyToUpdate.Address = companyReceivingDTO.Address;
            companyToUpdate.HeadId = companyReceivingDTO.HeadId;
            var updatedCompany = await _companyRepo.UpdateCompany(companyToUpdate);

            if (updatedCompany == null)
            {
                return new ApiResponse(500);
            }
            var companyTransferDTOs = _mapper.Map<CompanyTransferDTO>(updatedCompany);
            return new ApiOkResponse(companyTransferDTOs);


        }

    }
}
