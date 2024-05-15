// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Sowkoquiz.Infrastructure.Persistance;
using Sowkoquiz.Migration;


var cts = new CancellationTokenSource();

var db = args[0];

ArgumentException.ThrowIfNullOrEmpty(db);

var builder = new DbContextOptionsBuilder<SowkoquizDbContext>();
builder.UseSqlite($"Data Source = {db}");

await using var dbContext = new SowkoquizDbContext(builder.Options, null);

var loader = new SeedDataLoader(Console.Out);

var migrator = new Migrator(loader, dbContext);

await migrator.EnsureDatabaseAsync(cts.Token);
await migrator.RunMigrationAsync(cts.Token);
await migrator.SeedDataAsync(cts.Token);

cts.Dispose();
    


