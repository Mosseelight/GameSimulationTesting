using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

public class Saver
{
    static string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    static string objectSimPath = appPath + @"\ObjectSimSettings";

    //hardcoded object simulation settings
    string normalSettingOBJ = objectSimPath + @"\Normal.json";


    public void CreateFolder()
    {
        if(!Directory.Exists(objectSimPath))
        {
            Directory.CreateDirectory(objectSimPath);
        }
    }

    public void SaveSimSettings()
    {
        DataToSave dataToSave = new DataToSave();
        JsonSerializerOptions options = new JsonSerializerOptions();
        options.WriteIndented = true;

        dataToSave.objectSimSettings = SaverDataToSet.objectSimSettingsSet;
        Console.WriteLine(SaverDataToSet.objectSimSettingsSet);

        string jsonText = JsonSerializer.Serialize(dataToSave.objectSimSettings, options);
        File.WriteAllText(normalSettingOBJ, jsonText);
        Console.WriteLine(jsonText + " Write");
    }

    public void ReadSimSettings()
    {
        string jsonText = File.ReadAllText(normalSettingOBJ);
        Console.WriteLine(jsonText + " Read");
    }

}


[Serializable]public class DataToSave
{
    public ObjSimulation.ObjectSimSettings objectSimSettings = new ObjSimulation.ObjectSimSettings();
}