namespace CleanArchTemplate.Shared.Results;

public  class Error
{
    public static readonly Error None = new Error();
    public string Code { get; }
    public string Message { get; }
    private Error()
    {
        Code=string.Empty;
        Message=string.Empty;
    }
    protected Error(string code, string message)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }
    
    public void Deconstruct(out string code, out string message)
    {
        code = Code;
        message = Message;
    }

    public static Error Create(string code, string message)
    {
        return new Error(code, message);
    }
    public static Error Create( string message)
    {
        return new Error("Unknown", message);
    }

    public static implicit operator Error(Exception exception)
    {
        return Create(exception.Source!, exception.Message);
    }
   
}

public sealed class ValidationError : Error
{
    public void Deconstruct(out string code, out string message, out Dictionary<string, string[]> failures)
    {
        code = Code;
        message = Message;
        failures = Failures;
    }
    public Dictionary<string,string[]> Failures { get; }
    private ValidationError(string code, string message, Dictionary<string, string[]> failures) : base(code, message)
    {
        Failures = failures;
    }
    public static ValidationError Create(string code, string message, Dictionary<string, string[]> failures)
    {
        return new ValidationError(code, message, failures);
    }

    public static ValidationError Create(Dictionary<string, string[]> failures)
    {
        return  new("VALIDATION", "One or more validation has occurred", failures);
    }
}
