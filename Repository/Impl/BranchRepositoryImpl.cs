using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class BranchRepositoryImpl : IBranchRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<BranchRepositoryImpl> _logger;
        private readonly IOfficeRepository _officeRepository;
        public BranchRepositoryImpl(DataContext context, ILogger<BranchRepositoryImpl> logger, IOfficeRepository officeRepository)
        {
            this._logger = logger;
            this._context = context;
            this._officeRepository = officeRepository;
        }

        public async Task<Branch> SaveBranch(Branch branch)
        {
            var BranchEntity = await _context.Branches.AddAsync(branch);
            if(await SaveChanges())
            {
                return BranchEntity.Entity;
            }
            return null;
        }

        public async Task<Branch> FindBranchById(long Id)
        {
            return await _context.Branches
                .Include(branch => branch.Head)
                .Include(branch => branch.Offices
                .Where(office => office.IsDeleted == false))
                .FirstOrDefaultAsync( branch => branch.Id == Id && branch.IsDeleted == false);
        }

        public async Task<Branch> FindBranchByName(string name)
        {
            return await _context.Branches
                .Include(branch => branch.Head)
                .Include(branch => branch.Offices
                .Where(office => office.IsDeleted == false))
                .FirstOrDefaultAsync( branch => branch.Name == name && branch.IsDeleted == false);
        }

        public async Task<IEnumerable<Branch>> FindAllBranches()
        {
            return await _context.Branches.Where(branch => branch.IsDeleted == false)
                .Include(branch => branch.Head)
                .Include(branch => branch.Offices
                .Where(office => office.IsDeleted == false))
                .ToListAsync();
        }

        public async Task<Branch> UpdateBranch(Branch branch)
        {
            var branchEntity =  _context.Branches.Update(branch);
            if(await SaveChanges())
            {
                return branchEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteBranch(Branch branch)
        {           
            await _officeRepository.DeleteOfficeRange(branch.Offices);

            branch.IsDeleted = true;
            _context.Branches.Update(branch);
            return await SaveChanges();
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