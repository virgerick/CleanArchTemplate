using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Shared.Wrapper;

using MediatR;
using OneOf;
using CleanArchTemplate.Domain.Services;

namespace CleanArchTemplate.Application.Services.Commands;
public record struct DeleteServiceCommand(Guid Id):IRequest<OneOf<Result,Exception>>;
public sealed class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public DeleteServiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result, Exception>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = new ServiceId(request.Id);
            var repo = _context.Set<Service>();
            var found = await repo.FindAsync(id, cancellationToken);
            if (found is null) return new Exception($"Service '({request.Id})' not found.");
            //if (found.Deleted) return new Exception($"Service ({request.Id}) is alrady deleted.");
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
