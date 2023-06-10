namespace CleanArchTemplate.Shared.Requests.Vehicles;
public record struct CreateEditVehicleRequest(string plateNumber,  Guid modelId, string color);