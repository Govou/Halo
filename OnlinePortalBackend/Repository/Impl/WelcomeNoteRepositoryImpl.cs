using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models.OnlinePortal;
using HalobizMigrations.Data;

namespace OnlinePortalBackend.Repository.Impl
{
    public class WelcomeNoteRepositoryImpl : IWelcomeNoteRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<WelcomeNoteRepositoryImpl> _logger;
        public WelcomeNoteRepositoryImpl(HalobizContext context, ILogger<WelcomeNoteRepositoryImpl> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<WelcomeNote> SaveWelcomeNote(WelcomeNote welcomeNote)
        {
            /*var welcomeNoteEntity = await _context.WelcomeNotes.AddAsync(welcomeNote);
            if(await SaveChanges())
            {
                return welcomeNoteEntity.Entity;
            }*/
            return null;
        }

        public async Task<bool> UpdateWelcomeNotes(IEnumerable<WelcomeNote> welcomeNotes)
        {
            //_context.WelcomeNotes.UpdateRange(welcomeNotes);
            return await SaveChanges();
        }

        public async Task<WelcomeNote> FindWelcomeNoteById(long Id)
        {
            /*return await _context.WelcomeNotes
                .FirstOrDefaultAsync( user => user.Id == Id && user.IsDeleted == false);*/
            return null;
        }

        public async Task<IEnumerable<WelcomeNote>> FindAllWelcomeNotes()
        {
            /*return await _context.WelcomeNotes
                .Where(user => user.IsDeleted == false)
                .ToListAsync();*/
            return null;
        }

        public async Task<WelcomeNote> UpdateWelcomeNote(WelcomeNote welcomeNote)
        {
            /*var WelcomeNoteEntity =  _context.WelcomeNotes.Update(welcomeNote);
            if(await SaveChanges())
            {
                return WelcomeNoteEntity.Entity;
            }*/
            return null;
        }

        public async Task<bool> RemoveWelcomeNote(WelcomeNote welcomeNote)
        {
            /*welcomeNote.IsDeleted = true;
            _context.WelcomeNotes.Update(welcomeNote);*/
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            try{
                return await _context.SaveChangesAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}