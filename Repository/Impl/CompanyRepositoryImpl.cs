using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class CompanyRepositoryImpl : ICompanyRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<CompanyRepositoryImpl> _logger;
        public CompanyRepositoryImpl(HalobizContext context, ILogger<CompanyRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<Company> SaveCompany(Company company)
        {
            var CompanyEntity = await _context.Companies.AddAsync(company);
            if(await SaveChanges())
            {
                return CompanyEntity.Entity;
            }
            return null;
        }

        public async Task<Company> FindCompanyById(long Id)
        {
            return await _context.Companies
                .Include(company => company.Head)
                .FirstOrDefaultAsync( company => company.Id == Id && company.IsDeleted == false);
        }

        public async Task<Company> FindCompanyByName(string name)
        {
            return await _context.Companies
                .Include(company => company.Head)
                .FirstOrDefaultAsync( company => company.Name == name && company.IsDeleted == false);
        }

        public async Task<IEnumerable<Company>> FindAllCompanies()
        {
            return await _context.Companies.Where(company => company.IsDeleted == false)
                .Include(company => company.Head)
                .ToListAsync();
        }

        public async Task<Company> UpdateCompany(Company company)
        {
            var companyEntity =  _context.Companies.Update(company);
            if(await SaveChanges())
            {
                return companyEntity.Entity;
            }
            return null;
        }

        private async Task<bool> SaveChanges()
        {
           try
           {
               return  await _context.SaveChangesAsync() > 0;
           }
           catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}