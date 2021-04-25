using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;

namespace HaloBiz.Repository.Impl
{
    public class StateRepositoryImpl : IStateRepository
    {
        private readonly HalobizContext _context;
        public StateRepositoryImpl(HalobizContext context)
        {
            this._context = context;
        }

        public async Task<State> FindStateById(long Id)
        {
            return await _context.States.Include(state => state.Lgas)                 
                .FirstOrDefaultAsync( state => state.Id == Id);
        }

        public async Task<State> FindStateByName(string name)
        {
            return await _context.States
                .Include(state => state.Lgas) 
                .FirstOrDefaultAsync( state => state.Name == name);
        }

        public async Task<IEnumerable<State>> FindAllStates()
        {
            return await _context.States
                .Include(state => state.Lgas) 
                .ToListAsync();
        }

    }
}