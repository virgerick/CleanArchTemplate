using System;

namespace CleanArchTemplate.Application.Common.Interfaces.Services;

public interface IDateTimeService
{
    DateTimeOffset NowUtc { get; }
}