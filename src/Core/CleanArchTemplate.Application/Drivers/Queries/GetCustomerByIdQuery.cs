using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Drivers;
using CleanArchTemplate.Shared.Responses.Drivers;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Drivers.Queries;

public record struct GetDriverByIdQuery(Guid Id):IRequest<OneOf<Result<DriverResponse>,Exception>>;
public sealed class GetDriverByIdQueryHandler : IRequestHandler<GetDriverByIdQuery, OneOf<Result<DriverResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetDriverByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result<DriverResponse>, Exception>> Handle(GetDriverByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var DriverId = new DriverId(request.Id);
            var found = await _context.Set<Driver>()
                .Where(x =>  x.Id == DriverId)
            .Select(x => x.Map())
            .FirstOrDefaultAsync(cancellationToken);
            if (found is null) return new Exception($"Driver ({request.Id}) was not found.");
            return Result<DriverResponse>.Success(found);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}