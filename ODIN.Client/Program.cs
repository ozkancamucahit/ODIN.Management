using Blazored.LocalStorage;
using Client.Library.Helpers;
using Client.Library.Services.Contracts;
using Client.Library.Services.Implementations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ODIN.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddHttpClient("SystemApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7123");
}).AddHttpMessageHandler<CustomHttpHandler>();

builder.Services
    //.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7123") })
    .AddAuthorizationCore()
    .AddBlazoredLocalStorage()
    .AddScoped<GetHttpClient>()
    .AddScoped<LocalStorageService>()
    .AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>()
    .AddScoped<IUserAccount, UserAccountService>();

await builder.Build().RunAsync();
