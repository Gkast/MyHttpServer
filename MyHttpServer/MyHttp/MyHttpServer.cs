using System.Net;
using MyHttpServer.HttpHandlers;

namespace MyHttpServer.MyHttp;

public sealed class MyHttpServer
{
    private static readonly HttpListener HttpListener;

    private static readonly Dictionary<string, Dictionary<string, IMyHttpHandler>> routes;
    // private static readonly MyHttpRouter MyHttpRouter;

    static MyHttpServer()
    {
        HttpListener = new HttpListener();
        // MyHttpRouter = new MyHttpRouter();
        routes = new Dictionary<string, Dictionary<string, IMyHttpHandler>>
        {
            { "/", new Dictionary<string, IMyHttpHandler> { { "GET", new Home() } } },
            { "/home", new Dictionary<string, IMyHttpHandler> { { "GET", new Home() } } },
            { "/about", new Dictionary<string, IMyHttpHandler> { { "GET", new About() } } }
        };
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
            _ = MyHttpRequestHandler.HandleContext(context, routes).ConfigureAwait(false);
        }
    }

    public void Terminate()
    {
        HttpListener.Abort();
    }
}