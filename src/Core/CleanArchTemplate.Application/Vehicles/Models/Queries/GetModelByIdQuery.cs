using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Shared.Responses.Vehicles;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Models.Queries;
using Response = OneOf<Result<ModelResponse>, Exception>;
public record struct GetModelByIdQuery(Guid Id):IRequest<Response>;
public sealed class GetModelByIdQueryHandler : IRequestHandler<GetModelByIdQuery, Response>
{
    private readonly IApplicationDbContext _context;

    public GetModelByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(GetModelByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ModelId = new ModelId(request.Id);
            var found = await _context.Set<Model>()
                .Where(x => x.Id == ModelId)
            .Select(x => new ModelResponse(x.Id.Value, x.Name,x.Year,x.BrandId.Value,x.TypeId.Value))
            .FirstOrDefaultAsync(cancellationToken)!;
            if (found.Equals(default)) return new Exception($"Model ({request.Id}) was not found.");
            return Result.Success(found);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}