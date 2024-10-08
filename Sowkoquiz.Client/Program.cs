using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Blazored.Modal;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sowkoquiz;
using Sowkoquiz.Configuration;
using Sowkoquiz.Extensions;
using Sowkoquiz.Grpc;
using Sowkoquiz.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var config = new AppConfiguration
{
    GrpcBaseUrl = new Uri(builder.Configuration["AppConfiguration:GrpcBaseUrl"]!)
};


builder.Services.AddSingleton<VersionProvider>(sp => new VersionProvider(Assembly.GetExecutingAssembly()));


builder.Services.AddBlazoredLocalStorage(cfg =>
{
    cfg.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    cfg.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    cfg.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    cfg.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    cfg.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    cfg.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    cfg.JsonSerializerOptions.WriteIndented = false;
});

builder
    .Services
    .RegisterGrpcClient<QuizService.QuizServiceClient>(config.GrpcBaseUrl)
    .RegisterGrpcClient<SearchService.SearchServiceClient>(config.GrpcBaseUrl)
    .RegisterGrpcClient<AccessService.AccessServiceClient>(config.GrpcBaseUrl);

builder.Services.AddBlazoredModal();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();
