using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Application.Invoices.Queries;

public record GetInvoiceQuery(Guid Id=default, string? Status=null!,DateTime From=default,DateTime To=default, bool ShowDeleted=false)
    :IRequest<ResultList<InvoiceResponse>>;

public sealed class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, ResultList<InvoiceResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetInvoiceQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<ResultList<InvoiceResponse>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        var (id, invoiceStatus, from, to, showDeleted) = request;
        var invoiceId =new InvoiceId(id);
        var status = invoiceStatus != null ? new InvoiceStatus(invoiceStatus):default;
        return ResultList<InvoiceResponse>
        .TryCatch(async () =>await _context.Set<Invoice>()
        .Get(invoiceId,status,from,to,showDeleted)
        .ProjectTo()
        .ToResultListAsync(cancellationToken));
    }

}