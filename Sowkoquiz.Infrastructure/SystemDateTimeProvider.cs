using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Infrastructure;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}