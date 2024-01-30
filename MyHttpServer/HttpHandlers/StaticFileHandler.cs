using System.Net;
using System.Net.Http.Headers;
using MyHttpServer.MyHttp.Handler;
using MyHttpServer.MyHttp.Response;
using MyHttpServer.MyHttp.Utilities;
using MyHttpServer.Utilities;

namespace MyHttpServer.HttpHandlers;

public class StaticFileHandler(string? forcedUrl = null) : IMyHttpHandler
{
    public Func<HttpListenerRequest, Task<MyHttpResponse>> ResponseFunc { get; } = async req =>
    {
        var decodedPath = Uri.UnescapeDataString(req.Url.AbsolutePath).TrimStart('/');
        var requestedFilePath = GetFilePath(forcedUrl, decodedPath);
        var dirExists = Directory.Exists(Path.GetDirectoryName(requestedFilePath));

        if (!dirExists) return MyHttpResponseTemplate.NotFound();

        var file = new FileInfo(requestedFilePath);
        var ext = decodedPath.Split('.').LastOrDefault();

        if (!file.Exists || ext == null) return MyHttpResponseTemplate.NotFound();

        var forceDownload = req.Url.Query.Contains("download=1");
        var contentType = MyMimeTypes.GetMimeType(ext) ?? MyMimeTypes.GetMimeType("bin");

        try
        {
            var headers = new Dictionary<object, string>
            {
                { HttpResponseHeader.ContentType, contentType },
                { HttpResponseHeader.ContentLength, file.Length.ToString() },
                {
                    "Content-Disposition",
                    $"{(forceDownload ? "attachment" : "inline")}; filename=\"{file.Name}\""
                }
            };

            return new MyHttpResponse(MyHttpStatus.Ok, headers,
                async res =>
                {
                    Console.WriteLine("Entered CopyToAsync block...");
                    await using (var fileStream = new FileStream(requestedFilePath, FileMode.Open, FileAccess.Read,
                                     FileShare.Read, 4096, FileOptions.Asynchronous))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        await fileStream.CopyToAsync(res.OutputStream);
                        await res.OutputStream.FlushAsync();
                        res.OutputStream.Close();
                    }

                    Console.WriteLine("Exited CopyToAsync block...");
                });
        }
        catch (Exception e)
        {
            Logger.LogError("Error reading file:", e.Message, e.StackTrace ?? "");
            return new MyHttpResponse(
                MyHttpStatus.InternalServerError,
                new Dictionary<object, string> { { "content-type", MyMimeTypes.GetMimeType("pl") } },
                "Internal Server Error"
            );
        }
    };

    private static string GetFilePath(string? forcedUrl, string decodedPath)
    {
        var baseDirectory = Directory.GetCurrentDirectory();
        var assetPath = forcedUrl != null ? $"assets/public/{forcedUrl}" : decodedPath;
        return Path.GetFullPath(Path.Combine(baseDirectory, "../../../..", assetPath));
    }

    private static Task<IMyHttpHandler> SendFileStream()
    {
        var headers = new Dictionary<object, string>
        {
            { HttpResponseHeader.ContentType, contentType },
            { HttpResponseHeader.ContentLength, file.Length.ToString() },
            {
                "Content-Disposition",
                $"{(forceDownload ? "attachment" : "inline")}; filename=\"{file.Name}\""
            }
        };

        return new MyHttpResponse(MyHttpStatus.Ok, headers,
            async res =>
            {
                Console.WriteLine("Entered CopyToAsync block...");
                await using (var fileStream = new FileStream(requestedFilePath, FileMode.Open, FileAccess.Read,
                                 FileShare.Read, 4096, FileOptions.Asynchronous))
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    await fileStream.CopyToAsync(res.OutputStream);
                    await res.OutputStream.FlushAsync();
                    res.OutputStream.Close();
                }

                Console.WriteLine("Exited CopyToAsync block...");
            });
    }
}