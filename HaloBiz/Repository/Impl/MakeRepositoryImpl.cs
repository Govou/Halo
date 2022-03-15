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

    public class MakeRepositoryImpl : IMakeRepository
    {
        private readonly HalobizContext _context;
        //private readonly ILogger<MakeRepositoryImpl> _logger;
        public MakeRepositoryImpl(HalobizContext context, ILogger<MakeRepositoryImpl> logger)
        {
            //this._logger = logger;
            this._context = context;
        }
        public async Task<Make> SaveMake(Make make)
        {
            var makeEntity = await _context.Makes.AddAsync(make);
            if (await SaveChanges())
            {
                return makeEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<Make>> GetMakes()
        {
            return await _context.Makes
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<bool> DeleteMake(Make make)
        {
            make.IsDeleted = true;
            _context.Makes.Update(make);
            return await SaveChanges();
        }

        public async Task<Make> FindMakeById(long Id)
        {
            return await _context.Makes
             .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<List<IValidation>> ValidateMake(string makeCaption)
        {
            List<Make> validateName = await _context.Makes
                .Where(x => !x.IsDeleted && x.Caption == makeCaption)
                .OrderBy(x => x.Caption)
                .ToListAsync();

            List<IValidation> res = new List<IValidation>();

            if (validateName.Count > 0)
            {
                res.Add(new IValidation { Message = "Make Caption Already Exists", Field = "Name" });
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