namespace CursOnDate;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Linq;
using Logging;
using static XMLOperationsList.XMLOperations;
using CursStructs;

public class CursOnDateOperations
{
    /// <summary>
    /// Метод, находящий в ответе от ЦБ РФ дату, на которую действительны установленные курсы валют.
    /// </summary>
    /// <param name="xdoc">XML-ответ от API ЦБ РФ.</param>
    /// <returns>Дата в формате ISO.</returns>
    static string GetCursDateOrToday(XDocument xdoc)
    {
        XNamespace xs = "http://www.w3.org/2001/XMLSchema";
        XNamespace msprop = "urn:schemas-microsoft-com:xml-msprop";
        XAttribute? cursDate = xdoc.Descendants(xs + "element").First().Attribute(msprop + "OnDate");

        if (cursDate != null)
        {
            string unformattedDate = cursDate.Value;
            string year = unformattedDate[0..4];
            string month = unformattedDate[4..6];
            string day = unformattedDate[6..8];

            return $"{year}-{month}-{day}";
        }
        else
        {
            Logger.Warning("The was no date in Central Bank XML response." +
            "Be careful: if the time of this log is later then 12:00 UTC+3, date of currency exchange rates may be wrong.");
            return DateTime.Today.ToString("yyyy-MM-dd");
        }
    }

    /// <summary>
    /// Метод, который находит информацию о валюте в переданном XML-элементе.
    /// </summary>
    /// <param name="oneCurs">XML-элемент, содержащий информацию о валюте.</param>
    /// <returns></returns>
    static OneCursStruct ParseOneCursStruct(XElement oneCurs)
    {
        var oneCursStruct = new OneCursStruct();
        var oneCursProperties = oneCursStruct.GetType().GetProperties();

        // using object because SetValue always set "null" for structs
        object tempOneCursStruct = oneCursStruct;
        foreach (var property in oneCursProperties)
        {
            string propertyValue = GetXMLTagFirstValue(oneCurs, property.Name).Trim();
            property.SetValue(tempOneCursStruct, propertyValue);
        }

        oneCursStruct = (OneCursStruct)tempOneCursStruct;

        return oneCursStruct;
    }

    /// <summary>
    /// Метод, преобразующий XML-ответ ЦБ РФ в структуру CursOnDateStruct.
    /// </summary>
    /// <param name="response">XML-ответ от API ЦБ РФ.</param>
    /// <returns></returns>
    public static CursOnDateStruct ParseCbCursOnDate(string response)
    {
        Logger.Info("Parsing XML-response...");

        var cursesParsed = new CursOnDateStruct();
        XDocument cursesXDoc = XDocument.Parse(response);

        cursesParsed.cursDate = GetCursDateOrToday(cursesXDoc);
        cursesParsed.cursData = new List<OneCursStruct>();

        foreach (var oneCurs in cursesXDoc.Descendants("ValuteCursOnDate").ToList())
        {
            OneCursStruct oneCursStruct = ParseOneCursStruct(oneCurs);
            cursesParsed.cursData.Add(oneCursStruct);
        }

        Logger.Info("Parsed successfully.");

        return cursesParsed;
    }

    /// <summary>
    /// Метод, преобразующий структуру CursOnDateStruct в JSON-строку.
    /// </summary>
    /// <param name="cursOnDate"></param>
    /// <returns>JSON-строка.</returns>
    public static string JSONSerializeCbCursOnDate(CursOnDateStruct cursOnDate)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        };

        string jsonString = JsonSerializer.Serialize(cursOnDate, serializerOptions);

        return jsonString;
    }
}
