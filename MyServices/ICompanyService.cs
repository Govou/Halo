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
        Task<ApiResponse> AddCompany(CompanyReceivingDTO companyReceivingDTO);
        Task<ApiResponse> GetCompanyById(long id);
        Task<ApiResponse> GetCompanyByName(string name);
        Task<ApiResponse> GetAllCompanies();
        Task<ApiResponse> UpdateCompany(long id, CompanyReceivingDTO companyReceivingDTO);
       // Task<ApiResponse> DeleteCompany(long id);
    }
}
