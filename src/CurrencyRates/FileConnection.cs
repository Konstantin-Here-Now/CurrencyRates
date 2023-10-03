namespace FileConnection;

using Logging;

class WriterToFile
{
    /// <summary>
    /// Метод, осуществляющий запись в файл. Если файла не существует, он будет создан.
    /// </summary>
    /// <param name="stringToWrite">Строка для записи в файл.</param>
    /// <param name="filename">Имя файла (включая расширение), куда будет осуществляться запись.</param>
    public static void WriteToFile(string stringToWrite, string filename)
    {
        Logger.Info($"Writing results to file {filename}.");
        string pathToFile = Environment.CurrentDirectory + "/" + filename;


        using (var streamWriter = new StreamWriter(pathToFile, true))
        {
            streamWriter.WriteLine(stringToWrite);
        }
    }
}
