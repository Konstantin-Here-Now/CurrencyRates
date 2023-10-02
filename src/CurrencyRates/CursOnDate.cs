namespace CursOnDate;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Linq;
using Logging;
using static XMLOperationsList.XMLOperations;

/// <summary>Структура, содержащая информацию о курсе валюты.</summary>
public struct OneCursStruct
{
    /// <summary>Название валюты.</summary>
    public string Vname { get; set; }
    /// <summary>Номинал.</summary>
    public string Vnom { get; set; }
    /// <summary>Курс.</summary>
    public string Vcurs { get; set; }
    /// <summary>ISO Цифровой код валюты.</summary>
    public string Vcode { get; set; }
    /// <summary>ISO Символьный код валюты.</summary>
    public string VchCode { get; set; }
    /// <summary>Курс за 1 единицу валюты.</summary>
    public string VunitRate { get; set; }

    public OneCursStruct(string Vname, string Vnom, string Vcurs, string Vcode, string VchCode, string VunitRate)
    {
        this.Vname = Vname;
        this.Vnom = Vnom;
        this.Vcurs = Vcurs;
        this.Vcode = Vcode;
        this.VchCode = VchCode;
        this.VunitRate = VunitRate;
    }

    public override bool Equals(object? obj)
    {
        if (obj is OneCursStruct objectType)
        {
            return objectType.Vname == this.Vname
            && objectType.Vnom == this.Vnom
            && objectType.Vcurs == this.Vcurs
            && objectType.Vcode == this.Vcode
            && objectType.VchCode == this.VchCode
            && objectType.VunitRate == this.VunitRate;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

/// <summary>Структура, содержащая информацию о курсах валют на определенную дату.</summary>
public struct CursOnDateStruct
{
    public string cursDate { get; set; }
    public List<OneCursStruct> cursData { get; set; }

    public CursOnDateStruct(string cursDate, List<OneCursStruct> cursData)
    {
        this.cursDate = cursDate;
        this.cursData = cursData;
    }

    public override bool Equals(object? obj)
    {
        if (obj is CursOnDateStruct objectType)
        {
            return objectType.cursDate == this.cursDate && Enumerable.SequenceEqual(objectType.cursData, this.cursData);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

}

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
