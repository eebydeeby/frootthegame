// Script for reacting to the current worm count - plays SFX and spawns fruit, handles game over
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FruitSpawner : MonoBehaviour
{

	[SerializeField] private GameObject berry;
	[SerializeField] private GameObject apple;
	[SerializeField] private GameObject pear;
	// Loads in prefab fruit to spawn
	
	[SerializeField] private AudioSource[] _hitSound;
	// Audio source to control
	[SerializeField] private GameObject pauseBg;
	
	public int lives = 3;
	public int checkpointLevel = 1; // Basic stage parameter
	
	public float countdown = 20f;
	private bool timerIsRunning = true;
	
	private int randomFruit;
	public int score = -1;
	
	// Spawns a fruit at the start of the scene
    void Start()
    {
		Time.timeScale = 1;
		Spawn();
		_hitSound = GetComponents<AudioSource>();
    }
	
	// Handles timer and manages game over scenario.
	void Update()
	{
	    if (Input.GetKeyDown(KeyCode.Space)) // Sequence for pausing ga,e
	    {
	        if (Time.timeScale != 0)
			{
				_hitSound[4].Stop();
				Time.timeScale = 0; // Freezes game time
				pauseBg.SetActive(true); // Shows pause menu
				playHit(4);
			}
			else
			{
				_hitSound[3].Stop();
				Time.timeScale = 1;
				pauseBg.SetActive(false);
				playHit(5);
			}
	    }
		
		if (timerIsRunning == true) // Handles countdown sequence
		{
			if (countdown > 0)
			{
				countdown -= Time.deltaTime;			
			}
			else
			{
				if (lives > 0) // Checks if player has more than 0 lives when countdown is over...
				{
					lives--;
					playHit(6);
					countdown = 20 - (checkpointLevel * 2); // Resets timer
				}
				else // ...And loads game over scene if not
				{
					timerIsRunning = false;
					Destroy(gameObject);
					SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
				}
			}
		}
	}

	// Spawns random prefab
	public void Spawn()
	{
		if (score % 5 == 0 && checkpointLevel == 5)
		{
			Destroy(gameObject);
			SceneManager.LoadScene("YouWin");
		}
		score++;
		if (score % 5 == 0 && score != 0)
		{
			checkpointLevel++;
			countdown = 20 - (checkpointLevel * 2);
			if (score > 0) { playHit(3); }
		}
		if (GameObject.FindGameObjectWithTag("Fruit") == null)
			{
			randomFruit = Random.Range(1,4);
			switch (randomFruit)
			{
			case 1:
				Instantiate(pear);
				break;
			case 2:
				Instantiate(apple);
				break;
			case 3:
				Instantiate(berry);
				break;		
			}
		}
		countdown = 20 - (checkpointLevel * 2);
	}
	
	// Handles SFX t0 be played -- called by other scripts
	public void playHit(int soundID)
	{
		switch (soundID)
		{
			case 1:
				_hitSound[0].Play();
				break;
			case 2:
				_hitSound[1].Play();
				break;
			case 3:
				_hitSound[2].Play();
				break;
			case 4:
				_hitSound[3].Play();
				break;
			case 5:
				_hitSound[4].Play();
				break;
			case 6:
				_hitSound[5].Play();
				break;
		}
		
	}

}
