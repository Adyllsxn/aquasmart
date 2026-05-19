namespace Aquasmart.Server.Source.Domain.Shared.Constants;

public static class HttpStatusCodes
{
    // 2xx - Success
    public const string OK = "200";
    public const string Created = "201";
    public const string Accepted = "202";
    public const string NoContent = "204";
    
    // 3xx - Redirection
    public const string MovedPermanently = "302";
    public const string Found = "302";
    public const string SeeOther = "303";
    public const string NotModified = "304";
    
    // 4xx - Client Errors
    public const string BadRequest = "400";
    public const string Unauthorized = "401";
    public const string Forbidden = "403";
    public const string NotFound = "404";
    public const string MethodNotAllowed = "405";
    public const string Conflict = "409";
    public const string TooManyRequests = "429";
    
    // 5xx - Server Errors
    public const string InternalServerError = "500";
    public const string NotImplemented = "501";
    public const string BadGateway = "502";
    public const string ServiceUnavailable = "503";
    public const string GatewayTimeout = "504";
}