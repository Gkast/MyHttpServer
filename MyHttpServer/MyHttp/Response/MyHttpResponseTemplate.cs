using System.Net;
using MyHttpServer.MyHttp.Utilities;

namespace MyHttpServer.MyHttp.Response;

public static class MyHttpResponseTemplate
{
    public static MyHttpResponse Ok(string title, string? body = null)
    {
        const MyHttpStatus statusCode = MyHttpStatus.Ok;
        var headers = new Dictionary<object, string>
        {
            { HttpResponseHeader.ContentType, MyMimeTypes.GetMimeType("html") }
        };
        var htmlBody = MyPageHtmlTemplate.HtmlResponse(title, body);

        return new MyHttpResponse(statusCode, headers, htmlBody);
    }

    public static MyHttpResponse Found(string location)
    {
        const MyHttpStatus statusCode = MyHttpStatus.Found;
        var headers = new Dictionary<object, string>
        {
            { HttpResponseHeader.Location, location }
        };
        return new MyHttpResponse(statusCode, headers);
    }

    public static MyHttpResponse Unauthorized(string title = "Unauthorized", string body = "<h1>Unauthorized</h1>")
    {
        const MyHttpStatus statusCode = MyHttpStatus.Unauthorized;
        var headers = new Dictionary<object, string>
        {
            { HttpResponseHeader.ContentType, MyMimeTypes.GetMimeType("html") }
        };
        var htmlBody = MyPageHtmlTemplate.HtmlResponse(title, body);
        return new MyHttpResponse(statusCode, headers, htmlBody);
    }

    public static MyHttpResponse NotFound(string title = "Page Not Found", string body = "<h1>Page Not Found</h1>")
    {
        const MyHttpStatus statusCode = MyHttpStatus.NotFound;
        var headers = new Dictionary<object, string>
        {
            { HttpResponseHeader.ContentType, MyMimeTypes.GetMimeType("html") }
        };
        var htmlBody = MyPageHtmlTemplate.HtmlResponse(title, body);

        return new MyHttpResponse(statusCode, headers, htmlBody);
    }
}