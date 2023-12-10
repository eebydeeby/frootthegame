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

	[SerializeField] private bool isSlotOneTaken;
	[SerializeField] private bool isSlotTwoTaken;
	[SerializeField] private bool isSlotThreeTaken;
	[SerializeField] private int positionToSpawn;

	private GameObject instance;
	
	[SerializeField] private AudioSource[] _hitSound;
	// Audio source to control
	[SerializeField] private GameObject pauseBg;
	
	public int difficulty;
	
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
		_hitSound = GetComponents<AudioSource>();
		difficulty = 3;
		for (int i = 0; i < difficulty; i++)
		{
			Spawn();
		}
    }
	
	// Handles timer and manages game over scenario.
	void Update()
	{
	    if (Input.GetKeyDown(KeyCode.Space)) // Sequence for pausing game
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
		GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
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
		if (fruits.Length < difficulty)
			orderFruit();
			{
			randomFruit = Random.Range(1,4);
			switch (randomFruit)
			{
			case 1:
				instance = Instantiate(pear);
				instance.GetComponent<ShapeRotator>().fruitOrder = positionToSpawn;
				break;
			case 2:
				instance = Instantiate(apple);
				instance.GetComponent<ShapeRotator>().fruitOrder = positionToSpawn;
				break;
			case 3:
				instance = Instantiate(berry);
				instance.GetComponent<ShapeRotator>().fruitOrder = positionToSpawn;
				break;		
			}
		}
		countdown = 20 - (checkpointLevel * 2);
	}
	
	public void orderFruit()
	{
		GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
		foreach (GameObject fruit in fruits)
		{
			switch (fruit.GetComponent<ShapeRotator>().fruitOrder)
			{
				case 1:
					isSlotOneTaken = true;
					break;
				case 2:
					isSlotTwoTaken = true;
					break;
				case 3:
					isSlotThreeTaken = true;
					break;
			}
	}
	if (!isSlotOneTaken){
		positionToSpawn = 1;
	}
	else if (!isSlotTwoTaken){
		positionToSpawn = 2;
	}
	else if (!isSlotThreeTaken){
		positionToSpawn = 3;
	}
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