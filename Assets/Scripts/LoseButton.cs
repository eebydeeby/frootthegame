// Script for button used in game over scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseButton : MonoBehaviour
{
	public bool isGameRestarting = false;
	
    public void NewGameButton()
	{
		isGameRestarting = true;
		SceneManager.LoadScene("MainMenu");
	}
	
    public void RestartButton()
	{
		isGameRestarting = true;
		SceneManager.LoadScene("TestLevel");
	}
}