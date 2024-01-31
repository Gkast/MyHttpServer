using System.Net;
using MyHttpServer.HttpHandlers;
using MyHttpServer.MyHttp.Handler;
using MyHttpServer.MyHttp.Router;

namespace MyHttpServer.MyHttp;

public sealed class MyHttpServer
{
    private static readonly HttpListener HttpListener;
    private static readonly MyHttpRouter Router;

    static MyHttpServer()
    {
        HttpListener = new HttpListener();
        Router = new MyHttpRouter();
    }

    public MyHttpServer(string host = "localhost", int port = 7474)
    {
        var uriPrefix = $"http://{host}:{port}/";
        HttpListener.Prefixes.Add(uriPrefix);
    }

    public async Task Listen()
    {
        HttpListener.Start();
        while (HttpListener.IsListening)
        {
            var context = await HttpListener.GetContextAsync().ConfigureAwait(false);
            _ = MyHttpRequestHandler.HandleContext(context, Router).ConfigureAwait(false);
        }
    }

    public void InitRoutes()
    {
        Router.Get("/", new Home("vfdvf"));
        Router.Get("/test/:testy", new Home("bfdbdfbfdbf"));
        Router.Post("/home", new Home("jsdncsdunvisudiv"));
        Router.Get("/about", new About());
        Router.Get("/assets/public/*", new StaticFileHandler());
        // Router.Get("/assets/public/js/main.js", new StaticFileHandler());
        Router.Get("/robots.txt", new StaticFileHandler("robots.txt"));
        Router.Get("/sitemap.xml", new StaticFileHandler("sitemap.xml"));
    }

    public void Terminate()
    {
        HttpListener.Abort();
    }
}