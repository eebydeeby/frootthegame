// Script for button used in victory scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinButton : MonoBehaviour
{
    public void NewGameButton()
	{
		SceneManager.LoadScene("MainMenu");
	}
}