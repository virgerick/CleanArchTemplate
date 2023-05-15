using System;
using System.Diagnostics.Contracts;

namespace CleanArchTemplate.UnitTets.Invoices;

public class InvoicesTest
{
	
}

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public ICollection<Service> Services { get; set; }
    public ICollection<Contract> Contracts { get; set; }
}
public enum VehicleType
{
    Bus,
    Van,
    Truck,
    Car
}
public class Vehicle
{
    public string Registration { get; set; } // Matrícula del vehículo
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int Capacity { get; set; }
    public VehicleType Type { get; set; }
    public ICollection<Service> Services { get; set; }
}

public class Driver
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string License { get; set; }
    public DateTime HireDate { get; set; }

    public ICollection<Service> Services { get; set; }
}

public class Route
{
    public int Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public float Distance { get; set; }
    public float EstimatedTime { get; set; }
    public ICollection<Service> Services { get; set; }
}

public class Service
{
    public int Id { get; set; }
    public DateTime ServiceDate { get; set; }
    public TimeSpan DepartureTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }

    public int DriverId { get; set; }
    public Driver Driver { get; set; }

    public int RouteId { get; set; }
    public Route Route { get; set; }

    public int? ContractId { get; set; }
    public Contract Contract { get; set; }

    public Invoice Invoice { get; set; }
}

public class Invoice
{
    public int Id { get; set; }
    public DateTime InvoiceDate { get; set; }
    public float TotalCharged { get; set; }
    public InvoiceStatusEntity Status { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; }
}
public class InvoiceLine
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Total { get; set; }
}
public enum InvoiceStatus
{
    Draft,
    Issued,
    Paid,
    Overdue,
    Cancelled
}

public record InvoiceStatusEntity
{
    public InvoiceStatus Status { get; init; }

    public InvoiceStatusEntity(InvoiceStatus status)
    {
        Status = status;
    }

    public bool IsDraft()
    {
        return Status == InvoiceStatus.Draft;
    }

    public bool IsIssued()
    {
        return Status == InvoiceStatus.Issued;
    }

    public bool IsPaid()
    {
        return Status == InvoiceStatus.Paid;
    }

    public bool IsOverdue()
    {
        return Status == InvoiceStatus.Overdue;
    }

    public bool IsCancelled()
    {
        return Status == InvoiceStatus.Cancelled;
    }

    // Aquí puedes agregar más comportamiento si es necesario...
}

public class Account
{
    public int Id { get; set; }
    public string AccountName { get; set; }
    public string AccountType { get; set; }
    public float Balance { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
}
public enum AccountType
{
    Asset,
    Liability,
    Equity,
    Revenue,
    Expense
}
public class AccountsReceivable
{
    public int Id { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; }

    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }

    public float Amount { get; set; }

    public ReceivableStatusEntity Status { get; set; } // e.g., pending, paid, overdue
}
public enum ReceivableStatus
{
    Pending,
    Paid,
    Overdue
}
public record ReceivableStatusEntity
{
    public ReceivableStatus Status { get; init; }

    public ReceivableStatusEntity(ReceivableStatus status)
    {
        Status = status;
    }

    public bool IsPaid()
    {
        return Status == ReceivableStatus.Paid;
    }

    public bool IsPending()
    {
        return Status == ReceivableStatus.Pending;
    }

    public bool IsOverdue()
    {
        return Status == ReceivableStatus.Overdue;
    }

    // Aquí puedes agregar más comportamiento si es necesario...
}
public class Transaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public float Amount { get; set; }

    public int OriginAccountId { get; set; }
    public Account OriginAccount { get; set; }

    public int DestinationAccountId { get; set; }
    public Account DestinationAccount { get; set; }
}
public class Contract
{
    public int Id { get; set; }
    public string ContractType { get; set; } // viaje, mensual
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public float Price { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    public ICollection<Service> Services { get; set; }
}
public enum ContractType
{
    Trip,
    Monthly
}