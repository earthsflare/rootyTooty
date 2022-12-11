using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public static readonly string[] playFabDataKeys = {
        "Version", "SpawnPositionX", "SpawnPositionY", "SpawnSceneIndex", "PlayerHealth",
        "RollUnlocked", "WallJumpUnlocked", "DoubleJumpUnlocked", "FireballUnlocked"
    };

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

    //Relogin into playfab (to make sure that user's session is still valid) and store data
    public static IEnumerator SaveGameToPlayfab(SaveData save)
    {
        PlayfabManager.Instance.ReLogin();

        while (!PlayfabManager.Instance.ReLoginAttempt.HasValue)
            yield return null;

        //Leave method if login attempt failed
        if (!PlayfabManager.Instance.ReLoginAttempt.Value)
        {
            PlayfabManager.Instance.gameSaved = false;
            yield break;
        }

        //Check if there's any account saved
        try
        {
            UpdateUserDataRequest request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>()
                {
                    { playFabDataKeys[0], SaveData.gameVersion },

                    { playFabDataKeys[1], save.spawnPosition[0].ToString() },
                    { playFabDataKeys[2], save.spawnPosition[1].ToString() },
                    { playFabDataKeys[3], save.spawnSceneIndex.ToString() },

                    { playFabDataKeys[4], save.playerHealth.ToString() },
                    
                    { playFabDataKeys[5], save.rollUnlocked.ToString() },
                    { playFabDataKeys[6], save.wallJumpUnlocked.ToString() },
                    { playFabDataKeys[7], save.doubleJumpUnlocked.ToString() },
                    { playFabDataKeys[8], save.fireballUnlocked.ToString() },
                }
            };

            PlayFabClientAPI.UpdateUserData(request,
                result => {
                    if (PlayfabManager.Instance != null)
                        PlayfabManager.Instance.gameSaved = true;
                },
                error => {
                    Debug.Log("Login failed: " + error.ErrorMessage);
                    if (PlayfabManager.Instance != null)
                        PlayfabManager.Instance.gameSaved = false;
                });
        }
        catch(Exception ex) { 
            Debug.LogException(ex);
            if (PlayfabManager.Instance != null)
                PlayfabManager.Instance.gameSaved = false;
        }
    }

    public static IEnumerator LoadGameFromPlayfab(string id)
    {
        PlayfabManager.Instance.SetFinishedLoadingGame(false);
        PlayfabManager.Instance.SetLoadedSave(null);

        PlayfabManager.Instance.ReLogin();

        while (!PlayfabManager.Instance.ReLoginAttempt.HasValue)
            yield return null;

        if (!PlayfabManager.Instance.ReLoginAttempt.Value)
        {
            PlayfabManager.Instance.SetFinishedLoadingGame(true);
            yield break;
        }
        try
        {
            GetUserDataRequest request = new GetUserDataRequest
            {
                PlayFabId = id,
                Keys = null,
            };

            SaveData data;

            PlayFabClientAPI.GetUserData(request, GetUserDataSuccess,
                error => { 
                    Debug.Log("Load Data Failed");
                    PlayfabManager.Instance.SetLoadedSave(null);
                });
        }
        catch (Exception ex) { 
            Debug.LogException(ex);
            PlayfabManager.Instance.SetLoadedSave(null);
        }
        yield return new WaitForSeconds(1);
        Debug.Log("We're out: " + PlayfabManager.Instance.LoadedSave.doubleJumpUnlocked);
        PlayfabManager.Instance.SetFinishedLoadingGame(true);
    }

    private static void GetUserDataSuccess(GetUserDataResult result)
    {
        if (result.Data != null)
        {
            if (CheckPlayfabData(result.Data))
            {
                PlayfabManager.Instance.SetLoadedSave(new SaveData(
                    new float[2] { float.Parse(result.Data[playFabDataKeys[1]].Value),
                                    float.Parse(result.Data[playFabDataKeys[2]].Value) },
                    int.Parse(result.Data[playFabDataKeys[3]].Value),
                    int.Parse(result.Data[playFabDataKeys[4]].Value),
                    new bool[4] { bool.Parse(result.Data[playFabDataKeys[5]].Value),
                                bool.Parse(result.Data[SaveManager.playFabDataKeys[6]].Value),
                                bool.Parse(result.Data[SaveManager.playFabDataKeys[7]].Value),
                                bool.Parse(result.Data[SaveManager.playFabDataKeys[8]].Value) }
                    ));
            }
            else
                PlayfabManager.Instance.SetLoadedSave(null);
        }
        else
            PlayfabManager.Instance.SetLoadedSave(null);
    }
    private static bool CheckPlayfabData(Dictionary<string, UserDataRecord> data)
    {
        foreach(string key in playFabDataKeys)
        {
            if (!data.ContainsKey(key))
                return false;
        }
        return true;
    }
}
