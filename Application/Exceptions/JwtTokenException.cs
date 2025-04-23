namespace Application.Exceptions;

public class JwtTokenException : Exception
{
    public JwtTokenException() { }

    public JwtTokenException(string message) : base(message) { }
}
