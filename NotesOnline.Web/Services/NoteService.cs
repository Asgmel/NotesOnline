using MudBlazor;
using NotesOnline.Dtos;
using NotesOnline.Web.Services.Contracts;
using System.Net.Http;
using System.Net.Http.Json;

namespace NotesOnline.Web.Services
{
    public class NoteService : INoteService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackBar;

        public NoteService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackBar = snackbar;
        }
        public async Task<NoteReadDto?> CreateNote(NoteCreateDto noteCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/Notes", noteCreateDto);

            NoteReadDto? createdNote = null;

            if (response.IsSuccessStatusCode)
            {
                createdNote = await response.Content.ReadFromJsonAsync<NoteReadDto>();
            }
            else
            {
                _snackBar.Add("Error adding Note. Please try again later.", Severity.Error);
            }

            return createdNote;
        }

        public async Task<bool> DeleteNote(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/Notes/{id}");

            if (!response.IsSuccessStatusCode)
            {
                _snackBar.Add("Note deletion failed.", Severity.Error);
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<NoteReadDto> GetNote(int id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Notes/{id}");


            if (response.IsSuccessStatusCode)
            {
                var note = await response.Content.ReadFromJsonAsync<NoteReadDto>();

                if (note is not null)
                {
                    return note;
                }

            }
            else
            {
                _snackBar.Add("Error retrieving note from server. Please try again later.", Severity.Error);
            }

            return new NoteReadDto();
        }

        public async Task<IEnumerable<NoteReadDto>> GetNotes(int listId)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Notes?listId={listId}");

            if (response.IsSuccessStatusCode)
            {
                var notes = await response.Content.ReadFromJsonAsync<IEnumerable<NoteReadDto>>();

                if (notes is not null)
                {
                    return notes;
                }
            }
            else
            {
                _snackBar.Add("Error retrieving notes from server. Please try again later.", Severity.Error);
            }

            return Enumerable.Empty<NoteReadDto>();
        }

        public async Task<bool> UpdateNote(int id, NoteUpdateDto noteUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/v1/Notes/{id}", noteUpdateDto);

            if (!response.IsSuccessStatusCode)
            {
                _snackBar.Add("Note update failed.", Severity.Error);
            }

            return response.IsSuccessStatusCode;
        }
    }
}
