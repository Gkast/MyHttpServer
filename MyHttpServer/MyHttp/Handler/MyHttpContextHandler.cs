using System.Net;
using System.Text;
using MyHttpServer.MyHttp.Response;
using MyHttpServer.MyHttp.Router;
using MyHttpServer.MyHttp.Utilities;
using MyHttpServer.Utilities;

namespace MyHttpServer.MyHttp.Handler;

public static class MyHttpContextHandler
{
    public static async Task HandleContext(HttpListenerContext context, MyHttpRouter router)
    {
        var request = context.Request;
        var response = context.Response;
        try
        {
            if (request.Url is null)
            {
                await SendNativeResponse(
                    400,
                    "Bad Request",
                    "<h1>Bad Request</h1>"u8.ToArray(),
                    response);
                return;
            }

            var (myHandler, status, pathVariables) = router.Find(request.HttpMethod, request.Url.AbsolutePath);

            switch (status)
            {
                case MyRouteStatus.InvalidPath or MyRouteStatus.NoHandlers:
                    await SendNativeResponse(
                        404,
                        "Not Found",
                        "<h1>Page Not Found</h1>"u8.ToArray(),
                        response
                    );
                    break;
                case MyRouteStatus.InvalidHttpMethod:
                    await SendNativeResponse(
                        405,
                        "Method Not Allowed",
                        "<h1>Method Not Allowed</h1>"u8.ToArray(),
                        response
                    );
                    break;
                case MyRouteStatus.Valid:
                    var myResponse = await myHandler!.ResponseFunc(request, pathVariables);
                    await SendMyHttpResponse(myResponse, response).ConfigureAwait(false);
                    break;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(
                $"Type: {ex.GetType().Name}, Message: {ex.Message}{Environment.NewLine}Stack Trace: {ex.StackTrace}"
            );
            await SendNativeResponse(
                500,
                "Internal Server Error",
                "<h1>Internal Server Error</h1>"u8.ToArray(),
                response
            );
        }
    }

    private static async Task SendMyHttpResponse(IMyHttpResponse myResponse, HttpListenerResponse response)
    {
        response.StatusCode = myResponse.StatusCode;
        response.StatusDescription = myResponse.StatusMessage;

        response.Headers.Clear();
        foreach (var (headerName, headerValue) in myResponse.Headers)
            response.AddHeader(headerName.ToString()!, headerValue);
        response.Headers.Set("Server", "My Server");
        response.Headers.Set("Date", "No Date");

        if (myResponse.Body is not null)
            switch (myResponse.Body)
            {
                case string bodyString:
                {
                    var bodyBytes = Encoding.UTF8.GetBytes(bodyString);
                    response.ContentLength64 = bodyBytes.Length;
                    await response.OutputStream.WriteAsync(new ReadOnlyMemory<byte>(bodyBytes)).ConfigureAwait(false);
                    response.Close();
                    return;
                }
                case Func<HttpListenerResponse, Task> bodyFunc:
                    await bodyFunc(response);
                    break;
            }
    }

    private static async Task SendNativeResponse(int statusCode, string statusMessage, byte[] body,
        HttpListenerResponse response)
    {
        response.StatusCode = statusCode;
        response.StatusDescription = statusMessage;

        response.Headers.Clear();
        response.Headers.Set(HttpResponseHeader.ContentType, MyMimeTypes.GetMimeType("html"));
        response.Headers.Set("Server", "My Server");
        response.Headers.Set("Date", "No Date");

        response.ContentEncoding = Encoding.UTF8;
        response.ContentLength64 = body.Length;

        await response.OutputStream.WriteAsync(new ReadOnlyMemory<byte>(body)).ConfigureAwait(false);

        response.Close();
    }
}