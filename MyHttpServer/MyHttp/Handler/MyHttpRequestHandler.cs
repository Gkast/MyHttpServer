using System.Net;
using System.Text;
using MyHttpServer.MyHttp.Response;
using MyHttpServer.MyHttp.Router;
using MyHttpServer.MyHttp.Utilities;
using MyHttpServer.Utilities;

namespace MyHttpServer.MyHttp.Handler;

public static class MyHttpRequestHandler
{
    public static async Task HandleContext(HttpListenerContext context, MyHttpRouter router)
    {
        var request = context.Request;
        var response = context.Response;
        try
        {
            var (myHandler, status) = router.Find(request.HttpMethod, request.Url.AbsolutePath);
            switch (status)
            {
                case RouteStatus.InvalidPath:
                    await SendNativeResponse(
                        404,
                        "Not Found",
                        "<h1>Page Not Found</h1>"u8.ToArray(),
                        response
                    );
                    break;
                case RouteStatus.InvalidHttpMethod:
                    await SendNativeResponse(
                        405,
                        "Method Not Allowed",
                        "<h1>Method Not Allowed</h1>"u8.ToArray(),
                        response
                    );
                    break;
                case RouteStatus.Valid:
                    var myRes = await myHandler!.ResponseFunc(request);
                    await SendMyHttpResponse(myRes, response).ConfigureAwait(false);
                    break;
            }
        }
        catch (Exception e)
        {
            Logger.LogError($"Type: {e.GetType().Name}, Message: {e.Message}\nStack Trace: {e.StackTrace}");
            await SendNativeResponse(
                500,
                "Internal Server Error",
                "<h1>Internal Server Error</h1>"u8.ToArray(),
                response
            );
        }
    }

    private static async Task SendMyHttpResponse(MyHttpResponse myRes, HttpListenerResponse res)
    {
        res.StatusCode = myRes.StatusCode;
        res.StatusDescription = myRes.StatusMessage;

        res.Headers.Clear();
        foreach (var (headerName, headerValue) in myRes.Headers)
            res.AddHeader(headerName.ToString()!, headerValue);
        res.Headers.Set("Server", "\r\n\r\n");
        res.Headers.Set("Date", "");

        if (myRes.Body is not null)
        {
            if (myRes.Body is string bodyString)
            {
                var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
                res.ContentLength64 = bodyBytes.Length;
                await res.OutputStream.WriteAsync(new ReadOnlyMemory<byte>(bodyBytes)).ConfigureAwait(false);
                res.Close();
            }

            if (myRes.Body is Func<HttpListenerResponse, Task> bodyFunc)
            {
                Console.WriteLine("body func");
                await bodyFunc(res);
            }
        }
    }

    private static async Task SendNativeResponse(int statusCode, string statusMessage, byte[] body,
        HttpListenerResponse response)
    {
        response.StatusCode = statusCode;
        response.StatusDescription = statusMessage;

        response.ContentEncoding = Encoding.UTF8;

        response.Headers.Clear();
        response.Headers.Set(HttpResponseHeader.ContentType, MyMimeTypes.GetMimeType("html"));
        response.Headers.Set("Server", "\r\n\r\n");
        response.Headers.Set("Date", "");

        response.ContentLength64 = body.Length;

        await response.OutputStream.WriteAsync(new ReadOnlyMemory<byte>(body)).ConfigureAwait(false);
        
        response.Close();
    }
}