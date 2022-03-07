using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HalobizMigrations.Models;
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

        public async Task<ApiCommonResponse> AddCompany(CompanyReceivingDTO companyReceivingDTO)
        {
            var company = _mapper.Map<Company>(companyReceivingDTO);
            var savedCompany = await _companyRepo.SaveCompany(company);
            if (savedCompany == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var companyTransferDTOs = _mapper.Map<CompanyTransferDTO>(company);
            return CommonResponse.Send(ResponseCodes.SUCCESS,companyTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetAllCompanies()
        {
            var companyes = await _companyRepo.FindAllCompanies();
            if (companyes == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var companyTransferDTOs = _mapper.Map<IEnumerable<CompanyTransferDTO>>(companyes);
            return CommonResponse.Send(ResponseCodes.SUCCESS,companyTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetCompanyById(long id)
        {
            var company = await _companyRepo.FindCompanyById(id);
            if (company == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var companyTransferDTOs = _mapper.Map<CompanyTransferDTO>(company);
            return CommonResponse.Send(ResponseCodes.SUCCESS,companyTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetCompanyByName(string name)
        {
            var company = await _companyRepo.FindCompanyByName(name);
            if (company == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var companyTransferDTOs = _mapper.Map<CompanyTransferDTO>(company);
            return CommonResponse.Send(ResponseCodes.SUCCESS,companyTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateCompany(long id, CompanyReceivingDTO companyReceivingDTO)
        {
            var companyToUpdate = await _companyRepo.FindCompanyById(id);
            if (companyToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            companyToUpdate.Name = companyReceivingDTO.Name;
            companyToUpdate.Description = companyReceivingDTO.Description;
            companyToUpdate.Address = companyReceivingDTO.Address;
            companyToUpdate.HeadId = companyReceivingDTO.HeadId;
            var updatedCompany = await _companyRepo.UpdateCompany(companyToUpdate);

            if (updatedCompany == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var companyTransferDTOs = _mapper.Map<CompanyTransferDTO>(updatedCompany);
            return CommonResponse.Send(ResponseCodes.SUCCESS,companyTransferDTOs);


        }

    }
}
