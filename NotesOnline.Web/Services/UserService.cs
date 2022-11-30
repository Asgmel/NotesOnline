using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using NotesOnline.Dtos;
using NotesOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace NotesOnline.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackBar;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _customAuthStateProvider;

        public UserService(HttpClient httpClient, ISnackbar snackbar, ILocalStorageService localStorage, AuthenticationStateProvider customAuthStateProvider)
        {
            _httpClient = httpClient;
            _snackBar = snackbar;
            _localStorage = localStorage;
            _customAuthStateProvider = customAuthStateProvider;
        }

        public async Task<bool> Login(UserAuthDto userAuthDto)
        {
            var response = await _httpClient.PostAsJsonAsync<UserAuthDto>("api/v1/Users/Login", userAuthDto);

            await TriggerSnackbarResponse(response, "Login Successful!");

            // Login Successful
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                await _localStorage.SetItemAsStringAsync("token", token);
                await _customAuthStateProvider.GetAuthenticationStateAsync();
            }

            return response.IsSuccessStatusCode;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("token");
            await _customAuthStateProvider.GetAuthenticationStateAsync();
        }

        public async Task<bool> Register(UserCreateDto userCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync<UserCreateDto>("api/v1/Users", userCreateDto);

            await TriggerSnackbarResponse(response, "User Created Successfully.");

            return response.IsSuccessStatusCode;
        }

        private async Task TriggerSnackbarResponse(HttpResponseMessage response, string successMessage)
        {
            // Success
            if (response.IsSuccessStatusCode)
            {
                _snackBar!.Add(successMessage, Severity.Success);
            }
            // Internal Server Error
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _snackBar!.Add("Server Error. Please try again later.", Severity.Error);
            }
            // Other Error
            else
            {
                _snackBar!.Add(await response.Content.ReadAsStringAsync(), Severity.Error);
            }
        }
    }
}
