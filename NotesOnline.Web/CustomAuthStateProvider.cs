using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace NotesOnline.Web
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await GetTokenAsync();
            var identity = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(token) && TokenValidCheck(token))
            {

                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

                // Add the token to future request headers
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var keyValuePairs = DeserializeToken(jwt);
            return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
        }

        private static Dictionary<string, object> DeserializeToken(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)!;
        }

        private static bool TokenValidCheck(string jwt)
        {
            if (!string.IsNullOrEmpty(jwt))
            {
                var keyValuePairs = DeserializeToken(jwt);
                var expString = keyValuePairs["exp"].ToString();
                var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expString!));
                return exp > DateTime.UtcNow;
            }

            return false;
        }

        private async Task<string> GetTokenAsync()
        {
            return await _localStorage.GetItemAsStringAsync("token");
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
