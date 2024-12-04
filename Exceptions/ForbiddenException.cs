using System.Net;

namespace jwtAuth.Exceptions;

public class ForbiddenException(string message = "Access is forbidden")
    : ApiCustomException(message, HttpStatusCode.Forbidden) { }
