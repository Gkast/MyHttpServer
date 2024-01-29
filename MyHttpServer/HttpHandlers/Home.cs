using System.Net;
using MyHttpServer.MyHttp;

namespace MyHttpServer.HttpHandlers;

public static class Home
{
    public static Task<HttpResponseObject> Handle(HttpListenerRequest request)
    {
        const string htmlBody = "<p>Home</p>";
        return Task.FromResult(HttpResponse.Ok("Home", htmlBody));
    }
}