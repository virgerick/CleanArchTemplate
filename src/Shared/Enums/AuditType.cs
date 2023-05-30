namespace CleanArchTemplate.Shared.Enums;

public enum AuditType : byte
{
    None = 0,
    Create = 1,
    Update = 2,
    Delete = 3
}

public enum LoaderType : byte {
    Clock,
    Hamster,
    HoneyComb
}