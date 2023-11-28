// Script for button used in main menu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void NewGameButton()
	{
		SceneManager.LoadScene("TestLevel");
	}
	
    public void HowToPlayButton()
	{
		SceneManager.LoadScene("HowToPlay");
	}
	
    public void MainMenuButton()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
