using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Services;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchTemplate.Application.Services.Commands;

public record struct EditServiceCommand(Guid Id, string Name, Decimal Amount) : IRequest<OneOf<Guid, Exception>>;

public sealed class EditServiceCommandHandler : IRequestHandler<EditServiceCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;

    public EditServiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OneOf<Guid, Exception>> Handle(EditServiceCommand request, CancellationToken cancellationToken)
    {
        var repo = _context.Set<Service>();
        var serviceId = new ServiceId(request.Id);
        var found = await repo.SingleOrDefaultAsync(x => x.Id == serviceId, cancellationToken);
        if (found is null) return new Exception($"The Service ('{request.Id}') was not found.");
        found.Update(request.Name, request.Amount);
        await _context.SaveChangesAsync(cancellationToken);
        return found.Id.Value;
    }
}
