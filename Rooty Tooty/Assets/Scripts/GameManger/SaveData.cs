using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
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
}
