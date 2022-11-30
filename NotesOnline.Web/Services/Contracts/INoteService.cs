using NotesOnline.Dtos;

namespace NotesOnline.Web.Services.Contracts
{
    public interface INoteService
    {
        public Task<NoteReadDto> GetNote(int id);

        public Task<IEnumerable<NoteReadDto>> GetNotes(int listId);

        public Task<bool> DeleteNote(int id);

        public Task<bool> UpdateNote(int id, NoteUpdateDto noteUpdateDto);

        public Task<NoteReadDto?> CreateNote(NoteCreateDto noteCreateDto);
    }
}
