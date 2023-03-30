using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using LocalEdit;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using Modulight.Modules.Hosting;
//using StardustDL.RazorComponents.Markdown;
using Microsoft.Extensions.DependencyInjection;
//using StardustDL.RazorComponents.Markdown;
using MermaidJS.Blazor;
using BlazorPanzoom;
using Microsoft.JSInterop;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddBlazorDownloadFile();

// https://github.com/Blazored/LocalStorage
//builder.Services.AddBlazoredLocalStorage();

// https://github.com/StardustDL/RazorComponents.Markdown
//builder.Services.AddModules(builder =>
//{
//    builder.UseRazorComponentClientModules().AddMarkdownModule();
//});

builder.Services.AddMermaidJS(options =>
{
    options.MaxTextSize = 100000;
    options.SecurityLevel = MermaidSecurityLevels.Loose;
    //options.SecurityLevel = MermaidSecurityLevels.AntiScript;
});

builder.Services.AddBlazorPanzoomServices();

// ?? and add this line to register a "PWA updater" service to a DI container.
builder.Services.AddPWAUpdater();

// https://www.xyb.name/2020/08/02/correct-localtime-in-blazor-webassembly/
var host = builder.Build();

LocalEdit.Shared.DateTimeExtension.sTimezoneOffset = await host.Services.GetRequiredService<IJSRuntime>().InvokeAsync<int>("eval", "-new Date().getTimezoneOffset()");

//await builder.Build().RunAsync();
await host.RunAsync();
