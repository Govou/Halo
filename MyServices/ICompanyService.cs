using HaloBiz.DTOs.ApiDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ReceivingDTOs;

namespace HaloBiz.MyServices
{
    public interface ICompanyService
    {
        Task<ApiCommonResponse> AddCompany(CompanyReceivingDTO companyReceivingDTO);
        Task<ApiCommonResponse> GetCompanyById(long id);
        Task<ApiCommonResponse> GetCompanyByName(string name);
        Task<ApiCommonResponse> GetAllCompanies();
        Task<ApiCommonResponse> UpdateCompany(long id, CompanyReceivingDTO companyReceivingDTO);
       // Task<ApiCommonResponse> DeleteCompany(long id);
    }
}
