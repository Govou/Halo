using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class NoteRepositoryImpl : INoteRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<NoteRepositoryImpl> _logger;
        public NoteRepositoryImpl(HalobizContext context, ILogger<NoteRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteNote(Note note)
        {
            note.IsDeleted = true;
            _context.Notes.Update(note);
            return await SaveChanges();
        }

        public async Task<IEnumerable<Note>> FindAllNotes()
        {
            return await _context.Notes
               .Where(note => note.IsDeleted == false)
               .OrderBy(note => note.CreatedAt)
               .ToListAsync();
        }

        public async Task<Note> FindNoteById(long Id)
        {
            return await _context.Notes
                .Where(note => note.IsDeleted == false)
                .FirstOrDefaultAsync(note => note.Id == Id && note.IsDeleted == false);

        }

        public async Task<Note> FindNoteByName(string name)
        {
            return await _context.Notes
                 .Where(note => note.IsDeleted == false)
                 .FirstOrDefaultAsync(note => note.Caption == name && note.IsDeleted == false);

        }

        public async Task<Note> SaveNote(Note note)
        {
            var noteEntity = await _context.Notes.AddAsync(note);
            if (await SaveChanges())
            {
                return noteEntity.Entity;
            }
            return null;
        }

        public async Task<Note> UpdateNote(Note note)
        {
            var noteEntity = _context.Notes.Update(note);
            if (await SaveChanges())
            {
                return noteEntity.Entity;
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
