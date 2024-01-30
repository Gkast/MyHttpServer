namespace MyHttpServer.MyHttp.Router;

public enum RouteStatus
{
    Valid,
    InvalidPath,
    InvalidHttpMethod,
    NoHandlers
}