namespace SoapConnection;
using System.Net;

using Logging;

public class Connection
{
    private static HttpWebRequest CreatePOSTSoapWebRequest(string url, string soapEnvelope)
    {
        Logger.Info("Creating SOAP POST request...");
        //TODO replace with HttpClient
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        request.Method = "POST";
        request.ContentType = "application/soap+xml; charset=utf-8";

        StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
        streamWriter.Write(soapEnvelope);
        streamWriter.Close();

        request.ContentLength = soapEnvelope.Length;

        return request;
    }


    public static string GetSoapResponse(string url, string soapEnvelope)
    {
        Logger.Info("Getting SOAP requests' response...");
        try
        {
        HttpWebRequest request = CreatePOSTSoapWebRequest(url, soapEnvelope);
        WebResponse response = request.GetResponse();

        StreamReader streamReader = new StreamReader(response.GetResponseStream());
        string result = streamReader.ReadToEnd();
        // TODO is it necessary?
        streamReader.Close();

        }
        catch (System.Net.WebException ex)
        {
            Logger.Error(ex.ToString());
            return "";
        }
        
        string resultNoException = "KEK";
        Logger.Info("Got response successfully.");
        return resultNoException;
    }
}

