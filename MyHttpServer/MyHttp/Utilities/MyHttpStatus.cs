namespace MyHttpServer.MyHttp.Utilities;

public enum MyHttpStatus
{
    [MyHttpStatus(100, "Continue")] Continue,

    [MyHttpStatus(101, "Switching Protocols")]
    SwitchingProtocols,
    [MyHttpStatus(102, "Processing")] Processing,
    [MyHttpStatus(200, "OK")] Ok,
    [MyHttpStatus(201, "Created")] Created,
    [MyHttpStatus(202, "Accepted")] Accepted,

    [MyHttpStatus(203, "Non-Authoritative Information")]
    NonAuthoritativeInformation,
    [MyHttpStatus(204, "NoContent")] NoContent,
    [MyHttpStatus(205, "Reset Content")] ResetContent,
    [MyHttpStatus(206, "Partial Content")] PartialContent,
    [MyHttpStatus(207, "Multi-Status")] MultiStatus,

    [MyHttpStatus(208, "Already Reported")]
    AlreadyReported,
    [MyHttpStatus(226, "IM Used")] ImUsed,

    [MyHttpStatus(300, "Multiple Choices")]
    MultipleChoices,

    [MyHttpStatus(301, "Moved Permanently")]
    MovedPermanently,
    [MyHttpStatus(302, "Found")] Found,
    [MyHttpStatus(303, "See Other")] SeeOther,
    [MyHttpStatus(304, "Not Modified")] NotModified,
    [MyHttpStatus(306, "unused")] Unused,

    [MyHttpStatus(307, "Temporary Redirect")]
    TemporaryRedirect,

    [MyHttpStatus(308, "Permanent Redirect")]
    PermanentRedirect,
    [MyHttpStatus(400, "Bad Request")] BadRequest,
    [MyHttpStatus(401, "Unauthorized")] Unauthorized,
    [MyHttpStatus(403, "Forbidden")] Forbidden,
    [MyHttpStatus(404, "Not Found")] NotFound,

    [MyHttpStatus(405, "Method Not Allowed")]
    MethodNotAllowed,
    [MyHttpStatus(406, "Not Acceptable")] NotAcceptable,

    [MyHttpStatus(407, "Proxy Authentication Required")]
    ProxyAuthenticationRequired,
    [MyHttpStatus(408, "Request Timeout")] RequestTimeout,
    [MyHttpStatus(409, "Conflict")] Conflict,
    [MyHttpStatus(410, "Gone")] Gone,
    [MyHttpStatus(411, "Length Required")] LengthRequired,

    [MyHttpStatus(412, "Precondition Failed")]
    PreconditionFailed,

    [MyHttpStatus(413, "Payload Too Large")]
    PayloadTooLarge,
    [MyHttpStatus(414, "URI Too Long")] UriTooLong,

    [MyHttpStatus(415, "Unsupported Media Type")]
    UnsupportedMediaType,

    [MyHttpStatus(416, "Range Not Satisfiable")]
    RangeNotSatisfiable,

    [MyHttpStatus(417, "Expectation Failed")]
    ExpectationFailed,
    [MyHttpStatus(418, "I\\'m a teapot")] ImATeapot,

    [MyHttpStatus(421, "Misdirected Request")]
    MisdirectedRequest,

    [MyHttpStatus(422, "Unprocessable Content")]
    UnprocessableContent,
    [MyHttpStatus(423, "Locked")] Locked,

    [MyHttpStatus(424, "Failed Dependency")]
    FailedDependency,

    [MyHttpStatus(426, "Upgrade Required")]
    UpgradeRequired,

    [MyHttpStatus(428, "Precondition Required")]
    PreconditionRequired,

    [MyHttpStatus(429, "Too Many Requests")]
    TooManyRequests,

    [MyHttpStatus(431, "Request Header Fields Too Large")]
    RequestHeaderFieldsTooLarge,

    [MyHttpStatus(451, "Unavailable For Legal Reasons")]
    UnavailableForLegalReasons,

    [MyHttpStatus(500, "Internal Server Error")]
    InternalServerError,
    [MyHttpStatus(501, "Not Implemented")] NotImplemented,
    [MyHttpStatus(502, "Bad Gateway")] BadGateway,

    [MyHttpStatus(503, "Service Unavailable")]
    ServiceUnavailable,
    [MyHttpStatus(504, "Gateway Timeout")] GatewayTimeout,

    [MyHttpStatus(505, "HTTP Version Not Supported")]
    HttpVersionNotSupported,

    [MyHttpStatus(506, "Variant Also Negotiates")]
    VariantAlsoNegotiates,

    [MyHttpStatus(507, "Insufficient Storage")]
    InsufficientStorage,
    [MyHttpStatus(508, "Loop Detected")] LoopDetected,
    [MyHttpStatus(510, "Not Extended")] NotExtended,

    [MyHttpStatus(511, "Network Authentication Required")]
    NetworkAuthenticationRequired
}

[AttributeUsage(AttributeTargets.Field)]
internal sealed class MyHttpStatusAttribute(int code, string message) : Attribute
{
    public int Code { get; } = code;
    public string Message { get; } = message;
}

public static class MyHttpStatusExtensions
{
    public static int GetCode(this MyHttpStatus status)
    {
        var fieldInfo = status.GetType().GetField(status.ToString());
        if (fieldInfo is null) return 200;
        var attribute = (MyHttpStatusAttribute?)Attribute.GetCustomAttribute(fieldInfo, typeof(MyHttpStatusAttribute));
        return attribute?.Code ?? 200;
    }

    public static string GetMessage(this MyHttpStatus status)
    {
        var fieldInfo = status.GetType().GetField(status.ToString());
        if (fieldInfo is null) return "OK";
        var attribute = (MyHttpStatusAttribute?)Attribute.GetCustomAttribute(fieldInfo, typeof(MyHttpStatusAttribute));
        return attribute?.Message ?? "OK";
    }
}