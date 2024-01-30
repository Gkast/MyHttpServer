namespace MyHttpServer.MyHttp.Response;

public interface IMyHttpResponse
{
    public int StatusCode { get; }
    public string StatusMessage { get; }
    public Dictionary<object, string> Headers { get; }
    public object? Body { get; }
}