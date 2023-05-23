namespace CleanArchTemplate.Shared.Responses.Vehicles;
public record  VehicleResponse(
    Guid Id,
    string PlateNumber,
    string Brand,
    string Model,
    string Type,
    string Status
);