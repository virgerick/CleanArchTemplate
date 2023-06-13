using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Types.Queries;
using Response = OneOf<Result<IdNameResponse<Guid>>, Exception>;
public record struct GetVehicleTypeByIdQuery(Guid Id):IRequest<Response>;
public sealed class GetVehicleTypeByIdQueryHandler : IRequestHandler<GetVehicleTypeByIdQuery, Response>
{
    private readonly IApplicationDbContext _context;

    public GetVehicleTypeByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(GetVehicleTypeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var VehicleTypeId = new VehicleTypeId(request.Id);
            var found = await _context.Set<VehicleType>()
                .Where(x => x.Id == VehicleTypeId)
            .Select(x => new IdNameResponse<Guid>(x.Id.Value, x.Name))
            .FirstOrDefaultAsync(cancellationToken);
            if (found == default) return new Exception($"VehicleType ({request.Id}) was not found.");
            return Result<IdNameResponse<Guid>>.Success(found);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}