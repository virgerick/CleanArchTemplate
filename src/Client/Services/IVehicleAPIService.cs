using System;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IVehicleAPIService
{
    private const string EndPoint = "/Api/Vehicles";
    [Get(EndPoint)]
    public Task<ResultList<VehicleResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get(EndPoint+"/Default")]
    public Task<Result<VehicleDefaultResponse>> GetDefaultAsync(CancellationToken cancellationToken=default);
    [Get(EndPoint+"/{Id}")]
    public Task<Result<VehicleResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post(EndPoint)]
    public Task<Result<Guid>> CreateAsync([Body]CreateEditVehicleRequest request, CancellationToken cancellationToken = default);
    [Put(EndPoint+"/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body]CreateEditVehicleRequest request, CancellationToken cancellationToken = default);
    [Delete(EndPoint+"/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch(EndPoint+"/Restore/{Id}")]
    public Task<Result<Guid>> RestoreAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch(EndPoint+"/Activate/{Id}")]
    public Task<Result<Guid>> ActivateAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch(EndPoint+"/Maintenance/{Id}")]
    public Task<Result<Guid>> MaintenanceAsync(Guid Id, CancellationToken cancellationToken = default);
}
