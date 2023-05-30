using CleanArchTemplate.Shared.Requests.Customers;
using CleanArchTemplate.Shared.Responses.Customers;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface ICustomerAPIService
{
    [Get("/Customer/")]
    public Task<ResultList<CustomerResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get("/Customer/{Id}")]
    public Task<Result<CustomerResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post("/Customer/")]
    public Task<Result<Guid>> CreateAsync([Body]AddEditCustomerRequest request, CancellationToken cancellationToken = default);
    [Put("/Customer/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body]AddEditCustomerRequest request, CancellationToken cancellationToken = default);
    [Delete("/Customer/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch("/Customer/Restore/{Id}")]
    public Task<Result<Guid>> RestoreAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch("/Customer/Activate/{Id}")]
    public Task<Result<Guid>> ActivateAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch("/Customer/Maintenance/{Id}")]
    public Task<Result<Guid>> MaintenanceAsync(Guid Id, CancellationToken cancellationToken = default);
}

