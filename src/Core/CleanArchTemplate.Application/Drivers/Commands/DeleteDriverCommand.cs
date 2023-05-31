using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Shared.Wrapper;

using MediatR;
using OneOf;
using CleanArchTemplate.Domain.Drivers;

namespace CleanArchTemplate.Application.Drivers.Commands;
public record struct DeleteDriverCommand(Guid Id):IRequest<OneOf<Result,Exception>>;
public sealed class DeleteDriverCommandHandler : IRequestHandler<DeleteDriverCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public DeleteDriverCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result, Exception>> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = new DriverId(request.Id);
            var repo = _context.Set<Driver>();
            var found = await repo.FindAsync(id, cancellationToken);
            if (found is null) return new Exception($"Driver '({request.Id})' not found.");
            //if (found.Deleted) return new Exception($"Driver ({request.Id}) is alrady deleted.");
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
