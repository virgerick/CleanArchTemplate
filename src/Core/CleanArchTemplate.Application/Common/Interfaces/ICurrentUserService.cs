using System;
namespace CleanArchTemplate.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
}
