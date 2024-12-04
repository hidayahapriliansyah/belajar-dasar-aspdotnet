using System.Net;

namespace jwtAuth.Exceptions;

public class ApiCustomException(string message, HttpStatusCode statusCode) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}
