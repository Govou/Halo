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
    public class RequredServiceQualificationElementRepositoryImpl : IRequredServiceQualificationElementRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<RequredServiceQualificationElementRepositoryImpl> _logger;
        private readonly IServiceCategoryRepository _serviceCategoryRepository;
        public RequredServiceQualificationElementRepositoryImpl(DataContext context, ILogger<RequredServiceQualificationElementRepositoryImpl> logger, IServiceCategoryRepository serviceCategoryRepository)
        {
            this._logger = logger;
            this._context = context;
            _serviceCategoryRepository = serviceCategoryRepository;
        }

        public async Task<RequredServiceQualificationElement> SaveRequredServiceQualificationElement(RequredServiceQualificationElement RequredServiceQualificationElement)
        {
            var savedEntity = await _context.RequredServiceQualificationElements.AddAsync(RequredServiceQualificationElement);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<RequredServiceQualificationElement> FindRequredServiceQualificationElementById(long Id)
        {
            return await _context.RequredServiceQualificationElements
                .FirstOrDefaultAsync(RequredServiceQualificationElement => RequredServiceQualificationElement.Id == Id && RequredServiceQualificationElement.IsDeleted == false);
        }

        public async Task<RequredServiceQualificationElement> FindRequredServiceQualificationElementByName(string caption)
        {
            return await _context.RequredServiceQualificationElements
                .FirstOrDefaultAsync(RequredServiceQualificationElement => RequredServiceQualificationElement.Caption == caption && RequredServiceQualificationElement.IsDeleted == false);
        }

        public async Task<IEnumerable<RequredServiceQualificationElement>> FindAllRequredServiceQualificationElements()
        {
            return await _context.RequredServiceQualificationElements.Where(RequredServiceQualificationElement => RequredServiceQualificationElement.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<RequredServiceQualificationElement> UpdateRequredServiceQualificationElement(RequredServiceQualificationElement RequredServiceQualificationElement)
        {
            var updatedEntity = _context.RequredServiceQualificationElements.Update(RequredServiceQualificationElement);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteRequredServiceQualificationElement(RequredServiceQualificationElement RequredServiceQualificationElement)
        {
            RequredServiceQualificationElement.IsDeleted = true;
            _context.RequredServiceQualificationElements.Update(RequredServiceQualificationElement);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteRequredServiceQualificationElementRange(IEnumerable<RequredServiceQualificationElement> RequredServiceQualificationElement)
        {
            foreach (var sg in RequredServiceQualificationElement)
            {
                sg.IsDeleted = true;
            }
            _context.RequredServiceQualificationElements.UpdateRange(RequredServiceQualificationElement);
            return await SaveChanges();

        }


    }
}
