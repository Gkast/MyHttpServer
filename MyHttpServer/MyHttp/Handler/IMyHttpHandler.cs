using System.Net;
using MyHttpServer.MyHttp.Response;

namespace MyHttpServer.MyHttp.Handler;

public interface IMyHttpHandler
{
    Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc {get;}
    // Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc(string parameter);
    // Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc(params string[] parameters);
    // Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc(dynamic parameter);
    // Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc(params dynamic[] parameters);
    // Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc(int parameter);
    // Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc(params int[] parameters);
    // Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc(bool parameter);
    // Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc(params bool[] parameters);
    // Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc<T>(T parameter);
    // Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc<T>(params T[] parameters);
}