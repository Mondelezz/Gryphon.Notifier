namespace Application.Exceptions;

public class NullReferenceException : Exception
{
    public NullReferenceException() { }

    public NullReferenceException(string message) : base(message) { }
}
