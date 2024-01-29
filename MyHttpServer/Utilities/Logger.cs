namespace MyHttpServer.Utilities;

public static class Logger
{
    private static string CurrentTimestamp()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    private static void Log(string logLevel, params string[] messages)
    {
        var messageToWrite = messages.Aggregate("",
            (current, message) => current + (string.IsNullOrEmpty(current) ? "" : " ") + message);
        Console.WriteLine($"[{CurrentTimestamp()}] [{logLevel}]: {messageToWrite}");
    }

    public static void LogTrace(params string[] messages)
    {
        Log("TRACE", messages);
    }

    public static void LogDebug(params string[] messages)
    {
        Log("DEBUG", messages);
    }

    public static void LogInfo(params string[] messages)
    {
        Log("INFO", messages);
    }

    public static void LogWarn(params string[] messages)
    {
        Log("WARN", messages);
    }

    public static void LogError(params string[] messages)
    {
        Log("ERROR", messages);
    }

    public static void LogFatal(params string[] messages)
    {
        Log("FATAL", messages);
    }
}