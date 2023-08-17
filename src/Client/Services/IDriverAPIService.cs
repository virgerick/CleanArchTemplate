using CleanArchTemplate.Shared.Requests.Drivers;
using CleanArchTemplate.Shared.Responses.Drivers;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IDriverAPIService
{
    private const string EndPoint = "/Api/Drivers";
    [Get(EndPoint)]
    public Task<ResultList<DriverResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get(EndPoint+"/{Id}")]
    public Task<Result<DriverResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post(EndPoint)]
    public Task<Result<Guid>> CreateAsync([Body]AddEditDriverRequest request, CancellationToken cancellationToken = default);
    [Put(EndPoint+"/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body]AddEditDriverRequest request, CancellationToken cancellationToken = default);
    [Delete(EndPoint+"/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch(EndPoint+"/Restore/{Id}")]
    public Task<Result<Guid>> RestoreAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch(EndPoint+"/Activate/{Id}")]
    public Task<Result<Guid>> ActivateAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch(EndPoint+"/Maintenance/{Id}")]
    public Task<Result<Guid>> MaintenanceAsync(Guid Id, CancellationToken cancellationToken = default);
}

