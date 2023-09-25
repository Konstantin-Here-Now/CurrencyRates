namespace Logging;

public static class Logger
{

    public static void WriteLog(string logMessage, string logLevel)
    {
        string pathToFile = Environment.CurrentDirectory + "/log.log";
        string logMessageDateAndTime = $"[{DateTime.Now.ToShortDateString()}] [{DateTime.Now.ToLongTimeString()}]";
        string log = logMessageDateAndTime + $" [{logLevel}] " + logMessage;

        using (var streamWriter = new StreamWriter(pathToFile, true))
        {
            streamWriter.WriteLine(log);
        }
    }

    public static void Info(string logMessage) => WriteLog(logMessage, "Info");
    public static void Warning(string logMessage) => WriteLog(logMessage, "Warning");
    public static void Error(string logMessage) => WriteLog(logMessage, "Error");
    public static void Critical(string logMessage) => WriteLog(logMessage, "Critical");
}