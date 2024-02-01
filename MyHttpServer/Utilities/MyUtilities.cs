using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace MyHttpServer.Utilities;

public static class MyUtilities
{
    public static string XmlEscape(string unescaped)
    {
        var doc = new XmlDocument();
        XmlNode node = doc.CreateElement("root");
        node.InnerText = unescaped;
        return node.InnerXml;
    }

    public static string XmlUnescape(string escaped)
    {
        var doc = new XmlDocument();
        XmlNode node = doc.CreateElement("root");
        node.InnerXml = escaped;
        return node.InnerText;
        
    }
}