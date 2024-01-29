using System.Net;
using System.Text;
using MyHttpServer.Utilities;

namespace MyHttpServer.MyHttp;

public static class MyHttpRequestHandler
{
    public static async Task HandleContext(HttpListenerContext context,
        Dictionary<string, Dictionary<string, IMyHttpHandler>> routes)
    {
        var request = context.Request;
        var response = context.Response;
        try
        {
            if (routes.TryGetValue(request.Url.AbsolutePath, out var methodRoutes))
            {
                if (methodRoutes.TryGetValue(request.HttpMethod, out var handler))
                {
                    var myRes = await handler.ResponseFunc(request).ConfigureAwait(false);
                    await SendMyHttpResponse(myRes, response).ConfigureAwait(false);
                }
                else
                {
                    SendNativeResponse(
                        405,
                        "Method Not Allowed",
                        "<h1>Method Not Allowed</h1>"u8.ToArray(),
                        response
                    );
                }
            }
            else
            {
                SendNativeResponse(
                    404,
                    "Not Found",
                    "<h1>Page Not Found</h1>"u8.ToArray(),
                    response
                );
            }
        }
        catch (Exception e)
        {
            Logger.LogError($"Message: {e.Message} \nStack Trace: {e.StackTrace}");
            SendNativeResponse(
                500,
                "Internal Server Error",
                "<h1>Internal Server Error</h1>"u8.ToArray(),
                response
            );
        }
        finally
        {
            response.Close();
        }
    }

    private static async Task SendMyHttpResponse(MyHttpResponse myRes, HttpListenerResponse res)
    {
        res.StatusCode = myRes.StatusCode;
        res.StatusDescription = myRes.StatusMessage;

        res.Headers.Clear();
        foreach (var (headerName, headerValue) in myRes.Headers)
            res.AddHeader(headerName.ToString(), headerValue);
        res.Headers.Set("Server", "\r\n\r\n");
        res.Headers.Set("Date", "");

        if (!string.IsNullOrEmpty(myRes.Body))
        {
            var bodyBytes = Encoding.UTF8.GetBytes(myRes.Body);
            res.ContentLength64 = bodyBytes.Length;
            await res.OutputStream.WriteAsync(new ReadOnlyMemory<byte>(bodyBytes)).ConfigureAwait(false);
        }
    }

    private static async void SendNativeResponse(int statusCode, string statusMessage, byte[] body,
        HttpListenerResponse response)
    {
        response.StatusCode = statusCode;
        response.StatusDescription = statusMessage;

        response.ContentEncoding = Encoding.UTF8;

        response.Headers.Clear();
        response.Headers.Set(HttpResponseHeader.ContentType, "text/html");
        response.Headers.Set("Server", "\r\n\r\n");
        response.Headers.Set("Date", "");

        response.ContentLength64 = body.Length;

        await response.OutputStream.WriteAsync(new ReadOnlyMemory<byte>(body)).ConfigureAwait(false);
    }
}