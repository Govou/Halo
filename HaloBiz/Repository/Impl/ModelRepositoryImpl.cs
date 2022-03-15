using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Model;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{

    public class ModelRepositoryImpl : IModelRepository
    {
        private readonly HalobizContext _context;
        //private readonly ILogger<MakeRepositoryImpl> _logger;
        public ModelRepositoryImpl(HalobizContext context, ILogger<ModelRepositoryImpl> logger)
        {
            //this._logger = logger;
            this._context = context;
        }
        public async Task<HalobizMigrations.Models.Model> SaveModel(HalobizMigrations.Models.Model model)
        {
            var makeEntity = await _context.Models.AddAsync(model);
            if (await SaveChanges())
            {
                return makeEntity.Entity;
            }
            return null;
        }

        public async Task<IEnumerable<HalobizMigrations.Models.Model>> GetModel(int makeId)
        {
            return await _context.Models
                .Where(x => !x.IsDeleted && x.MakeId == makeId)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<bool> DeleteModel(HalobizMigrations.Models.Model model)
        {
            model.IsDeleted = true;
            _context.Models.Update(model);
            return await SaveChanges();
        }

        public async Task<HalobizMigrations.Models.Model> FindModelById(long Id)
        {
            return await _context.Models
             .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<List<IValidation>> ValidateModel(string modelCaption)
        {
            List<HalobizMigrations.Models.Model> validateName = await _context.Models
                .Where(x => !x.IsDeleted && x.Caption == modelCaption)
                .OrderBy(x => x.Caption)
                .ToListAsync();

            List<IValidation> res = new List<IValidation>();

            if (validateName.Count > 0)
            {
                res.Add(new IValidation { Message = "Model Already Exists", Field = "Name" });
            }

            return res;

        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return false;
            }
        }


    }
}