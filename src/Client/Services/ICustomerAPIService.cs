using CleanArchTemplate.Shared.Requests.Customers;
using CleanArchTemplate.Shared.Responses.Customers;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface ICustomerAPIService
{
    private const string EndPoint = "/Api/Customers";
    [Get(EndPoint)]
    public Task<ResultList<CustomerResponse>> GetAsync(CancellationToken cancellationToken=default);
    [Get(EndPoint+"/{Id}")]
    public Task<Result<CustomerResponse>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    [Post(EndPoint)]
    public Task<Result<Guid>> CreateAsync([Body]AddEditCustomerRequest request, CancellationToken cancellationToken = default);
    [Put(EndPoint+"/{Id}")]
    public Task<Result<Guid>> EditAsync(Guid Id, [Body]AddEditCustomerRequest request, CancellationToken cancellationToken = default);
    [Delete(EndPoint+"/{Id}")]
    public Task<Result<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch(EndPoint+"/Restore/{Id}")]
    public Task<Result<Guid>> RestoreAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch(EndPoint+"/Activate/{Id}")]
    public Task<Result<Guid>> ActivateAsync(Guid Id, CancellationToken cancellationToken = default);
    [Patch(EndPoint+"/Maintenance/{Id}")]
    public Task<Result<Guid>> MaintenanceAsync(Guid Id, CancellationToken cancellationToken = default);
}

