using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace MyHttpServer.Utilities;

public static class MySerializer
{
    internal static class SStream
    {
        public static async Task<string> StreamToString(Stream stream)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }

        public static async Task<Stream> StringToStream(string content)
        {
            var stream = new MemoryStream();

            await using (var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                await writer.WriteAsync(content);
                await writer.FlushAsync();
            }

            stream.Position = 0;

            return stream;
        }
    }
    internal static class Json
    {
        public static async Task<string?> JsonObjectToString<T>(T obj)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await JsonSerializer.SerializeAsync(memoryStream, obj, obj?.GetType() ?? typeof(T));
                memoryStream.Seek(0, SeekOrigin.Begin);

                using var streamReader = new StreamReader(memoryStream);
                return await streamReader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing JSON: {ex.Message}");
                return null;
            }
        }

        public static async Task<T?> StringToJsonObject<T>(string jsonString)
        {
            try
            {
                using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                return await JsonSerializer.DeserializeAsync<T>(memoryStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                return default;
            }
        }
    }

    internal static class Xml
    {
        public static async Task<XElement?> StringToXml(string xmlString)
        {
            try
            {
                using var stringReader = new StringReader(xmlString);
                return await XElement.LoadAsync(stringReader, LoadOptions.None, default);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error parsing XML: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return null;
            }
        }

        public static async Task<string?> XmlToString(XElement xmlElement)
        {
            try
            {
                
                await using var stringWriter = new StringWriter();
                await using (var xmlWriter =
                             XmlWriter.Create(stringWriter, new XmlWriterSettings { Async = true, Indent = true }))
                {
                    xmlElement.WriteTo(xmlWriter);
                    await xmlWriter.FlushAsync();
                }

                return stringWriter.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error serializing XML: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return null;
            }
        }
    }

    internal static class UrlEncodedForm
    {
        public static NameValueCollection FormDataStringToCollection(string formDataString)
        {
            return HttpUtility.ParseQueryString(formDataString);
        }
        public static string FormDataCollectionToString(NameValueCollection formData)
        {
            var stringBuilder = new StringBuilder();
            foreach (var key in formData.AllKeys)
            {
                var value = formData[key];
                if (value != null) stringBuilder.Append($"{key}={Uri.EscapeDataString(value)}&");
            }
            if (stringBuilder.Length > 0) stringBuilder.Length--;

            return stringBuilder.ToString();
        }
    }
}
