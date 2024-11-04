// Handles status of fruit, namely rotation and worm status
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Netcode;

public class ShapeRotator : NetworkBehaviour
{
	[SerializeField] private GameObject spawner; // Refers to fruit spawner object
	[SerializeField] private FruitSpawner spawnerScript;

	private int difficulty;
	public int fruitOrder; // Parameter for checking where fruit has spawned, checked by fruitspawner script

	private Vector3 startMousePos; // Handles where the mouse was at upon first click
	private Vector3 newMousePos; // Handles current mouse position
	private Vector3 rotateMousePos; // Difference between previous two variables, used to calculate rotation

	private int maxWorms; // Starting number of worms for each fruit
	public int currentWorms; // Current number of worms
	public float fruitCountdown; // Current lifespan of fruit: If this reaches 0, player loses a life

	public float rotateSpeed; // How fast the fruit should rotate
	private bool dragged; // If the fruit is being dragged or no, ie, if player clicked on fruit
	
	private bool gameIsShuttingDown;
	public GameObject restarterObject;
	public LoseButton restarter;
	// Loads lose button script: parameters are set there to see if game is restarting
	// Used to help make sure this script deletes fruit properly between scenes

	public GameObject fruitText;
	public TMP_Text fruitString;



	public GameObject particle; // Particle effect used when fruit is destroyed
	
	void Awake()
	{
		restarterObject = GameObject.Find("ButtonController");
		restarter = restarterObject.GetComponent<LoseButton>();

		spawner = GameObject.Find("FruitSpawner");
		spawnerScript = spawner.GetComponent<FruitSpawner>();

		fruitText = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
		fruitString = fruitText.GetComponent<TMP_Text>();


		difficulty = spawnerScript.difficulty;

		maxWorms = Random.Range(3, 7); // Random number of worms to start
		currentWorms = maxWorms; // Sets current worms to max number of worms when fruit is spawned

		fruitCountdown = 33 - (spawnerScript.checkpointLevel * 2); // Sets fruit lifespawn
		particle = GameObject.Find("FruitPop");
	}

	void Start()
	{
		if (fruitOrder == 0){
			GetComponent<NetworkObject>().Despawn();
		}
		SetPosition();
	}
	
	void OnMouseDown()
	{
		dragged = true; // Set if the fruit is currently being dragged by the player
		startMousePos = Input.mousePosition;
	}
	
	// Releases fruit
	void OnMouseUp()
	{
		dragged = false;
	}

    void Update()
    {	
		if (currentWorms == 0)
		{
			print("No worms left!");
			particle.transform.position = this.transform.position;
			particle.GetComponent<ParticleSystem>().Play();
			if (IsHost){
				print("It's me, the server!");
				GetComponent<NetworkObject>().Despawn();
			}
			// Sets the particle effect to location of the fruit and plays it when fruit is destroyed
		}
		
		// If fruit is being dragged, start rotating the fruit according to difference between curent and original position of cursor.
        if (dragged == true)
		{
			newMousePos = Input.mousePosition;
			rotateMousePos = (Vector3.Normalize(startMousePos - newMousePos) * (rotateSpeed/Time.timeScale));
        	this.transform.Rotate(-rotateMousePos.y * Time.deltaTime, rotateMousePos.x * Time.deltaTime, 0, Space.World);
        }

		if (spawnerScript.timerIsRunning == true) // Handles countdown sequence
		{
			if (fruitCountdown > 0)
			{
				fruitCountdown -= Time.deltaTime;			
			}
			else
			{
				if (spawnerScript.lives.Value > 0) // Checks if player has more than 0 lives when countdown is over...
				{
					spawnerScript.lives.Value--;
					spawnerScript.playHit(6);
					fruitCountdown = 33 - (spawnerScript.checkpointLevel * 2); // Resets timer
				}
				else // ...And loads game over scene if not
				{
					spawnerScript.timerIsRunning = false;
					if (IsHost){
						GetComponent<NetworkObject>().Despawn();
					}
					SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
				}
			}
		}
    }

	void LateUpdate() // Handles position and rotation of fruit stat text below fruit model
	{
		fruitText.transform.rotation = Quaternion.Euler(0,0,0);
		fruitText.transform.position = new Vector3(((this.gameObject.transform.position.x)),
			((this.gameObject.transform.position.y)-4), 0);
		fruitString.text += ("\nTime left: " + fruitCountdown.ToString("#.00") + "\nWorms left: " + currentWorms);
	}
	
	// Once all the worms are cleared, destroy the fruit and spawn a new one.
	public override void OnNetworkDespawn()
	{
		if (IsHost){
			spawnerScript.score.Value++;
			Debug.Log("FRUIT IS BEING RESPAWNED NOW!!!!");
			if (this.gameIsShuttingDown == false && restarter.isGameRestarting == false){
				switch (fruitOrder)
				{
					case 1:
						spawner.GetComponent<FruitSpawner>().isSlotOneTaken = false;
						break;
					case 2:
						spawner.GetComponent<FruitSpawner>().isSlotTwoTaken = false;
						break;
					case 3:
						spawner.GetComponent<FruitSpawner>().isSlotThreeTaken = false;
						break;
				}
				spawner.GetComponent<FruitSpawner>().orderFruit();
				spawner.GetComponent<FruitSpawner>().SpawnServerRpc();
				//spawner.GetComponent<FruitSpawner>().SpawnClientRpc();
			}
		}
	}

	void SetPosition() // Checks what order the fruit should be in and places it depending on its place and game difficulty
	{
		if (fruitOrder == 0 || fruitOrder == 1){
			switch (difficulty)
			{	
				case 1:
					this.transform.Translate(0, 0, 0);
					break;
				case 2:
					this.transform.Translate(-4, 0, 0);
					break;
				case 3:
					this.transform.Translate(-8, 0, 0);
					break;
			}
		}
		if (fruitOrder == 2){
			switch (difficulty)
			{	
				case 2:
					this.transform.Translate(4, 0, 0);
					break;
				case 3:
					this.transform.Translate(0, 0, 0);
					break;
			}
		}
		if (fruitOrder == 3){
			this.transform.Translate(8, 0, 0);
		}
	}

    void OnApplicationQuit()
    {
    	gameIsShuttingDown = true;
    }
}