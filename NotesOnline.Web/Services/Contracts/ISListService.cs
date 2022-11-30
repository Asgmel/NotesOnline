using NotesOnline.Dtos;

namespace NotesOnline.Web.Services.Contracts
{
    public interface ISListService
    {
        public Task<IEnumerable<SListReadDto>> GetSLists();

        public Task<SListReadDto?> GetSList(int id);

        public Task<bool> CreateSList(SListCreateDto sListCreateDto);

        public Task<bool> DeleteSList(int listId);

        public Task<bool> UpdateSList(SListUpdateDto sListUpdateDto, int listId);
    }
}
