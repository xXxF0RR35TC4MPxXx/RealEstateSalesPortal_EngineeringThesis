using Blazored.LocalStorage;
using BlazorStyled;
using Inżynierka.Client;
using Inżynierka.Client.AuthProviders;
using Inżynierka.Client.Handlers;
using Inżynierka.Client.Logics;
using MatBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Majorsoft.Blazor.Components.Common.JsInterop;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddOptions();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazorStyled();
builder.Services.AddMatBlazor();
builder.Services.AddAuthorizationCore();
builder.Services.AddHxServices();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddJsInteropExtensions();
builder.Services.AddMatToaster(config =>
{
    config.Position = MatToastPosition.TopRight;
    config.PreventDuplicates = true;
    config.NewestOnTop = true;
    config.ShowCloseButton = true;
    config.MaximumOpacity = 95;
    config.VisibleStateDuration = 3000;
});
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//Let's register the 'Named' HttpClient where we define the API domain and the DelegateHandler.
builder.Services.AddHttpClient("API", options => {
    options.BaseAddress = new Uri("https://localhost:7212/");
})
.AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IApiLogic, ApiLogic>();
builder.Services.AddScoped<CookieHandler>();
await builder.Build().RunAsync();
