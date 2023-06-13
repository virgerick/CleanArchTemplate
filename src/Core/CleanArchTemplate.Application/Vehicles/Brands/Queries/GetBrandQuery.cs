using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using OneOf;
namespace CleanArchTemplate.Application.Vehicles.Brands.Queries;
using Response = OneOf<ResultList<BrandResponse>, Exception>;
public record struct GetBrandQuery():IRequest<Response>;
public sealed class GetBrandQueryHandler : IRequestHandler<GetBrandQuery, Response>
{
    private readonly IApplicationDbContext _context;

    public GetBrandQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Response> Handle(GetBrandQuery request, CancellationToken cancellationToken)
    {
        try
        {
           return await _context.Set<Brand>()
                .Select(x => new BrandResponse(x.Id.Value, x.Name,x.Logo))
                .ToResultListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}