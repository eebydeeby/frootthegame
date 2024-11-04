// Script for button used in main menu
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : NetworkBehaviour
{
	public bool isGameRestarting = false;
	private GameObject nvm;
	public NetworkObject localPlayer;

	public void Awake()
	{
		localPlayer = NetworkManager.LocalClient.PlayerObject;
	}

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

	public void LobbyButton()
	{
		SceneManager.LoadScene("Lobby");
	}

	public void MultiplayerButton()
	{
		localPlayer = NetworkManager.LocalClient.PlayerObject;
		nvm = GameObject.Find("/NetworkVariableManager");
		if (nvm.GetComponent<NetworkVariableManager>().is_connected.Value == true && localPlayer.IsOwnedByServer && localPlayer.IsLocalPlayer)
		{
			GameObject.Find("GameManager").GetComponent<GameManager>().difficulty = 1;
			NetworkManager.SceneManager.LoadScene("OnlineLevel", LoadSceneMode.Single);
		}
		else 
		{
			print ("something went wrong: ");
		}
	}

	public void StartServer(){
        NetworkManager.Singleton.StartServer();
    }

    public void StartClient(){
        NetworkManager.Singleton.StartClient();
    }

    public void StartHost(){
        NetworkManager.Singleton.StartHost();
    }
}
