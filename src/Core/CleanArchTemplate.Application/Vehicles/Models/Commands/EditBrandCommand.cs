using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using OneOf;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Domain;
using System;

namespace CleanArchTemplate.Application.Vehicles.Models.Commands;
public record struct EditModelCommand(Guid Id, string Name, int Year, Guid BrandId, Guid TypeId) : IRequest<OneOf<Guid, Exception>>;
public sealed class EditModelCommandHandler : IRequestHandler<EditModelCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public EditModelCommandHandler(IApplicationDbContext context)
    {
        _context = context;

    }
    public async Task<OneOf<Guid, Exception>> Handle(EditModelCommand request, CancellationToken cancellationToken)
    {
        var repo = _context.Set<Model>();
        var ModelId = new ModelId(request.Id);
        var found = await repo
        .SingleOrDefaultAsync(x=>x.Id == ModelId, cancellationToken);
        if(found is null) return new Exception($"The Model ('{request.Id}') was not found.");
        var existingPlate =await repo.AnyAsync(x => x.Name == request.Name && x.Id != ModelId);
        if(existingPlate) return new Exception($"There is an existing Model with the  plateNumber: '{request.Name}'.");
        var brandId = new BrandId(request.BrandId);
        var typeId = new VehicleTypeId(request.TypeId);
        bool hasChange = found.Update(request.Name, request.Year, brandId, typeId);
        
        if(hasChange){
            await _context.SaveChangesAsync(cancellationToken);
        }

        return found.Id.Value;
    }
}
