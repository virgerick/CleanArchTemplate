using System;
namespace CleanArchTemplate.Shared.Extensions;

public static class ListStringExtension
{
	public static Exception ToException(this IEnumerable<string> errorMessage)
	{
		if(!errorMessage.Any())
		{
			return null!;
		}
        var message = errorMessage.First();
        var restante = errorMessage.Where(x => x != message).ToList();
        var exception = new Exception(message, restante.ToException());
        return exception;
    }
    public static IEnumerable<string> GetMessages(this Exception exception)
    {
        var messages = new List<string>();
        messages.Add(exception.Message);
        
        if (exception.InnerException != null)
            messages.AddRange(exception.InnerException.GetMessages());
        return messages;

    }
}
