using System;

namespace CleanArchTemplate.Shared.Requests.Services;

public record struct AddEditServiceRequest(
    string Name,
    decimal Amount
);
