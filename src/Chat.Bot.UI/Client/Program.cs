using Blazored.LocalStorage;
using Chat.Bot.UI.Authentication;
using Chat.Bot.UI.Model;
using Chat.Bot.UI.Services;
using Chat.Bot.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;

namespace Chat.Bot.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            

            string apiUrl = string.Empty;
            if(builder.HostEnvironment.IsDevelopment())
                apiUrl = builder.Configuration["ApiUrl"];

            else
            {
                var http = new HttpClient()
                {
                    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                };

                builder.Services.AddScoped(sp => http);

                using var response = await http.GetAsync("docker.json");
                var api = JsonSerializer.Deserialize<ConfigDocker>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                apiUrl = api.ApiUrl;
            }          

            Console.WriteLine(apiUrl);

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri(apiUrl);
            });

            Config options = new Config(apiUrl);
            builder.Services.AddSingleton(options);

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            await builder.Build().RunAsync();
        }
    }
}