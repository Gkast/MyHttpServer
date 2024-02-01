using System.Net;
using MyHttpServer.HttpHandlers;
using MyHttpServer.MyHttp.Handler;
using MyHttpServer.MyHttp.Router;

namespace MyHttpServer.MyHttp;

public sealed class MyHttpServer
{
    private readonly HttpListener _httpListener = new();
    private readonly MyHttpRouter _router = new();

    public async Task Listen(string host = "localhost", int port = 7474)
    {
        var uriPrefix = $"http://{host}:{port}/";
        _httpListener.Prefixes.Add(uriPrefix);
        _httpListener.Start();
        while (_httpListener.IsListening)
        {
            var context = await _httpListener.GetContextAsync().ConfigureAwait(false);
            _ = MyHttpContextHandler.HandleContext(context, _router).ConfigureAwait(false);
        }
    }

    public void InitRoutes()
    {
        _router.Get("/", new Home());
        _router.Get("/test/:tre", new Test());
        _router.Post("/home", new Home());
        _router.Get("/about", new About());
        _router.Get("/assets/public/*", new StaticFileHandler());
        _router.Get("/robots.txt", new StaticFileHandler("robots.txt"));
        _router.Get("/sitemap.xml", new StaticFileHandler("sitemap.xml"));
    }

    public void Terminate()
    {
        _httpListener.Abort();
    }
}