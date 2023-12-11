// Script for button used in main menu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
	public bool isGameRestarting = false;

    public void NewGameButton()
	{
		isGameRestarting = true;
		SceneManager.LoadScene("DifficultyPicker");
	}
	
    public void HowToPlayButton()
	{
		SceneManager.LoadScene("HowToPlay");
	}
	
    public void MainMenuButton()
	{
		isGameRestarting = true;
		SceneManager.LoadScene("MainMenu");
	}
}
