using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDesign : MonoBehaviour
{
    public enum team {PKPeach=0,BooMario=1}
    public GameObject[] playerPrefab;
    public Transform SpawnPos;
    private void Start()
    {
        int index = PlayerPrefs.GetInt("team");
        if(index != 0 && index != 1)
        {
            index = (int)team.PKPeach;
        }
        
        if (SpawnPos != null)
            Instantiate(playerPrefab[index],SpawnPos);
        else
            throw new Exception("Spawn Pos not set");
    }
}
