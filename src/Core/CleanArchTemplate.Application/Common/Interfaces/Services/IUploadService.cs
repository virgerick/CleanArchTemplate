using CleanArchTemplate.Shared.Requests;

namespace CleanArchTemplate.Application.Common.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}