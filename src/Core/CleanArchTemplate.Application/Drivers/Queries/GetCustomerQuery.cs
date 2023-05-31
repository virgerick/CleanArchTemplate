using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Drivers;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Responses.Drivers;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Drivers.Queries;
public record struct GetDriverQuery():IRequest<OneOf<ResultList<DriverResponse>,Exception>>;
public sealed class GetDriverQueryHandler : IRequestHandler<GetDriverQuery, OneOf<ResultList<DriverResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetDriverQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<ResultList<DriverResponse>, Exception>> Handle(GetDriverQuery request, CancellationToken cancellationToken)
    {
        try
        {
           return await _context.Set<Driver>()
                .Select(x => x.Map())
                .ToResultListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}