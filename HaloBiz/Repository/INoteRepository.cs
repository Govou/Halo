using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface INoteRepository
    {
        Task<Note> SaveNote(Note note);
        Task<Note> FindNoteById(long Id);
        Task<Note> FindNoteByName(string name);
        Task<IEnumerable<Note>> FindAllNotes();
        Task<Note> UpdateNote(Note note);
        Task<bool> DeleteNote(Note note);
    }
}
