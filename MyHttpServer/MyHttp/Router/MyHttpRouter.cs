using MyHttpServer.MyHttp.Handler;

namespace MyHttpServer.MyHttp.Router;

public sealed class MyHttpRouter
{
    private readonly RadixTrieHttpRouter _router = new();

    public (IMyHttpHandler? handler, MyRouteStatus status, Dictionary<string, string>? pathVariables) Find(
        string httpMethod,
        string path)
    {
        return _router.FindRoute(httpMethod.ToUpper(), path.Trim());
    }

    public void Get(string path, IMyHttpHandler handler)
    {
        _router.AddRoute("GET", path, handler);
    }

    public void Post(string path, IMyHttpHandler handler)
    {
        _router.AddRoute("POST", path, handler);
    }

    public void Put(string path, IMyHttpHandler handler)
    {
        _router.AddRoute("PUT", path, handler);
    }

    public void Delete(string path, IMyHttpHandler handler)
    {
        _router.AddRoute("DELETE", path, handler);
    }

    public void Patch(string path, IMyHttpHandler handler)
    {
        _router.AddRoute("PATCH", path, handler);
    }

    public void Head(string path, IMyHttpHandler handler)
    {
        _router.AddRoute("HEAD", path, handler);
    }

    public void Connect(string path, IMyHttpHandler handler)
    {
        _router.AddRoute("CONNECT", path, handler);
    }

    public void Options(string path, IMyHttpHandler handler)
    {
        _router.AddRoute("OPTIONS", path, handler);
    }

    public void Trace(string path, IMyHttpHandler handler)
    {
        _router.AddRoute("TRACE", path, handler);
    }
}