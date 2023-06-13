using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Brands.Brands.Queries;
using Response = OneOf<Result<BrandResponse>, Exception>;
public record struct GetBrandByIdQuery(Guid Id):IRequest<Response>;
public sealed class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Response>
{
    private readonly IApplicationDbContext _context;

    public GetBrandByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var BrandId = new BrandId(request.Id);
            var found = await _context.Set<Brand>()
                .Where(x => x.Id == BrandId)
            .Select(x => new BrandResponse(x.Id.Value, x.Name,x.Logo))
            .FirstOrDefaultAsync(cancellationToken)!;
            if (found == null) return new Exception($"Brand ({request.Id}) was not found.");
            return Result.Success(found);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}