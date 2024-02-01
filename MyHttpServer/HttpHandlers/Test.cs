using System.Net;
using MyHttpServer.MyHttp.Handler;
using MyHttpServer.MyHttp.Response;

namespace MyHttpServer.HttpHandlers;

public sealed class Test : IMyHttpHandler
{
    public Func<HttpListenerRequest, Dictionary<string, string>?, Task<MyHttpResponse>> ResponseFunc { get; } =
        (_, pathVars) =>
        {
            var pathVarsToString = "";
            if (pathVars != null && pathVars.Count != 0)
                pathVarsToString = string.Join(" ", pathVars.Select(pathVar => $"{pathVar.Key} : {pathVar.Value}"));

            var htmlBody = $"<p>Test Page {pathVarsToString}</p>";
            return Task.FromResult(MyHttpResponseTemplate.Ok("Test", htmlBody));
        };
}