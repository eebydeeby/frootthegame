// Script for reacting to the current worm count - plays SFX and spawns fruit, handles game over
// Handles most of the game logic during the main level
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

	[SerializeField] private GameObject heart; // Heart powerup prefab
	[SerializeField] private GameObject watch; // Stopwatch powerup prefab

	public bool isSlotOneTaken;
	public bool isSlotTwoTaken;
	public bool isSlotThreeTaken;
	[SerializeField] private int positionToSpawn;
	// Checks to see what fruit slots are taken, and spawns a fruit in which ever one is free
	// Difficulty increases the number of fruit slots

	private GameObject instance;
	
	[SerializeField] private AudioSource[] _hitSound;
	// Audio source to control
	[SerializeField] private GameObject pauseBg;
	
	public int difficulty; // Parameter which adjusts the number of fruit to spawn
	
	public int lives;
	public int checkpointLevel = 1; // Basic stage parameter, reduces timer countdown as levels progress

	[SerializeField] private float powerCountdown; // Countdown until next powerup is spawned

	public bool timerIsRunning = true; // Decides whether or not to progress with the normal fruit countdown process
	
	private int randomFruit; // Variable for deciding which fruit prefab to spawn
	public int score;

	public float slowTime; // Slows down delta time when this variable is aboce zero - reduces down to 0 over time
	public float unpauseTime; // Stores whether or not time was slowed down when game is paused

	void Awake()
	{
		lives = 3;
		score = 0;
	}

	// Spawns a fruit at the start of the scene
    void Start()
    {
		Time.timeScale = 1;
		powerCountdown = Random.Range(5.0f, 15.0f);
		_hitSound = GetComponents<AudioSource>();
		difficulty = GameObject.Find("GameManager").GetComponent<GameManager>().difficulty;
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
	        if (Time.timeScale > 0)
			{
				unpauseTime = Time.timeScale;
				_hitSound[4].Stop();
				Time.timeScale = 0f; // Freezes game time
				timerIsRunning = false;
				pauseBg.SetActive(true); // Shows pause menu
				playHit(4);
			}
			else
			{
				_hitSound[3].Stop();
				Time.timeScale = unpauseTime;
				timerIsRunning = true;
				pauseBg.SetActive(false);
				playHit(5);
			}
	    }

		if (timerIsRunning == true) // Handles countdown sequences
		{
			if (powerCountdown > 0)
			{
				powerCountdown -= Time.deltaTime;
			}
			else // Spawns powerups
			{
				if ((Random.Range(0.0f, 2.0f)) > 1.0f)
				{
					Instantiate(heart);
				}
				else
				{
					Instantiate(watch);
				}
				powerCountdown = Random.Range(10.0f, 60.0f);
			}
			if (slowTime > 0)
			{
				slowTime -= Time.deltaTime;
			}
			if (slowTime <= 0) // Returns game speed to normal once slowdown powerup time is over
			{
				Time.timeScale = 1f;
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
		if (score % 5 == 0 && score != 0)
		{
			checkpointLevel++;
			//countdown = 20 - (checkpointLevel * 2);
			if (score > 0) { playHit(3); }
		}
		if (fruits.Length < difficulty)
			{
			randomFruit = Random.Range(1,4);
			orderFruit();
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
	if (!isSlotOneTaken){ // Checks if a slot/space is empty and spawns a fruit there if so
		positionToSpawn = 1;
	}
	else if (!isSlotTwoTaken){
		positionToSpawn = 2;
	}
	else if (!isSlotThreeTaken){
		positionToSpawn = 3;
	}
	}

	// Handles SFX to be played -- called by other scripts
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
			case 7:
				_hitSound[6].Play();
				break;
			case 8:
				_hitSound[7].Play();
				break;
		}
		
	}
}
