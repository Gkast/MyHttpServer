using System.Security;
using System.Text;
using System.Xml;

namespace MyHttpServer.Utilities;

public static class MyEscaper
{
    internal static class Xml
    {
        public static string Escape(string unescaped)
        {
            return SecurityElement.Escape(unescaped);
        }

        public static string Unescape(string escaped)
        {
            var doc = new XmlDocument();
            doc.LoadXml($"<root>{escaped}</root>");
            return doc.DocumentElement != null ? doc.DocumentElement.InnerText : string.Empty;
        }
    }

    internal static class Html
    {
        public static string Escape(string unescaped)
        {
            return System.Net.WebUtility.HtmlEncode(unescaped);
        }

        public static string Unescape(string escaped)
        {
            return System.Net.WebUtility.HtmlDecode(escaped);
        }
    }

    internal static class Url
    {
        public static string Escape(string unescaped)
        {
            return Uri.EscapeDataString(unescaped);
        }

        public static string Unescape(string escaped)
        {
            return Uri.UnescapeDataString(escaped);
        }
    }

    internal static class Sql
    {
        public static string Escape(string unescaped)
        {
            return unescaped.Replace("'", "''");
        }

        public static string Unescape(string escaped)
        {
            return escaped.Replace("''", "'");
        }
    }

    internal class Json
    {
        private static readonly Dictionary<char, char> EscapeMappings = new()
        {
            { '\"', '\"' },
            { '\\', '\\' },
            { 'b', '\b' },
            { 'f', '\f' },
            { 'n', '\n' },
            { 'r', '\r' },
            { 't', '\t' }
        };

        public static string Escape(string inputJson)
        {
            var escapedJson = new StringBuilder();

            foreach (var c in inputJson)
            {
                switch (c)
                {
                    case '\"':
                        escapedJson.Append("\\\"");
                        break;
                    case '\\':
                        escapedJson.Append("\\\\");
                        break;
                    case '\b':
                        escapedJson.Append("\\b");
                        break;
                    case '\f':
                        escapedJson.Append("\\f");
                        break;
                    case '\n':
                        escapedJson.Append("\\n");
                        break;
                    case '\r':
                        escapedJson.Append("\\r");
                        break;
                    case '\t':
                        escapedJson.Append("\\t");
                        break;
                    default:
                        escapedJson.Append(c);
                        break;
                }
            }

            return escapedJson.ToString();
        }

        public static string Unescape(string inputJson)
        {
            var originalJson = new StringBuilder();

            var isEscaped = false;

            foreach (var c in inputJson)
            {
                if (isEscaped)
                {
                    originalJson.Append(EscapeMappings.GetValueOrDefault(c, c));
                    isEscaped = false;
                }
                else if (c == '\\')
                    isEscaped = true;
                else
                    originalJson.Append(c);
            }

            return originalJson.ToString();
        }
    }
}