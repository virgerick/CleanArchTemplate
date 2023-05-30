namespace CleanArchTemplate.Shared.Enums;

public enum OperationType : byte
{
    None = 0,
    Create = 1,
    Update = 2,
    Delete = 3,
    
}
public enum OperationStatus : byte
{
    None = 0,
    Success = 1,
    Failure = 2,
    Loading = 3,
    
}