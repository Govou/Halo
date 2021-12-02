using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IPriceRegisterRepository
    {
        Task<PriceRegister> SavePriceRegister(PriceRegister priceRegister);

        Task<PriceRegister> FindPriceRegisterById(long Id);

        Task<IEnumerable<PriceRegister>> FindAllPriceRegisters();

        //PriceRegister GetTypename(string rankName);

        Task<PriceRegister> UpdatePriceRegister(PriceRegister priceRegister);

        Task<bool> DeletePriceRegister(PriceRegister priceRegister);
    }
}
