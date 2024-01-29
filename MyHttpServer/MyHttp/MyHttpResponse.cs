using System.Net;

namespace MyHttpServer.MyHttp;

public sealed class MyHttpResponse(
    MyHttpStatus statusCode,
    Dictionary<HttpResponseHeader, string> headers,
    string? body)
{
    public int StatusCode { get; } = statusCode.GetCode();
    public string StatusMessage { get; } = statusCode.GetMessage();
    public Dictionary<HttpResponseHeader, string> Headers { get; } = headers;
    public string? Body { get; } = body;
}