namespace MyHttpServer.MyHttp;

// Definition of HTTP methods supported by the router
public enum HttpMethod
{
    GET,
    POST,
    PUT,
    DELETE,
    HEAD,
    CONNECT,
    OPTIONS,
    TRACE,
    PATCH
}

// Constants representing different node kinds in the router tree
public static class NodeKind
{
    public const int SKIND = 0;
    public const int PKIND = 1;
    public const int AKIND = 2;
    public const int STAR = 42;
    public const int SLASH = 47;
    public const int COLON = 58;
}

// Represents a node in the router tree
public class Node
{
    public List<Node> Children;
    public int Kind;
    public int Label;
    public Dictionary<string, (Delegate? handler, string[] pnames)> Map;
    public string Prefix;

    // Updated constructor to accept 4 arguments
    public Node(string prefix = "/", List<Node>? children = null, int kind = NodeKind.SKIND,
        Dictionary<string, (Delegate? handler, string[] pnames)>? map = null)
    {
        Label = prefix[0];
        Prefix = prefix;
        Children = children ?? new List<Node>();
        Kind = kind;
        Map = map ?? new Dictionary<string, (Delegate? handler, string[] pnames)>();
    }

    public void AddChild(Node n)
    {
        Children.Add(n);
    }

    public Node FindChild(int c, int t)
    {
        return Children.Find(e => c == e.Label && t == e.Kind);
    }

    public Node FindChildWithLabel(int c)
    {
        return Children.Find(e => c == e.Label);
    }

    public Node FindChildByKind(int t)
    {
        return Children.Find(e => t == e.Kind);
    }

    public void AddHandler(HttpMethod method, Delegate handler, string[] pnames = null)
    {
        var pNamesArray = pnames ?? Array.Empty<string>();
        Map[method.ToString()] = (handler, pNamesArray);
    }

    public (Delegate handler, string[] pnames)? FindHandler(string method)
    {
        return Map.TryGetValue(method, out var result) ? result : null;
    }
}

// MyHttpRouter class that manages routing using a tree-based structure
public class MyHttpRouter<THandler> where THandler : Delegate
{
    private readonly Node _tree = new();

    // Constructor to create a MyHttpRouter

    // Adds a route to the router
    public void Add(HttpMethod method, string path, THandler handler)
    {
        var i = 0;
        var l = path.Length;
        var pnames = new List<string>();
        var ch = 0;
        var j = 0;

        while (i < l)
        {
            ch = path[i];
            if (ch == NodeKind.COLON)
            {
                j = i + 1;

                Insert(method, path.Substring(0, i), NodeKind.SKIND, pnames, handler);
                while (i < l && path[i] != NodeKind.SLASH) i++;

                pnames.Add(path.Substring(j, i - j));
                path = path.Substring(0, j) + path.Substring(i);
                i = j;
                l = path.Length;

                if (i == l)
                {
                    Insert(method, path.Substring(0, i), NodeKind.PKIND, pnames, handler);
                    return;
                }

                Insert(method, path.Substring(0, i), NodeKind.PKIND, pnames);
            }
            else if (ch == NodeKind.STAR)
            {
                Insert(method, path.Substring(0, i), NodeKind.SKIND, pnames);
                pnames.Add("*");
                Insert(method, path.Substring(0, l), NodeKind.AKIND, pnames, handler);
                return;
            }

            i++;
        }

        Insert(method, path, NodeKind.SKIND, pnames, handler);
    }

    // Finds a handler for a given method and path
    public (THandler handler, (string name, string value)[] pvalues) Find(HttpMethod method, string path)
    {
        return InternalFind(method.ToString(), path, _tree);
    }

