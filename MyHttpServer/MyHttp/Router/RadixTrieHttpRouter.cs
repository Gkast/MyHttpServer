using MyHttpServer.MyHttp.Handler;

namespace MyHttpServer.MyHttp.Router;

public sealed class RadixTrieHttpRouter
{
    private readonly Dictionary<string, string> _pathVariables = new();
    private readonly RadixTrieNode _root = new();

    public void AddRoute(string httpMethod, string path, IMyHttpHandler handler)
    {
        var pathParts = GetPathParts(path);
        AddRouteRecursive(_root, httpMethod, pathParts, handler);
    }

    public (IMyHttpHandler? handler, MyRouteStatus status, Dictionary<string, string>? pathVariables) FindRoute(
        string httpMethod, string path)
    {
        var pathParts = GetPathParts(path);
        _pathVariables.Clear();
        return FindRouteRecursive(_root, httpMethod, pathParts);
    }

    private static string[] GetPathParts(string path)
    {
        return path.ToLower().Split('/').Where(part => !string.IsNullOrEmpty(part)).ToArray();
    }

    private static void AddRouteRecursive(RadixTrieNode node, string httpMethod, IReadOnlyList<string> pathParts,
        IMyHttpHandler? handler)
    {
        if (pathParts.Count == 0)
        {
            node.AddHandler(httpMethod, handler);
            return;
        }

        var currentPart = pathParts[0];
        var remainingParts = pathParts.Skip(1).ToArray();

        if (currentPart == "*")
        {
            var newNode = new RadixTrieNode();
            newNode.AddHandler(httpMethod, handler);
            node.AddChild(currentPart, newNode);
            return;
        }

        if (node.HasChild(currentPart))
        {
            AddRouteRecursive(node.GetChild(currentPart), httpMethod, remainingParts, handler);
        }
        else
        {
            var newNode = new RadixTrieNode();

            if (currentPart.StartsWith(':'))
            {
                newNode.AddPathVariable(currentPart[1..]);
                currentPart = ":";
            }

            node.AddChild(currentPart, newNode);
            AddRouteRecursive(newNode, httpMethod, remainingParts, handler);
        }
    }


    private (IMyHttpHandler? handler, MyRouteStatus status, Dictionary<string, string>? pathVariables)
        FindRouteRecursive(RadixTrieNode node,
            string httpMethod, IReadOnlyList<string> pathParts)
    {
        if (pathParts.Count == 0)
        {
            var handler = node.GetHandler(httpMethod);

            if (handler != null)
                return (handler, MyRouteStatus.Valid, _pathVariables.Count > 0 ? _pathVariables : null);

            if (node.Handlers.Count != 0)
                return (null, MyRouteStatus.InvalidHttpMethod, _pathVariables.Count > 0 ? _pathVariables : null);

            return (null, MyRouteStatus.NoHandlers, _pathVariables.Count > 0 ? _pathVariables : null);
        }

        var currentPart = pathParts[0];
        var remainingParts = pathParts.Skip(1).ToArray();

        if (node.HasChild(":"))
        {
            var childNode = node.GetChild(":");
            var pathVariable = childNode.GetPathVariable();
            if (pathVariable is not null) _pathVariables[pathVariable] = currentPart;
            return FindRouteRecursive(childNode, httpMethod, remainingParts);
        }

        if (!node.HasChild("*"))
            return node.HasChild(currentPart)
                ? FindRouteRecursive(node.GetChild(currentPart), httpMethod, remainingParts)
                : (null, MyRouteStatus.InvalidPath, _pathVariables.Count > 0 ? _pathVariables : null);

        var childrenNode = node.GetChild("*");
        var childrenHandler = childrenNode.GetHandler(httpMethod);

        if (childrenHandler != null)
            return (childrenHandler, MyRouteStatus.Valid, _pathVariables.Count > 0 ? _pathVariables : null);

        if (childrenNode.Handlers.Count != 0)
            return (null, MyRouteStatus.InvalidHttpMethod, _pathVariables.Count > 0 ? _pathVariables : null);

        return (null, MyRouteStatus.NoHandlers, _pathVariables.Count > 0 ? _pathVariables : null);
    }
}

// public class RadixTrieHttpRouter
// {
//     private readonly RadixTrieNode _root = new();
//
//     public void AddRoute(string httpMethod, string path, IMyHttpHandler handler)
//     {
//         var pathParts = path.Split('/').Where(part => !string.IsNullOrEmpty(part)).ToArray();
//         AddRouteRecursive(_root, httpMethod, pathParts, handler);
//     }
//
//     // Modified FindRouteRecursive to return a tuple indicating the handler and the status
//     public (IMyHttpHandler? handler, MyRouteStatus status) FindRoute(string httpMethod, string path)
//     {
//         var pathParts = path.Split('/').Where(part => !string.IsNullOrEmpty(part)).ToArray();
//         return FindRouteRecursive(_root, httpMethod.ToUpper(), pathParts);
//     }
//
//     private static void AddRouteRecursive(RadixTrieNode node, string httpMethod, IReadOnlyList<string> pathParts,
//         IMyHttpHandler? handler)
//     {
//         if (pathParts.Count == 0)
//         {
//             // Reached the end of the path, assign the handler to the current node
//             node.AddHandler(httpMethod, handler);
//             return;
//         }
//
//         var currentPart = pathParts[0];
//         var remainingParts = pathParts.Skip(1).ToArray();
//
//         if (node.HasChild(currentPart))
//         {
//             // If a child with the current part already exists, continue down the trie
//             AddRouteRecursive(node.GetChild(currentPart), httpMethod, remainingParts, handler);
//         }
//         else
//         {
//             // Create a new node for the current part and continue down the trie
//             var newNode = new RadixTrieNode();
//             node.AddChild(currentPart, newNode);
//             AddRouteRecursive(newNode, httpMethod, remainingParts, handler);
//         }
//     }
//
//     private static (IMyHttpHandler? handler, MyRouteStatus status) FindRouteRecursive(RadixTrieNode node,
//         string httpMethod, IReadOnlyList<string> pathParts)
//     {
//         if (pathParts.Count == 0)
//         {
//             // Reached the end of the path
//             var handler = node.GetHandler(httpMethod);
//
//             if (handler != null)
//             {
//                 // Valid path and valid HTTP method
//                 return (handler, MyRouteStatus.Valid);
//             }
//
//             if (node.Handlers.Count != 0)
//             {
//                 // Valid path but invalid HTTP method
//                 return (null, MyRouteStatus.InvalidHttpMethod);
//             }
//
//             // Valid path but no handlers for any HTTP method
//             return (null, MyRouteStatus.NoHandlers);
//         }
//
//         var currentPart = pathParts[0];
//         var remainingParts = pathParts.Skip(1).ToArray();
//
//         return node.HasChild(currentPart)
//             ?
//             // If a child with the current part exists, continue down the trie
//             FindRouteRecursive(node.GetChild(currentPart), httpMethod, remainingParts)
//             :
//             // Invalid path
//             (null, MyRouteStatus.InvalidPath);
//     }
// }