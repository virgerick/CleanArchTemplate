using CleanArchTemplate.Shared.Requests.Invoices;
using CleanArchTemplate.Shared.Responses.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using Refit;

namespace CleanArchTemplate.Client.Services;

public interface IInvoiceApiService
{
    private const string EndPoint = "/Api/Invoices";
    [Post(EndPoint)]
    public Task<Result<Guid>> CreateAsync(CreateInvoiceRequest request, CancellationToken cancellationToken=default); 
    
    [Get(EndPoint)]
    public Task<ResultList<InvoiceResponse>> GetAsync(CancellationToken cancellationToken=default);
    
}