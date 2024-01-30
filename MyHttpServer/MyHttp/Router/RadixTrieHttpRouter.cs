using MyHttpServer.MyHttp.Handler;

namespace MyHttpServer.MyHttp.Router;

public class RadixTrieHttpRouter
{
    private readonly RadixTrieNode _root = new();

    public void AddRoute(string httpMethod, string path, IMyHttpHandler handler)
    {
        AddRouteRecursive(_root, httpMethod, path.Split('/'), handler);
    }

    // Modified FindRouteRecursive to return a tuple indicating the handler and the status
    public (IMyHttpHandler? handler, RouteStatus status) FindRoute(string httpMethod, string path)
    {
        return FindRouteRecursive(_root, httpMethod, path.Split('/'));
    }

    private static void AddRouteRecursive(RadixTrieNode node, string httpMethod, IReadOnlyList<string> pathParts,
        IMyHttpHandler? handler)
    {
        if (pathParts.Count == 0)
        {
            // Reached the end of the path, assign the handler to the current node
            node.AddHandler(httpMethod, handler);
            return;
        }

        var currentPart = pathParts[0];
        var remainingParts = pathParts.Skip(1).ToArray();

        if (node.HasChild(currentPart))
        {
            // If a child with the current part already exists, continue down the trie
            AddRouteRecursive(node.GetChild(currentPart), httpMethod, remainingParts, handler);
        }
        else
        {
            // Create a new node for the current part and continue down the trie
            var newNode = new RadixTrieNode();
            node.AddChild(currentPart, newNode);
            AddRouteRecursive(newNode, httpMethod, remainingParts, handler);
        }
    }

    // Modified FindRouteRecursive to return a tuple indicating the handler and the status
    private static (IMyHttpHandler? handler, RouteStatus status) FindRouteRecursive(RadixTrieNode node,
        string httpMethod, IReadOnlyList<string> pathParts)
    {
        if (pathParts.Count == 0)
        {
            // Reached the end of the path
            var handler = node.GetHandler(httpMethod);

            if (handler != null)
            {
                // Valid path and valid HTTP method
                return (handler, RouteStatus.Valid);
            }

            if (node.Handlers.Count != 0)
            {
                // Valid path but invalid HTTP method
                return (null, RouteStatus.InvalidHttpMethod);
            }

            // Valid path but no handlers for any HTTP method
            // return (null, RouteStatus.NoHandlers);
        }

        var currentPart = pathParts[0];
        var remainingParts = pathParts.Skip(1).ToArray();

        return node.HasChild(currentPart)
            ?
            // If a child with the current part exists, continue down the trie
            FindRouteRecursive(node.GetChild(currentPart), httpMethod, remainingParts)
            :
            // Invalid path
            (null, RouteStatus.InvalidPath);
    }
}