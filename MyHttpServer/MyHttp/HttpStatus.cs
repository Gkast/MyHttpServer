namespace MyHttpServer.MyHttp;

public enum HttpStatusCodeType
{
    [HttpStatus(100, "Continue")] Continue,

    [HttpStatus(101, "Switching Protocols")]
    SwitchingProtocols,
    [HttpStatus(102, "Processing")] Processing,
    [HttpStatus(200, "OK")] Ok,
    [HttpStatus(201, "Created")] Created,
    [HttpStatus(202, "Accepted")] Accepted,

    [HttpStatus(203, "Non-Authoritative Information")]
    NonAuthoritativeInformation,
    [HttpStatus(204, "NoContent")] NoContent,
    [HttpStatus(205, "Reset Content")] ResetContent,
    [HttpStatus(206, "Partial Content")] PartialContent,
    [HttpStatus(207, "Multi-Status")] MultiStatus,
    [HttpStatus(208, "Already Reported")] AlreadyReported,
    [HttpStatus(226, "IM Used")] ImUsed,
    [HttpStatus(300, "Multiple Choices")] MultipleChoices,
    [HttpStatus(301, "Moved Permanently")] MovedPermanently,
    [HttpStatus(302, "Found")] Found,
    [HttpStatus(303, "See Other")] SeeOther,
    [HttpStatus(304, "Not Modified")] NotModified,
    [HttpStatus(306, "unused")] Unused,

    [HttpStatus(307, "Temporary Redirect")]
    TemporaryRedirect,

    [HttpStatus(308, "Permanent Redirect")]
    PermanentRedirect,
    [HttpStatus(400, "Bad Request")] BadRequest,
    [HttpStatus(401, "Unauthorized")] Unauthorized,
    [HttpStatus(403, "Forbidden")] Forbidden,
    [HttpStatus(404, "Not Found")] NotFound,

    [HttpStatus(405, "Method Not Allowed")]
    MethodNotAllowed,
    [HttpStatus(406, "Not Acceptable")] NotAcceptable,

    [HttpStatus(407, "Proxy Authentication Required")]
    ProxyAuthenticationRequired,
    [HttpStatus(408, "Request Timeout")] RequestTimeout,
    [HttpStatus(409, "Conflict")] Conflict,
    [HttpStatus(410, "Gone")] Gone,
    [HttpStatus(411, "Length Required")] LengthRequired,

    [HttpStatus(412, "Precondition Failed")]
    PreconditionFailed,
    [HttpStatus(413, "Payload Too Large")] PayloadTooLarge,
    [HttpStatus(414, "URI Too Long")] UriTooLong,

    [HttpStatus(415, "Unsupported Media Type")]
    UnsupportedMediaType,

    [HttpStatus(416, "Range Not Satisfiable")]
    RangeNotSatisfiable,

    [HttpStatus(417, "Expectation Failed")]
    ExpectationFailed,
    [HttpStatus(418, "I\\'m a teapot")] ImATeapot,

    [HttpStatus(421, "Misdirected Request")]
    MisdirectedRequest,

    [HttpStatus(422, "Unprocessable Content")]
    UnprocessableContent,
    [HttpStatus(423, "Locked")] Locked,
    [HttpStatus(424, "Failed Dependency")] FailedDependency,
    [HttpStatus(426, "Upgrade Required")] UpgradeRequired,

    [HttpStatus(428, "Precondition Required")]
    PreconditionRequired,
    [HttpStatus(429, "Too Many Requests")] TooManyRequests,

    [HttpStatus(431, "Request Header Fields Too Large")]
    RequestHeaderFieldsTooLarge,

    [HttpStatus(451, "Unavailable For Legal Reasons")]
    UnavailableForLegalReasons,

    [HttpStatus(500, "Internal Server Error")]
    InternalServerError,
    [HttpStatus(501, "Not Implemented")] NotImplemented,
    [HttpStatus(502, "Bad Gateway")] BadGateway,

    [HttpStatus(503, "Service Unavailable")]
    ServiceUnavailable,
    [HttpStatus(504, "Gateway Timeout")] GatewayTimeout,

    [HttpStatus(505, "HTTP Version Not Supported")]
    HttpVersionNotSupported,

    [HttpStatus(506, "Variant Also Negotiates")]
    VariantAlsoNegotiates,

    [HttpStatus(507, "Insufficient Storage")]
    InsufficientStorage,
    [HttpStatus(508, "Loop Detected")] LoopDetected,
    [HttpStatus(510, "Not Extended")] NotExtended,

    [HttpStatus(511, "Network Authentication Required")]
    NetworkAuthenticationRequired
}

[AttributeUsage(AttributeTargets.Field)]
internal sealed class HttpStatusAttribute(int code, string message) : Attribute
{
    public int Code { get; } = code;
    public string Message { get; } = message;
}

public static class HttpStatusExtensions
{
    public static int GetCode(this HttpStatusCodeType status)
    {
        var fieldInfo = status.GetType().GetField(status.ToString());
        if (fieldInfo is null) return 200;
        var attribute = (HttpStatusAttribute?)Attribute.GetCustomAttribute(fieldInfo, typeof(HttpStatusAttribute));
        return attribute?.Code ?? 200;
    }

    public static string GetMessage(this HttpStatusCodeType status)
    {
        var fieldInfo = status.GetType().GetField(status.ToString());
        if (fieldInfo is null) return "OK";
        var attribute = (HttpStatusAttribute?)Attribute.GetCustomAttribute(fieldInfo, typeof(HttpStatusAttribute));
        return attribute?.Message ?? "OK";
    }
}