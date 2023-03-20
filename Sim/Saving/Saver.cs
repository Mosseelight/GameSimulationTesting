using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace GameTesting
{

    public class Saver
    {
        static string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        static string objectSimPath = appPath + @"\ObjectSimSettings";
        static string neuralNetworkSimPath = appPath + @"\NeuralNetworkSaves";

        //hardcoded object simulation settings
        string normalSettingOBJ = objectSimPath + @"\Normal.json";
        string sunSettingOBJ = objectSimPath + @"\Sun.json";
        string funSettingOBJ = objectSimPath + @"\Fun.json";
        string bugsSettingOBJ = objectSimPath + @"\Bugs.json";
        string testingSettingOBJ = objectSimPath + @"\Testing.json";


        string nueralNetworkSettings = neuralNetworkSimPath + @"\NeuralNetworkSave";


        public void CreateFolder()
        {
            if(!Directory.Exists(objectSimPath))
            {
                Directory.CreateDirectory(objectSimPath);
            }
        }

        public void SaveObjectSimSettings()
        {
            DataToSave dataToSave = new DataToSave();
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;

            dataToSave.objectSimSettings = SaverDataToSet.objectSimSettingsSet;

            string jsonText = JsonSerializer.Serialize(dataToSave.objectSimSettings, options);
            File.WriteAllText(testingSettingOBJ, jsonText);
        }

        public void ReadObjectSimSettings()
        {
            string jsonText = File.ReadAllText(testingSettingOBJ);
        }

        public void SaveNeuralNetworkSimSettings()
        {
            DataToSave dataToSave = new DataToSave();
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;

            dataToSave.nerualNetworkSettings = SaverDataToSet.nerualNetworkSettings;

            string jsonText = JsonSerializer.Serialize(dataToSave.nerualNetworkSettings, options);
            File.WriteAllText(nueralNetworkSettings + dataToSave.nerualNetworkSettings.saveCount, jsonText);
        }

        public void ReadNerualNetworkSimSettings()
        {
            string jsonText = File.ReadAllText(nueralNetworkSettings);
        }

    }


    [Serializable]public class DataToSave
    {
        public ObjSimulation.ObjectSimSettings objectSimSettings = new ObjSimulation.ObjectSimSettings();
        public NeuralNetworkHandler.NerualNetworkSettings nerualNetworkSettings = new NeuralNetworkHandler.NerualNetworkSettings();
    }
}