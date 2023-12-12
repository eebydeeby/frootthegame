// Simple script for storing choice between difficulty selection screen and testlevel screen
// Called by fruitspawner and shape rotator scripts to decide how many fruit to spawn on screen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int difficulty;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
