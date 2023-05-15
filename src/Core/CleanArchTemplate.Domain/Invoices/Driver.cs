namespace CleanArchTemplate.Domain.Invoices;

public class Driver
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string License { get; set; }
    public DateTime HireDate { get; set; }

    public ICollection<Service> Services { get; set; }
}
