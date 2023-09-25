using System.Configuration;

using static SoapConnection.Connection;
using static CbSoapEnvelope.SoapEnvelope;
using static FileConnection.WriterToFile;
using CursOnDate;
using static CursOnDate.CursOnDateOperations;
using Logging;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Dictionary<string, dynamic> config = GetConfig();
            int interval = checked(config["timerIntervalHours"] * 60 * 60 * 1000);
            var timer = new Timer(delegate { CallBack(config); }, config, 0, interval);

            // for stopping program just press "Enter"
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
            throw;
        }
    }

    static void CallBack(Dictionary<string, dynamic> config)
    {
        Logger.Info("<<<Starting another loop...>>>");
        string URL = config["cbURL"];
        string resultsFilename = config["resultsFilename"];
        bool serializeNeeded = config["serializeNeeded"];


        if (URL == null | resultsFilename == null) throw new ArgumentNullException("Missing args URL or resultsFilename in \"app.config\" file.");

        string SoapEnvelope = CreateSoapEnvelopeCbCurs(DateTime.Today);
        string response = GetSoapResponse(URL!, SoapEnvelope);

        if (serializeNeeded == true && response != "")
        {
            List<CursOnDateStruct> parsedResult = ParseCbCursOnDate(response);
            string serializedResult = JSONSerializeCbCursOnDate(parsedResult);

            WriteResultToFile(serializedResult, resultsFilename!);
        }
        else if (response == "")
        {
            WriteResultToFile("Something went wrong, check logs.", resultsFilename!);
            Logger.Error("<<<Something went wrong...>>>");
            return;
        }
        else
            WriteResultToFile(response, resultsFilename!);

        Logger.Info("<<<Everything was successful.>>>");
    }

    static Dictionary<string, dynamic> GetConfig()
    {
        Logger.Info("Setting config...");
        var config = new Dictionary<string, dynamic>()
        {
            {"cbURL", ConfigurationManager.AppSettings["cbURL"]!},
            {"resultsFilename", ConfigurationManager.AppSettings["resultsFilename"]!},
            {"serializeNeeded", Convert.ToBoolean(ConfigurationManager.AppSettings["serializeNeeded"])},
            {"timerIntervalHours", Convert.ToInt32(ConfigurationManager.AppSettings["timerIntervalHours"])}
        };

        if (config["cbURL"] == null | config["resultsFilename"] == null) throw new ArgumentNullException("Missing args URL or resultsFilename in \"app.config\" file.");
        if (config["timerIntervalHours"] <= 0) throw new ArgumentException("Timer interval must be greater than zero.");

        Logger.Info("Config have been set up.");
        return config;
    }
}
