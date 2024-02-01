using System.Net;
using MyHttpServer.MyHttp.Utilities;

namespace MyHttpServer.MyHttp.Response;

public sealed class MyHttpResponse : IMyHttpResponse
{
    public MyHttpResponse(MyHttpStatus statusCode, Dictionary<object, string> headers, string? body = null)
    {
        StatusCode = statusCode.GetCode();
        StatusMessage = statusCode.GetMessage();
        Headers = headers;
        Body = body;
    }

    public MyHttpResponse(MyHttpStatus statusCode, Dictionary<object, string> headers,
        Func<HttpListenerResponse, Task> bodyFunc)
    {
        StatusCode = statusCode.GetCode();
        StatusMessage = statusCode.GetMessage();
        Headers = headers;
        Body = bodyFunc ?? throw new ArgumentNullException(nameof(bodyFunc));
    }

    public int StatusCode { get; }
    public string StatusMessage { get; }
    public Dictionary<object, string> Headers { get; }
    public object? Body { get; }
}