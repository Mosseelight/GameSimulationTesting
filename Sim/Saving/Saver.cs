using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace GameTesting
{

    public class Saver
    {
        static string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        static string objectSimPath = appPath + @"\ObjectSimSettings";
        static string neuralNetworkSimPath = appPath + @"\NeuralNetworkSaves";
        string testingSettingOBJ = objectSimPath + @"\ObjectSimSettings.json";


        string nueralNetworkSettings = neuralNetworkSimPath + @"\NeuralNetworkSave";


        public void CreateFolder()
        {
            if(!Directory.Exists(objectSimPath))
            {
                Directory.CreateDirectory(objectSimPath);
            }
            if(!Directory.Exists(neuralNetworkSimPath))
            {
                Directory.CreateDirectory(neuralNetworkSimPath);
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
            if(File.Exists(testingSettingOBJ))
            {
                string jsonText = File.ReadAllText(testingSettingOBJ);
            }
        }

        public void SaveNeuralNetworkSimSettings()
        {
            DataToSave dataToSave = new DataToSave();

            dataToSave.nerualNetworkSettings = SaverDataToSet.nerualNetworkSettings;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(nueralNetworkSettings + dataToSave.nerualNetworkSettings.saveCount + ".save", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, dataToSave);
            stream.Close();
        }

        public void SaveNeuralNetworkSimCount()
        {
            DataToSave dataToSave = new DataToSave();

            dataToSave.nerualNetworkSettings.saveCount = SaverDataToSet.nerualNetworkSettings.saveCount;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(neuralNetworkSimPath + @"\SaveFileCount.save", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, dataToSave);
            stream.Close();
        }

        public void ReadNerualNetworkSimCount()
        {
            if(File.Exists(neuralNetworkSimPath + @"\SaveFileCount.save"))
            {
                Stream stream = new FileStream(neuralNetworkSimPath + @"\SaveFileCount.save", FileMode.Open, FileAccess.Read);
                IFormatter formatter = new BinaryFormatter();
                DataToSave settings = (DataToSave)formatter.Deserialize(stream);
                SaverDataToSet.nerualNetworkSettings.saveCount = settings.nerualNetworkSettings.saveCount;
                stream.Close();
            }
        }
        public void ReadNerualNetworkSimSettings()
        {
            if(File.Exists(nueralNetworkSettings + SaverDataToSet.nerualNetworkSettings.saveCount + ".save"))
            {
                Stream stream = new FileStream(nueralNetworkSettings + SaverDataToSet.nerualNetworkSettings.saveCount + ".save", FileMode.Open, FileAccess.Read);
                IFormatter formatter = new BinaryFormatter();
                DataToSave settings = (DataToSave)formatter.Deserialize(stream);
                SaverDataToSet.nerualNetworkSettings = settings.nerualNetworkSettings;
                stream.Close();
            }
            
        }

    }


    [Serializable]public class DataToSave
    {
        public ObjSimulation.ObjectSimSettings objectSimSettings = new ObjSimulation.ObjectSimSettings();
        public NeuralNetworkHandler.NerualNetworkSettings nerualNetworkSettings = new NeuralNetworkHandler.NerualNetworkSettings();
    }
}