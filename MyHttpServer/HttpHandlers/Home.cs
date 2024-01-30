using System.Net;
using MyHttpServer.MyHttp;
using MyHttpServer.MyHttp.Handler;
using MyHttpServer.MyHttp.Response;

namespace MyHttpServer.HttpHandlers;

public class Home(string pe) : IMyHttpHandler
{
    public Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc { get; } = request =>
    {
        var htmlBody = "<p>Home Page</p>" + pe;
        return Task.FromResult(MyHttpResponseTemplate.Ok("Home", htmlBody));
    };
}