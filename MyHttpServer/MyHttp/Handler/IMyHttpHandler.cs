using System.Net;
using MyHttpServer.MyHttp.Response;

namespace MyHttpServer.MyHttp.Handler;

public interface IMyHttpHandler
{
    Func<HttpListenerRequest, Dictionary<string, string>?, Task<MyHttpResponse>> ResponseFunc { get; }
}