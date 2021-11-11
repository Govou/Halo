﻿using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class ArmedEscortRegistrationRepositoryImpl:IArmedEscortRegistrationRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ArmedEscortRegistrationRepositoryImpl> _logger;
        public ArmedEscortRegistrationRepositoryImpl(HalobizContext context, ILogger<ArmedEscortRegistrationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteArmedEscort(ArmedEscortProfile armedEscortProfile)
        {
            armedEscortProfile.IsDeleted = true;
            _context.ArmedEscortProfiles.Update(armedEscortProfile);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ArmedEscortProfile>> FindAllArmedEscorts()
        {
            return await _context.ArmedEscortProfiles.Where(ae => ae.IsDeleted == false)
                          .ToListAsync();
        }

        public async Task<ArmedEscortProfile> FindArmedEscortById(long Id)
        {
            return await _context.ArmedEscortProfiles.FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<ArmedEscortProfile> SaveArmedEscort(ArmedEscortProfile armedEscortProfile)
        {
            var savedEntity = await _context.ArmedEscortProfiles.AddAsync(armedEscortProfile);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmedEscortProfile> UpdateArmedEscort(ArmedEscortProfile armedEscortProfile)
        {
            var updatedEntity = _context.ArmedEscortProfiles.Update(armedEscortProfile);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
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
    }
}
