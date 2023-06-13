using System;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Models.Queries;
using Response = OneOf<Result<ModelDefaultResponse>, Exception>;
public record struct GetModelDefaultQuery:IRequest<Response>;
public sealed class GetModelDefaultQueryHandler : IRequestHandler<GetModelDefaultQuery, Response>
{
    private readonly IApplicationDbContext _context;

    public GetModelDefaultQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Response> Handle(GetModelDefaultQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var models = await _context.Set<Model>().Select(x => x.Map()).ToListAsync(cancellationToken);
            var brands= await _context.Set<Brand>().Select(x => x.Map()).ToListAsync(cancellationToken);
            var vehicleTypes= await _context.Set<VehicleType>().Select(x => x.Map()).ToListAsync(cancellationToken);
            var response = new ModelDefaultResponse(models.ToArray(), brands.ToArray(), vehicleTypes.ToArray());
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}

