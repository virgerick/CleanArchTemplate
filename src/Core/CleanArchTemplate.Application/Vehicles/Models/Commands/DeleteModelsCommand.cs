using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;

using MediatR;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Models.Commands;
public record struct DeleteModelsCommand(Guid Id):IRequest<OneOf<Result,Exception>>;
public sealed class DeleteModelsCommandHandler : IRequestHandler<DeleteModelsCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public DeleteModelsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result, Exception>> Handle(DeleteModelsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = new ModelId(request.Id);
            var repo = _context.Set<Model>();
            var found = await repo.FindAsync(id, cancellationToken);
            if (found is null) return new Exception($"Models '({request.Id})' not found.");
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
