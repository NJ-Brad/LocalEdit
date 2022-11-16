using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using LocalEdit;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Modulight.Modules.Hosting;
//using StardustDL.RazorComponents.Markdown;
using Microsoft.Extensions.DependencyInjection;
using StardustDL.RazorComponents.Markdown;

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
builder.Services.AddModules(builder =>
{
    builder.UseRazorComponentClientModules().AddMarkdownModule();
});

//builder.Services.AddMermaidJS();

await builder.Build().RunAsyncWithModules();
//await builder.Build().RunAsync();
