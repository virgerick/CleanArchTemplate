using CleanArchTemplate.Application.Common.Interfaces.Common;

namespace CleanArchTemplate.Application.Common.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}