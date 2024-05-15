using Grpc.Core;

namespace Sowkoquiz.Grpc.Services;

public class AccessService : Grpc.AccessService.AccessServiceBase
{
    public override Task<AcquireAccessKeyResponse> AcquireAccessKey(AcquireAccessKeyMessage request, ServerCallContext context)
    {
        return Task.FromResult(new AcquireAccessKeyResponse
        {
            AccessKey = Guid.NewGuid().ToString()
        });
    }
}