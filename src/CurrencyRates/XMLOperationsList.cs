namespace XMLOperationsList;

using System.Linq.Expressions;
using System.Xml.Linq;
using Logging;

public class XMLOperations
{
    public static string GetXMLTagFirstValue(XElement xml, string tag)
    {
        try { return xml.Descendants(tag).First().Value; }
        catch (Exception ex)
        {
            if (ex is InvalidOperationException)
            {
                Logger.Warning("Failed to get XML tag first value.");
                Logger.Warning(ex.Message);
                return "";
            }
            throw;
        }
    }
}