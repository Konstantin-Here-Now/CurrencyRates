namespace XMLOperationsList;

using System.Xml.Linq;
using Logging;

/// <summary>
/// Класс, содержащий операции над XML-элементами.
/// </summary>
public class XMLOperations
{
    /// <summary>
    /// Метод, возвращающий значение первого в переданном XML-элементе тэга. Если такого значения нет, возвращает пустую строку.
    /// </summary>
    /// <param name="xml">XML-элемент.</param>
    /// <param name="tag">XML-тэг.</param>
    /// <returns></returns>
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