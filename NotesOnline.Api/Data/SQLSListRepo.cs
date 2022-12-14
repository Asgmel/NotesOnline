using Microsoft.EntityFrameworkCore;
using NotesOnline.Api.Models;

namespace NotesOnline.Api.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class SQLSListRepo : ISListRepo
    {
        private readonly SQLDbContext _context;

        public SQLSListRepo(SQLDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateListAsync(SList sList)
        {
            if (sList == null)
            {
                throw new ArgumentNullException(nameof(sList));
            }

            await _context.Lists.AddAsync(sList);
        }

        public void DeleteList(SList sList)
        {
            if (sList == null)
            {
                throw new ArgumentNullException(nameof(sList));
            }

            _context.Lists.Remove(sList);
        }

        public void DeleteLists(IEnumerable<SList> sLists)
        {
            sLists.ToList().ForEach(l => _context.Lists.Remove(l));
        }

        public async Task<IEnumerable<SList>> GetAllListsAsync(int userId)
        {
            return await _context.Lists.Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task<SList?> GetListByIdAsync(int id)
        {
            return await _context.Lists.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
