using System.Net;
using System.Text;
using MyHttpServer.HttpHandlers;

namespace MyHttpServer.MyHttp;

public sealed class HttpServer : IDisposable
{
    private readonly HttpListener _httpListener;
    private readonly HttpRouter _httpRouter;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public HttpServer(string host = "localhost", int port = 7474)
    {
        var uriPrefix = $"http://{host}:{port}/";

        _httpListener = new HttpListener();
        _httpRouter = new HttpRouter();
        _cancellationTokenSource = new CancellationTokenSource();

        _httpListener.Prefixes.Add(uriPrefix);
    }

    public async Task Listen()
    {
        try
        {

        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine(e);
            throw;
        }
        _httpListener.Start();
        while (_httpListener.IsListening)
        {
            var context = await _httpListener.GetContextAsync();
            _ = HandleContext(context);
        }
    }

    public void Stop()
    {
        _httpListener.Close();
    }

    public void Terminate()
    {
        _httpListener.Abort();
    }

    private static async Task HandleContext(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;
        try
        {
            if (request.Url.PathAndQuery is "/home" or "/")
            {
                var res = await Home.Handle(request);
                response.StatusCode = res.StatusCode;
                response.StatusDescription = res.StatusMessage;
                response.ContentType = res.Headers[HttpResponseHeader.ContentType];
                if (res.Body is not null)
                {
                    var bodyBytes = Encoding.UTF8.GetBytes(res.Body);
                    response.ContentLength64 = bodyBytes.Length;
                    await response.OutputStream.WriteAsync(new ReadOnlyMemory<byte>(bodyBytes), CancellationToken.None);
                }
            }
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync("Error:");
            await Console.Error.WriteLineAsync($"Error Message: {e.Message}");
            await Console.Error.WriteLineAsync($"Error Stack Trace: {e.StackTrace}");
        }
        finally
        {
            response.Close();
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _httpListener.Close();
        _cancellationTokenSource.Dispose();
    }
}