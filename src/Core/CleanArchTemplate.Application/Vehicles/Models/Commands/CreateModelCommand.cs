using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Models.Commands;

public record struct CreateModelCommand(string Name, int Year, Guid BrandId, Guid TypeId) : IRequest<OneOf<Guid,Exception>>;
public sealed class CreateModelCommandHandler : IRequestHandler<CreateModelCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public CreateModelCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Guid, Exception>> Handle(CreateModelCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repo = _context.Set<Model>();
            var existed = await repo.AnyAsync(x => x.Name == request.Name);
            if (existed) return new Exception($"There is an existing Model with the  name: '{request.Name}' ");
            Model create = null!;
            List<ValidationFailure> validationErrors = new();
            var brandId = new BrandId(request.BrandId);
            var typeId = new VehicleTypeId(request.TypeId);
            create=Model.Create(request.Name,request.Year,brandId,typeId);
            repo.Add(create);
            await _context.SaveChangesAsync(cancellationToken);
            return create.Id.Value;
        }
        catch (System.Exception ex)
        {
            return ex;
        }

    }
}
