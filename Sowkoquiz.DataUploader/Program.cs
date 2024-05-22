using Microsoft.EntityFrameworkCore;
using Sowkoquiz.Application;
using Sowkoquiz.DataUploader;
using Sowkoquiz.Domain.Common;
using Sowkoquiz.Infrastructure;
using Sowkoquiz.Infrastructure.Persistance;

var db = args[0];
var dataFolder = args[1];

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<SowkoquizDbContext>(opt => opt.UseSqlite($"DataSource = {db}"));
builder.Services.AddSingleton<DataReader>(sp =>
    new DataReader(dataFolder, sp.GetRequiredService<ILogger<DataReader>>()));

builder.Services.AddSingleton<IDomainEventsQueue, EmptyDataQueue>();
builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
builder.Services.AddHostedService<DataUploaderWorker>(sp =>
    new DataUploaderWorker(sp.GetRequiredService<ILogger<DataUploaderWorker>>(),
        sp.GetRequiredService<IServiceScopeFactory>(), sp.GetRequiredService<DataReader>(),
        sp.GetRequiredService<IDateTimeProvider>()));

var host = builder.Build();
host.Run();