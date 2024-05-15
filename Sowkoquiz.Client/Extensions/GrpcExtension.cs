using Grpc.Core;
using Grpc.Net.Client.Web;

namespace Sowkoquiz.Extensions;

public static class GrpcExtension
{
    public static IServiceCollection RegisterGrpcClient<TClient>(this IServiceCollection services, Uri? baseUrl) where TClient : ClientBase<TClient>
    {
        services.AddGrpcClient<TClient>(options =>
        {
            if (baseUrl is not null)
            {
                options.Address = baseUrl;
            }
        }).ConfigureChannel(options =>
            options.HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler()));

        return services;
    }
}