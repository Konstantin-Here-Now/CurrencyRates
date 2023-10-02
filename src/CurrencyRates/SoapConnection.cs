namespace SoapConnection;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Logging;

public class Connection
{
    /// <summary>
    /// Создание POST-запроса по протоколу SOAP.
    /// </summary>
    /// <param name="url">Адрес, на который отправится запрос.</param>
    /// <param name="soapEnvelope">Элемент Envelope (тело запроса).</param>
    /// <returns>POST-запрос по протоколу SOAP.</returns>
    private static HttpRequestMessage CreatePOSTSoapWebRequest(string url, string soapEnvelope)
    {
        Logger.Info("Creating SOAP POST request...");
        var request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Add("ContentType", "application/soap+xml; charset=utf-8");
        request.Headers.Add("ContentLength", soapEnvelope.Length.ToString());
        XDocument xmlContent = XDocument.Parse(soapEnvelope);
        request.Content = new StringContent(xmlContent.ToString(), Encoding.UTF8, "text/xml");

        return request;
    }

    /// <summary>
    /// Метод, возврающий ответ на POST-запрос по протоколу SOAP.
    /// </summary>
    /// <param name="url">Адрес, на который отправится запрос.</param>
    /// <param name="soapEnvelope">Элемент Envelope (тело запроса).</param>
    /// <returns>Ответ в виде XML-строки.</returns>
    public static string GetSoapResponse(string url, string soapEnvelope)
    {
        Logger.Info("Getting SOAP requests' response...");
        try
        {
            HttpRequestMessage requestMessage = CreatePOSTSoapWebRequest(url, soapEnvelope);

            var httpClient = new HttpClient();
            var response = httpClient.Send(requestMessage);

            Stream contentStream = response.Content.ReadAsStream();
            using (var streamReader = new StreamReader(contentStream, Encoding.UTF8))
            {
                string responseContent = streamReader.ReadToEnd();
                Logger.Info("Got response successfully.");
                return responseContent;
            }
        }
        catch (HttpRequestException ex)
        {
            Logger.Error(ex.ToString());
            return "";
        }
    }
}

