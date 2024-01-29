using MyHttpServer.Utilities;

namespace MyHttpServer;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var httpServer = new MyHttp.MyHttpServer();
        try
        {
            await httpServer.Listen();
        }
        catch (Exception e)
        {
            Logger.LogFatal("Error starting up Program", e.Message, e.StackTrace ?? "");
            httpServer.Terminate();
        }
    }
}