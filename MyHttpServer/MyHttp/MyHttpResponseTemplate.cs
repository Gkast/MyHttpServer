using System.Net;

namespace MyHttpServer.MyHttp;

public static class MyHttpResponseTemplate
{
    public static MyHttpResponse Ok(string title, string? body = null)
    {
        const MyHttpStatus statusCode = MyHttpStatus.Ok;
        var headers = new Dictionary<HttpResponseHeader, string>
        {
            { HttpResponseHeader.ContentType, "text/html" }
        };
        var htmlBody = MyPageHtmlTemplate.HtmlResponse(title, body);

        return new MyHttpResponse(statusCode, headers, htmlBody);
    }

    public static MyHttpResponse Found(string location)
    {
        const MyHttpStatus statusCode = MyHttpStatus.Found;
        var headers = new Dictionary<HttpResponseHeader, string>
        {
            { HttpResponseHeader.Location, location }
        };
        return new MyHttpResponse(statusCode, headers, null);
    }

    public static MyHttpResponse Unauthorized(string title = "Unauthorized", string body = "<h1>Unauthorized</h1>")
    {
        const MyHttpStatus statusCode = MyHttpStatus.Unauthorized;
        var headers = new Dictionary<HttpResponseHeader, string>
        {
            { HttpResponseHeader.ContentType, "text/html" }
        };
        var htmlBody = MyPageHtmlTemplate.HtmlResponse(title, body);
        return new MyHttpResponse(statusCode, headers, htmlBody);
    }

    public static MyHttpResponse NotFound(string title = "Page Not Found", string body = "<h1>Page Not Found</h1>")
    {
        const MyHttpStatus statusCode = MyHttpStatus.NotFound;
        var headers = new Dictionary<HttpResponseHeader, string>
        {
            { HttpResponseHeader.ContentType, "text/html" }
        };
        var htmlBody = MyPageHtmlTemplate.HtmlResponse(title, body);

        return new MyHttpResponse(statusCode, headers, htmlBody);
    }
}