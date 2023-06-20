

using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Routes;
using CleanArchTemplate.Shared.Wrapper;
using MediatR;
using OneOf;

namespace CleanArchTemplate.Application.Routes.Commands;
public record struct DeleteRouteCommand(Guid Id):IRequest<OneOf<Result,Exception>>;
public sealed class DeleteRouteCommandHandler : IRequestHandler<DeleteRouteCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public DeleteRouteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result, Exception>> Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
    {
         try
        {
            var id = new RouteId(request.Id);
            var repo = _context.Set<Route>();
            var found = await repo.FindAsync(id, cancellationToken);
            if (found is null) return new Exception($"Route '({request.Id})' not found.");
            repo.Remove(found);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch(Exception ex)
        {
            
            return ex;
        }
    }
}
