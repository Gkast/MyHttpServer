using System.Net;
using MyHttpServer.MyHttp;

namespace MyHttpServer.HttpHandlers;

public class About : IMyHttpHandler
{
    public Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc { get; } = request =>
    {
        const string htmlBody = "<p>About Page</p>";
        return Task.FromResult(MyHttpResponseTemplate.Ok("Home", htmlBody));
    };
}