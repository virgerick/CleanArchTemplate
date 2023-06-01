using CleanArchTemplate.Shared.Requests.Drivers;
using CleanArchTemplate.Shared.Responses.Drivers;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IDriverAPIService
{
    [Get("/Driver/")]
    public Task<ResultList<DriverResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get("/Driver/{Id}")]
    public Task<Result<DriverResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post("/Driver/")]
    public Task<Result<Guid>> CreateAsync([Body]AddEditDriverRequest request, CancellationToken cancellationToken = default);
    [Put("/Driver/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body]AddEditDriverRequest request, CancellationToken cancellationToken = default);
    [Delete("/Driver/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch("/Driver/Restore/{Id}")]
    public Task<Result<Guid>> RestoreAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch("/Driver/Activate/{Id}")]
    public Task<Result<Guid>> ActivateAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch("/Driver/Maintenance/{Id}")]
    public Task<Result<Guid>> MaintenanceAsync(Guid Id, CancellationToken cancellationToken = default);
}

