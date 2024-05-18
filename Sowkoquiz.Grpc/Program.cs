using Sowkoquiz.Application;
using Sowkoquiz.Common.Options;
using Sowkoquiz.Grpc.Configuration;
using Sowkoquiz.Grpc.Services;
using Sowkoquiz.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var opt = builder.Services.RegisterAndGetOptions<AppConfiguration>();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", b =>
{
    b.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding", "X-Grpc-Web", "User-Agent");
}));

// Add services to the container.
builder.Services.AddGrpc();

builder
    .Services
    .AddInfrastructure(opt.Value.QuizzRetentionTimeInMinutes)
    .AddApplication();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseGrpcWeb(new GrpcWebOptions {DefaultEnabled = true});

// Configure the HTTP request pipeline.
app.MapGrpcService<QuizService>().RequireCors("AllowAll");
app.MapGrpcService<SearchService>().RequireCors("AllowAll");
app.MapGrpcService<AccessService>().RequireCors("AllowAll");
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();