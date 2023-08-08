using CleanArchTemplate.Shared.Requests.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IInvoiceApiService
{
    private const string Controller = "/Invoice";
    [Post($"{Controller}/")]
    public Task<Result<Guid>> CreateAsync(CreateInvoiceRequest request, CancellationToken cancellationToken=default);
}