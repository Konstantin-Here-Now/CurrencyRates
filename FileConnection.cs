namespace FileConnection;

class JSONFileWriter
{
    public static void WriteResultToFile(string result, string resultsFilename)
    {
        string pathToFile = Environment.CurrentDirectory + "/" + resultsFilename;


        using (var streamWriter = new StreamWriter(pathToFile, true))
        {
            streamWriter.WriteLine(result);
        }

    }
}
