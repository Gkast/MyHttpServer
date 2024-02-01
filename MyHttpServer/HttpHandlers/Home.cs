using System.Net;
using MyHttpServer.MyHttp.Handler;
using MyHttpServer.MyHttp.Response;

namespace MyHttpServer.HttpHandlers;

public sealed class Home : IMyHttpHandler
{
    public Func<HttpListenerRequest, Dictionary<string, string>?, Task<MyHttpResponse>> ResponseFunc { get; } =
        (request, pathVars) =>
        {
            const string htmlBody = "<p>Home Page</p>";
            return Task.FromResult(MyHttpResponseTemplate.Ok("Home", htmlBody));
        };
}