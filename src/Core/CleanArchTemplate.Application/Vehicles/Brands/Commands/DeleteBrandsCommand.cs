using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;

using MediatR;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Brands.Commands;
public record struct DeleteBrandsCommand(Guid Id):IRequest<OneOf<Result,Exception>>;
public sealed class DeleteBrandsCommandHandler : IRequestHandler<DeleteBrandsCommand, OneOf<Result, Exception>>
{
    private readonly IApplicationDbContext _context;

    public DeleteBrandsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Result, Exception>> Handle(DeleteBrandsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = new BrandId(request.Id);
            var repo = _context.Set<Brand>();
            var found = await repo.FindAsync(id, cancellationToken);
            if (found is null) return new Exception($"Brands '({request.Id})' not found.");
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
