using System.Net;
using MyHttpServer.MyHttp;
using MyHttpServer.MyHttp.Handler;
using MyHttpServer.MyHttp.Response;

namespace MyHttpServer.HttpHandlers;

public class About : IMyHttpHandler
{
    public Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc { get; } = _ =>
    {
        const string htmlBody = "<p>About Page</p>";
        return Task.FromResult(MyHttpResponseTemplate.Ok("Home", htmlBody));
    };
}