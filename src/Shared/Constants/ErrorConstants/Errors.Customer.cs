using CleanArchTemplate.Shared.Results;

namespace CleanArchTemplate.Shared.Constants.ErrorConstants;

public partial class Errors
{
    public static class Customer
    {
        private const string Code = "CUSTOMER";
        private const string NotFoundMessage = "Customer '{0}' not found";
        public static Error NotFound(Guid id) =>Error.Create($"{Code}.{Errors.NotFoundCode}", string.Format(NotFoundMessage, id));

    }
}