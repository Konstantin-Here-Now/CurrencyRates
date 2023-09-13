namespace CbSoapEnvelope;
using Logging;

public class SoapEnvelope
{
    public static string CreateSoapEnvelopeCbCurs(DateTime dateTime)
    {
        Logger.Info("Creating envelope for SOAP request...");

        string iso8601String = dateTime.ToString("yyyy-MM-dd");
        string soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
        @"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">" +
        @"<soap12:Body>" +
        @"<GetCursOnDate xmlns=""http://web.cbr.ru/"">" +
        $@"<On_date>{iso8601String}</On_date>" +
        @"</GetCursOnDate>" +
        @"</soap12:Body>" +
        @"</soap12:Envelope>";

        return soapEnvelope;
    }
}