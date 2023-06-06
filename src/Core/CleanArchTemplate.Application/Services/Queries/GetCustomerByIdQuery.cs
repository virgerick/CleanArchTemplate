using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Application.Mapping;
using CleanArchTemplate.Domain.Services;
using CleanArchTemplate.Shared.Responses.Services;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Services.Queries;

public record struct GetServiceByIdQuery(Guid Id):IRequest<OneOf<Result<ServiceResponse>,Exception>>;
public sealed class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, OneOf<Result<ServiceResponse>, Exception>>
{
    private readonly IApplicationDbContext _context;

    public GetServiceByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result<ServiceResponse>, Exception>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ServiceId = new ServiceId(request.Id);
            var found = await _context.Set<Service>()
                .Where(x =>  x.Id == ServiceId)
            .Select(x => x.Map())
            .FirstOrDefaultAsync(cancellationToken);
            if (found is null) return new Exception($"Service ({request.Id}) was not found.");
            return Result<ServiceResponse>.Success(found);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}