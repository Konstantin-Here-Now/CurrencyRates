namespace XMLOperationsList;

using System.Xml.Linq;

public class XMLOperations
{
    public static string GetXMLTagFirstValue(XElement xml, string tag)
    {
        return xml.Descendants(tag).First().Value;
    }
}