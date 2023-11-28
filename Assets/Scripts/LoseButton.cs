// Script for button used in game over scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseButton : MonoBehaviour
{
    public void NewGameButton()
	{
		SceneManager.LoadScene("MainMenu");
	}
	
    public void RestartButton()
	{
		SceneManager.LoadScene("TestLevel");
	}
}