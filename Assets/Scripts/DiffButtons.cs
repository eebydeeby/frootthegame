// Script for button used in game over scene
// Works by assinging a difficulty to parameter to the gamemanager object, which carries it onto the testlevel scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiffButton : MonoBehaviour
{
	public bool isGameRestarting = false;

    public void EasyButton()
	{
		isGameRestarting = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().difficulty = 1;
		SceneManager.LoadScene("TestLevel");
	}

    public void MediumButton()
	{
		isGameRestarting = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().difficulty = 2;
		SceneManager.LoadScene("TestLevel");
	}

    public void HardButton()
	{
		isGameRestarting = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().difficulty = 3;
		SceneManager.LoadScene("TestLevel");
	}
}