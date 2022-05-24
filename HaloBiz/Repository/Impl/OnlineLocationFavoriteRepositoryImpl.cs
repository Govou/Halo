using HalobizMigrations.Data;
using HalobizMigrations.Models.Armada;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class OnlineLocationFavoriteRepositoryImpl: IOnlineLocationFavoriteRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<OnlineLocationFavoriteRepositoryImpl> _logger;
        public OnlineLocationFavoriteRepositoryImpl(HalobizContext context, ILogger<OnlineLocationFavoriteRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteLocationFavorite(OnlineLocationFavourite locationFavourite)
        {
            locationFavourite.IsDeleted = true;
            _context.OnlineLocationFavourites.Update(locationFavourite);
            return await SaveChanges();
        }

        public async Task<IEnumerable<OnlineLocationFavourite>> FindAllLocationFavorites()
        {
            return await _context.OnlineLocationFavourites.Where(online => online.IsDeleted == false)
                .Include(ct => ct.Client).Include(on=>on.OnlineProfile).OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<OnlineLocationFavourite>> FindAllLocationFavoritesByClientId(long clientId)
        {
            return await _context.OnlineLocationFavourites
                 .Where(aer => aer.ClientId == clientId && aer.IsDeleted == false)
                 .Include(r => r.Client).Include(x => x.OnlineProfile).ToListAsync();
        }

        public async Task<OnlineLocationFavourite> FindLocationFavoriteById(long Id)
        {
            return await _context.OnlineLocationFavourites.Include(r => r.Client).Include(x => x.OnlineProfile)
                .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<OnlineLocationFavourite> SaveLocationFavorite(OnlineLocationFavourite locationFavourite)
        {
            var savedEntity = await _context.OnlineLocationFavourites.AddAsync(locationFavourite);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<OnlineLocationFavourite> UpdateLocationFavorite(OnlineLocationFavourite locationFavourite)
        {
            var updatedEntity = _context.OnlineLocationFavourites.Update(locationFavourite);
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
