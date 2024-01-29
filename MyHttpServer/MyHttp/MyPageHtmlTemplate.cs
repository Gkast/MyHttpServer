namespace MyHttpServer.MyHttp;

public static class MyPageHtmlTemplate
{
    public static string HtmlResponse(string title, string? body)
    {
        return GetHtmlTop(title) + body + GetHtmlBottom();
    }

    private static string GetHtmlTop(string title)
    {
        return $"""
                <!DOCTYPE html>
                <html lang="en">
                <head>
                    <meta charset="UTF-8">
                    <title>{title}</title>
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                </head>
                <body>
                    <header>
                        {GetHeader()}
                    </header>
                    <main>
                        <article>
                """;
    }

    private static string GetHtmlBottom()
    {
        return $$"""
                 </article>
                     </main>
                     <footer>
                         {{GetFooter()}}
                     </footer>
                     <script type="text/javascript">{
                 var deps = {};
                 var funcs = {};
                 var cache = {};
                 function define(key, d, func) {
                     deps[key] = d;
                     funcs[key] = func;
                 }
                 function require(key) {
                     if (key === 'require') return require;
                     if (key === 'exports') return {};
                     var cached = cache[key];
                     if(cached) return cached;
                     var resolved = deps[key].map(require);
                     funcs[key].apply(null, resolved);
                     return cache[key] = resolved[1];
                 }</script>
                 <script src='/assets/public/js/main.js'></script>
                 <script type="text/javascript">{Object.keys(deps).forEach(depName => require(depName))}</script>
                 </body>
                 </html>
                 """;
    }

    private static string GetHeader()
    {
        return $"""
                <div>
                    <h1><a href="/" title="Home">My HTTP Header</a></h1>
                </div>
                <nav>
                    {GetNavigationBar()}
                </nav>
                """;
    }

    private static string GetFooter()
    {
        return """
               <div>
                   <span>My HTTP Footer</span>
               </div>
               """;
    }

    private static string GetNavigationBar()
    {
        return """
               <div>
                    <a href="/login">
                        <button>Log In</button>
                    </a>
                    <a href="/register">
                        <button>Register</button>
                    </a>
               </div>
               """;
    }
}