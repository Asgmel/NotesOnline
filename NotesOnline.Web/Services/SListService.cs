using MudBlazor;
using NotesOnline.Dtos;
using NotesOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace NotesOnline.Web.Services
{
    public class SListService : ISListService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackBar;

        public SListService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackBar = snackbar;
        }

        public async Task<bool> CreateSList(SListCreateDto sListCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/SLists", sListCreateDto);

            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("List added successfully.", Severity.Success);
            }
            else
            {
                _snackBar.Add("Error adding list. Please try again later.", Severity.Error);
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSList(int listId)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/SLists/{listId}");

            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("List deleted.", Severity.Success);
            }
            else
            {
                _snackBar.Add("List deletion failed.", Severity.Error);
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<SListReadDto?> GetSList(int id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/SLists/{id}");


            if (response.IsSuccessStatusCode)
            {
                var sList = await response.Content.ReadFromJsonAsync<SListReadDto>();

                if (sList is not null)
                {
                    return sList;
                }

            }
            else
            {
                _snackBar.Add("Error retrieving list from server. Please try again later.", Severity.Error);
            }

            return new SListReadDto();
        }

        public async Task<IEnumerable<SListReadDto>> GetSLists()
        {
            var response = await _httpClient.GetAsync("/api/v1/SLists");

            if (response.IsSuccessStatusCode)
            {
                var sLists = await response.Content.ReadFromJsonAsync<IEnumerable<SListReadDto>>();

                if (sLists is not null)
                {
                    return sLists;
                }
            }
            else
            {
                _snackBar.Add("Error retrieving lists from server. Please try again later.", Severity.Error);
            }
            
            return Enumerable.Empty<SListReadDto>();
        }

        public async Task<bool> UpdateSList(SListUpdateDto sListUpdateDto, int listId)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/v1/SLists/{listId}", sListUpdateDto);

            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("List name updated.", Severity.Success);
            }
            else
            {
                _snackBar.Add("List name update failed.", Severity.Error);
            }

            return response.IsSuccessStatusCode;
        }
    }
}
