using System.Configuration;

using static SoapConnection.Connection;
using static CbSoapEnvelope.SoapEnvelope;
using static FileConnection.JSONFileWriter;
using CursOnDate;
using static CursOnDate.CursOnDateOperations;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Dictionary<string, dynamic> config = GetConfig();
            int interval = checked(config["timerIntervalHours"] * 60 * 60 * 1000);
            var timer = new Timer(delegate { CallBack(config); }, config, 0, 30000);

            // for stopping program just press "Enter"
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void CallBack(Dictionary<string, dynamic> config)
    {
        string URL = config["cbURL"];
        string resultsFilename = config["resultsFilename"];
        bool serializeNeeded = config["serializeNeeded"];


        if (URL == null | resultsFilename == null) throw new ArgumentNullException("Missing args URL or resultsFilename");

        string SoapEnvelope = CreateSoapEnvelopeCbCurs(DateTime.Today);
        string response = GetSoapResponse(URL, SoapEnvelope);

        if (serializeNeeded == true)
        {
            List<CursOnDateStruct> parsedResult = ParseCbCursOnDate(response);
            string serializedResult = JSONSerializeCbCursOnDate(parsedResult);

            WriteResultToFile(serializedResult, resultsFilename);
        }
        else
            WriteResultToFile(response, resultsFilename);
    }
    static Dictionary<string, dynamic> GetConfig()
    {
        var config = new Dictionary<string, dynamic>()
        {
            {"cbURL", ConfigurationManager.AppSettings["cbURL"]},
            {"resultsFilename", ConfigurationManager.AppSettings["resultsFilename"]},
            {"serializeNeeded", Convert.ToBoolean(ConfigurationManager.AppSettings["serializeNeeded"])},
            {"timerIntervalHours", Convert.ToInt32(ConfigurationManager.AppSettings["timerIntervalHours"])}
        };

        if (config["cbURL"] == null | config["resultsFilename"] == null) throw new ArgumentNullException("Missing args URL or resultsFilename.");
        if (config["timerIntervalHours"] <= 0) throw new ArgumentException("Timer interval must be greater than zero.");

        return config;
    }
}
