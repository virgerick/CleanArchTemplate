using System;
using CleanArchTemplate.Shared.Requests.Vehicles;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IVehicleAPIService
{
    [Get("/Vehicle/")]
    public Task<ResultList<VehicleResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get("/Vehicle/Default")]
    public Task<Result<VehicleDefaultResponse>> GetDefaultAsync(CancellationToken cancellationToken=default);
    [Get("/Vehicle/{Id}")]
    public Task<Result<VehicleResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post("/Vehicle/")]
    public Task<Result<Guid>> CreateAsync([Body]CreateEditVehicleRequest request, CancellationToken cancellationToken = default);
    [Put("/Vehicle/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body]CreateEditVehicleRequest request, CancellationToken cancellationToken = default);
    [Delete("/Vehicle/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch("/Vehicle/Restore/{Id}")]
    public Task<Result<Guid>> RestoreAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch("/Vehicle/Activate/{Id}")]
    public Task<Result<Guid>> ActivateAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch("/Vehicle/Maintenance/{Id}")]
    public Task<Result<Guid>> MaintenanceAsync(Guid Id, CancellationToken cancellationToken = default);
}

