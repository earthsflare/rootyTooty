using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public static void SaveGameToFile(SaveData save)
    {
        //Make a file directory in the game application if it doesn't exist
        string filePath = Application.dataPath + "/SaveFiles";

        if(!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        filePath += "/saveFile.rootyTooty";


        //Make a save file from the game
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Create);
        
        formatter.Serialize(stream, save);
        stream.Close();
    }
    public static SaveData LoadGameFromFile()
    {
        //Find if the file exists
        string filePath = Application.dataPath + "/SaveFiles/saveFile.rootyTooty";
        if (File.Exists(filePath))
        {
            //Get and deserialize file to get the data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            SaveData save = (SaveData)formatter.Deserialize(stream);
            stream.Close();

            return save;
        }
        else
        {
            Debug.Log("Save file not found in " + filePath);
            return null;
        }
    }
}
