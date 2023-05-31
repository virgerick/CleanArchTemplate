using System;
namespace CleanArchTemplate.Shared.Requests.Drivers;

public record struct AddEditDriverRequest(string Name, string PhoneNumber, string License, DateTime HireDate);


