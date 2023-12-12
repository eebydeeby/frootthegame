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
