using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Vehicles.Brands.Commands;

public record struct CreateBrandCommand(string Name,string? Icon):IRequest<OneOf<Guid,Exception>>;
public sealed class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public CreateBrandCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Guid, Exception>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repo = _context.Set<Brand>();
            var existed = await repo.AnyAsync(x => x.Name == request.Name);
            if (existed) return new Exception($"There is an existing Brand with the  name: '{request.Name}' ");
            Brand create = null!;
            List<ValidationFailure> validationErrors = new();
            create=Brand.Create(request.Name,request.Icon);
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
