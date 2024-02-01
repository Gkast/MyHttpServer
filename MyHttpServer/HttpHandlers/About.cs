using System.Net;
using MyHttpServer.MyHttp.Handler;
using MyHttpServer.MyHttp.Response;

namespace MyHttpServer.HttpHandlers;

public sealed class About : IMyHttpHandler
{
    public Func<HttpListenerRequest, Dictionary<string, string>?, Task<MyHttpResponse>> ResponseFunc { get; } =
        (_, pathVars) =>
        {
            const string htmlBody = "<p>About Page</p>";
            return Task.FromResult(MyHttpResponseTemplate.Ok("Home", htmlBody));
        };
}