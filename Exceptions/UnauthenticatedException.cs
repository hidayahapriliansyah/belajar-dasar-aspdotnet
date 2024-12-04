using System.Net;

namespace jwtAuth.Exceptions;

public class UnauthenticatedException(string message = "Unauthenticated")
    : ApiCustomException(message, HttpStatusCode.Unauthorized) { }
