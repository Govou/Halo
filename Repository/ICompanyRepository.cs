using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface ICompanyRepository
    {

        Task<Company> SaveCompany(Company company);

        Task<Company> FindCompanyById(long Id);

        Task<Company> FindCompanyByName(string name);

        Task<IEnumerable<Company>> FindAllCompanies();

        Task<Company> UpdateCompany(Company company);

       // Task<bool> DeleteCompany(Company company);

       
    }
}