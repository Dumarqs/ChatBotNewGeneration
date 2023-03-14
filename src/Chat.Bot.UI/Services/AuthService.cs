using Blazored.LocalStorage;
using Chat.Bot.UI.Authentication;
using Chat.Bot.UI.Model;
using Chat.Bot.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Chat.Bot.UI.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly Config _config;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage,
                           Config config)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            _config = config;
        }

        public async Task<ResponseModel> Register(RegisterModel register)
        {
            var requestJson = JsonSerializer.Serialize(register);
            var data = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_config.GetBaseUrl() + "/User/Register", data);
            var result = new ResponseModel();
            if (response.IsSuccessStatusCode)
                result.Successful = true;
            else
                result.Successful = false;

            return result;
        }

        public async Task<LoginResponseModel> Login(LoginModel loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var response = await _httpClient.PostAsync(_config.GetBaseUrl() + "/User/Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var user = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var loginResponse = new LoginResponseModel();
            if (!response.IsSuccessStatusCode)
            {
                loginResponse.Successful = false;
                loginResponse.Error = await response.Content.ReadAsStringAsync();
                return loginResponse;
            }

            loginResponse.Successful = true;

            await _localStorage.SetItemAsync("authToken", user.Token);
            await _localStorage.SetItemAsync("user", user);

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(user.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", user.Token);

            return loginResponse;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
