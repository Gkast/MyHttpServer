using System.Net;
using MyHttpServer.MyHttp;

namespace MyHttpServer.HttpHandlers;

public class Home : IMyHttpHandler
{
    public Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc { get; } = request =>
    {
        const string htmlBody = "<p>Home Page</p>";
        return Task.FromResult(MyHttpResponseTemplate.Ok("Home", htmlBody));
    };
}