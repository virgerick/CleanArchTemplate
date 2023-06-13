using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using OneOf;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain;

namespace CleanArchTemplate.Application.Vehicles.Brands.Commands;
public record struct EditBrandCommand(Guid Id, string Name,string? Logo) : IRequest<OneOf<Guid, Exception>>;
public sealed class EditBrandCommandHandler : IRequestHandler<EditBrandCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public EditBrandCommandHandler(IApplicationDbContext context)
    {
        _context = context;

    }
    public async Task<OneOf<Guid, Exception>> Handle(EditBrandCommand request, CancellationToken cancellationToken)
    {
        var repo = _context.Set<Brand>();
        var BrandId = new BrandId(request.Id);
        var found = await repo
        .SingleOrDefaultAsync(x=>x.Id == BrandId, cancellationToken);
        if(found is null) return new Exception($"The Brand ('{request.Id}') was not found.");
        var existingPlate =await repo.AnyAsync(x => x.Name == request.Name && x.Id != BrandId);
        if(existingPlate) return new Exception($"There is an existing Brand with the  plateNumber: '{request.Name}'.");
        List<ValidationFailure> validationErrors = new();
        bool hasChange = found.Update(request.Name,request.Logo);
        
        if(hasChange){
            await _context.SaveChangesAsync(cancellationToken);
        }

        return found.Id.Value;
    }
}
