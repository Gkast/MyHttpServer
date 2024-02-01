namespace MyHttpServer.MyHttp.Router;

public enum MyRouteStatus
{
    Valid,
    InvalidPath,
    InvalidHttpMethod,
    NoHandlers
}