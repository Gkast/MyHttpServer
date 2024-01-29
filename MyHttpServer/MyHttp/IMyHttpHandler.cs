using System.Net;

namespace MyHttpServer.MyHttp;

public interface IMyHttpHandler
{
    public Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc { get; }
}