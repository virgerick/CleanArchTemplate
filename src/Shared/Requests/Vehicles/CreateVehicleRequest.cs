namespace CleanArchTemplate.Shared.Requests.Vehicles;
public record struct CreateEditVehicleRequest(string plateNumber,  Guid modelId, string color);
public record struct CreateEditBrandRequest(string Name, string Logo);
public record struct CreateEditModelRequest(string Name, int Year, Guid BrandId, Guid TypeId);
