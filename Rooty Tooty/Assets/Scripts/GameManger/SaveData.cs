using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public const string gameVersion = "Pre-Alpha 1.0";

    public float[] spawnPosition = new float[2];
    public int spawnSceneIndex = 1;

    public int playerHealth = 1;

    public bool rollUnlocked = false;
    public bool wallJumpUnlocked = false;
    public bool doubleJumpUnlocked = false;
    public bool fireballUnlocked = false;

    public SaveData()
    {

    }

    public SaveData(float[] spawnLoc, int spawnIndex, int pHealth, bool[] unlocked)
    {
        spawnPosition[0] = spawnLoc[0];
        spawnPosition[1] = spawnLoc[1];

        spawnSceneIndex = spawnIndex;

        playerHealth = pHealth;

        doubleJumpUnlocked = unlocked[0];
        fireballUnlocked = unlocked[1];
        rollUnlocked = unlocked[2];
        wallJumpUnlocked = unlocked[3];
    }

    public SaveData(Dictionary<string, string> data)
    {
        spawnPosition = new float[2] { 
            float.Parse(data[SaveManager.playFabDataKeys[1]]), float.Parse(data[SaveManager.playFabDataKeys[2]]) };
        spawnSceneIndex = int.Parse(data[SaveManager.playFabDataKeys[3]]);

        playerHealth = int.Parse(data[SaveManager.playFabDataKeys[4]]);

        rollUnlocked = bool.Parse(data[SaveManager.playFabDataKeys[5]]);
        wallJumpUnlocked = bool.Parse(data[SaveManager.playFabDataKeys[6]]);
        doubleJumpUnlocked = bool.Parse(data[SaveManager.playFabDataKeys[7]]);
        fireballUnlocked = bool.Parse(data[SaveManager.playFabDataKeys[8]]);
    }

    public SaveData(Dictionary<string, UserDataRecord> data)
    {
        spawnPosition = new float[2] {
            float.Parse(data[SaveManager.playFabDataKeys[1]].Value), float.Parse(data[SaveManager.playFabDataKeys[2]].Value) };
        spawnSceneIndex = int.Parse(data[SaveManager.playFabDataKeys[3]].Value);

        playerHealth = int.Parse(data[SaveManager.playFabDataKeys[4]].Value);

        rollUnlocked = bool.Parse(data[SaveManager.playFabDataKeys[5]].Value);
        wallJumpUnlocked = bool.Parse(data[SaveManager.playFabDataKeys[6]].Value);
        doubleJumpUnlocked = bool.Parse(data[SaveManager.playFabDataKeys[7]].Value);
        fireballUnlocked = bool.Parse(data[SaveManager.playFabDataKeys[8]].Value);
    }
}
