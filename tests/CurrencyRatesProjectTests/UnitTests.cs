namespace CurrencyRatesProjectTests;


using System.Xml.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using static XMLOperationsList.XMLOperations;
using static CbSoapEnvelope.SoapEnvelope;
using static CursOnDate.CursOnDateOperations;

public class UnitTestXMLOperationsList
{
    [Fact]
    public void TestGetXMLTagFirstValueOrdinal()
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
    public void TestGetXMLTagFirstValueNoTagInXML()
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
    public void TestGetXMLTagFirstValueEmptySequenceError()
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
    public void TestCreateSoapEnvelopeCbCursOrdinal()
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