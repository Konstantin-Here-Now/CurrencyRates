namespace CurrencyRatesProjectTests;


using System.Xml.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using static XMLOperationsList.XMLOperations;
using static CbSoapEnvelope.SoapEnvelope;
using static CursOnDate.CursOnDateOperations;
using CursStructs;

public class UnitTestXMLOperationsList
{
    [Fact]
    public void TestGetXMLTagFirstValue_Ordinal()
    {
        var testInput = (
            new XElement("Parent", new XElement("Child", "first"), new XElement("Child", "second")),
            "Child"
        );
        var expectedOutput = "first";

        var result = GetXMLTagFirstValue(testInput.Item1, testInput.Item2);

        Assert.Equal(result, expectedOutput);
    }

    [Fact]
    public void TestGetXMLTagFirstValue_NoTagInXML()
    {
        var testInput = (
            new XElement("Parent", new XElement("Child", "first"), new XElement("Child", "second")),
            "AnotherChild"
        );
        var expectedOutput = "";

        var result = GetXMLTagFirstValue(testInput.Item1, testInput.Item2);

        Assert.Equal(result, expectedOutput);
    }

    [Fact]
    public void TestGetXMLTagFirstValue_EmptySequenceError()
    {
        var testInput = (
            new XElement("Parent"),
            "Child"
        );
        var expectedOutput = "";

        var result = GetXMLTagFirstValue(testInput.Item1, testInput.Item2);

        Assert.Equal(result, expectedOutput);
    }
}

public class UnitTestSoapEnvelope
{
    [Fact]
    public void TestCreateSoapEnvelopeCbCurs_Ordinal()
    {
        var testInput = new DateTime(2023, 5, 9, 12, 0, 0, 0, 0);
        var expectedOutput = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
        @"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">" +
        @"<soap12:Body>" +
        @"<GetCursOnDate xmlns=""http://web.cbr.ru/"">" +
        $@"<On_date>2023-05-09</On_date>" +
        @"</GetCursOnDate>" +
        @"</soap12:Body>" +
        @"</soap12:Envelope>";

        var result = CreateSoapEnvelopeCbCurs(testInput);

        Assert.Equal(result, expectedOutput);
    }
}

public class UnitTestCursOnDate
{
    [Fact]
    public void TestParseCbCursOnDate_Ordinal()
    {
        var testInput = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
        "<soap:Body><GetCursOnDateResponse xmlns=\"http://web.cbr.ru/\"><GetCursOnDateResult><xs:schema id=\"ValuteData\" xmlns=\"\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\" xmlns:msprop=\"urn:schemas-microsoft-com:xml-msprop\"><xs:element name=\"ValuteData\" msdata:IsDataSet=\"true\" msdata:UseCurrentLocale=\"true\" msprop:OnDate=\"20230930\"><xs:complexType><xs:choice minOccurs=\"0\" maxOccurs=\"unbounded\"><xs:element name=\"ValuteCursOnDate\"><xs:complexType><xs:sequence><xs:element name=\"Vname\" type=\"xs:string\" minOccurs=\"0\" /><xs:element name=\"Vnom\" type=\"xs:decimal\" minOccurs=\"0\" /><xs:element name=\"Vcurs\" type=\"xs:decimal\" minOccurs=\"0\" /><xs:element name=\"Vcode\" type=\"xs:int\" minOccurs=\"0\" /><xs:element name=\"VchCode\" type=\"xs:string\" minOccurs=\"0\" /><xs:element name=\"VunitRate\" type=\"xs:double\" minOccurs=\"0\" /></xs:sequence></xs:complexType></xs:element></xs:choice></xs:complexType></xs:element></xs:schema><diffgr:diffgram xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\" xmlns:diffgr=\"urn:schemas-microsoft-com:xml-diffgram-v1\"><ValuteData xmlns=\"\">" +
        "<ValuteCursOnDate diffgr:id=\"ValuteCursOnDate1\" msdata:rowOrder=\"0\"><Vname>Австралийский доллар</Vname><Vnom>1</Vnom><Vcurs>62.9104</Vcurs><Vcode>36</Vcode><VchCode>AUD</VchCode><VunitRate>62.9104</VunitRate></ValuteCursOnDate>" +
        "<ValuteCursOnDate diffgr:id=\"ValuteCursOnDate2\" msdata:rowOrder=\"1\"><Vname>Азербайджанский манат</Vname><Vnom>1</Vnom><Vcurs></Vcurs><Vcode>944</Vcode><VchCode>AZN</VchCode><VunitRate>57.3028</VunitRate></ValuteCursOnDate>" +
        "</ValuteData></diffgr:diffgram></GetCursOnDateResult></GetCursOnDateResponse></soap:Body></soap:Envelope>";
        var expectedOutput = new CursOnDateStruct(cursDate: "2023-09-30", cursData: new List<OneCursStruct>());
        expectedOutput.cursData.Add(new OneCursStruct(
            Vname: "Австралийский доллар",
            Vnom: "1",
            Vcurs: "62.9104",
            Vcode: "36",
            VchCode: "AUD",
            VunitRate: "62.9104"
        ));
        expectedOutput.cursData.Add(new OneCursStruct(
            Vname: "Азербайджанский манат",
            Vnom: "1",
            Vcurs: "",
            Vcode: "944",
            VchCode: "AZN",
            VunitRate: "57.3028"
        ));

        var result = ParseCbCursOnDate(testInput);

        Assert.Equal(result, expectedOutput);
    }

    [Fact]
    public void TestJSONSerializeCbCursOnDate_Ordinal()
    {
        var testInput = new CursOnDateStruct(cursDate: "2023-09-30", cursData: new List<OneCursStruct>());
        testInput.cursData.Add(new OneCursStruct(
            Vname: "Австралийский доллар",
            Vnom: "1",
            Vcurs: "62.9104",
            Vcode: "36",
            VchCode: "AUD",
            VunitRate: "62.9104"
        ));
        testInput.cursData.Add(new OneCursStruct(
            Vname: "Азербайджанский манат",
            Vnom: "1",
            Vcurs: "",
            Vcode: "944",
            VchCode: "AZN",
            VunitRate: "57.3028"
        ));
        var expectedOutput = "{\"cursDate\":\"2023-09-30\",\"cursData\":[{\"Vname\":\"Австралийский доллар\",\"Vnom\":\"1\",\"Vcurs\":\"62.9104\",\"Vcode\":\"36\",\"VchCode\":\"AUD\",\"VunitRate\":\"62.9104\"},{\"Vname\":\"Азербайджанский манат\",\"Vnom\":\"1\",\"Vcurs\":\"\",\"Vcode\":\"944\",\"VchCode\":\"AZN\",\"VunitRate\":\"57.3028\"}]}";

        var result = JSONSerializeCbCursOnDate(testInput);
        Console.WriteLine(result);

        Assert.Equal(result, expectedOutput);
    }
}