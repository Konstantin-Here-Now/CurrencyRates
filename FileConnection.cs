namespace FileConnection;
using Logging;

class WriterToFile
{
    public static void WriteResultToFile(string result, string resultsFilename)
    {
        Logger.Info($"Writing results to file {resultsFilename}.");
        string pathToFile = Environment.CurrentDirectory + "/" + resultsFilename;


        using (var streamWriter = new StreamWriter(pathToFile, true))
        {
            streamWriter.WriteLine(result);
        }
    }
}
