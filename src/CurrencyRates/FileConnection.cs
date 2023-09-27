namespace FileConnection;
using Logging;

class WriterToFile
{
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
