using CleanArchTemplate.Shared.Responses.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;

namespace CleanArchTemplate.Application.Invoices.Queries;

public record GetInvoiceByIdQuery(Guid Id):IRequest<Result<InvoiceResponse>>;