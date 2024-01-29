using System.Net;
using System.Net.Http.Headers;

namespace MyHttpServer.MyHttp;

public static class HttpResponse
{
    public static HttpResponseObject CustomResponse(HttpStatusCodeType statusCode,
        Dictionary<HttpResponseHeader, string> headers, string? body)
    {
        return new HttpResponseObject(statusCode, headers, body);
    }

    public static HttpResponseObject Ok(string title, string? body = null)
    {
        const HttpStatusCodeType statusCode = HttpStatusCodeType.Ok;
        const string statusMessage = "Ok";
        var headers = new Dictionary<HttpResponseHeader, string>
        {
            { HttpResponseHeader.ContentType, "text/html" }
        };
        var htmlBody = HtmlTemplate.HtmlResponse(title, body);

        return new HttpResponseObject(statusCode, headers, htmlBody);
    }

    public static HttpResponseObject Found(string location)
    {
        const HttpStatusCodeType statusCode = HttpStatusCodeType.Found;
        var headers = new Dictionary<HttpResponseHeader, string>
        {
            { HttpResponseHeader.Location, location }
        };
        return new HttpResponseObject(statusCode, headers, null);
    }

    public static HttpResponseObject Unauthorized(string title = "Unauthorized", string body = "<h1>Unauthorized</h1>")
    {
        const HttpStatusCodeType statusCode = HttpStatusCodeType.Unauthorized;
        const string statusMessage = "Unauthorized";
        var headers = new Dictionary<HttpResponseHeader, string>
        {
            { HttpResponseHeader.ContentType, "text/html" }
        };
        var htmlBody = HtmlTemplate.HtmlResponse(title, body);
        return new HttpResponseObject(statusCode, headers, htmlBody);
    }

    public static HttpResponseObject NotFound(string title = "Page Not Found", string body = "<h1>Page Not Found</h1>")
    {
        const HttpStatusCodeType statusCode = HttpStatusCodeType.NotFound;
        var headers = new Dictionary<HttpResponseHeader, string>
        {
            { HttpResponseHeader.ContentType, "text/html" }
        };
        var htmlBody = HtmlTemplate.HtmlResponse(title, body);

        return new HttpResponseObject(statusCode, headers, htmlBody);
    }
}

public sealed class HttpResponseObject(
    HttpStatusCodeType statusCode,
    Dictionary<HttpResponseHeader, string> headers,
    string? body)
{
    public int StatusCode { get; } = statusCode.GetCode();
    public string StatusMessage { get; } = statusCode.GetMessage();
    public Dictionary<HttpResponseHeader, string> Headers { get; } = headers;
    public string? Body { get; } = body;
}