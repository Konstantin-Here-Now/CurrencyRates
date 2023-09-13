namespace CursOnDate;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Linq;
using Logging;
using static XMLOperationsList.XMLOperations;

public struct OneCursStruct
{
    public string Vname { get; set; }
    public string Vnom { get; set; }
    public string Vcurs { get; set; }
    public string Vcode { get; set; }
    public string VchCode { get; set; }
}

public struct CursOnDateStruct
{
    public string cursDate { get; set; }
    public OneCursStruct cursData { get; set; }
}

public class CursOnDateOperations
{
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
            Logger.Warning("The was no date in Central Bank XML response." +
            "Be careful: if the time of this log is later then 12:00 UTC+3, date of currency exchange rates may be wrong.");
            return DateTime.Today.ToString("yyyy-MM-dd");
    }

    public static List<CursOnDateStruct> ParseCbCursOnDate(string response)
    {
        Logger.Info("Parsing XML-response...");

        var cursesParsed = new List<CursOnDateStruct>();
        XDocument cursesXDoc = XDocument.Parse(response);
        string cursDate = GetCursDateOrToday(cursesXDoc);

        foreach (var oneCurs in cursesXDoc.Descendants("ValuteCursOnDate").ToList())
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

            var oneCursOnDate = new CursOnDateStruct
            {
                cursDate = cursDate,
                cursData = oneCursStruct
            };

            cursesParsed.Add(oneCursOnDate);
        }

        Logger.Info("Parsed successfully.");

        return cursesParsed;
    }

    public static string JSONSerializeCbCursOnDate(List<CursOnDateStruct> cursOnDate)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        };

        string jsonString = JsonSerializer.Serialize(cursOnDate, serializerOptions);

        return jsonString;
    }
}
