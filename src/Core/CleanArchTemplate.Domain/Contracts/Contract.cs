using CleanArchTemplate.Domain.Common;
using CleanArchTemplate.Domain.Customers;
using CleanArchTemplate.Domain.Services;

namespace CleanArchTemplate.Domain.Contracts;
public record struct ContractId(Guid Value);
public class Contract : AuditableRootEntity<ContractId>
{
    private List<Service> _services=new();

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public ContractType Type { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public IEnumerable<Service> Services { get => _services; }

    protected Contract() { } // Constructor protegido para EF Core

    public static Contract Create(ContractId id, DateTime startDate, DateTime endDate, ContractType type, CustomerId clientId)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException("End date must be greater than start date.", nameof(endDate));
        }

        return new Contract { Id = id, StartDate = startDate, EndDate = endDate, Type = type, CustomerId = clientId };
    }
    public void ExtendContract(DateTime newEndDate)
    {
        if (newEndDate <= EndDate)
        {
            throw new ArgumentException("New end date must be greater than current end date.", nameof(newEndDate));
        }

        EndDate = newEndDate;
    }
}
