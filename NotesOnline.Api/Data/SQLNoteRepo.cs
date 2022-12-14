using Microsoft.EntityFrameworkCore;
using NotesOnline.Api.Models;

namespace NotesOnline.Api.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class SQLNoteRepo : INoteRepo
    {
        private readonly SQLDbContext _context;

        public SQLNoteRepo(SQLDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateNoteAsync(Note note)
        {
            if (note == null)
            {
                throw new ArgumentNullException(nameof(note));
            }

            await _context.Notes.AddAsync(note);
        }

        public void DeleteNote(Note note)
        {
            if (note == null)
            {
                throw new ArgumentNullException(nameof(note));
            }
            _context.Notes.Remove(note);
        }

        public void DeleteNotes(IEnumerable<Note> notes)
        {
            notes.ToList().ForEach(n => _context.Notes.Remove(n));
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(int userId)
        {
            var sLists = await _context.Lists.Where(l => l.UserId == userId).ToListAsync();

            var notes = new List<Note>();

            foreach (var sList in sLists)
            {
                var tempNotes = await _context.Notes.Where(n => n.SListId == sList.Id).ToListAsync();
                notes.AddRange(tempNotes);
            }

            return notes;
        }

        public async Task<IEnumerable<Note>> GetAllNotesForListAsync(int sListId)
        {
            return await _context.Notes.Where(n => n.SListId == sListId).ToListAsync();
        }

        public async Task<Note?> GetNoteByIdAsync(int id)
        {
            return await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
