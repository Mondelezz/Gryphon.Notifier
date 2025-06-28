namespace Application.Exceptions;

public class AccessException : Exception
{
    public AccessException() { }

    public AccessException(string message) : base(message) { }
}
