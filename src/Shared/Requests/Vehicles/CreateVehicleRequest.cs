namespace CleanArchTemplate.Shared.Requests.Vehicles;
public record struct CreateEditVehicleRequest(string plateNumber, string brand, string model, string type);