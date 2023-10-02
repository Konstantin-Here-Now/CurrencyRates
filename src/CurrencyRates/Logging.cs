namespace Logging;

/// <summary>Класс, реализующий ведение логов.</summary>
public static class Logger
{
    /// <summary>
    /// Запись лога в файл.
    /// </summary>
    /// <param name="logMessage">Сообщение для логгирования.</param>
    /// <param name="logLevel">Уровень логирования.</param>
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

    /// <summary>Запись лога уровня DEBUG в файл</summary>
    /// <param name="logMessage">Сообщение для логгирования.</param>
    public static void Debug(string logMessage) => WriteLog(logMessage, "Debug");
    /// <summary>Запись лога уровня INFO в файл</summary>
    /// <param name="logMessage">Сообщение для логгирования.</param>
    public static void Info(string logMessage) => WriteLog(logMessage, "Info");
    /// <summary>Запись лога уровня WARNING в файл</summary>
    /// <param name="logMessage">Сообщение для логгирования.</param>
    public static void Warning(string logMessage) => WriteLog(logMessage, "Warning");
    /// <summary>Запись лога уровня ERROR в файл</summary>
    /// <param name="logMessage">Сообщение для логгирования.</param>
    public static void Error(string logMessage) => WriteLog(logMessage, "Error");
    /// <summary>Запись лога уровня CRITICAL в файл</summary>
    /// <param name="logMessage">Сообщение для логгирования.</param>
    public static void Critical(string logMessage) => WriteLog(logMessage, "Critical");
}