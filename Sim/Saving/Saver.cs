using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

public class Saver
{
    static string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    string objectSimPath = appPath + @"\ObjectSimSettings";


    public void CreateFolder()
    {
        if(!Directory.Exists(objectSimPath))
        {
            Directory.CreateDirectory(objectSimPath);
        }
    }

}