using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using OneOf;
namespace CleanArchTemplate.Application.Vehicles.Models.Queries;
using Response = OneOf<ResultList<ModelResponse>, Exception>;
public record struct GetModelQuery():IRequest<Response>;
public sealed class GetModelQueryHandler : IRequestHandler<GetModelQuery, Response>
{
    private readonly IApplicationDbContext _context;

    public GetModelQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Response> Handle(GetModelQuery request, CancellationToken cancellationToken)
    {
        try
        {
           return await _context.Set<Model>()
                .Select(x => x.Map())
                .ToResultListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}