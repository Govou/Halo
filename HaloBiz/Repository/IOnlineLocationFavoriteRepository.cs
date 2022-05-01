using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IOnlineLocationFavoriteRepository
    {
        Task<OnlineLocationFavourite> SaveLocationFavorite(OnlineLocationFavourite locationFavourite);

        Task<OnlineLocationFavourite> FindLocationFavoriteById(long Id);
        Task<OnlineLocationFavourite> FindAllLocationFavoritesByClientId(long clientId);

        Task<IEnumerable<OnlineLocationFavourite>> FindAllLocationFavorites();

        //OnlineLocationFavourite GetTypename(string rankName);

        Task<OnlineLocationFavourite> UpdateLocationFavorite(OnlineLocationFavourite locationFavourite);

        Task<bool> DeleteLocationFavorite(OnlineLocationFavourite locationFavourite);
    }
}
