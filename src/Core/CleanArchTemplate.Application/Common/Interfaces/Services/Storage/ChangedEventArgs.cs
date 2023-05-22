using System.Diagnostics.CodeAnalysis;

namespace CleanArchTemplate.Application.Common.Interfaces.Services.Storage
{
    [ExcludeFromCodeCoverage]
    public class ChangedEventArgs
    {
       
        public required string Key { get; set; }
        public  object? OldValue { get; set; }
        public required object NewValue { get; set; }
    }
}