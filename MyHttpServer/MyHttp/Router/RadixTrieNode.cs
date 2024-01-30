using MyHttpServer.MyHttp.Handler;

namespace MyHttpServer.MyHttp.Router;

internal class RadixTrieNode
{
    private Dictionary<string, RadixTrieNode> Children { get; } = new();
    public Dictionary<string, IMyHttpHandler?> Handlers { get; } = new();

    public void AddChild(string key, RadixTrieNode child)
    {
        Children[key] = child;
    }

    public bool HasChild(string key)
    {
        return Children.ContainsKey(key);
    }

    public RadixTrieNode GetChild(string key)
    {
        return Children[key];
    }

    public void AddHandler(string httpMethod, IMyHttpHandler? handler)
    {
        Handlers[httpMethod] = handler;
    }

    public IMyHttpHandler? GetHandler(string httpMethod)
    {
        return Handlers.GetValueOrDefault(httpMethod, null);
    }
}