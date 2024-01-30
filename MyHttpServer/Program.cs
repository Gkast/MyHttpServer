using MyHttpServer.Utilities;

namespace MyHttpServer;

internal static class Program
{
    public static async Task Main()
    {
        try
        {
            var httpServer = new MyHttp.MyHttpServer();
            httpServer.InitRoutes();
            await httpServer.Listen();
        }
        catch (Exception e)
        {
            Logger.LogFatal("Error starting up Program", e.Message, e.StackTrace ?? "");
            Environment.Exit(1);
        }
    }
}