using System.ComponentModel;
using System.Xml.Linq;
using CleanArchTemplate.Application.Common.Interfaces;
using CleanArchTemplate.Domain.Services;
using CleanArchTemplate.Domain.Invoices;
using CleanArchTemplate.Shared.Wrapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using CleanArchTemplate.Domain.Routes;

namespace CleanArchTemplate.Application.Services.Commands;

public record struct CreateServiceCommand(string Name, decimal Amount, DateTime Date, Guid? RouteId) : IRequest<OneOf<Guid,Exception>>;
public sealed class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, OneOf<Guid, Exception>>
{
    private readonly IApplicationDbContext _context;
    public CreateServiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<OneOf<Guid, Exception>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var repo = _context.Set<Service>();
            if (repo.Any(x => x.Name == request.Name))
                return new Exception($"There is an existing Service named: '{request.Name}' ");
            Service create = null!;
            RouteId routeId = request.RouteId!=null? new RouteId(request.RouteId.Value) : new();
            Service.Create(request.Name,request.Amount,request.Date, routeId)
                .Switch(
                service=>create=service,
                error=> throw error
                );

            repo.Add(create!);
            await _context.SaveChangesAsync(cancellationToken);
            return create!.Id.Value;
        }
        catch (Exception ex)
        {
            return ex;
        }

    }
}
