using CleanArchTemplate.Application.Common.Interfaces.Services;

namespace CleanArchTemplate.Abstraction.Services;

public class SystemDateTimeService : IDateTimeService
{
    public DateTimeOffset NowUtc => DateTimeOffset.UtcNow;
}