    // Inserts a route into the router _tree
    private void Insert(HttpMethod method, string path, int t, List<string> pnames = null, THandler handler = default)
    {
        var cn = _tree;
        string prefix;
        int sl, pl, l, max;
        Node n, c;

        while (true)
        {
            prefix = cn.Prefix;
            sl = path.Length;
            pl = prefix.Length;
            l = 0;

            max = sl < pl ? sl : pl;
            while (l < max && path[l] == prefix[l]) l++;

            if (l < pl)
            {
                n = new Node(prefix.Substring(l), new List<Node>(cn.Children), cn.Kind,
                    new Dictionary<string, (Delegate, string[])>(cn.Map));
                cn.Children = new List<Node> { n };

                cn.Label = prefix[0];
                cn.Prefix = prefix.Substring(0, l);
                cn.Map = new Dictionary<string, (Delegate, string[])>();
                cn.Kind = NodeKind.SKIND;

                if (l == sl)
                {
                    cn.AddHandler(method, handler, pnames?.ToArray());
                    cn.Kind = t;
                }
                else
                {
                    n = new Node(path.Substring(l), new List<Node>(), t);
                    n.AddHandler(method, handler, pnames?.ToArray());
                    cn.AddChild(n);
                }
            }
            else if (l < sl)
            {
                path = path.Substring(l);
                c = cn.FindChildWithLabel(path[0]);
                if (c != null)
                {
                    cn = c;
                    continue;
                }

                n = new Node(path, new List<Node>(), t);
                n.AddHandler(method, handler, pnames?.ToArray());
                cn.AddChild(n);
            }
            else if (handler != null)
            {
                cn.AddHandler(method, handler, pnames?.ToArray());
            }

            return;
        }
    }

    // Finds a handler for a given method and path
    private (THandler handler, (string name, string value)[] pvalues) InternalFind(string method, string path,
        Node cn = null, int n = 0)
    {
        cn ??= _tree;
        var sl = path.Length;
        var prefix = cn.Prefix;
        var pvalues = new List<(string name, string value)>();
        int i;
        var pl = prefix.Length;
        int l;
        int max;
        Node c;
        string preSearch;

        if (sl == 0 || path == prefix)
        {
            var r = cn.FindHandler(method);
            if (r != null && r.handler != null)
            {
                var pnames = r.pnames;
                if (pnames != null)
                    for (i = 0; i < pnames.Length; ++i)
                        pvalues.Add((pnames[i], pvalues[i].value));
            }

            return (r.handler, pvalues.ToArray());
        }

        pl = prefix.Length;
        l = 0;

        max = sl < pl ? sl : pl;
        while (l < max && path[l] == prefix[l]) l++;

        if (l == pl) path = path.Substring(l);
        preSearch = path;

        c = cn.FindChild(path[0], NodeKind.SKIND);
        if (c != null)
        {
            InternalFind(method, path, c, n, (cn.FindHandler(method).handler, pvalues.ToArray()));
            if (cn.FindHandler(method).handler != null) return (cn.FindHandler(method).handler, pvalues.ToArray());
            path = preSearch;
        }

        if (l != pl) return (default, pvalues.ToArray());

        c = cn.FindChildByKind(NodeKind.PKIND);
        if (c != null)
        {
            l = path.Length;
            i = 0;
            while (i < l && path[i] != NodeKind.SLASH) i++;

            pvalues.Add((path.Substring(0, i), pvalues[n].value));

            n++;
            preSearch = path;
            path = path.Substring(i);

            InternalFind(method, path, c, n, (cn.FindHandler(method).handler, pvalues.ToArray()));
            if (cn.FindHandler(method).handler != null) return (cn.FindHandler(method).handler, pvalues.ToArray());

            n--;
            pvalues.RemoveAt(pvalues.Count - 1);
            path = preSearch;
        }

        c = cn.FindChildByKind(NodeKind.AKIND);
        if (c != null)
        {
            pvalues.Add((path, ""));
            path = "";
            InternalFind(method, path, c, n, (cn.FindHandler(method).handler, pvalues.ToArray()));
        }

        return (cn.FindHandler(method).handler, pvalues.ToArray());
    }
}