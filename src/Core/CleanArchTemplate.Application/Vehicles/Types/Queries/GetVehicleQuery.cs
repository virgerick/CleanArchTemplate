using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using OneOf;
namespace CleanArchTemplate.Application.Vehicles.Types.Queries;
using Response = OneOf<ResultList<IdNameResponse<Guid>>, Exception>;
public record struct GetVehicleTypeQuery():IRequest<Response>;
public sealed class GetVehicleTypeQueryHandler : IRequestHandler<GetVehicleTypeQuery, Response>
{
    private readonly IApplicationDbContext _context;

    public GetVehicleTypeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Response> Handle(GetVehicleTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
           return await _context.Set<VehicleType>()
                .Select(x => new IdNameResponse<Guid>(x.Id.Value, x.Name))
                .ToResultListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}