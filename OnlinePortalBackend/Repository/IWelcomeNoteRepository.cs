using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models.OnlinePortal;

namespace OnlinePortalBackend.Repository
{
    public interface IWelcomeNoteRepository
    {
        Task<WelcomeNote> SaveWelcomeNote(WelcomeNote welcomeNote);
        Task<bool> UpdateWelcomeNotes(IEnumerable<WelcomeNote> welcomeNote);
        Task<WelcomeNote> FindWelcomeNoteById(long Id);
        Task<IEnumerable<WelcomeNote>> FindAllWelcomeNotes();
        Task<WelcomeNote> UpdateWelcomeNote(WelcomeNote welcomeNote);
        Task<bool> RemoveWelcomeNote(WelcomeNote welcomeNote);
    }
}